using Ardalis.ApiEndpoints;
using BeautyControl.API.Domain._Common.Errors;
using BeautyControl.API.Extensions;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BeautyControl.API.Features._Common.Endpoints
{
    public static class BaseEndpointExtensions
    {
        public static ActionResult Response<TResponse>(this EndpointBase endpoint, Result<TResponse> result)
        {
            if (result.IsSuccess) return endpoint.Ok(result.Value);

            return ErrorResponse(endpoint, result);
        }
        
        public static ActionResult Response<TResponse>(this EndpointBase endpoint, Result<TResponse> result, HttpStatusCode statusCode)
        {
            if (result.IsSuccess && statusCode.IsSuccessful()) 
                return endpoint.StatusCode(statusCode.ToInt(), result.Value);

            if (result.IsFailed && statusCode.IsFailed())
                return endpoint.StatusCode(statusCode.ToInt(), result.GetErrorsMessages());

            throw new InvalidOperationException("O objeto Result passado como argumento é divergente do HttpStatusCode desejado.");
        }

        public static ActionResult Response(this EndpointBase endpoint, Result result)
        {
            if (result.IsSuccess) return endpoint.NoContent();

            return ErrorResponse(endpoint, result);
        }

        public static ActionResult ErrorResponse(this EndpointBase endpoint, params string[] errors)
        {
            if (errors == null || errors.Any() == false)
                throw new ArgumentException("A chamada deste método ErrorResponse precisa conter ao menos uma mensagem de erro.");

            return endpoint.BadRequest(errors);
        }

        private static ActionResult ErrorResponse(this EndpointBase endpoint, ResultBase result)
        {
            if (result.HasError<NotFoundError>())
                return endpoint.NotFound();

            return endpoint.BadRequest(result.GetErrorsMessages());
        }
    }
}
