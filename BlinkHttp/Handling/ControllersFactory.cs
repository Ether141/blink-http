using BlinkHttp.DependencyInjection;
using BlinkHttp.Http;

namespace BlinkHttp.Handling;

internal class ControllersFactory
{
#pragma warning disable CS8618
    internal static ControllersFactory Factory { get; private set; }
#pragma warning restore CS8618

    private readonly ServicesContainer services;

    private ControllersFactory(ServicesContainer services)
    {
        this.services = services;
    }

    internal Controller? CreateController(Type type, HttpContext context)
    {
        Controller? controller;

        try
        {
            controller = (Controller)services.Installator.InstantiateClass(type);
        }
        catch
        {
            return null;
        }

        controller.Request = context.Request;
        controller.Response = context.Response;
        controller.User = context.User;

        return controller;
    }

    internal static void Initialize(ServicesContainer services) => Factory = new ControllersFactory(services);
}
