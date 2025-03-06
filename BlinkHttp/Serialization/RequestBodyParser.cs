using BlinkHttp.Http;
using BlinkHttp.Serialization.Mapping;
using System.Reflection;

namespace BlinkHttp.Serialization;

internal static class RequestBodyParser
{
    internal static object?[]? ParseBody(RequestContent content, MethodInfo methodInfo)
    {
        string[] split = content.ContentType.Split(';');
        string mimeType = split.Length == 0 ? content.ContentType : split[0];

        IDataParser parser = GetParser(mimeType);
        RequestValue[] values = parser.Parse(content);
        return ConvertToArguments(values, methodInfo);
    }

    internal static ParameterInfo[] GetFromFormParameters(MethodInfo methodInfo)
        => [.. methodInfo.GetParameters().Where(p => p.GetCustomAttribute<FromBodyAttribute>() != null)];

    private static object?[]? ConvertToArguments(RequestValue[] values, MethodInfo methodInfo)
    {
        ParameterInfo[] parameters = [.. methodInfo.GetParameters().Where(p => p.GetCustomAttribute<FromBodyAttribute>() != null)];
        List<object?> args = [];
        RequestBodyMapper mapper = new RequestBodyMapper(values);

        foreach (ParameterInfo param in parameters)
        {
            object? obj = mapper.Map(param.ParameterType, param.Name!);

            if (obj == null)
            {
                if (param.GetCustomAttribute<OptionalAttribute>() != null)
                {
                    args.Add(null);
                    continue;
                }
                else
                {
                    throw new RequestBodyInvalidException();
                }
            }

            args.Add(obj);
        }

        return [.. args];
    }

    private static IDataParser GetParser(string mimeType) => mimeType switch
    {
        MimeTypes.MultipartFormData => new FormDataParser(),
        MimeTypes.ApplicationJson => new JsonDataParser(),
        _ => throw new NotSupportedException()
    };
}
