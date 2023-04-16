using Ardalis.ApiEndpoints;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace BeautyControl.API.Features.@Common.Endpoints
{
    public static class BaseEndpointExtensions
    {
        public static ActionResult Response<TResponse>(this EndpointBase endpoint, Result<TResponse> result)
        {
            if (result.IsSuccess) return endpoint.Ok(result.Value);

            return endpoint.BadRequest(result.Errors.Select(e => e.Message));
        }

        public static ActionResult Response(this EndpointBase endpoint, Result result)
        {
            if (result.IsSuccess) return endpoint.NoContent();

            return endpoint.BadRequest(result.Errors.Select(e => e.Message));
        }
    }
}
