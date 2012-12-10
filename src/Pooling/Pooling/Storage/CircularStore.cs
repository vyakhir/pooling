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
using System.Collections.Generic;

namespace Pooling.Storage
{
    /// <summary>
    /// A circular implementation of <see cref="IItemStore{T}"/>.
    /// </summary>
    /// <remarks>The implementation is not thread safe.</remarks>
    /// <typeparam name="T">type of object to store</typeparam>
    public class CircularStore<T> : IItemStore<T>
    {
        private readonly List<Slot> slots;
        private int freeSlotCount;
        private int position = -1;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CircularStore()
        {
            slots = new List<Slot>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="capacity">store capacity</param>
        public CircularStore(int capacity)
        {
            slots = new List<Slot>(capacity);
        }

        #region Implementation of IItemStore

        public T Fetch()
        {
            if (Count == 0)
                throw new InvalidOperationException("The buffer is empty.");

            int startPosition = position;
            do
            {
                Advance();
                Slot slot = slots[position];
                if (!slot.IsInUse)
                {
                    slot.IsInUse = true;
                    --freeSlotCount;
                    return slot.Item;
                }
            } while (startPosition != position);
            throw new InvalidOperationException("No free slots.");
        }

        public void Store(T item)
        {
            Slot slot = slots.Find(s => Equals(s.Item, item));
            if (slot == null)
            {
                slot = new Slot(item);
                slots.Add(slot);
            }
            slot.IsInUse = false;
            ++freeSlotCount;
        }

        public int Count
        {
            get { return freeSlotCount; }
        }

        #endregion

        private void Advance()
        {
            position = (position + 1) % slots.Count;
        }


        /// <summary>
        /// Circular store slot.
        /// </summary>
        private class Slot
        {
            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="item">item to store in slot</param>
            public Slot(T item)
            {
                this.Item = item;
            }

            /// <summary>
            /// Slot item.
            /// </summary>
            public T Item { get; private set; }

            /// <summary>
            /// Returns <c>true</c> if slot is used.
            /// </summary>
            public bool IsInUse { get; set; }
        }
    }
}
