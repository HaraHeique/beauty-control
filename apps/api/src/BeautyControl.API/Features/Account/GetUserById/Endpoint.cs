using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;
using Swashbuckle.AspNetCore.Annotations;
using BeautyControl.API.Features.Account._Common;
using MediatR;

namespace BeautyControl.API.Features.Account.GetUserById
{
    [Authorize]
    [ApiVersion("1")]
    [Route(Routes.AccountUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<int>
        .WithActionResult<UserResponse>
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpGet("users/{id:int}")]
        [SwaggerOperation(
            Summary = "Obter um usuário específico do sistema.",
            Description = "Obtém um usuário do sistema pelo seu identificador único (ID).",
            OperationId = "Account.GetUserById",
            Tags = new[] { Tags.Account }
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async override Task<ActionResult<UserResponse>> HandleAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new Query(id), cancellationToken);

            return this.Response(result);
        }
    }
}
