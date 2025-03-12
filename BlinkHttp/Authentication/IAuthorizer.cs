using System.Net;

namespace BlinkHttp.Authentication;

public interface IAuthorizer
{
    AuthorizationResult Authorize(HttpListenerRequest request, AuthenticationRules? rules);
}
