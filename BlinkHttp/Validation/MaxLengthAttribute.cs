#pragma warning disable CS1591

namespace BlinkHttp.Validation;

/// <summary>
/// Ensures that length of string value is below given maximal length.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public sealed class MaxLengthAttribute : ValidationAttribute
{
    public int MaxLength { get; }

    public MaxLengthAttribute(int maxLength) : base($"Value length cannot be greater than {maxLength}.")
    {
        MaxLength = maxLength;
    }

    public MaxLengthAttribute(int maxLength, string errorMessage) : base(errorMessage)
    {
        MaxLength = maxLength;
    }

    public override string? ValidateAndGetErrorMessage(object? value) => value != null && value.ToString()!.Length > MaxLength ? ErrorMessage : null;
}
