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
using Pooling.Storage;

namespace Pooling.Loading
{
    /// <summary>
    /// Lazy implementation of <see cref="IPoolItemLoader{T}"/>.
    /// </summary>
    /// <remarks>The class is not thread safe.</remarks>
    /// <typeparam name="T">type of object to handle</typeparam>
    public class LazyLoader<T> : IPoolItemLoader<T> 
    {
        private int count;

        #region Implementation of IPoolItemLoader<T>

        public IItemStore<T> ItemStore { get; set; }

        public Func<T> Factory { get; set; }

        public bool IsPreloadSupported
        {
            get { return false; }
        }
        
        public void Preload(int size)
        {
            throw new NotSupportedException("Lazy initialization does not support item preloading");
        }

        public T LoadItem()
        {
            if (ItemStore == null)
            {
                throw new InvalidOperationException("Item store is not set");
            }
            if (Factory == null)
            {
                throw new InvalidOperationException("Factory is not set");
            }

            if (ItemStore.Count > 0)
            {
                return ItemStore.Fetch();
            }
            count++;
            return Factory();
        }

        #endregion
    }
}
