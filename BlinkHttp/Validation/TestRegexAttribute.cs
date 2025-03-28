namespace BlinkHttp.Validation;

/// <summary>
/// Ensures that the string value matches given regular expression.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
public sealed class TestRegexAttribute : ValidationAttribute
{
    public string Regex { get; }

    public TestRegexAttribute(string regex) : base("Value does not match defined regular expression.")
    {
        Regex = regex;
    }

    public TestRegexAttribute(string regex, string errorMessage) : base(errorMessage)
    {
        Regex = regex;
    }

    public override string? ValidateAndGetErrorMessage(object? value) => value != null && value.ToString() != null && !System.Text.RegularExpressions.Regex.IsMatch(value.ToString()!, Regex) ? ErrorMessage : null;
}
