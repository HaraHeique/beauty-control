using FluentResults;
using FluentValidation;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Account.ActiveDeactiveAccount
{
    [DisplayName("ActiveOrDeactiveUserRequest")]
    public record Command(int Id, bool Active)
        : IRequest<Result>;

    public class CommandValidation : AbstractValidator<Command>
    {
        public CommandValidation()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("O Id do usuário está com valor inválido.")
                .GreaterThanOrEqualTo(0).WithMessage("O Id do usuário não pode ser negativo.");
        }
    }
}
