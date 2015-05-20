using System;

namespace MvcMocker
{
    public interface IMockDependencyResolver
    {
        void LoadAllOnce();

        T GetService<T>() where T : class, IMockBuilder, new();

        IMockBuilder[] GetAllMocks();
    }
}