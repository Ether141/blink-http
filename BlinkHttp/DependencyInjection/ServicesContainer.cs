using System.Reflection;
using System.Linq;
using System.Reflection.Metadata;
using BlinkDatabase.General;
using BlinkDatabase.PostgreSql;
using BlinkHttp.Configuration;
using BlinkHttp.Handling;
using BlinkHttp.Http;

namespace BlinkHttp.DependencyInjection;

/// <summary>
/// Manages and stores services for dependency injections.
/// </summary>
public class ServicesContainer
{
    internal Installator Installator { get; } = new Installator();

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
    /// Adds new <seealso cref="PostgreSqlConnection"/> as singleton, which will be used for handling database operations and supplying new <seealso cref="IRepository{T}"/>.
    /// </summary>
    /// <remarks>Note: When you add a database connection using this method, firstly you also need to add <seealso cref="IConfiguration"/> using the AddConfiguration method.</remarks>
    public ServicesContainer AddPostgreSql()
    {
        IConfiguration configuration = Installator.GetSingletonByService<IConfiguration>();
        bool loggingOn = configuration["sql:logging_on"] != null ? configuration.Get<bool>("sql:logging_on") : false;
        AddPostgreSql(configuration.Get("sql:hostname")!, configuration.Get("sql:username")!, configuration.Get("sql:password")!, configuration.Get("sql:database")!, loggingOn);
        return this;
    }

    /// <summary>
    /// Adds new <seealso cref="PostgreSqlConnection"/> as singleton, which will be used for handling database operations and supplying new <seealso cref="IRepository{T}"/>.
    /// </summary>
    public ServicesContainer AddPostgreSql(string hostname, string username, string password, string database, bool loggingOn)
    {
        PostgreSqlConnection conn = new PostgreSqlConnection(hostname, username, password, database);
        conn.SqlQueriesLogging = loggingOn;
        AddSingleton<IDatabaseConnection, PostgreSqlConnection>(conn);
        Installator.RepositoryType = typeof(PostgreSqlRepository<>);
        return this;
    }
}
