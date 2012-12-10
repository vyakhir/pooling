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

namespace Pooling.Management
{
    /// <summary>
    /// A generic pool item manager. Manages extracting and putting back items.
    /// Defines a strategy how and when items are created and how they are put back.
    /// E.g. items can be created in advance (preloading) or on first use (lazy).
    /// </summary>
    /// <remarks>The class is not thread safe.</remarks>
    /// <typeparam name="T">type of object to handle</typeparam>
    public interface IPoolItemManager<T> : IDisposable
        where T : IDisposable
    {
        /// <summary>
        /// Gets an item of type <c>T</c>.
        /// </summary>
        /// <returns>item instance</returns>
        T GetItem();

        /// <summary>
        /// Receives an item of type <c>T</c> and considers it as free for future use.
        /// </summary>
        /// <param name="item">item to put back</param>
        void PutItem(T item);
    }
}
