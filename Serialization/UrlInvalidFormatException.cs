namespace BlinkHttp.Serialization;

[Serializable]
internal class UrlInvalidFormatException : Exception
{
    public UrlInvalidFormatException() { }
    public UrlInvalidFormatException(string message) : base(message) { }
    public UrlInvalidFormatException(string message, Exception inner) : base(message, inner) { }
}
