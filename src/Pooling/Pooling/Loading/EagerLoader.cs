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
using Pooling.Storage;

namespace Pooling.Loading
{
    /// <summary>
    /// Eager implementation of <see cref="IPoolItemLoader{T}"/>.
    /// </summary>
    /// <remarks>The class is not thread safe.</remarks>
    /// <typeparam name="T">type of object to handle</typeparam>
    public class EagerLoader<T> : IPoolItemLoader<T>
    {
        #region Implementation of IPoolItemLoader<T>

        public IItemStore<T> ItemStore { get; set; }

        public Func<T> Factory { get; set; }

        public bool IsPreloadSupported
        {
            get { return true; }
        }

        public void Preload(int size)
        {
            if (ItemStore == null)
            {
                throw new InvalidOperationException("Item store is not set");
            }
            if (Factory == null)
            {
                throw new InvalidOperationException("Factory is not set");
            }

            for (int i = 0; i < size; i++)
            {
                T item = Factory();
                ItemStore.Store(item);
            }
        }

        public T LoadItem()
        {
            if (ItemStore == null)
            {
                throw new InvalidOperationException("Item store is not set");
            }

            return ItemStore.Fetch();
        }

        #endregion
    }
}
