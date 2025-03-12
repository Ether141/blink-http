namespace BlinkHttp.Authentication.Session;

[Serializable]
public class SessionIdAlreadyExistsException : Exception
{
	public SessionIdAlreadyExistsException() : base("Session with given ID already exists.") { }

	public SessionIdAlreadyExistsException(string message) : base(message) { }

	public SessionIdAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
}
