namespace BlinkHttp.Validation;

/// <summary>
/// Ensures that value is not null.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public sealed class DisallowNullAttribute : ValidationAttribute
{
    public DisallowNullAttribute() : base($"Value cannot be null.") { }

    public DisallowNullAttribute(string errorMessage) : base(errorMessage) { }

    public override string? ValidateAndGetErrorMessage(object? value) => value == null ? ErrorMessage : null;
}