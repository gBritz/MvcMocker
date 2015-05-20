using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvcMocker.MockBuilders;
using System;
using System.Collections.Generic;
using System.Web;

namespace MvcMocker.Test.MockBuilders
{
    [TestClass]
    public class HttpRequestMockTest
    {
        [TestMethod]
        public void HttpRequestMock_should_implement_IMockBuilder()
        {
            typeof(HttpRequestMock).Should()
                .BeAssignableTo<IMockBuilder>();
        }

        [TestMethod]
        public void Given_request_is_ajax_when_build_in_context_header_should_contains_XMLHttpRequest()
        {
            var controller = WhenBuildIn(mock => mock.IsAjax());

            controller.Request
                .Headers["X-Requested-With"]
                .Should().Be("XMLHttpRequest");
        }

        [TestMethod]
        public void When_add_accept_language_enUS_header_should_contains_accept_language_enUS()
        {
            var controller = WhenBuildIn(mock => mock.AddHeader("Accept-Language", "en-US"));

            controller.Request
                .Headers["Accept-Language"]
                .Should().Be("en-US");
        }

        [TestMethod]
        public void When_set_headers_should_have_all_values()
        {
            var headers = new Dictionary<String, String>
            {
                { "Authorization", "Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==" },
                { "Cookie", "asdfasd asas  asd " },
                { "User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.152 Safari/537.36" }
            };

            var controller = WhenBuildIn(mock => mock.SetHeaders(headers));

            var values = new String[headers.Count];
            controller.Request.Headers.CopyTo(values, 0);

            values.Should().ContainInOrder(headers.Values);
        }

        [TestMethod]
        public void When_set_headers_with_null_method_should_throw_argument_null_exception()
        {
            var mock = new HttpRequestMock();

            Action methodWithNull = () => mock.SetHeaders(null);

            methodWithNull.ShouldThrow<ArgumentNullException>()
                .And.ParamName.Should().Be("values");
        }

        [TestMethod]
        public void When_add_header_with_key_null_or_empty_method_should_throw_argument_null_exception()
        {
            var mock = new HttpRequestMock();
            
            Action methodWithNull = () => mock.AddHeader(null, " ");
            Action methodWithEmpty = () => mock.AddHeader(null, " ");

            methodWithNull.ShouldThrow<ArgumentNullException>()
                .And.ParamName.Should().Be("name");
            methodWithEmpty.ShouldThrow<ArgumentNullException>()
                .And.ParamName.Should().Be("name");
        }

        private static HttpContextBase WhenBuildIn(Action<HttpRequestMock> visit)
        {
            var contextMock = new Mock<HttpContextBase>();

            var mock = new HttpRequestMock();
            visit(mock);
            ((IMockBuilder)mock).BuildIn(contextMock);

            return contextMock.Object;
        }
    }
}