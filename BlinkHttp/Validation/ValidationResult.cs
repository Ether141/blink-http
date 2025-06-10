#pragma warning disable CS1591

namespace BlinkHttp.Validation;

/// <summary>
/// Encapsulates result of model validation process and error messages in case of invalid model.
/// </summary>
public class ValidationResult
{
    public bool IsValid { get; }
    public Dictionary<string, List<string>> Errors { get; }

    public static ValidationResult SuccessResult => new ValidationResult([]);

    public ValidationResult(Dictionary<string, List<string>> errors)
    {
        IsValid = errors.Count == 0;
        Errors = errors;
    }

    public override string? ToString() => string.Join(", ", Errors.Select(e => $"{e.Key} = ({string.Join(", ", e.Value)})"));
}
