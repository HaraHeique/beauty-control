using BeautyControl.API.Domain.Employees;
using BeautyControl.API.Features.Account._Common;
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
        private readonly IMediator _mediator;

        public CommandHandler(
            UserManager<AppUser> userManager,
            JwtGenerator jwtGenerator, 
            IMediator mediator)
        {
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            _mediator = mediator;
        }

        public async Task<Result<LoggedUserResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            AppUser userCreated = CreateUserInstance(request);

            var identityResult = await _userManager.CreateAsync(userCreated, request.Password!);

            if (!identityResult.Succeeded)
                return Result.Fail(GetErrors(identityResult));

            var role = UserRoles.From(request.Position);
            identityResult = await _userManager.AddToRoleAsync(userCreated, role.Name);

            if (!identityResult.Succeeded)
                return Result.Fail(GetErrors(identityResult));

            await _mediator.Publish(new NewAccountCreatedEvent(userCreated.Id, request.FullName!, userCreated.Email!, request.Position), cancellationToken);

            var response = await _jwtGenerator.GenerateToken(userCreated.Email!);

            return Result.Ok(response);
        }

        private static IEnumerable<string> GetErrors(IdentityResult result) => result.Errors.Select(error => error.Description);

        private static AppUser CreateUserInstance(Command request)
        {
            return new AppUser
            {
                UserName = request.UserName,
                Email = request.Email,
                EmailConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                Active = true
            };
        }
    }
}
