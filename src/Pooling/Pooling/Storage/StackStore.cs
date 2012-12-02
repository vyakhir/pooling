/*************************************************************************************************************
 * 
 * Taken from:
 *     http://stackoverflow.com/questions/2510975/c-sharp-object-pooling-pattern-implementation/2572919
 *
 * Adapted by:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *************************************************************************************************************/

using System.Collections.Generic;

namespace Pooling.Storage
{
    /// <summary>
    /// A stack-based implementation of <see cref="IItemStore{T}"/>.
    /// </summary>
    /// <remarks>The implementation is not thread safe.</remarks>
    /// <typeparam name="T">type of object to store</typeparam>
    public class StackStore<T> : Stack<T>, IItemStore<T>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public StackStore()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="capacity">store capacity</param>
        public StackStore(int capacity)
            : base(capacity)
        {
        }

        #region Implementation of IItemStore

        public T Fetch()
        {
            return Pop();
        }

        public void Store(T item)
        {
            Push(item);
        }

        #endregion
    }
}
