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
    /// Eager implementation of <see cref="IPoolItemManager{T}"/>.
    /// </summary>
    /// <remarks>The class is not thread safe.</remarks>
    /// <typeparam name="T">type of object to handle</typeparam>
    public class EagerManager<T> : PoolItemManagerBase<T> 
        where T : IDisposable
    {
        private readonly Func<T> factory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="itemStore">item storage</param>
        /// <param name="factory">factory to create items</param>
        /// <param name="preloadCount">number of items to preload</param>
        /// <exception cref="ArgumentNullException">if either <c>itemStore</c> or <c>factory</c> is <c>null</c></exception>
        /// <exception cref="ArgumentNullException">if <c>preloadCount</c> is less or equal to zero</exception>
        public EagerManager(IItemStore<T> itemStore, Func<T> factory, int preloadCount)
            : base(itemStore)
        {
            Assert.ArgumentNotNull(factory, "factory");
            Assert.ArgumentInRange(preloadCount > 0, "preloadCount", preloadCount);

            this.factory = factory;
            Preload(preloadCount);
        }

        #region Implementation of PoolItemManagerBase

        public override T GetItem()
        {
            return ItemStore.Fetch();
        }

        public override void PutItem(T item)
        {
            ItemStore.Store(item);
        }

        #endregion

        private void Preload(int preloadCount)
        {
            for (int i = 0; i < preloadCount; i++)
            {
                ItemStore.Store(factory());
            }
        }
    }
}
