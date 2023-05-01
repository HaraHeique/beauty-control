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

            return GetErrorResponse(endpoint, result);
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

            return GetErrorResponse(endpoint, result);
        }

        private static ActionResult GetErrorResponse(this EndpointBase endpoint, ResultBase result)
        {
            if (result.HasError(error => error is NotFoundError))
                return endpoint.NotFound();

            return endpoint.BadRequest(result.GetErrorsMessages());
        }
    }
}
