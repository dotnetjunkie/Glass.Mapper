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

        static List<Tuple<Type, Func<object>>>  _constructors = new List<Tuple<Type, Func<object>>>();
 
        public T Resolve<T>() where T: class
        {
            var type = typeof (T);
            var constructor = _constructors
                .FirstOrDefault(x => x.Item1 == type);

            return constructor ==null ? null : constructor.Item2() as T;

        }

        public IEnumerable<T> ResolveAll<T>() where T: class
        {
            var type = typeof (T);
            return _constructors
                .Where(x => x.Item1 == type)
                .Select(x => x.Item2())
                .Cast<T>();
        }

        public void RegisterTransient<T, TComponent>() where T : class
        {
            var type = typeof (T);

            var invoker = Utilities.CreateConstructorDelegates(type).First().Value;
         
            _constructors.Add(
                new Tuple<Type, Func<object>>(typeof (T), () => invoker.DynamicInvoke())
                );
        }

        public void RegisterInstance<T>(T instance) where T : class
        {
            _constructors.Add(
                new Tuple<Type, Func<object>>(typeof (T), () => instance)
                );
        }
    }
}
