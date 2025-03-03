namespace BlinkHttp.Routing
{
    internal interface IEndpointMethod
    {
        object? Invoke(object? obj, object?[]? args);
    }
}
