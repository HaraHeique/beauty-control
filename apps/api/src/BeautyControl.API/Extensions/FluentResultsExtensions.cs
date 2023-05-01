using FluentResults;

namespace BeautyControl.API.Extensions
{
    public static class FluentResultsExtensions
    {
        public static IEnumerable<string> GetErrorsMessages(this ResultBase result) 
            => result.IsFailed ? result.Errors.Select(error => error.Message) : Enumerable.Empty<string>();

        public static IEnumerable<string> GetReasonsMessages(this ResultBase result)
            => result.Reasons.Select(reason => reason.Message);
    }
}
