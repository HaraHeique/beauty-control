using BeautyControl.API.Features.Products._Common;
using BeautyControl.API.Features.Products._Common.Uploads;
using FluentResults;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace BeautyControl.API.Features.Products.CreateProduct
{
    [DisplayName("ProductCreateRequest")]
    public record Command : BasicInfoProductCommand, IRequest<Result<int>>, IImageUploadRequest
    {
        public IFormFile? ImageUpload { get; init; }

        #region Campos de suporte e não considerados ao ser feita a request

        const string swaggerDescriptions = "Este campo deve ser ignorado ao enviar o formulário para o servidor. Preencher ele não fará nenhuma diferença ou impacto.";

        [JsonIgnore]
        [SwaggerSchema(Description = swaggerDescriptions, ReadOnly = true)]
        public int? Id { get; init; }

        [JsonIgnore]
        [SwaggerSchema(Description = swaggerDescriptions, ReadOnly = true)]
        public string? Image { get; set; }

        [JsonIgnore]
        [SwaggerSchema(Description = swaggerDescriptions, ReadOnly = true)]
        public string? ImageUrlUpload { get; set; }

        #endregion
    }

    public class CommandValidation : AbstractValidator<Command>
    {
        public CommandValidation()
        {
            Include(new BasicInfoProductCommandValidation());
        }
    }
}
