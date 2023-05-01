using FluentValidation.Results;

namespace BeautyControl.API.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IEnumerable<string> GetErrorsMessages(this ValidationResult validationResult) 
            => validationResult.Errors.Select(vl => vl.ErrorMessage);
    }
}
