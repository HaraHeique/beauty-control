﻿using FluentResults;
using FluentValidation;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Products.DeleteProduct
{
    [DisplayName("DeleteProductRequest")]
    public record Command(int Id) : IRequest<Result>;

    public class CommandValidation : AbstractValidator<Command>
    {
        public CommandValidation()
        {
            RuleFor(q => q.Id)
                .NotEmpty().WithMessage("O {PropertyName} está com valor inválido.")
                .GreaterThanOrEqualTo(0).WithMessage("O {PropertyName} não pode ser negativo.");
        }
    }
}
