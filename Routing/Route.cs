namespace BlinkHttp.Routing
{
    internal class Route
    {
        internal string Path { get; }
        internal Http.HttpMethod HttpMethod { get; }

        internal ControllerRoute? AssociatedRoute { get; private set; }
        internal IEndpoint? Endpoint { get; private set; }

        internal Route(string path, Http.HttpMethod httpMethod)
        {
            Path = path;
            HttpMethod = httpMethod;
        }

        internal void CreateEndpoint(IEndpointMethod endpointMethod) => CreateEndpoint(null, endpointMethod);

        internal void CreateEndpoint(ControllerRoute? associatedRoute, IEndpointMethod endpointMethod)
        {
            AssociatedRoute = associatedRoute;
            
            if (associatedRoute != null)
            {
                Endpoint = new ControllerEndpoint(HttpMethod, associatedRoute.Controller, endpointMethod);
            }
            else
            {
                Endpoint = new SingleEndpoint(HttpMethod, endpointMethod);
            }
        }

        public override string? ToString() => $"[{HttpMethod}] {Path}";
    }
}
