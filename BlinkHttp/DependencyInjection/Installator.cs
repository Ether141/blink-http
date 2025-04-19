using BlinkDatabase.General;
using BlinkHttp.Handling;
using System.Reflection;

namespace BlinkHttp.DependencyInjection;

internal class Installator
{
    internal Dictionary<Type, Type> Singletons { get; } = [];
    internal Dictionary<Type, object> SingletonInstances { get; } = [];

    internal Dictionary<Type, Type> Scopeds { get; } = [];
    internal List<Type> Repositories { get; } = [];

    internal List<Type> Middlewares { get; } = [];
    internal List<IMiddleware> MiddlewareInstances { get; } = [];

    internal T InstantiateClass<T>() where T : class => (T)InstantiateClass(typeof(T));

    internal T GetSingletonByService<T>() => (T)GetSingleton(Singletons[typeof(T)]);

    internal object InstantiateClass(Type type)
    {
        ConstructorInfo[] constructors = type.GetConstructors();

        if (constructors.Length == 0)
        {
            return Activator.CreateInstance(type)!;
        }

        ConstructorInfo? candidate = null;
        int matchedParamsNum = 0;
        ParameterInfo[] parameters = [];

        foreach (ConstructorInfo constructor in constructors)
        {
            parameters = constructor.GetParameters();
            int thisMatchedParamsNum = parameters.Where(parameter => Singletons.Any(s => s.Key == parameter.ParameterType) || Scopeds.Any(s => s.Key == parameter.ParameterType) || Repositories.Any(r => AreRepositoriesCompatible(parameter.ParameterType, r))).Count();

            if (thisMatchedParamsNum > matchedParamsNum)
            {
                matchedParamsNum = thisMatchedParamsNum;
                candidate = constructor;
            }
        }

        if (candidate == null)
        {
            return Activator.CreateInstance(type, null)!;
        }

        object?[] args = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            ParameterInfo parameter = parameters[i];
            Type? implementationType = Singletons.FirstOrDefault(s => s.Key == parameter.ParameterType).Value;

            if (implementationType != null && implementationType == type)
            {
                continue;
            }

            if (implementationType == null)
            {
                implementationType = Scopeds.FirstOrDefault(s => s.Key == parameter.ParameterType).Value;

                if (implementationType != null)
                {
                    args[i] = GetScoped(implementationType);
                }
                else
                {
                    implementationType = Repositories.FirstOrDefault(r => AreRepositoriesCompatible(parameter.ParameterType, r));
                    args[i] = implementationType != null ? GetRepository(implementationType) : null;
                }

                continue;
            }

            object arg = GetSingleton(implementationType);
            args[i] = arg;
        }

        return Activator.CreateInstance(type, args)!;
    }

    internal MiddlewareHandler ResolveMiddlewareHandler()
    {
        IEnumerable<IMiddleware> middlewares = Middlewares.Select(m => MiddlewareInstances.FirstOrDefault(i => i.GetType() == m) ?? InstantiateClass(m)).Cast<IMiddleware>();
        return new MiddlewareHandler(middlewares);
    }

    private bool AreRepositoriesCompatible(Type interfaceType, Type implementationType)
    {
        if (!interfaceType.IsGenericType || interfaceType.GetGenericTypeDefinition() != typeof(IRepository<>))
        {
            return false;
        }

        Type interfaceGenericType = interfaceType.GetGenericArguments().First();
        IEnumerable<Type> implementedInterfaces = implementationType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>));

        return implementedInterfaces.Any(i => i.GetGenericArguments().First() == interfaceGenericType);
    }

    private object GetSingleton(Type implementationType)
    {
        if (SingletonInstances.TryGetValue(implementationType, out object? instance))
        {
            return instance;
        }

        instance = InstantiateClass(implementationType);
        SingletonInstances[implementationType] = instance;
        return instance;
    }

    private object GetScoped(Type implementationType) => InstantiateClass(implementationType);

    private object GetRepository(Type implementationType) => Activator.CreateInstance(implementationType, GetSingleton(Singletons[typeof(IDatabaseConnection)]))!;
}
