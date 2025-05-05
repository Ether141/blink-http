using BlinkDatabase.General;
using BlinkHttp.Application;
using BlinkHttp.Background;
using BlinkHttp.Configuration;
using BlinkHttp.Handling;

namespace BlinkHttp.DependencyInjection;

/// <summary>
/// Manages and stores services for dependency injections.
/// </summary>
public class ServicesContainer
{
    internal Installator Installator { get; } = new Installator();

    internal ServicesContainer() { }

    /// <summary>
    /// Gets the configuration instance used throughout the application runtime, if it was configured using <seealso cref="AddConfiguration{TConfigurationImplementation}(TConfigurationImplementation)"/>. Otherwise returns null.
    /// </summary>
    public IConfiguration? Configuration { get; private set; }

    /// <summary>
    /// Adds new singleton definition to the services that will be later used to resolve dependencies. A singleton will be created the first time a reference to it is requested, and the same instance will be used throughout the runtime of the application.
    /// </summary>
    public ServicesContainer AddSingleton<TService, TImplementation>() where TService : class where TImplementation : class, TService
    {
        if (Installator.Singletons.ContainsKey(typeof(TService)))
        {
            return this;
        }

        Installator.Singletons[typeof(TService)] = typeof(TImplementation);
        return this;
    }

    /// <summary>
    /// Adds new singleton definition to the services that will be later used to resolve dependencies. Given already created instance will be used throughout the runtime of the application.
    /// </summary>
    public ServicesContainer AddSingleton<TService, TImplementation>(TImplementation implementation) where TService : class where TImplementation : class, TService
    {
        if (Installator.Singletons.ContainsKey(typeof(TService)))
        {
            return this;
        }

        Installator.Singletons[typeof(TService)] = typeof(TImplementation);
        Installator.SingletonInstances[typeof(TImplementation)] = implementation;
        return this;
    }

    /// <summary>
    /// Adds new singleton definition to the services that will be later used to resolve dependencies. A singleton will be created the first time a reference to it is requested, and the same instance will be used throughout the runtime of the application.
    /// </summary>
    public ServicesContainer AddSingleton<TImplementation>(TImplementation implementation) where TImplementation : class
    {
        if (Installator.Singletons.ContainsKey(typeof(TImplementation)))
        {
            return this;
        }

        Installator.Singletons[typeof(TImplementation)] = typeof(TImplementation);
        Installator.SingletonInstances[typeof(TImplementation)] = implementation;
        return this;
    }

    /// <summary>
    /// Adds new scoped to the services that will be later used to resolve dependencies. A scoped will be created for every newly instantiated controller.
    /// </summary>
    public ServicesContainer AddScoped<TService, TImplementation>() where TService : class where TImplementation : class, TService
    {
        if (Installator.Scopeds.ContainsKey(typeof(TService)))
        {
            return this;
        }

        Installator.Scopeds[typeof(TService)] = typeof(TImplementation);
        return this;
    }

    /// <summary>
    /// Adds new <seealso cref="IConfiguration"/> implementation which will be singleton, and used throughout the runtime of the application.
    /// </summary>
    public ServicesContainer AddConfiguration<TConfigurationImplementation>(TConfigurationImplementation implementation) where TConfigurationImplementation : class, IConfiguration
    {
        if (Installator.Singletons.ContainsKey(typeof(IConfiguration)))
        {
            return this;
        }

        Installator.Singletons[typeof(IConfiguration)] = typeof(TConfigurationImplementation);
        Installator.SingletonInstances[typeof(TConfigurationImplementation)] = implementation;
        Configuration = implementation;
        return this;
    }

    /// <summary>
    /// Registers a new middleware to be used in the application pipeline. The middleware will be instantiated later by the middleware handling mechanism.
    /// </summary>
    /// <remarks>You can call this method multiple times and add different middleware, which together will create a pipeline and will be executed in the order in which this method was called for them.</remarks>
    public ServicesContainer UseMiddleware<TMiddlewareImplementation>() where TMiddlewareImplementation : IMiddleware
    {
        if (Installator.Middlewares.Contains(typeof(TMiddlewareImplementation)))
        {
            return this;
        }

        Installator.Middlewares.Add(typeof(TMiddlewareImplementation));
        return this;
    }

    /// <summary>
    /// Registers a new middleware to be used in the application pipeline. Given middleware instance will be used throughout the runtime of the application.
    /// </summary>
    /// <remarks>You can call this method multiple times and add different middleware, which together will create a pipeline and will be executed in the order in which this method was called for them.</remarks>
    public ServicesContainer UseMiddleware<TMiddlewareImplementation>(TMiddlewareImplementation implementation) where TMiddlewareImplementation : IMiddleware
    {
        if (Installator.Middlewares.Contains(typeof(TMiddlewareImplementation)))
        {
            return this;
        }

        Installator.Middlewares.Add(typeof(TMiddlewareImplementation));
        Installator.MiddlewareInstances.Add(implementation);
        return this;
    }

    /// <summary>
    /// Adds a repository to the services container with the specified database connection and repository type.
    /// </summary>
    /// <param name="connection">The database connection to be used by the repository.</param>
    /// <param name="repositoryType">The type of the repository to be added. Must implement <see cref="IRepository{T}"/>.</param>
    /// <exception cref="ArgumentException">Thrown when the provided <paramref name="repositoryType"/> does not implement <see cref="IRepository{T}"/>.</exception>
    public ServicesContainer AddRepository(IDatabaseConnection connection, Type repositoryType)
    {
        if (!repositoryType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IRepository<>)))
        {
            throw new ArgumentException($"{repositoryType.Name} must implement IRepository<T> interface.", nameof(repositoryType));
        }

        AddSingleton<IDatabaseConnection>(connection);
        Installator.RepositoryType = repositoryType;
        return this;
    }

    /// <summary>
    /// Adds a background service to the services container.
    /// </summary>
    /// <param name="service">The background service to be added.</param>
    /// <param name="autoStart">Indicates whether the background service should start automatically with server start.</param>
    public ServicesContainer AddBackgroundService(IBackgroundService service, bool autoStart)
    {
        if (!Installator.BackgroundServices.Any(s => s.service == service))
        {
            Installator.BackgroundServices.Add((service, autoStart));
        }

        return this;
    }
}