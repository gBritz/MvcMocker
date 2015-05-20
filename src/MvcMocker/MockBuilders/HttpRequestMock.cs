using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace MvcMocker.MockBuilders
{
    public class HttpRequestMock : IMockBuilder
    {
        private String httpMethodType = "Get";
        private readonly NameValueCollection headers;

        public HttpRequestMock()
        {
            headers = new NameValueCollection();
        }

        /// <summary>
        /// Using System.Net.WebRequestMethods.Http.<methods> in 
        /// </summary>
        /// <param name="methodType"></param>
        /// <returns></returns>
        public HttpRequestMock Method(String methodType)
        {
            if (String.IsNullOrEmpty(methodType))
                throw new ArgumentNullException("methodType");

            httpMethodType = methodType;
            return this;
        }

        public HttpRequestMock IsAjax()
        {
            headers.Add("X-Requested-With", "XMLHttpRequest");
            return this;
        }

        public HttpRequestMock AddHeader(String name, String value)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            headers.Add(name, value);
            return this;
        }

        public HttpRequestMock SetHeaders(IDictionary<String, String> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            foreach (var item in values)
                headers.Add(item.Key, item.Value);

            return this;
        }

        void IMockBuilder.BuildIn(Mock<HttpContextBase> contextMock)
        {
            var mock = new Mock<HttpRequestBase>();

            mock.Setup(r => r.HttpMethod).Returns(httpMethodType);
            mock.Setup(r => r.Headers).Returns(headers);
            mock.Setup(r => r.Form).Returns(new NameValueCollection());
            mock.Setup(r => r.QueryString).Returns(new NameValueCollection());

            contextMock.SetupGet(c => c.Request)
                .Returns(mock.Object);
        }
    }
}