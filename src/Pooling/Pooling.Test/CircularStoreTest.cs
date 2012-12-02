/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using System.Collections;
using System.Reflection;
using NUnit.Framework;
using Pooling.Storage;

namespace Pooling.Test
{
    /// <summary>
    /// Test cases for <see cref="CircularStore{T}"/>.
    /// </summary>
    [TestFixture]
    class CircularStoreTest
    {
        private IItemStore<object> itemStore;

        [SetUp]
        public void SetUp()
        {
            itemStore = new CircularStore<object>();
        }

        [Test]
        public void TestStore()
        {
            var o1 = new object();
            var o2 = new object();
            itemStore.Store(o1);
            itemStore.Store(o2);

            var slots = typeof (CircularStore<object>).InvokeMember(
                "slots",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField,
                null,
                itemStore,
                null
            ) as IList;
            Assert.AreEqual(2, slots.Count);
        }

        [Test]
        public void TestFetch()
        {
            var o1 = new object();
            var o2 = new object();
            itemStore.Store(o1);
            itemStore.Store(o2);

            Assert.AreEqual(o1, itemStore.Fetch());
            Assert.AreEqual(o2, itemStore.Fetch());
        }
    }
}
