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
            var serviceLocator = ServiceLocator.Current;
            serviceLocator.ShouldNotNull();
            serviceLocator.GetType().ShouldEqual(typeof(NullServiceLocator));
            serviceLocator.GetServices<object>().ShouldEmpty();
            serviceLocator.GetService<object>().ShouldNull();
        }

        [TestMethod]
        public void Current_Init_Should_ReturnMock()
        {
            ServiceLocator.Initialize(Create());

            var serviceLocator = ServiceLocator.Current;
            serviceLocator.ShouldNotNull();
            serviceLocator.GetType().ShouldEqual(typeof(MockServiceLocator));

            ServiceLocator.Reset();
        }

        [TestMethod]
        public void Current_Reset_Should_ReturnReturnDefault()
        {
            ServiceLocator.Reset();

            var serviceLocator = ServiceLocator.Current;
            serviceLocator.ShouldNotNull();
            serviceLocator.GetType().ShouldEqual(typeof(NullServiceLocator));
            serviceLocator.GetServices<object>().ShouldEmpty();
            serviceLocator.GetService<object>().ShouldNull();
        }

        private IServiceLocator Create()
        {
            var mockServiceLocator = new MockServiceLocator();
            return mockServiceLocator;
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
