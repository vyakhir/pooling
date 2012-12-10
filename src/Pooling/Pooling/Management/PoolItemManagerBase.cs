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
using Pooling.Assertions;
using Pooling.Storage;

namespace Pooling.Management
{
    /// <summary>
    /// Base class for pool item management.
    /// </summary>
    /// <typeparam name="T">type of object to handle</typeparam>
    public abstract class PoolItemManagerBase<T> : IPoolItemManager<T>
        where T : IDisposable
    {
        protected readonly IItemStore<T> ItemStore;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="itemStore">item storage</param>
        /// <exception cref="ArgumentNullException">if either <c>itemStore</c> is <c>null</c></exception>
        protected PoolItemManagerBase(IItemStore<T> itemStore)
        {
            Assert.ArgumentNotNull(itemStore, "itemStore");
            ItemStore = itemStore;
        }

        public abstract T GetItem();

        public abstract void PutItem(T item);

        public void Dispose()
        {
            while (ItemStore.Count > 0)
            {
                var item = ItemStore.Fetch();
                ((IDisposable)item).Dispose();
            }
        }
    }
}
