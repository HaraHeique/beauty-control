using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using BeautyControl.API.Infra.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;

namespace BeautyControl.API.Features.Products.DeleteProduct
{
    [Authorize(Roles = UserRoles.AdminName)]
    [ApiVersion("1")]
    [Route(Routes.ProductsUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<int>
        .WithActionResult
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpDelete("{id:int}")]
        [SwaggerOperation(
            Summary = "Remover um produto.",
            Description = "Remove um produto existente na base de dados, assim como sua imagem também.",
            OperationId = "Products.DeleteProduct",
            Tags = new[] { Tags.Products }
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async override Task<ActionResult> HandleAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new Command(id), cancellationToken);

            return this.Response(result);
        }
    }
}
