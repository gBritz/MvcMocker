using System;
using System.Linq;
using System.Collections.Generic;

namespace MvcMocker.MockBuilders
{
    public class HttpMockResolver : IMockDependencyResolver
    {
        private readonly IDictionary<Type, IMockBuilder> mocks;

        public HttpMockResolver()
        {
            mocks = new Dictionary<Type, IMockBuilder>();
        }

        public void LoadAllOnce()
        {
            AddOnce<HttpRequestMock>();
            AddOnce<HttpResponseMock>();
            AddOnce<HttpServerUtilityMock>();
            AddOnce<HttpSessionStateMock>();
        }

        public IMockBuilder[] GetAllMocks()
        {
            return mocks.Values.ToArray();
        }

        protected T GetLazyService<T>() where T : class, IMockBuilder, new()
        {
            T instance = null;
            var type = typeof(T);

            if (mocks.ContainsKey(type))
            {
                instance = (T)mocks[type];
            }
            else
            {
                instance = new T();
                mocks.Add(type, instance);
            }

            return instance;
        }

        private void AddOnce<T>() where T : class, IMockBuilder, new()
        {
            var type = typeof(T);
            if (!mocks.ContainsKey(type))
                mocks.Add(type, new T());
        }

        T IMockDependencyResolver.GetService<T>()
        {
            return GetLazyService<T>();
        }
    }
}