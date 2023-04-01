using Ardalis.ApiEndpoints;
using BeautyControl.API.Features.Account.Common;
using BeautyControl.API.Features.Common.Endpoints;
using BeautyControl.API.Infra.Identity.Models;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace BeautyControl.API.Features.Account.CreateNewAccount
{
    [DisplayName("CreateNewAccountRequest")]
    public record class Command: IRequest<Result<LoggedUserResponse>>
    {
        public string? UserName { get; }
        public string? Email { get; }
        public string? Password { get; }
        public string? PasswordConfirmation { get; }

        public Command(string? userName, string? email, string? password, string? passwordConfirmation)
        {
            UserName = userName;
            Email = email;
            Password = password;
            PasswordConfirmation = passwordConfirmation;
        }

        public class CommandValidation : AbstractValidator<Command>
        {
            private const string mensageCampoObrigatorio = "O campo {PropertyName} é obrigatório";

            public CommandValidation()
            {
                RuleFor(c => c.UserName)
                    .NotEmpty().WithMessage(mensageCampoObrigatorio).WithName("nome usuário");

                RuleFor(c => c.Email)
                    .NotEmpty().WithMessage(mensageCampoObrigatorio).WithName("email")
                    .EmailAddress().WithMessage("O {PropertyName} é inválido").WithName("email");

                RuleFor(c => c.Password)
                    .NotEmpty().WithMessage(mensageCampoObrigatorio).WithName("senha")
                    .Must(HasValidPassword!).WithMessage("A senha inserida é inválida. É necessário conter de 8 a 15 carecteres com ao menos um número, um caractere espacial, letras minúsculas e maiúsculas");

                RuleFor(c => c.PasswordConfirmation)
                    .NotEmpty().WithMessage(mensageCampoObrigatorio).WithName("confirmação de senha")
                    .Equal(c => c.Password).WithMessage("A confirmação de senha deve ser igual a senha");
            }

            private bool HasValidPassword(string password)
            {
                var hasNumber = new Regex(@"[0-9]+");
                var hasUpperChar = new Regex(@"[A-Z]+");
                var hasLowerChar = new Regex(@"[a-z]+");
                var hasMinAndMaxChars = new Regex(@".{8,15}");
                var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

                return hasNumber.IsMatch(password) &&
                       hasUpperChar.IsMatch(password) &&
                       hasLowerChar.IsMatch(password) &&
                       hasMinAndMaxChars.IsMatch(password) &&
                       hasSymbols.IsMatch(password);
            }
        }
    }

    [AllowAnonymous]
    [Route(Routes.AccountUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<Command>
        .WithActionResult
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpPost("new-account")]
        [SwaggerOperation(
            Summary = "Criar um novo usuário",
            Description = "Criar um novo usuário na base de dados",
            OperationId = "Account.CreateNewAccount",
            Tags = new[] { "Account" }
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest)]
        public override async Task<ActionResult> HandleAsync([FromBody] Command request, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(request, cancellationToken);

            if (result.IsFailed)
                return BadRequest(result.Errors.Select(e => e.Message));

            return NoContent();
        }
    }

    public class CommandHandler : IRequestHandler<Command, Result<LoggedUserResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public CommandHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
            
            //var identityResult = await _userManager.CreateAsync(userCreated, request.Password!);

            //if (!identityResult.Succeeded)
            //    return Result.Fail(identityResult.Errors.Select(error => error.Description));

            return Result.Ok(new LoggedUserResponse());
        }

        private async Task<LoggedUserResponse> GenerateToken(string email)
        {
            throw new NotImplementedException();
        }
    }
}
