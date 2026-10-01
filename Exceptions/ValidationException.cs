using FluentValidation.Results;

public class ValidationException : Exception
{
    public ValidationResult ValidationResult {get;}

    public ValidationException(ValidationResult validationResult)
    {
        ValidationResult = validationResult;
    }
}