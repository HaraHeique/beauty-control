using BeautyControl.API.Domain._Common.Errors;
using BeautyControl.API.Domain.Employees;
using BeautyControl.API.Infra.Identity.Models;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BeautyControl.API.Features.Account.UpdateUserNames
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

            user.UserName = request.UserName;
            await _userManager.UpdateAsync(user);
            await _userManager.UpdateNormalizedUserNameAsync(user);

            await _mediator.Publish(new UserAccountNameUpdatedEvent(user.Id, request.FullName!), cancellationToken);

            // TODO: Publicar evento informando via email ao usuário que seu nome de usuário e/ou nome pessoal foi alterado e que deve contatar

            return Result.Ok();
        }
    }
}
