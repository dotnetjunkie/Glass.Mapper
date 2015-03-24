using System;

namespace Glass.Mapper
{
    public interface IDependencyRegistrar
    {
        void RegisterTransient<T>(Func<IDependencyResolver, T> builder) where T : class;

        void RegisterInstance<T>(Func<IDependencyResolver, T> builder) where T : class;
    }
}
