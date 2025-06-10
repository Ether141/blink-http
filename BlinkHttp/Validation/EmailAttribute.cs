#pragma warning disable CS1591

namespace BlinkHttp.Validation;

/// <summary>
/// Ensures that string value is valid email address.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public sealed class EmailAttribute : ValidationAttribute
{
    public EmailAttribute() : base("Value is not valid email address.") { }

    public EmailAttribute(string errorMessage) : base(errorMessage) { }

    public override string? ValidateAndGetErrorMessage(object? value)
    {
        if (value == null || value.ToString() == null)
        {
            return null;
        }

        try
        {
            new System.Net.Mail.MailAddress(value.ToString()!);
        }
        catch
        {
            return ErrorMessage;
        }

        return null;
    }
}
