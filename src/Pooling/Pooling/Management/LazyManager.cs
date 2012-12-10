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
    /// Lazy implementation of <see cref="IPoolItemManager{T}"/>.
    /// </summary>
    /// <remarks>The class is not thread safe.</remarks>
    /// <typeparam name="T">type of object to handle</typeparam>
    public class LazyManager<T> : PoolItemManagerBase<T> 
        where T : IDisposable
    {
        private readonly Func<T> factory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="itemStore">item storage</param>
        /// <param name="factory">factory to create items</param>
        /// <exception cref="ArgumentNullException">if either <c>itemStore</c> or <c>factory</c> is <c>null</c></exception>
        public LazyManager(IItemStore<T> itemStore, Func<T> factory)
            : base (itemStore)
        {
            Assert.ArgumentNotNull(factory, "factory");
            this.factory = factory;
        }

        #region Implementation of IPoolItemManager<T>

        public override T GetItem()
        {
            if (ItemStore.Count > 0)
            {
                return ItemStore.Fetch();
            }
            return factory();
        }

        public override void PutItem(T item)
        {
            ItemStore.Store(item);
        }

        #endregion
    }
}
