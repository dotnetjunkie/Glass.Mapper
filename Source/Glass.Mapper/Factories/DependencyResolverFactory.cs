using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Glass.Mapper.Factories
{
    public class DependencyResolverFactory : IDependencyRegistrar, IDependencyResolver
    {

        static List<Tuple<Type, Func<IDependencyResolver, object>>> _constructors = new List<Tuple<Type, Func<IDependencyResolver, object>>>();

        public T Resolve<T>() where T : class
        {
            var type = typeof(T);
            var constructor = _constructors
                .FirstOrDefault(x => x.Item1 == type);

            return constructor == null ? null : constructor.Item2(this) as T;

        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            var type = typeof(T);
            return _constructors
                .Where(x => x.Item1 == type)
                .Select(x => x.Item2(this))
                .Cast<T>();
        }

        public void RegisterTransient<T>(Func<IDependencyResolver, T> builder) where T : class
        {
            _constructors.Add(
                 new Tuple<Type, Func<IDependencyResolver, object>>(typeof(T), builder)
                 );
        }

        public void RegisterInstance<T>(Func<IDependencyResolver, T> builder) where T : class
        {
            _constructors.Add(
                 new Tuple<Type, Func<IDependencyResolver, object>>(typeof(T), builder)
                 );
        }
    }
}
