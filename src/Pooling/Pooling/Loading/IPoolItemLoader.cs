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
    /// A generic pool item loader. Defines a strategy how and when items are created.
    /// E.g. items can be created in advance (preloading) or on first use (lazy).
    /// </summary>
    /// <typeparam name="T">type of object to handle</typeparam>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// IPoolItemLoader<Object> loader;
    /// ]]> 
    /// //  intialize your loader here - ItemStore and Factory must be set
    /// 
    /// if (loader.IsPreloadSupported)
    /// {
    ///     loader.Preload(5);
    /// }
    /// 
    /// ...
    /// 
    /// var obj = loader.LoadItem();
    /// </code>
    /// </example>
    public interface IPoolItemLoader<T>
    {
        /// <summary>
        /// Item Storage.
        /// </summary>
        IItemStore<T> ItemStore { get; set; }

        /// <summary>
        /// Factory to create items.
        /// </summary>
        Func<T> Factory { get; set; }

        /// <summary>
        /// Returns <c>true</c> if implementation supports preloading.
        /// </summary>
        bool IsPreloadSupported { get; }

        /// <summary>
        /// Preloads given number of elements.
        /// </summary>
        /// <param name="size">number of items to preload</param>
        /// <exception cref="NotSupportedException">if preloading is not supported</exception>
        /// <exception cref="InvalidOperationException">if either factory or storage is not set</exception>
        void Preload(int size);

        /// <summary>
        /// Creates an item of type <c>T</c>.
        /// </summary>
        /// <returns>item instance</returns>
        /// <exception cref="InvalidOperationException">if either factory or storage is not set</exception>
        T LoadItem();
    }
}
