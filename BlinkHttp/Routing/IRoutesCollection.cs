using System.Reflection;

namespace BlinkHttp.Routing;

internal interface IRoutesCollection
{
    IReadOnlyList<Route> Routes { get; }
    string ControllerPath { get; }

    void AddRoute(string path, Http.HttpMethod httpMethod, MethodInfo method);
    bool RouteExists(string path, Http.HttpMethod method);
    Route? GetRoute(string path, Http.HttpMethod method);
    Route? GetRoute(string path);
}
