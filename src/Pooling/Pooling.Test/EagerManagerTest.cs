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
    /// Test cases for <see cref="EagerManager{T}"/>.
    /// </summary>
    [TestFixture]
    class EagerManagerTest
    {
        private IItemStore<ObjectStub> itemStoreMock;
        private IPoolItemManager<ObjectStub> manager;

        [SetUp]
        public void SetUp()
        {
            itemStoreMock = MockRepository.GenerateMock<IItemStore<ObjectStub>>();
            manager = new EagerManager<ObjectStub>(itemStoreMock, () => new ObjectStub(), 1);
        }

        [TearDown]
        public void TearDown()
        {
            itemStoreMock.VerifyAllExpectations();
        }

        [Test]
        public void TestConstructor()
        {
            itemStoreMock.Expect(m => m.Store(Arg<ObjectStub>.Is.Anything));
            new EagerManager<ObjectStub>(itemStoreMock, () => new ObjectStub(), 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFailStore()
        {
            new EagerManager<ObjectStub>(null, () => new ObjectStub(), 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFailFactory()
        {
            new EagerManager<ObjectStub>(itemStoreMock, null, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConstructorFailCount()
        {
            new EagerManager<ObjectStub>(itemStoreMock, () => new ObjectStub(), 0);
        }

        [Test]
        public void TestGetItem()
        {
            var o = new ObjectStub();

            itemStoreMock.Expect(m => m.Fetch()).Return(o);
            var item = manager.GetItem();
            Assert.AreEqual(o, item);
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
