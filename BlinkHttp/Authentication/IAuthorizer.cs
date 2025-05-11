using BlinkHttp.Http;

namespace BlinkHttp.Authentication;

/// <summary>
/// Provides authorization functionality of <seealso cref="HttpRequest"/> for secured resource.
/// </summary>
public interface IAuthorizer
{
    /// <summary>
    /// <seealso cref="IUserInfoProvider"/> which is used by this instance of <seealso cref="IAuthorizer"/>.
    /// </summary>
    IUserInfoProvider UserInfoProvider { get; }

    /// <summary>
    /// Attempts to authorize given <seealso cref="HttpRequest"/>. Also optionally checks user priviliges, based on given <seealso cref="AuthenticationRules"/> if provided.
    /// </summary>
    AuthorizationResult Authorize(HttpRequest request, AuthenticationRules? rules);
}
