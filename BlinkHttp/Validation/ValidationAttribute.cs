using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlinkHttp.Validation;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public abstract class ValidationAttribute : Attribute
{
    public string ErrorMessage { get; }

    protected ValidationAttribute(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    public abstract string? ValidateAndGetErrorMessage(object? value);
}