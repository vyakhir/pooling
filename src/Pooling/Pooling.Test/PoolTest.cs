/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using System;
using NUnit.Framework;
using Pooling.Loading;
using Pooling.Storage;
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

        private IPoolItemLoader<IDisposable> loaderMock;
        private IItemStore<IDisposable> itemStoreMock;
        private IPool<IDisposable> pool;
            
        [SetUp]
        public void SetUp()
        {
            loaderMock = MockRepository.GenerateMock<IPoolItemLoader<IDisposable>>();
            itemStoreMock = MockRepository.GenerateMock<IItemStore<IDisposable>>();
            pool = new Pool<IDisposable>(PoolSize, p => new ObjectStub(), loaderMock, itemStoreMock);
        }

        [TearDown]
        public void TearDown()
        {
            loaderMock.VerifyAllExpectations();
            itemStoreMock.VerifyAllExpectations();
        }

        [Test]
        public void TestSize()
        {
            Assert.AreEqual(PoolSize, pool.Size);
        }

        [Test]
        public void TestIsDisposed()
        {
            Assert.IsFalse(pool.IsDisposed);
        }

        [Test]
        public void TestDispose()
        {
            var o = MockRepository.GenerateMock<IDisposable>();
            o.Expect(m => m.Dispose());

            itemStoreMock.Expect(m => m.Count).Return(1).Repeat.Once();
            itemStoreMock.Expect(m => m.Fetch()).Return(o);
            itemStoreMock.Expect(m => m.Count).Return(0);

            pool.Dispose();
            Assert.IsTrue(pool.IsDisposed);
            o.VerifyAllExpectations();
        }

        [Test]
        public void TestAcquire()
        {
            var o = new ObjectStub();

            loaderMock.Expect(m => m.LoadItem()).Return(o);
            Assert.AreEqual(o, pool.Acquire());
        }

        [Test]
        public void TestRelease()
        {
            var o = new ObjectStub();

            pool.Acquire();

            itemStoreMock.Expect(m => m.Store(o));
            pool.Release(o);
        }
    }

    public class ObjectStub : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
