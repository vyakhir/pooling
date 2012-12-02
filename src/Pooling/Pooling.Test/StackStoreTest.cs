/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using System.Collections.Generic;
using NUnit.Framework;
using Pooling.Storage;

namespace Pooling.Test
{
    /// <summary>
    /// Test cases for <see cref="StackStore{T}"/>.
    /// </summary>
    [TestFixture]
    class StackStoreTest
    {
        private IItemStore<object> itemStore;

        [SetUp]
        public void SetUp()
        {
            itemStore = new StackStore<object>();
        }

        [Test]
        public void TestStore()
        {
            var o1 = new object();
            var o2 = new object();
            itemStore.Store(o1);
            itemStore.Store(o2);

            Assert.AreEqual(o2, (itemStore as Stack<object>).Pop());
            Assert.AreEqual(o1, (itemStore as Stack<object>).Pop());
        }

        [Test]
        public void TestFetch()
        {
            var o1 = new object();
            var o2 = new object();
            itemStore.Store(o1);
            itemStore.Store(o2);

            Assert.AreEqual(o2, itemStore.Fetch());
            Assert.AreEqual(o1, itemStore.Fetch());
        }
    }
}
