#pragma warning disable CS1591

namespace BlinkHttp.Validation;

/// <summary>
/// Ensures that the specified number of years have passed since the date value.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public sealed class DateOfBirthAttribute : ValidationAttribute
{
    public int MinAge { get; }

    /// <summary>
    /// Creates new instance of <seealso cref="DateOfBirthAttribute"/> with given minimal age, and default error message.
    /// </summary>
    public DateOfBirthAttribute(int minAge) : base($"Date does not meet minimum age requirements ({minAge}).")
    {
        MinAge = minAge;
    }

    /// <summary>
    /// Creates new instance of <seealso cref="DateOfBirthAttribute"/> with given minimal age, and custom error message.
    /// </summary>
    public DateOfBirthAttribute(int minAge, string errorMessage) : base(errorMessage)
    {
        MinAge = minAge;
    }

    public override string? ValidateAndGetErrorMessage(object? value)
    {
        if (value is not DateTime)
        {
            return null;
        }

        DateTime date = (DateTime)value;
        return date.AddYears(MinAge) > DateTime.Now ? ErrorMessage : null;
    }
}
