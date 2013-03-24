using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using NUnit.Framework;

namespace SearcherTests.Common
{
    [TestFixture]
    public class AppContextTest : IDisposable
    {
        private bool _disposedCalled;

        [SetUp]
        public void Setup()
        {
            _disposedCalled = false;
        }

        [Test]
        public void RegisterTest()
        {
            AppContext.RegisterService(this, typeof(AppContextTest));
        }

        [Test]
        public void RegisterTwiceSameServiceTest()
        {
            AppContext.RegisterService(this, typeof(AppContextTest));
            AppContext.RegisterService(this, typeof(AppContextTest));    
            Assert.IsFalse(_disposedCalled);
        }

        [Test]
        public void RegisterOtherServiceTest()
        {
            AppContext.RegisterService(this, typeof(AppContextTest));
            AppContext.RegisterService(new AppContextTest(), typeof(AppContextTest));
            Assert.IsTrue(_disposedCalled);
        }

        public void Dispose()
        {
            _disposedCalled = true;
        }
    }
}
