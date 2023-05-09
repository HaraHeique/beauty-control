using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using BeautyControl.API.Features.Products._Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using BeautyControl.API.Features._Common.Contracts;

namespace BeautyControl.API.Features.Products.GetProducts.V2
{
    [Authorize]
    [ApiVersion("2")]
    [Route(Routes.ProductsUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<Query>
        .WithActionResult<PaginatedResponse<ProductResponse>>
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [SwaggerOperation(
            Summary = "Obter todos produtos paginados.",
            Description = "Este endpoint é responsável por obter os produtos de maneira paginada, tornando o processo mais eficiente na busca dos resultados. " +
            "Logo deve ser passado no mínimo o tamanho da página e a quantidade de registros desejados por página.",
            OperationId = "Products.GetProducts.V2",
            Tags = new[] { Tags.Products }
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResponse<ProductResponse>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        public async override Task<ActionResult<PaginatedResponse<ProductResponse>>> HandleAsync([FromQuery] Query request, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return this.Response(result);
        }
    }
}
