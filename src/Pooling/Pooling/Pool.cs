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
using Pooling.Assertions;
using Pooling.Management;
using Pooling.Storage;

namespace Pooling
{
    /// <summary>
    /// Implementation of <see cref="IPool"/>. 
    /// <br />
    /// This is a generic implementation, which responsibilityis just thread safety. 
    /// Actual pool management is delegated to pool item manager.
    /// </summary>
    /// <remarks>The class is thread safe.</remarks>
    /// <typeparam name="T">type of object to handle</typeparam>
    public class Pool<T> : IPool<T> 
        where T : IDisposable
    {
        /// <summary>
        /// Default pool capacity.
        /// </summary>
        private const int DefaultCapacity = 100;

        private readonly IPoolItemManager<T> manager;
        private readonly Semaphore poolSemaphore;

        private int count;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="factory">factory method to create items</param>
        /// <param name="capacity">pool capacity (optional)</param>
        public Pool(Func<T> factory, int capacity = DefaultCapacity)
            : this(capacity)
        {
            manager = new EagerManager<T>(new QueueStore<T>(capacity), factory, capacity);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="factory">factory method to create items</param>
        /// <param name="capacity">pool capacity (optional)</param>
        public Pool(Func<IPool<T>, T> factory, int capacity = DefaultCapacity)
            : this(capacity)
        {
            manager = new EagerManager<T>(new QueueStore<T>(capacity), () => factory(this), capacity);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="manager">pool item manager</param>
        /// <param name="capacity">pool capacity (optional)</param>
        public Pool(IPoolItemManager<T> manager, int capacity = DefaultCapacity)
            : this(capacity)
        {
            this.manager = manager;
        }

        private Pool(int capacity)
        {
            Assert.ArgumentInRange(capacity > 0, "capacity", capacity);
            poolSemaphore = new Semaphore(capacity, capacity);
            count = Capacity = capacity;
        }

        #region Implementation of IPool

        public T Acquire()
        {
            poolSemaphore.WaitOne();
            lock (manager)
            {
                try
                {
                    var item = manager.GetItem();
                    count--;
                    return item;
                }
                catch
                {
                    poolSemaphore.Release();
                    throw;
                }
            }
        }

        public int Release(T item)
        {
            lock (manager)
            {
                manager.PutItem(item);
                count++;
                poolSemaphore.Release();
                return Count;
            }
        }

        public void Dispose()
        {
            lock (manager)
            {
                if (IsDisposed)
                {
                    return;
                }

                IsDisposed = true;
                manager.Dispose();
            }
            poolSemaphore.Close();
        }

        public bool IsDisposed { get; private set; }

        public int Capacity { get; private set; }

        public int Count
        {
            get
            {
                lock (manager)
                {
                    return count;
                }
            }
        }

        #endregion
    }
}
