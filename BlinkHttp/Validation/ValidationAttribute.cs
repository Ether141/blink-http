using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlinkHttp.Validation;

/// <summary>
/// Represents a base class for creating custom validation attributes.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public abstract class ValidationAttribute : Attribute
{
    /// <summary>
    /// Gets the error message associated with the validation failure.
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationAttribute"/> class with a specified error message.
    /// </summary>
    protected ValidationAttribute(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Validates the specified value and returns an error message if validation fails.
    /// </summary>
    /// <returns>
    /// A string containing the error message if validation fails; otherwise, <c>null</c>.
    /// </returns>
    public abstract string? ValidateAndGetErrorMessage(object? value);
}
