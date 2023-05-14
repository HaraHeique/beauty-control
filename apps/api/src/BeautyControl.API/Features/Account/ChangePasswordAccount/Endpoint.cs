using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using BeautyControl.API.Infra.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BeautyControl.API.Features.Account.ChangePasswordAccount
{
    #pragma warning disable CS8618
    public record Request
    {
        [FromRoute(Name = "id")] public int Id { get; init; }
        [FromBody] public ChangePasswordRequest Body { get; init; }

        public record ChangePasswordRequest(string NewPassword, string PasswordConfirmation);
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

        [HttpPatch("users/{id:int}/change-password")]
        [SwaggerOperation(
            Summary = "Alterar a senha da conta de um determinado usuário",
            Description = "Alteração da senha do usuário a partir de seu ID. Lembrando que a mudança só é feito por um administrador.",
            OperationId = "Account.ChangePasswordAccount",
            Tags = new[] { SwaggerOperations.Tags.Account }
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public override async Task<ActionResult> HandleAsync([FromRoute] Request request, CancellationToken cancellationToken = default)
        {
            (var newPassword, var newPasswordConfirmation) = request.Body;
            var result = await _mediator.Send(new Command(request.Id, newPassword, newPasswordConfirmation), cancellationToken);

            return this.Response(result);
        }
    }
}