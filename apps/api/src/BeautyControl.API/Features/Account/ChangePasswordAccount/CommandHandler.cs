using BeautyControl.API.Domain._Common.Errors;
using BeautyControl.API.Infra.Identity.Models;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BeautyControl.API.Features.Account.ChangePasswordAccount
{
    public class CommandHandler : IRequestHandler<Command, Result>
    {
        private readonly UserManager<AppUser> _userManager;

        public CommandHandler(UserManager<AppUser> userManager) => _userManager = userManager;

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user is null)
                return Result.Fail(new NotFoundError($"Usuário com ID {request.UserId} não encontrado.")).LogIfFailed<CommandHandler>();

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword!);

            if (!result.Succeeded)
                return Result.Fail("Falha ao realizar mudança de senha do usuário.");

            // TODO: Publicar evento informando via email ao usuário que sua senha foi alterada e que deve contatar seu admin para saber nova senha. NÃO ENVIE A SENHA PARA O EMAIL POR QUESTÕES DE SEGURANÇA

            return Result.Ok();
        }
    }
}
