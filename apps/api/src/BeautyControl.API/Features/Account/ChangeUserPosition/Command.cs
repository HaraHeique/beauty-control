using BeautyControl.API.Domain.Employees;
using FluentResults;
using FluentValidation;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Account.ChangeUserPosition
{
    [DisplayName("ChangeUserPositionRequest")]
    public record Command(int UserId, Position Position) : IRequest<Result>;

    public class CommandValidation : AbstractValidator<Command>
    {
        public CommandValidation()
        {
            RuleFor(c => c.UserId)
                .GreaterThan(0).WithMessage("O Id do usuário não pode ser zero ou negativo.");

            RuleFor(c => c.Position)
                .IsInEnum().WithMessage("O novo cargo definido para o usuário é inválido.");
        }
    }
}
