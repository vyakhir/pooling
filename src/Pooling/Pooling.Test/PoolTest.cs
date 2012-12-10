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
    /// Test cases for <see cref="Pool{T}"/>.
    /// </summary>
    [TestFixture]
    class PoolTest
    {
        private const int PoolSize = 5;

        private IPoolItemManager<ObjectStub> managerMock;
        private IPool<ObjectStub> pool;
            
        [SetUp]
        public void SetUp()
        {
            managerMock = MockRepository.GenerateMock<IPoolItemManager<ObjectStub>>();
            pool = new Pool<ObjectStub>(managerMock, PoolSize);
        }

        [TearDown]
        public void TearDown()
        {
            managerMock.VerifyAllExpectations();
        }

        [Test]
        public void TestCapacity()
        {
            Assert.AreEqual(PoolSize, pool.Capacity);
        }

        [Test]
        public void TestIsDisposed()
        {
            Assert.IsFalse(pool.IsDisposed);
        }

        [Test]
        public void TestDispose()
        {
            managerMock.Expect(m => m.Dispose());
            pool.Dispose();
        }

        [Test]
        public void TestAcquire()
        {
            var o = new ObjectStub();

            managerMock.Expect(m => m.GetItem()).Return(o);
            Assert.AreEqual(o, pool.Acquire());
            Assert.AreEqual(PoolSize - 1, pool.Count);
        }

        [Test]
        public void TestRelease()
        {
            var o = new ObjectStub();

            pool.Acquire();

            managerMock.Expect(m => m.PutItem(o));
            Assert.AreEqual(PoolSize, pool.Release(o));
            Assert.AreEqual(PoolSize, pool.Count);
        }
    }
}
