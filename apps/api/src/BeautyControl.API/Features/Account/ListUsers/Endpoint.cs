using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;
using Swashbuckle.AspNetCore.Annotations;
using BeautyControl.API.Features.Account._Common;
using MediatR;

namespace BeautyControl.API.Features.Account.ListUsers
{
    [Authorize]
    [ApiVersion("1")]
    [Route(Routes.AccountUri)]
    public class Endpoint : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<IEnumerable<UserResponse>>
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpGet("users")]
        [SwaggerOperation(
            Summary = "Obter os usuários cadastrados do sistema.",
            Description = "Obtém todos os usuários cadastrados no sistema, exceto o que tá logado e que solicitou este recurso.",
            OperationId = "Account.ListUsers",
            Tags = new[] { Tags.Account }
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserResponse>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async override Task<ActionResult<IEnumerable<UserResponse>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new Query(), cancellationToken);

            return Ok(response);
        }
    }
}
