using BlinkHttp.Http;
using System.Reflection;

namespace BlinkHttp.Routing;

internal static class RoutingReflectionUtility
{
    internal static List<MethodInfo> GetAllEndpointMethods(Type controllerType)
    {
        Type iResultType = typeof(IHttpResult);
        Type iAsyncResultType = typeof(Task<IHttpResult>);
        Type httpAttributeType = typeof(HttpAttribute);

        var methods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(method => (iResultType.IsAssignableFrom(method.ReturnType) || iAsyncResultType.IsAssignableFrom(method.ReturnType)) && method.GetCustomAttribute(httpAttributeType) != null)
            .ToList();

        return methods;
    }

    internal static List<Type> GetAllControllers()
    {
        Type controllerType = typeof(Controller);
        List<Type> result = [];

        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (Assembly assembly in assemblies)
        {
            List<Type> controllerTypes = [.. assembly.GetTypes().Where(type => type != controllerType && controllerType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)];
            result.AddRange(controllerTypes);
        }

        return result;
    }
}
