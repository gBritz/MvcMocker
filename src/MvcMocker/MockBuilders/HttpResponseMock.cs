using Moq;
using System;
using System.IO;
using System.Web;

namespace MvcMocker.MockBuilders
{
    public class HttpResponseMock : IMockBuilder
    {
        private TextWriter writer = Console.Out;

        public HttpResponseMock SetWriter(TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            this.writer = writer;
            return this;
        }

        void IMockBuilder.BuildIn(Mock<HttpContextBase> contextMock)
        {
            var httpResponse = new HttpResponse(writer);
            var httpResponseBase = new HttpResponseWrapper(httpResponse);

            contextMock.SetupGet(c => c.Response)
                .Returns(httpResponseBase);
        }
    }
}