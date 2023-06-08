using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using BeautyControl.API.Infra.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;

namespace BeautyControl.API.Features.Products.AddQuantityInStock
{
    #pragma warning disable CS8618
    public sealed record Request
    {
        [FromRoute(Name = "id")] public int Id { get; init; }
        [FromBody] public AddQuantityInStockRequest Body { get; init; }

        public record AddQuantityInStockRequest(int ProductId, int SupplierId, int Quantity);
    }

    [Authorize(Roles = UserRoles.AdminName)]
    [ApiVersion("1")]
    [Route(Routes.ProductsUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<Request>
        .WithActionResult
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpPatch("{id:int}/add-quantity")]
        [SwaggerOperation(
            Summary = "Adicionar produtos em estoque",
            Description = "Adicionar unidades de um determinado produto no estoque. " +
            "Também é passado de qual fornecedor é proveniente os produtos adicionados ao estoque.",
            OperationId = "Products.AddQuantityInStock",
            Tags = new[] { Tags.Products }
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string[]))]
        public async override Task<ActionResult> HandleAsync([FromRoute] Request request, CancellationToken cancellationToken = default)
        {
            if (request.Id != request.Body.ProductId)
                return this.ErrorResponse("ID do produto passado na rota é diferente do informado no corpo da requisição.");

            var command = new Command
            {
                ProductId = request.Body.ProductId,
                SupplierId = request.Body.SupplierId,
                Quantity = request.Body.Quantity
            };

            var result = await _mediator.Send(command, cancellationToken);

            return this.Response(result);
        }
    }
}
