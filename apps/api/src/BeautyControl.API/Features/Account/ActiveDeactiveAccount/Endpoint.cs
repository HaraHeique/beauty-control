using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using BeautyControl.API.Infra.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;

namespace BeautyControl.API.Features.Account.ActiveDeactiveAccount
{
    public record Request
    {
        [FromRoute(Name = "id")] public int Id { get; init; }
        [FromBody] public ActiveOrDeactiveUserRequest Body { get; init; }

        public record ActiveOrDeactiveUserRequest(bool Active);
    }

    [Authorize(Roles = UserRoles.AdminName)] // TODO: Melhorar aqui olhando para a role uma claim específica
    [ApiVersion("1")]
    [Route(Routes.AccountUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<Request>
        .WithActionResult
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpPatch("users/{id:int}/active")]
        [SwaggerOperation(
            Summary = "Ativar ou desativar um usuário existente.",
            Description = "Responsável por ativar ou desativer um usuário através do seu Identificar e também pelo booleano indicando se vai ser ativado (true) ou desativado (false).",
            OperationId = "Account.ActiveDeactiveAccount",
            Tags = new[] { Tags.Account }
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public override async Task<ActionResult> HandleAsync([FromRoute] Request request, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new Command(request.Id, request.Body.Active), cancellationToken);

            return this.Response(result);
        }
    }
}
