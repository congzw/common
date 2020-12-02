using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common
{
    [TestClass]
    public class ServiceLocatorSpec
    {
        [TestMethod]
        public void Current_NotInit_Should_ReturnDefault()
        {
            var theLocator = ServiceLocator.Current;
            theLocator.ShouldNotNull();
            theLocator.GetType().ShouldEqual(typeof(NullServiceLocator));
            theLocator.GetServices<object>().ShouldEmpty();
            theLocator.GetService<object>().ShouldNull();
        }

        [TestMethod]
        public void Current_Replace_Should_ReturnMock()
        {
            var serviceLocator = new ServiceLocator();
            var rootServiceProvider = new MockRootServiceProvider(true);
            serviceLocator.Initialize(rootServiceProvider);

            var theLocator = serviceLocator.Resolve();
            theLocator.ShouldNotNull();
            theLocator.GetType().ShouldEqual(typeof(MockServiceLocator));
        }

        [TestMethod]
        public void Current_NotReplace_Should_ReturnDefault()
        {
            var serviceLocator = new ServiceLocator();
            var rootServiceProvider = new MockRootServiceProvider(false);
            serviceLocator.Initialize(rootServiceProvider);

            var theLocator = serviceLocator.Resolve();
            theLocator.ShouldNotNull();
            theLocator.GetType().ShouldEqual(typeof(NullServiceLocator));
        }
    }

    public class MockRootServiceProvider : IServiceProvider 
    {
        public bool Replaced { get; }

        public MockRootServiceProvider(bool replaced)
        {
            Replaced = replaced;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType.IsAssignableFrom(typeof(IServiceProvider)))
            {
                return this;
            }

            if (serviceType.IsAssignableFrom(typeof(IServiceLocator)))
            {
                return Replaced ? new MockServiceLocator() : null;
            }

            return null;
        }
    }

    public class MockServiceLocator : IServiceLocator
    {
        public bool Invoked { get; set; }

        public T GetService<T>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetServices<T>()
        {
            throw new NotImplementedException();
        }

        public object GetService(Type type)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetServices(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
