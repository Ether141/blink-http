namespace BlinkHttp.Routing;

internal class RouterOptions
{
    private string? routePrefix;

    internal string? RoutePrefix
    {
        get => routePrefix;
        set => routePrefix = value == null ? value : value.Trim('/', '\\');
    }

    internal Type[]? IgnoredControllerTypes { get; set; }
}
