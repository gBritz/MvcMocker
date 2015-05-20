using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MvcMocker.Test
{
    [TestClass]
    public class MockControllerTest
    {
        [TestMethod]
        public void Given_username_roberto_when_mock_session_username_should_be_roberto()
        {
            var mock = new MockController<HomeController>();
            mock.Session.Add("userName", "roberto");
            var home = mock.Create();

            home.GetUserName().Should().Be("roberto");
        }

        [TestMethod]
        public void Given_user_and_countSellingAttempt_when_mock_session_countSellingAttempt_should_be_5()
        {
            var mock = new MockController<HomeController>();
            mock.Session.Add(new Dictionary<String, Object> 
            {
                { "user", new UserModel { Id = 1, Name = "Gabriela", Age = 26, Email = "gabizinha26@gmail.com" } },
                { "countSellingAttempt", 5 }
            });
            var home = mock.Create();

            home.CountSellingAttempt().Should().Be(5);
        }

        [TestMethod]
        public void Given_mocked_controller_session_should_not_be_null()
        {
            var controller = new MockController<HomeController>()
                .Create();

            controller.Session.Should().NotBeNull();
        }

        [TestMethod]
        public void Given_mocked_controller_request_should_not_be_null()
        {
            var controller = new MockController<HomeController>()
                .Create();

            controller.Request.Should().NotBeNull();
        }

        [TestMethod]
        public void Given_mocked_controller_response_should_not_be_null()
        {
            var controller = new MockController<HomeController>()
                .Create();

            controller.Response.Should().NotBeNull();
        }

        [TestMethod]
        public void Given_mocked_controller_server_should_not_be_null()
        {
            var controller = new MockController<HomeController>()
                .Create();

            controller.Server.Should().NotBeNull();
        }
    }
}