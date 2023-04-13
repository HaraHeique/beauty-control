using BeautyControl.API.Features.Account.Common;
using BeautyControl.API.Infra.Identity.Models;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BeautyControl.API.Features.Account.CreateNewAccount
{
    public class CommandHandler : IRequestHandler<Command, Result<LoggedUserResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtGenerator _jwtGenerator;

        public CommandHandler(UserManager<AppUser> userManager, JwtGenerator jwtGenerator)
        {
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<Result<LoggedUserResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var userCreated = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email,
                EmailConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                Active = true
            };

            var identityResult = await _userManager.CreateAsync(userCreated, request.Password!);

            if (!identityResult.Succeeded)
                return Result.Fail(identityResult.Errors.Select(error => error.Description));

            var response = await _jwtGenerator.GenerateToken(userCreated.Email!);

            return Result.Ok(response);
        }
    }
}
