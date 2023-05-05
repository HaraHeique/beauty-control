using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;

namespace BeautyControl.API.Features.Products.GetProductImage
{
    [Authorize]
    [ApiVersion("1")]
    [Route(Routes.ProductsUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<int>
        .WithActionResult<ImageResponse>
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpGet("{id:int}/image")]
        [SwaggerOperation(
            Summary = "Obter a imagem de um determinado produto.",
            Description = "Este endpoint é responsável por fornecer uma imagem de um determinado produto através do ID do produto em si.",
            OperationId = "Products.GetProductImage",
            Tags = new[] { Tags.Products }
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async override Task<ActionResult<ImageResponse>> HandleAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new Query(id), cancellationToken);

            return this.Response(result);
        }
    }
}
