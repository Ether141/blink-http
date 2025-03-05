using BlinkHttp.Routing;
using System.Net;

namespace BlinkHttp.Serialization;

internal static class RequestDataHandler
{
    internal static object?[]? GetArguments(Route route, string path, HttpListenerRequest request)
    {
        List<object?>? args = null;

        object?[]? argsFromUrl = GetArgumentsFromUrl(route, path);
        object?[]? argsFromBody = GetArgumentsFromBody(request);

        if (argsFromUrl != null)
        {
            args = [.. argsFromUrl];
        }

        if (argsFromBody != null)
        {
            args ??= [];
            args.AddRange(argsFromBody);
        }

        return args?.ToArray() ?? null;
    }

    private static object?[]? GetArgumentsFromUrl(Route route, string path)
    {
        UrlParameter[]? urlParameters = GetRequestParameters.GetUrlParameters(route.Path, path);
        IEndpoint endpoint = route.Endpoint!;

        if (endpoint.Method.MethodHasParameters)
        {
            return GetRequestParameters.ExtractArguments(urlParameters, endpoint.Method.MethodInfo);
        }
        else if (route.HasRouteParameters)
        {
            throw new UrlInvalidFormatException();
        }

        return null;
    }

    private static object?[]? GetArgumentsFromBody(HttpListenerRequest request)
    {
        if (!request.HasEntityBody || string.IsNullOrEmpty(request.ContentType))
        {
            return null;
        }

        Console.WriteLine(request.ContentType);
        Console.WriteLine(request.ContentLength64);
        Console.WriteLine(request.ContentEncoding);

        RequestContent content = new(request.ContentType, request.ContentLength64, request.InputStream, request.ContentEncoding);
        RequestBodyParser.ParseBody(content);
        return null;
    }
}
