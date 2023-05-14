using BeautyControl.API.Features.Account._Common;
using FluentResults;
using FluentValidation;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Account.ChangePasswordAccount
{
    [DisplayName("ChangePasswordRequest")]
    public record Command(int UserId, string? NewPassword, string? NewPasswordConfirmation) 
        : IRequest<Result>;

    public class CommandValidation : AbstractValidator<Command>
    {
        private const string mensageCampoObrigatorio = "O campo {PropertyName} é obrigatório";

        public CommandValidation()
        {
            RuleFor(c => c.UserId)
                .NotEmpty().WithMessage("O Id do usuário está com valor inválido.")
                .GreaterThanOrEqualTo(0).WithMessage("O Id do usuário não pode ser negativo.");

            RuleFor(c => c.NewPassword)
                .NotEmpty().WithMessage(mensageCampoObrigatorio).WithName("senha")
                .Must(ValidatePasswordRules.HasValidPassword!).WithMessage("A nova senha inserida é inválida. É necessário conter de 8 a 15 carecteres com ao menos um número, um caractere espacial, letras minúsculas e maiúsculas.");

            RuleFor(c => c.NewPasswordConfirmation)
                .NotEmpty().WithMessage(mensageCampoObrigatorio).WithName("confirmação de senha")
                .Equal(c => c.NewPassword).WithMessage("A confirmação da nova senha deve ser igual a senha.");
        }
    }
}
