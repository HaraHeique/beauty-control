using BeautyControl.API.Features.Account._Common;
using FluentResults;
using FluentValidation;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Account.GetUserById
{
    [DisplayName("GetUserByIdRequest")]
    public record Query(int Id) : IRequest<Result<UserResponse>>;

    public class QueryValidation : AbstractValidator<Query>
    {
        public QueryValidation()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0).WithMessage("O Id do usuário não pode ser zero ou negativo.");
        }
    }
}
