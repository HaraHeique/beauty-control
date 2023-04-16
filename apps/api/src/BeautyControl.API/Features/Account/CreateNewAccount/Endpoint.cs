using Ardalis.ApiEndpoints;
using BeautyControl.API.Features.Account.Common;
using BeautyControl.API.Features.Common.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BeautyControl.API.Features.Account.CreateNewAccount
{
    [AllowAnonymous]
    [ApiVersion("1")]
    [Route(Routes.AccountUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<Command>
        .WithActionResult<LoggedUserResponse>
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpPost("new-account")]
        [SwaggerOperation(
            Summary = "Criar um novo usuário",
            Description = "Criar um novo usuário na base de dados",
            OperationId = "Account.CreateNewAccount",
            Tags = new[] { "Account" }
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoggedUserResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        public override async Task<ActionResult<LoggedUserResponse>> HandleAsync([FromBody] Command request, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return this.Response(result);
        }
    }
}
