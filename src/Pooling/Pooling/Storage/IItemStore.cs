/*************************************************************************************************************
 * 
 * Taken from:
 *     http://stackoverflow.com/questions/2510975/c-sharp-object-pooling-pattern-implementation/2572919
 *
 * Adapted by:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *************************************************************************************************************/

namespace Pooling.Storage
{
    /// <summary>
    /// Generic item storage.
    /// </summary>
    /// <typeparam name="T">type of object to store</typeparam>
    public interface IItemStore<T>
    {
        /// <summary>
        /// Fetches an object from storage.
        /// </summary>
        /// <returns>object instance</returns>
        T Fetch();

        /// <summary>
        /// Puts an object into storage.
        /// </summary>
        /// <param name="item">object to store</param>
        void Store(T item);

        /// <summary>
        /// Returns number of object in storage.
        /// </summary>
        int Count { get; }
    }
}
