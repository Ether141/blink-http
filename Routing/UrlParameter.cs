namespace BlinkHttp.Routing
{
    internal class UrlParameter
    {
        public string Name { get; }
        public string Value { get; }

        public UrlParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
