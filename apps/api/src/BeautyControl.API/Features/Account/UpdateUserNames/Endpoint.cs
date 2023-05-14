using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using BeautyControl.API.Infra.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;

namespace BeautyControl.API.Features.Account.UpdateUserNames
{
#pragma warning disable CS8618
    public record Request
    {
        [FromRoute(Name = "id")] public int Id { get; init; }
        [FromBody] public UpdateUserNamesRequest Body { get; init; }

        public record UpdateUserNamesRequest(string UserName, string FullName);
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

        [HttpPatch("users/{id:int}/update-names")]
        [SwaggerOperation(
            Summary = "Alterar informações de nome de um determinado usuário",
            Description = "Atualizar o nome de usuário e nome real de um usuário através do seu ID. Somente administradores podem fazer isto",
            OperationId = "Account.UpdateUserNames",
            Tags = new[] { Tags.Account }
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public override async Task<ActionResult> HandleAsync([FromRoute] Request request, CancellationToken cancellationToken = default)
        {
            request.Body.Deconstruct(out string? userName, out string? fullName);
            var result = await _mediator.Send(new Command(request.Id, userName, fullName), cancellationToken);

            return this.Response(result);
        }
    }
}