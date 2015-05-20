using Moq;
using MvcMocker.MockBuilders;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcMocker
{
    public class MockController<T> where T : ControllerBase
    {
        private readonly Object[] parametersToNewInstance;
        private readonly IMockDependencyResolver resolver;

        private Func<Object[], T> newInstance;

        public MockController() : this(new HttpMockResolver(), new Object[0]) {}

        public MockController(params Object[] parameters) : this(new HttpMockResolver(), parameters) {}

        public MockController(IMockDependencyResolver mockResolver, params Object[] parameters)
        {
            if (mockResolver == null)
                throw new ArgumentNullException("mockResolver");

            if (parameters == null)
                throw new ArgumentNullException("parameters");

            this.parametersToNewInstance = parameters;
            this.resolver = mockResolver;
            this.newInstance = CreateNewInstanceWithParameters;
        }

        public HttpServerUtilityMock Server
        {
            get { return resolver.GetService<HttpServerUtilityMock>(); }
        }

        public HttpResponseMock Response
        {
            get { return resolver.GetService<HttpResponseMock>(); }
        }

        public HttpRequestMock Request
        {
            get { return resolver.GetService<HttpRequestMock>(); }
        }

        public HttpSessionStateMock Session
        {
            get { return resolver.GetService<HttpSessionStateMock>(); }
        }

        public MockController<T> SetInstanceFactory(Func<Object[], T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            this.newInstance = factory;
            return this;
        }

        public T Create()
        {
            var mockHttpContext = new Mock<HttpContextBase>();

            resolver.LoadAllOnce();
            var mocks = resolver.GetAllMocks();

            foreach (var mock in mocks)
            {
                mock.BuildIn(mockHttpContext);
            }

            var result = newInstance(parametersToNewInstance);

            result.ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);

            return result;
        }

        private static T CreateNewInstanceWithParameters(params Object[] parameters)
        {
            var typesFromArgs = parameters.Select(a => a.GetType()).ToArray();
            var constructor = typeof(T).GetConstructor(typesFromArgs);
            return (T)constructor.Invoke(parameters);
        }
    }
}