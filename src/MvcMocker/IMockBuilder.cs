using Moq;
using System.Web;

namespace MvcMocker
{
    public interface IMockBuilder
    {
        void BuildIn(Mock<HttpContextBase> contextMock);
    }
}