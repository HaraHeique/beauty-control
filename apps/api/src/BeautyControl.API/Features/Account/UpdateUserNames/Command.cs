using FluentResults;
using FluentValidation;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Account.UpdateUserNames
{
    [DisplayName("UpdateUserNamesRequest")]
    public record Command(int UserId, string? UserName, string? FullName)
        : IRequest<Result>;

    public class CommandValidation : AbstractValidator<Command>
    {
        private const string mensageCampoObrigatorio = "O campo {PropertyName} é obrigatório";

        public CommandValidation()
        {
            RuleFor(c => c.UserId)
                .GreaterThan(0).WithMessage("O Id do usuário não pode ser zero ou negativo.");

            RuleFor(c => c.UserName)
                .NotEmpty().WithMessage(mensageCampoObrigatorio).WithName("nome de usuário");

            RuleFor(c => c.FullName)
                .NotEmpty().WithMessage(mensageCampoObrigatorio).WithName("nome pessoal")
                .Length(min: 4, max: 200).WithMessage("O campo {PropertyName} está com tamanho inválido.").WithName("nome pessoal");
        }
    }
}
