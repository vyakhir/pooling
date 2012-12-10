/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using System;
using NUnit.Framework;
using Pooling.Management;
using Pooling.Storage;
using Pooling.Test.Stubs;
using Rhino.Mocks;

namespace Pooling.Test
{
    /// <summary>
    /// Test cases for <see cref="LazyManager{T}"/>.
    /// </summary>
    [TestFixture]
    class LazyManagerTest
    {
        private IItemStore<ObjectStub> itemStoreMock;
        private IPoolItemManager<ObjectStub> manager;

        [SetUp]
        public void SetUp()
        {
            itemStoreMock = MockRepository.GenerateMock<IItemStore<ObjectStub>>();
            manager = new LazyManager<ObjectStub>(itemStoreMock, () => new ObjectStub());
        }

        [TearDown]
        public void TearDown()
        {
            itemStoreMock.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFailStore()
        {
            new LazyManager<ObjectStub>(null, () => new ObjectStub());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFailFactory()
        {
            new LazyManager<ObjectStub>(itemStoreMock, null);
        }

        [Test]
        public void TestGetItem()
        {
            var o = new ObjectStub();
            itemStoreMock.Expect(m => m.Count).Return(1);
            itemStoreMock.Expect(m => m.Fetch()).Return(o);
            var item = manager.GetItem();
            Assert.AreEqual(o, item);
        }

        [Test]
        public void TestLoadItemViaFactory()
        {
            itemStoreMock.Expect(m => m.Count).Return(0);
            var item = manager.GetItem();
            Assert.NotNull(item);
        }

        [Test]
        public void TestPutItem()
        {
            var o = new ObjectStub();
            itemStoreMock.Expect(m => m.Store(o));
            manager.PutItem(o);
        }

        [Test]
        public void TestDispose()
        {
            var o = MockRepository.GenerateMock<ObjectStub>();
            o.Expect(m => m.Dispose());

            itemStoreMock.Expect(m => m.Count).Return(1).Repeat.Once();
            itemStoreMock.Expect(m => m.Fetch()).Return(o);
            itemStoreMock.Expect(m => m.Count).Return(0);

            manager.Dispose();
            o.VerifyAllExpectations();
        }
    }
}
