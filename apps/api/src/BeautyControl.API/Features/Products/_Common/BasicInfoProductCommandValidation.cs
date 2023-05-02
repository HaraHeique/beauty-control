using BeautyControl.API.Domain.Products;
using FluentValidation;

namespace BeautyControl.API.Features.Products._Common
{
    public record BasicInfoProductCommand
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public int RunningOutOfStock { get; init; }
        public Category Category { get; init; }
    }

    public class BasicInfoProductCommandValidation : AbstractValidator<BasicInfoProductCommand>
    {
        private const string messageRequiredField = "O campo {PropertyName} é obrigatório.";

        public BasicInfoProductCommandValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage(messageRequiredField).WithName("nome")
                .Length(min: 4, max: 70).WithMessage("O campo nome deve conter de {MinLength} a {MaxLength} caracteres.");

            RuleFor(c => c.Description)
                .NotEmpty().WithMessage(messageRequiredField).WithName("descrição")
                .MaximumLength(2000).WithMessage("O campo descrição deve no máximo {MaxLength} caracteres.");

            RuleFor(c => c.RunningOutOfStock)
                .GreaterThan(0).WithName("O campo de baixo estoque não pode ser negativo.");

            RuleFor(c => c.Category)
                .IsInEnum().WithMessage("O campo categoria não abrange valores válidos");
        }
    }
}
