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

namespace Pooling
{
    /// <summary>
    /// Defines generic object pooling api.
    /// </summary>
    /// <typeparam name="T">type of object to pool</typeparam>
    public interface IPool<T> : IDisposable 
        where T : IDisposable
    {
        /// <summary>
        /// Takes an element from pool. If no elements available, waits for a free object to come.
        /// </summary>
        /// <returns>a pooled object to use</returns>
        T Acquire();

        /// <summary>
        /// Puts an objects back into pool.
        /// </summary>
        /// <param name="item">a pooled object</param>
        /// <returns>number of available items in the pool</returns>
        int Release(T item);

        /// <summary>
        /// Returns <c>true</c> if the pool has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Returns pool capacity (maximum number of items the pool can handle).
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// Returns number of available items in the pool.
        /// </summary>
        int Count { get; }
    }
}
