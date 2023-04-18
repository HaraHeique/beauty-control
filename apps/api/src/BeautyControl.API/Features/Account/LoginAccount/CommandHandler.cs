using BeautyControl.API.Features.Account._Common;
using BeautyControl.API.Infra.Identity.Models;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BeautyControl.API.Features.Account.LoginAccount
{
    public class CommandHandler : IRequestHandler<Command, Result<LoggedUserResponse>>
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtGenerator _jwtGenerator;

        public CommandHandler(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            JwtGenerator jwtGenerator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<Result<LoggedUserResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email!);

            if (user is null) return Result.Fail("Usuário e/ou senha inválidos");

            var signInResult = await _signInManager.PasswordSignInAsync(
                user!, request.Password!, 
                isPersistent: false, lockoutOnFailure: true
            );

            if (signInResult.IsLockedOut)
                return Result.Fail("Usuário temporariamente bloqueado por vários tentativas inválidas");

            if (!signInResult.Succeeded)
                return Result.Fail("Usuário e/ou senha inválidos");

            // TODO: Talvez publicar um evento para envio de email de que o usuário entrou na conta

            var jwtToken = await _jwtGenerator.GenerateToken(request.Email!);

            return Result.Ok(jwtToken);
        }
    }
}
