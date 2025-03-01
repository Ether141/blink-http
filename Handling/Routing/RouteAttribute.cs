namespace BlinkHttp.Handling.Routing
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    internal sealed class RouteAttribute : Attribute
    {
        public string Routing { get; }

        public RouteAttribute(string routing)
        {
            Routing = routing;
        }
    }
}
