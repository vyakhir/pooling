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
    /// Test cases for <see cref="EagerLoader{T}"/>.
    /// </summary>
    [TestFixture]
    class EagerLoaderTest
    {
        private IItemStore<object> itemStoreMock;
        private IPoolItemLoader<object> loader;

        [SetUp]
        public void SetUp()
        {
            itemStoreMock = MockRepository.GenerateMock<IItemStore<object>>();
            loader = new EagerLoader<object> {Factory = () => new object(), ItemStore = itemStoreMock};
        }

        [TearDown]
        public void TearDown()
        {
            itemStoreMock.VerifyAllExpectations();
        }

        [Test]
        public void TestIsPreloadSupported()
        {
            Assert.IsTrue(loader.IsPreloadSupported);
        }

        [Test]
        public void TestPreload()
        {
            const int preloadAmount = 2;

            itemStoreMock.Expect(m => m.Store(Arg<object>.Is.Anything)).Repeat.Times(preloadAmount);
            loader.Preload(preloadAmount);
        }

        [Test]
        public void TestLoadItem()
        {
            var o = new object();
            
            itemStoreMock.Expect(m => m.Fetch()).Return(o);
            var item = loader.LoadItem();
            Assert.AreEqual(o, item);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestPreloadFailStore()
        {
            new EagerLoader<object>() {Factory = () => new object()}.Preload(1);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestPreloadFailFactory()
        {
            new EagerLoader<object>() { ItemStore = itemStoreMock }.Preload(1);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestLoadItemFail()
        {
            new EagerLoader<object>() { Factory = () => new object() }.Preload(1);
        }
    }
}
