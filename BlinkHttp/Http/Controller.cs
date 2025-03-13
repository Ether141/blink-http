namespace BlinkHttp.Http;

/// <summary>
/// Basic class for all server controllers. It contains endpoints, that can be used in HTTP requests.
/// </summary>
public abstract class Controller
{
#pragma warning disable CS8618
    /// <summary>
    /// HTTP context for handling requests and responses.
    /// </summary>
    public HttpContext Context { get; internal set; }
#pragma warning restore CS8618

    /// <summary>
    /// This method is called when controller is newly created and values for HTTP context are set (except Request and Response). This method can be used to initialize database repositories, authentication stuff etc.
    /// </summary>
    public virtual void Initialize() { }
}
