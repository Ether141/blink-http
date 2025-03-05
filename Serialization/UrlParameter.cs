namespace BlinkHttp.Serialization
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

        public override string? ToString() => $"{Name} = {Value}";
    }
}
