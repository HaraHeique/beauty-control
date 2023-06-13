using BeautyControl.API.Domain.Employees;
using BeautyControl.API.Domain.Suppliers;
using FluentValidation;

namespace BeautyControl.API.Features.Suppliers._Common
{
    public abstract record BasicInfoSupplierCommand
    {
        public string? Name { get; init; }
        public string? Observation { get; init; }
        public string[] Telephones { get; init; } = Array.Empty<string>();
        public string[] Emails { get; init; } = Array.Empty<string>();
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

            RuleFor(c => c.Telephones)
                .NotEmpty().WithMessage(messageRequiredField).WithName("telefones")
                .Must(telephones => telephones.Length == telephones.Distinct().Count()).WithMessage("Não pode conter telefones repetidos.")
                .ForEach(rule =>
                {
                    rule.Must(Telephone.Validate!)
                        .WithMessage("O telefone {PropertyValue} não está com tamanho e formato válido.");
                });
            
            RuleFor(c => c.Emails)
                .NotEmpty().WithMessage(messageRequiredField).WithName("emails")
                .Must(emails => emails.Length == emails.Distinct().Count()).WithMessage("Não pode conter emails repetidos.")
                .ForEach(rule =>
                {
                    rule.EmailAddress()
                        .WithMessage("O email {PropertyValue} não é válido.");
                });
        }
    }
}
