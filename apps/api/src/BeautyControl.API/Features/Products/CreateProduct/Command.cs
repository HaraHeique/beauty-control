using BeautyControl.API.Domain.Products;
using BeautyControl.API.Features.Products._Common;
using FluentResults;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace BeautyControl.API.Features.Products.CreateProduct
{
    [DisplayName("ProductCreateRequest")]
    public record Command : IRequest<Result<int>>, IImageUploadRequest
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public int RunningOutOfStock { get; init; }
        public Category Category { get; init; }
        public IFormFile? ImageUpload { get; init; }

        #region Campos de suporte e não considerados ao ser feita a request

        const string swaggerDescriptions = "Este campo deve ser ignorado ao enviar o formulário para o servidor. Preencher ele não fará nenhuma diferença ou impacto.";

        [JsonIgnore]
        [SwaggerSchema(Description = swaggerDescriptions, ReadOnly = true)]
        public int? Id { get; init; }

        [JsonIgnore]
        [SwaggerSchema(Description = swaggerDescriptions, ReadOnly = true)]
        public string? Image { get; set; }

        #endregion
    }

    public class CommandValidation : AbstractValidator<Command>
    {
        private const string messageRequiredField = "O campo {PropertyName} é obrigatório.";

        public CommandValidation()
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
