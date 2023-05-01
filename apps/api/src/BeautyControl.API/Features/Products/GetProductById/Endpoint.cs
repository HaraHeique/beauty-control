using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using BeautyControl.API.Features.Products._Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;

namespace BeautyControl.API.Features.Products.GetProductById
{
    [Authorize]
    [ApiVersion("1")]
    [Route(Routes.ProductsUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<int>
        .WithActionResult<ProductResponse>
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Obter um produto pelo Id",
            Description = "Este endpoint é responsável por fornecer um produto a partir do seu identificador proveniente na rota.",
            OperationId = "Products.GetProductById",
            Tags = new[] { Tags.Products }
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        public async override Task<ActionResult<ProductResponse>> HandleAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            Query query = new(id);

            var result = await _mediator.Send(query, cancellationToken);

            return this.Response(result);
        }
    }
}
