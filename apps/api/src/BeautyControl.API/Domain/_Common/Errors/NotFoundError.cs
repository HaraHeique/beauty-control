using FluentResults;
using System.Net;

namespace BeautyControl.API.Domain._Common.Errors
{
    public class NotFoundError : Error
    {
        public NotFoundError(string? message = null) : base(message) 
        {
            Metadata.Add("StatusCode", HttpStatusCode.NotFound);
        }
    }
}
