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
    /// A queue-based implementation of <see cref="IItemStore{T}"/>.
    /// </summary>
    /// <remarks>The implementation is not thread safe.</remarks>
    /// <typeparam name="T">type of object to store</typeparam>
    public class QueueStore<T> : Queue<T>, IItemStore<T>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public QueueStore()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="capacity">storage capacity</param>
        public QueueStore(int capacity)
            : base(capacity)
        {
        }

        #region Implementation of IItemStore

        public T Fetch()
        {
            return Dequeue();
        }

        public void Store(T item)
        {
            Enqueue(item);
        }

        #endregion
    }
}
