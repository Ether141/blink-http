namespace BlinkHttp.Configuration;

[Serializable]
public class ApplicationConfigurationException : Exception
{
    public ApplicationConfigurationException() { }
    public ApplicationConfigurationException(string message) : base(message) { }
    public ApplicationConfigurationException(string message, Exception inner) : base(message, inner) { }
}
