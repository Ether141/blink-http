using BlinkHttp.Http;
using System.Reflection;

namespace BlinkHttp.Serialization.Mapping;

internal class RequestBodyMapper
{
    private readonly List<RequestValue> availableValues = [];
    private readonly Queue<string> currentDependency = [];

    internal MapperSettings Settings { get; }

    internal RequestBodyMapper(IEnumerable<RequestValue> values)
    {
        Settings = MapperSettings.Default;
        availableValues.AddRange(values);
    }

    internal RequestBodyMapper(MapperSettings settings, IEnumerable<RequestValue> values)
    {
        Settings = settings;
        availableValues.AddRange(values);
    }

    internal object? Map(Type type, string parameterName)
    {
        if (type == typeof(string))
        {
            return GetString(parameterName);
        }

        if (type == typeof(int))
        {
            return GetInt(parameterName);
        }

        if (type == typeof(float))
        {
            return GetFloat(parameterName);
        }

        if (type == typeof(double))
        {
            return GetDouble(parameterName);
        }

        if (type == typeof(decimal))
        {
            return GetDecimal(parameterName);
        }

        if (type == typeof(bool))
        {
            return GetBool(parameterName);
        }

        if (type == typeof(RequestFile))
        {
            return GetFile(parameterName);
        }

        if (IsCollection(type))
        {
            return GetCollection(type, parameterName);
        }

        PropertyInfo[] properties = [.. type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetCustomAttribute<IgnoreInMappingAttribute>() == null)];
        object obj = Activator.CreateInstance(type)!;

        foreach (PropertyInfo prop in properties)
        {
            RequestValue? matchedValue = GetValue(prop.Name, true);

            if (matchedValue == null)
            {
                if (prop.GetCustomAttribute<OptionalInMappingAttribute>() != null)
                {
                    prop.SetValue(obj, null);
                    continue;
                }

                currentDependency.Enqueue(prop.Name.ToLower());

                if (!IsAnyValueForCurrentDependency())
                {
                    currentDependency.Dequeue();
                    return null;
                }

                object? convertedNext = Map(prop.PropertyType, prop.Name);

                if (convertedNext == null)
                {
                    return null;
                }

                prop.SetValue(obj, convertedNext);
                currentDependency.Dequeue();
                continue;
            }

            object? converted = Map(prop.PropertyType, matchedValue.Name);

            if (converted == null)
            {
                return null;
            }

            prop.SetValue(obj, converted);
            availableValues.Remove(matchedValue);
        }

        return obj;
    }

    private RequestValue? GetValue(string name, bool allowFile = true)
    {
        string? internalObjectName = currentDependency.Count == 0 ? null : string.Join('.', currentDependency);
        RequestValue? val = availableValues.FirstOrDefault(v => Settings.NamesComparer.Equals(v.Name, name) && (internalObjectName == null || Settings.NamesComparer.Equals(v.InternalObjectName, internalObjectName)));
        return val != null && !allowFile && val.IsFile ? null : val;
    }

    private bool IsAnyValueForCurrentDependency()
    {
        string internalObjectName = string.Join('.', currentDependency);
        return availableValues.Any(v => v.IsInternalObject &&
                                        (Settings.NamesComparer.Equals(v.InternalObjectName!, internalObjectName) ||
                                        (v.InternalObjectName!.Contains('.') && Settings.NamesComparer.Equals(string.Join('.', v.InternalObjectName!.Split('.').SkipLast(1)), internalObjectName))));
    }

    private static bool IsCollection(Type type) => type.IsArray || typeof(System.Collections.IList).IsAssignableFrom(type) || typeof(IEnumerable<>).IsAssignableFrom(type);

#pragma warning disable CA1859
    private object? GetString(string parameterName)
    {
        RequestValue? matchedValue = GetValue(parameterName, false);

        if (matchedValue != null)
        {
            availableValues.Remove(matchedValue);
        }
        else
        {
            return null;
        }

        return matchedValue?.Values![0];
    }

    private object? GetInt(string parameterName)
    {
        RequestValue? matchedValue = GetValue(parameterName, false);

        if (matchedValue != null)
        {
            availableValues.Remove(matchedValue);
        }
        else
        {
            return null;
        }

        return int.TryParse(matchedValue.Values![0], out int result) ? result : null;
    }

    private object? GetFloat(string parameterName)
    {
        RequestValue? matchedValue = GetValue(parameterName, false);

        if (matchedValue != null)
        {
            availableValues.Remove(matchedValue);
        }
        else
        {
            return null;
        }

        return float.TryParse(matchedValue.Values![0], CultureHelper.DefaultNumberFormat, out float result) ? result : null;
    }

    private object? GetDouble(string parameterName)
    {
        RequestValue? matchedValue = GetValue(parameterName, false);

        if (matchedValue != null)
        {
            availableValues.Remove(matchedValue);
        }
        else
        {
            return null;
        }

        return double.TryParse(matchedValue.Values![0], CultureHelper.DefaultNumberFormat, out double result) ? result : null;
    }

    private object? GetDecimal(string parameterName)
    {
        RequestValue? matchedValue = GetValue(parameterName, false);

        if (matchedValue != null)
        {
            availableValues.Remove(matchedValue);
        }
        else
        {
            return null;
        }

        return decimal.TryParse(matchedValue.Values![0], CultureHelper.DefaultNumberFormat, out decimal result) ? result : null;
    }

    private object? GetBool(string parameterName)
    {
        RequestValue? matchedValue = GetValue(parameterName, false);

        if (matchedValue != null)
        {
            availableValues.Remove(matchedValue);
        }
        else
        {
            return null;
        }

        return bool.TryParse(matchedValue.Values![0].ToLower(), out bool result) ? result : null;
    }

    private object? GetFile(string parameterName)
    {
        RequestValue? matchedValue = GetValue(parameterName);

        if (matchedValue == null || !matchedValue.IsFile)
        {
            return null;
        }

        return matchedValue.File!;
    }

    private object? GetCollection(Type type, string parameterName, RequestValue? matchedValue = null)
    {
        matchedValue ??= GetValue(parameterName, false);

        if (matchedValue != null)
        {
            availableValues.Remove(matchedValue);
        }
        else
        {
            return null;
        }

        Type elementType = type.IsArray ? type.GetElementType()! : type.GetGenericArguments()[0];

        try
        {
            System.Collections.IList list = (System.Collections.IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType))!;

            foreach (string value in matchedValue.Values!)
            {
                object? converted = ChangeType(value, elementType);

                if (converted == null)
                {
                    return null;
                }

                list.Add(converted);
            }

            if (type.IsArray || type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                MethodInfo toArrayMethod = list.GetType().GetMethod("ToArray")!;
                return toArrayMethod.Invoke(list, null);
            }

            return list;
        }
        catch
        {
            return null;
        }
    }
#pragma warning restore CA1859

    private static object? ChangeType(string value, Type type)
    {
        try
        {
            return Convert.ChangeType(value, type, CultureHelper.DefaultCulture);
        }
        catch
        {
            return null;
        }
    }
}
