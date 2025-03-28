using BlinkHttp.Serialization.Mapping;
using System.Reflection;

namespace BlinkHttp.Validation;

/// <summary>
/// Allows to validate model class, using attributes which can be used to mark properties of model.
/// </summary>
public class ModelValidator
{
    private readonly Dictionary<string, List<string>> errors = [];

    private PropertyInfo currentProperty;
    private object currentModel;

    /// <summary>
    /// Validates given model instance and returns <seealso cref="ValidationResult"/>.
    /// </summary>
    public ValidationResult Validate<T>(T model) where T : class
    {
        errors.Clear();
        currentModel = model;
        PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo property in properties)
        {
            currentProperty = property;
            HandleValidation();
        }

        return new ValidationResult(errors);
    }

    private void HandleValidation()
    {
        string? msg = null;
        IEnumerable<ValidationAttribute> attributes = currentProperty.GetCustomAttributes<ValidationAttribute>();

        foreach (ValidationAttribute attribute in attributes)
        {
            msg = attribute.ValidateAndGetErrorMessage(currentProperty.GetValue(currentModel));
            AddError(ref msg);
        }
    }

    private void AddError(ref string? msg)
    {
        string propName = NamesComparer.NormalizeName(currentProperty.Name, "_")!;

        if (msg != null)
        {
            if (!errors.ContainsKey(propName))
            {
                errors.Add(propName, []);
            }

            errors[propName].Add(msg);
            msg = null;
        }
    }
}
