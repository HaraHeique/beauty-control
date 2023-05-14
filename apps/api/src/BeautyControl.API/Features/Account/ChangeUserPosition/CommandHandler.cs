using BeautyControl.API.Domain._Common.Errors;
using BeautyControl.API.Domain.Employees;
using BeautyControl.API.Infra.Identity.Models;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BeautyControl.API.Features.Account.ChangeUserPosition
{
    public class CommandHandler : IRequestHandler<Command, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMediator _mediator;

        public CommandHandler(UserManager<AppUser> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user is null)
                return Result.Fail(new NotFoundError($"Usuário com ID {request.UserId} não encontrado."));

            var newUserRole = UserRoles.From(request.Position);

            var result = await ChangeUserRole(user, newUserRole);

            if (result.IsFailed) return result;

            await _mediator.Publish(new UserPositionChangedEvent(user.Id, newUserRole.Position), cancellationToken);

            // TODO: Publicar evento informando via email ao usuário que sua cargo mudou dentro do sistema

            return Result.Ok();
        }

        public async Task<Result> ChangeUserRole(AppUser user, UserRoles newUserRole)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Any(role => role.Equals(newUserRole.Name)))
                return Result.Fail("O usuário já contém a role definida.");

            var removeRoleResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
            var addNewRoleResult = await _userManager.AddToRoleAsync(user, newUserRole.Name);

            if (!removeRoleResult.Succeeded || !addNewRoleResult.Succeeded)
                return Result.Fail("Não foi possível alterar a role do usuário");

            return Result.Ok();
        }
    }
}
