namespace BlinkHttp.Http;

/// <summary>
/// An attribute to indicate that a endpoint should not enforce CORS (Cross-Origin Resource Sharing) policies.
/// </summary>
/// <remarks>
/// Use this attribute if you enabled global CORS when building the <seealso cref="BlinkHttp.Application.WebApplication"/> and you do not want the selected endpoint to use it.
/// </remarks>
[System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public sealed class NoCorsAttribute : Attribute { }
