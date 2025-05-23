using System.Text;
using FluentValidation.Results;
using Strategyo.Results.Contracts.Results;

namespace App.InvoiSysTest.Application.Extensions;

public static class ValidationResultExtensions
{
    public static List<Error> HandleErrors(this List<ValidationFailure> validationFailures)
    {
        var errors = new List<Error>();
        
        var currentErrorBuilder = new StringBuilder();
        
        foreach (var validationFailure in validationFailures)
        {
            currentErrorBuilder.Append($"PropertyName: {validationFailure.PropertyName}");
            currentErrorBuilder.Append($"ErrorMessage: {validationFailure.ErrorMessage}");
            currentErrorBuilder.Append($"Severity: {validationFailure.Severity}");

            var currentError = currentErrorBuilder.ToString();
            
            errors.Add(Errors.Failure(currentError));
            
            currentErrorBuilder.Clear();
        }
        
        return errors;
    }
}