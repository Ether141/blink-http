#pragma warning disable CS1591

namespace BlinkHttp.Validation;

/// <summary>
/// Ensures that length of string value is above given minimal length.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public sealed class MinLengthAttribute : ValidationAttribute
{
    public int MinLength { get; }

    public MinLengthAttribute(int minLength) : base($"Value length cannot be less than {minLength}.")
    {
        MinLength = minLength;
    }

    public MinLengthAttribute(int minLength, string errorMessage) : base(errorMessage)
    {
        MinLength = minLength;
    }

    public override string? ValidateAndGetErrorMessage(object? value) => value != null && value.ToString()!.Length < MinLength ? ErrorMessage : null;
}
