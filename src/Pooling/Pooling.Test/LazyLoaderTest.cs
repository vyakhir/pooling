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
    /// Test cases for <see cref="LazyLoader{T}"/>.
    /// </summary>
    [TestFixture]
    class LazyLoaderTest
    {
        private IItemStore<object> itemStoreMock;
        private IPoolItemLoader<object> loader;

        [SetUp]
        public void SetUp()
        {
            itemStoreMock = MockRepository.GenerateMock<IItemStore<object>>();
            loader = new LazyLoader<object> { Factory = () => new object(), ItemStore = itemStoreMock };
        }

        [TearDown]
        public void TearDown()
        {
            itemStoreMock.VerifyAllExpectations();
        }

        [Test]
        public void TestIsPreloadSupported()
        {
            Assert.IsFalse(loader.IsPreloadSupported);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestPreload()
        {
            loader.Preload(1);
        }

        [Test]
        public void TestLoadItem()
        {
            var o = new object();
            itemStoreMock.Expect(m => m.Count).Return(1);
            itemStoreMock.Expect(m => m.Fetch()).Return(o);
            var item = loader.LoadItem();
            Assert.AreEqual(o, item);
        }

        [Test]
        public void TestLoadItemViaFactory()
        {
            itemStoreMock.Expect(m => m.Count).Return(0);
            var item = loader.LoadItem();
            Assert.NotNull(item);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestLoadItemFailStore()
        {
            new LazyLoader<object>() { Factory = () => new object() }.LoadItem();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestLoadItemFailFactory()
        {
            new LazyLoader<object>() { ItemStore = itemStoreMock }.LoadItem();
        }
    }
}
