using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcMocker.MockBuilders;

namespace MvcMocker.Test.MockBuilders
{
    [TestClass]
    public class HttpMockResolverTest
    {
        [TestMethod]
        public void Given_resolver_than_load_all_when_get_all_mocks_result_should_be_4_instances()
        {
            var resolver = new HttpMockResolver();
            resolver.LoadAllOnce();

            var mocks = resolver.GetAllMocks();

            mocks.Should().HaveCount(4);
        }
    }
}