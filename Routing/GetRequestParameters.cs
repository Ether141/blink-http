using System.Web;

namespace BlinkHttp.Routing
{
    internal static class GetRequestParameters
    {
        internal static UrlParameter[]? GetParameters(string template, string url)
        {
            template = RouteUrlUtility.TrimAndLowerUrl(template);
            url = RouteUrlUtility.TrimAndLowerUrl(url);

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
            int startIndex = template.IndexOf('{') - 1;

            if (startIndex < 0)
            {
                return null;
            }

            string templateOnlyPath = template[..startIndex];

            if (!url.StartsWith(templateOnlyPath))
            {
                return null;
            }

            int index = url.IndexOf('?');

            if (index > -1)
            {
                url = url[..index];
            }

            string[] paramsFromTemplate = template[startIndex..].Split('/', StringSplitOptions.RemoveEmptyEntries);
            string[] paramsFromUrl = url[startIndex..].Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (paramsFromTemplate.Length != paramsFromUrl.Length || paramsFromTemplate.Length == 0)
            {
                return null;
            }

            UrlParameter[] parameters = new UrlParameter[paramsFromTemplate.Length];

            for (int i = 0; i < paramsFromTemplate.Length; i++)
            {
                parameters[i] = new UrlParameter(paramsFromTemplate[i][1..^1], paramsFromUrl[i]);
            }

            return parameters;
        }

        private static UrlParameter[]? GetQueryParameters(string url)
        {
            int startIndex = url.IndexOf('?') + 1;

            if (startIndex < 0)
            {
                return null;
            }

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
    }
}
