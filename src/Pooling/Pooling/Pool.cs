/*************************************************************************************************************
 * 
 * Taken from:
 *     http://stackoverflow.com/questions/2510975/c-sharp-object-pooling-pattern-implementation/2572919
 *
 * Adapted by:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *************************************************************************************************************/

using System;
using System.Threading;
using Pooling.Loading;
using Pooling.Storage;

namespace Pooling
{
    /// <summary>
    /// Implementation of <see cref="IPool"/>. This is an abstract implementation responsible only pooling.
    /// The strategies of creating and storing pooled items are delegated to external entities.
    /// </summary>
    /// <remarks>The class is thread safe.</remarks>
    /// <typeparam name="T"></typeparam>
    public class Pool<T> : IPool<T> 
        where T : IDisposable
    {
        private readonly object syncRoot = new object();
        private readonly IPoolItemLoader<T> loader;
        private readonly IItemStore<T> itemStore;
        private readonly int size;
        private readonly Semaphore poolSizeSemaphore;
        private bool isDisposed;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="size">max pool size</param>
        /// <param name="factory">factory method to create pool items</param>
        /// <param name="loader">pool item loader</param>
        /// <param name="itemStore">pool item storage</param>
        public Pool(int size, Func<Pool<T>, T> factory, 
                    IPoolItemLoader<T> loader, IItemStore<T> itemStore)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException("size", size, "Argument 'size' must be greater than zero.");
            }
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            this.size = size;
            this.loader = loader;
            loader.Factory = () => factory(this);
            loader.ItemStore = itemStore;
            this.itemStore = itemStore;

            poolSizeSemaphore = new Semaphore(size, size);

            if (loader.IsPreloadSupported)
            {
                loader.Preload(size);
            }
        }

        #region Implementation of IPool

        public T Acquire()
        {
            poolSizeSemaphore.WaitOne();
            lock (syncRoot)
            {
                return loader.LoadItem();
            }
        }

        public void Release(T item)
        {
            lock (syncRoot)
            {
                itemStore.Store(item);
            }
            poolSizeSemaphore.Release();
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;
            lock (syncRoot)
            {
                while (itemStore.Count > 0)
                {
                    var disposable = itemStore.Fetch();
                    disposable.Dispose();
                }
            }
            poolSizeSemaphore.Close();
        }

        public bool IsDisposed
        {
            get { return isDisposed; }
        }

        public int Size
        {
            get { return size; }
        }

        #endregion
    }
}
