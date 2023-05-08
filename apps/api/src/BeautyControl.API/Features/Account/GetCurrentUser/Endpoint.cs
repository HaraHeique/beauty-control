using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;
using Swashbuckle.AspNetCore.Annotations;
using BeautyControl.API.Features._Common.Users;

namespace BeautyControl.API.Features.Account.GetCurrentUser
{
    [Authorize]
    [ApiVersion("1")]
    [Route(Routes.AccountUri)]
    public class Endpoint : EndpointBaseSync
        .WithoutRequest
        .WithActionResult<CurrentUserResponse>
    {
        private readonly CurrentUser _currentUser;

        public Endpoint(CurrentUser currentUser) => _currentUser = currentUser;

        [HttpGet("current-user")]
        [SwaggerOperation(
            Summary = "Obter o usuário logado corrente.",
            Description = "Responsável por retornar as informações do usuário corrente logado no sistema. " +
            "Claramente para obter essas informações deve haver um usuário autenticado.",
            OperationId = "Account.GetCurrentUser",
            Tags = new[] { Tags.Account }
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrentUserResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public override ActionResult<CurrentUserResponse> Handle()
        {
            return new CurrentUserResponse
            {
                Id = _currentUser.Id,
                Email = _currentUser.Email,
                UserName = _currentUser.Name,
                Roles = _currentUser.GetUserRoles(),
                Claims = Array.Empty<CurrentUserClaimResponse>()
            };
        }
    }
}
