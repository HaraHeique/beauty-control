using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace BeautyControl.API.Features.Products.CreateProduct
{
    //[Authorize(Roles = UserRoles.AdminDisplayName)]
    [ApiVersion("1")]
    [Route(Routes.ProductsUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<Command>
        .WithActionResult<int>
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar novo produto",
            Description = "Criar um novo produto passando as informações básicas e valor de configuração chamado RunningOutOfStock (indicando a partir de qual valor que determina que está saindo fora do estoque). " +
            "A imagem é um dado opcional e não precisa conter no produto.",
            OperationId = "Products.CreateProduct",
            Tags = new[] { SwaggerOperations.Tags.Products }
        )]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        public async override Task<ActionResult<int>> HandleAsync([FromForm] Command request, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return this.Response(result, result.IsSuccess ? HttpStatusCode.Created : HttpStatusCode.BadRequest);
        }
    }
}
