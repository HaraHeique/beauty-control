using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using BeautyControl.API.Features.Products._Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;

namespace BeautyControl.API.Features.Products.GetProducts
{
    [Authorize]
    [ApiVersion("1")]
    [Route(Routes.ProductsUri)]
    public class Endpoint : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<IEnumerable<ProductResponse>>
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpGet()]
        [SwaggerOperation(
            Summary = "Obter todos produtos",
            Description = "Este endpoint é responsável obter todos os produtos da base de dados. " +
            "Uma observação que ainda não tem paginação, mas tudo bem porque a ideia é que o projeto seja de simples propósito",
            OperationId = "Products.GetProducts",
            Tags = new[] { Tags.Products }
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResponse))]
        public async override Task<ActionResult<IEnumerable<ProductResponse>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new Query(), cancellationToken);

            return Ok(response);
        }
    }
}
