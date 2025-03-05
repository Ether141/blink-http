using BlinkHttp.Routing;
using System.Reflection;
using System.Web;

namespace BlinkHttp.Serialization;

internal static class GetRequestParameters
{
    internal static bool CompareArgumentsAndParametersCount(Route route, MethodInfo methodInfo)
        => GetRouteParameters(route.Path)?.Length == methodInfo.GetParameters().Where(m => m.GetCustomAttribute<OptionalAttribute>() == null).Count();

    internal static object?[]? ExtractArguments(UrlParameter[]? urlParameters, MethodInfo methodInfo)
    {
        List<ParameterInfo> parameters = [.. methodInfo.GetParameters()];
        ParameterInfo[] optionalParameters = parameters.Where(p => p.GetCustomAttribute<OptionalAttribute>() != null).ToArray();
        int requiredParameters = parameters.Count;

        if (parameters.Count > 0 && urlParameters == null || urlParameters!.Length < optionalParameters.Length)
        {
            throw new UrlInvalidFormatException();
        }

        List<object?> args = [];
        List<ParameterInfo> handledParams = [];

        foreach (ParameterInfo parameterInfo in parameters)
        {
            UrlParameter? urlParameter = urlParameters.FirstOrDefault(p => p.Name.Equals(parameterInfo.Name, StringComparison.OrdinalIgnoreCase));
            handledParams.Add(parameterInfo);

            if (urlParameter == null && !optionalParameters.Any(p => p == parameterInfo))
            {
                throw new UrlInvalidFormatException();
            }

            if (urlParameter == null)
            {
                continue;
            }

            try
            {
                args.Add(Convert.ChangeType(urlParameter.Value, parameterInfo.ParameterType));
            }
            catch
            {
                throw new UrlInvalidFormatException();
            }
        }

        foreach (ParameterInfo parameter in parameters.Except(handledParams))
        {
            if (!optionalParameters.Any(p => p == parameter))
            {
                throw new UrlInvalidFormatException();
            }
        }

        while (args.Count < requiredParameters)
        {
            args.Add(null);
        }

        return [.. args];
    }

    internal static UrlParameter[]? GetUrlParameters(string route, string path) =>
        GetParameters(route, string.Join('/', path.Split('/').TakeLast(route.Count(c => c == '/') + 1)));

    internal static UrlParameter[]? GetParameters(string template, string url)
    {
        url = HttpUtility.UrlDecode(url);

        List<UrlParameter> parameters = [];
        UrlParameter[]? routeParameters = GetRouteParameters(template, url);
        UrlParameter[]? queryParameters = GetQueryParameters(url);

        if (routeParameters != null)
        {
            parameters.AddRange(routeParameters);
        }

        if (queryParameters != null)
        {
            parameters.AddRange(queryParameters);
        }

        return parameters.Count > 0 ? [.. parameters] : null;
    }

    private static UrlParameter[]? GetRouteParameters(string template, string url)
    {
        url = RouteUrlUtility.RemoveQuery(url);

        string[] templateSplit = template.Split('/', StringSplitOptions.RemoveEmptyEntries);
        string[] urlSplit = url.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (templateSplit.Length != urlSplit.Length)
        {
            return null;
        }

        List<UrlParameter> parameters = new List<UrlParameter>();

        for (int i = 0; i < templateSplit.Length; i++)
        {
            if (RouteUrlUtility.IsRouteParameter(templateSplit[i]))
            {
                parameters.Add(new UrlParameter(templateSplit[i][1..^1], urlSplit[i]));
            }
        }

        return [.. parameters];
    }

    private static UrlParameter[]? GetQueryParameters(string url)
    {
        int startIndex = url.IndexOf('?');

        if (startIndex < 0)
        {
            return null;
        }

        startIndex++;
        string query = url[startIndex..];
        string[] paramsFromUrl = query.Split('&', StringSplitOptions.RemoveEmptyEntries);

        if (paramsFromUrl.Length == 0)
        {
            return null;
        }

        UrlParameter[] parameters = new UrlParameter[paramsFromUrl.Length];

        for (int i = 0; i < paramsFromUrl.Length; i++)
        {
            string[] split = paramsFromUrl[i].Split('=');
            string key = split[0];
            string value = split.Length == 2 ? split[1] : string.Empty;

            parameters[i] = new UrlParameter(key, value);
        }

        return parameters;
    }

    private static string[]? GetRouteParameters(string template)
    {
        string[] templateSplit = template.Split('/', StringSplitOptions.RemoveEmptyEntries);
        List<string> parameters = [];

        foreach (string s in templateSplit)
        {
            if (RouteUrlUtility.IsRouteParameter(s))
            {
                parameters.Add(s);
            }
        }

        return [.. parameters];
    }
}
