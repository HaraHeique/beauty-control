using BeautyControl.API.Domain.Suppliers;
using FluentValidation;

namespace BeautyControl.API.Features.Suppliers._Common
{
    public abstract record BasicInfoSupplierCommand
    {
        public string? Name { get; init; }
        public string? Observation { get; init; }
        public string? Telephone { get; init; }
    }

    public class BasicInfoSupplierCommandValidation : AbstractValidator<BasicInfoSupplierCommand>
    {
        private const string messageRequiredField = "O campo {PropertyName} é obrigatório.";

        public BasicInfoSupplierCommandValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage(messageRequiredField).WithName("nome")
                .Length(min: 4, max: 70).WithMessage("O campo nome deve conter de {MinLength} a {MaxLength} caracteres.");

            RuleFor(c => c.Observation)
                .NotEmpty().WithMessage(messageRequiredField).WithName("observação")
                .MaximumLength(10000).WithMessage("O campo observação deve no máximo {MaxLength} caracteres.");

            RuleFor(c => c.Telephone)
                .NotEmpty().WithMessage(messageRequiredField).WithName("telefone")
                .Must(Telephone.Validate!).WithName("O campo telefone não está com tamanho e formato válido.");
        }
    }
}
