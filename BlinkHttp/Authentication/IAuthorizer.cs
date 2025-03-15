using System.Net;

namespace BlinkHttp.Authentication;

/// <summary>
/// Provides authorization functionality of <seealso cref="HttpListenerRequest"/> for secured resource.
/// </summary>
public interface IAuthorizer
{
    /// <summary>
    /// <seealso cref="IUserInfoProvider"/> which is used by this instance of <seealso cref="IAuthorizer"/>.
    /// </summary>
    IUserInfoProvider UserInfoProvider { get; }

    /// <summary>
    /// Attempts to authorize given <seealso cref="HttpListenerRequest"/>. Also optionally checks user priviliges, based on given <seealso cref="AuthenticationRules"/> if provided.
    /// </summary>
    AuthorizationResult Authorize(HttpListenerRequest request, AuthenticationRules? rules);
}
