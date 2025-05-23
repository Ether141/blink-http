﻿using BlinkHttp.Http;
using BlinkHttp.Routing;
using System.Net;
using System.Reflection;

namespace BlinkHttp.Serialization;

internal static class RequestDataHandler
{
    internal static object?[]? GetArguments(Route route, string path, HttpRequest request)
    {
        List<object?>? args = null;

        object?[]? argsFromUrl = GetArgumentsFromUrl(route, path);
        object?[]? argsFromBody = GetArgumentsFromBody(route, request);

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
        UrlParameter[]? urlParameters = GetRequestParameters.GetUrlParameters(route.PathWithQuery, path);
        IEndpoint endpoint = route.Endpoint;

        if (endpoint.MethodHasParameters)
        {
            return GetRequestParameters.ExtractArguments(urlParameters, endpoint.MethodInfo);
        }
        else if (route.HasRouteParameters)
        {
            throw new UrlInvalidFormatException();
        }

        return null;
    }

    private static object?[]? GetArgumentsFromBody(Route route, HttpRequest request)
    {
        MethodInfo methodInfo = route.Endpoint.MethodInfo;

        if (RequestBodyParser.GetFromFormParameters(methodInfo).Length == 0)
        {
            return null;
        }

        if (!request.HasEntityBody || string.IsNullOrEmpty(request.ContentType))
        {
            throw new RequestBodyInvalidException();
        }
        else
        {
            RequestContent content = new RequestContent(request.ContentType!, request.ContentLength64, request.InputStream, request.ContentEncoding);
            return RequestBodyParser.ParseBody(content, methodInfo);
        }
    }
}
