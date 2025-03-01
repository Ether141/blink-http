using System.Reflection;

namespace BlinkHttp.Handling.Routing
{
    internal class ControllersManager
    {
        private readonly IReadOnlyList<MethodInfo> allGetMethods;

        public ControllersManager()
        {
            allGetMethods = GetHttpMethodsFromControllers<HttpGetAttribute>();
        }

        internal void CheckIfControllerIsAvailable(string routing)
        {
            
        }

        private static List<MethodInfo> GetHttpMethodsFromControllers<T>() where T : Attribute
        {
            var methodsWithAttribute = new List<MethodInfo>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                IEnumerable<Type> controllerTypes = assembly.GetTypes()
                                              .Where(t => t.IsSubclassOf(typeof(Controller)) && !t.IsAbstract);

                foreach (Type controllerType in controllerTypes)
                {
                    var methods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance);

                    foreach (var method in methods)
                    {
                        if (method.GetCustomAttribute<T>() != null)
                        {
                            methodsWithAttribute.Add(method);
                        }
                    }
                }
            }

            return methodsWithAttribute;
        }
    }
}
