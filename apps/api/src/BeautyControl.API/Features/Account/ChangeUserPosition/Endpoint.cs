using Ardalis.ApiEndpoints;
using BeautyControl.API.Domain.Employees;
using BeautyControl.API.Features._Common.Endpoints;
using BeautyControl.API.Infra.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;

namespace BeautyControl.API.Features.Account.ChangeUserPosition
{
    #pragma warning disable CS8618
    public record Request
    {
        [FromRoute(Name = "id")] public int Id { get; init; }
        [FromBody] public ChangeUserPositionRequest Body { get; init; }

        public record ChangeUserPositionRequest(Position Position);
    }

    [Authorize(Roles = UserRoles.AdminName)]
    [ApiVersion("1")]
    [Route(Routes.AccountUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<Request>
        .WithActionResult
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpPatch("users/{id:int}/change-position")]
        [SwaggerOperation(
            Summary = "Alterar o cargo do usuário dentro do sistema",
            Description = "Atualizar o cargo de um usuário através do seu ID. Somente administradores podem fazer isto.",
            OperationId = "Account.ChangeUserPosition",
            Tags = new[] { Tags.Account }
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public override async Task<ActionResult> HandleAsync([FromRoute] Request request, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new Command(request.Id, request.Body.Position), cancellationToken);

            return this.Response(result);
        }
    }
}