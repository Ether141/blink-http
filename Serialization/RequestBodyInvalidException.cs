namespace BlinkHttp.Serialization;

[Serializable]
public class RequestBodyInvalidException : Exception
{
	public RequestBodyInvalidException() { }
	public RequestBodyInvalidException(string message) : base(message) { }
	public RequestBodyInvalidException(string message, Exception inner) : base(message, inner) { }
}
