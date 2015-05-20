using Moq;
using System;
using System.Web;

namespace MvcMocker.MockBuilders
{
    public class HttpServerUtilityMock : IMockBuilder
    {
        private readonly Mock<HttpServerUtilityBase> mock;

        public HttpServerUtilityMock()
        {
            mock = new Mock<HttpServerUtilityBase>();
        }

        public HttpServerUtilityMock SetMapPath(String path)
        {
            mock.Setup(m => m.MapPath(It.IsAny<String>()))
                .Returns(path);

            return this;
        }

        void IMockBuilder.BuildIn(Mock<HttpContextBase> contextMock)
        {
            contextMock
                .SetupGet(c => c.Server)
                .Returns(mock.Object);
        }
    }
}