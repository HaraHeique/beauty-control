using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using BeautyControl.API.Features.Products._Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;
using BeautyControl.API.Features.Suppliers._Common;

namespace BeautyControl.API.Features.Suppliers.GetSupplierById
{
    [Authorize]
    [ApiVersion("1")]
    [Route(Routes.SuppliersUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<int>
        .WithActionResult<SupplierResponse>
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Obter um fornecedor pelo Id",
            Description = "Este endpoint é responsável por obter um fornecedor a partir do seu identificador proveniente na rota.",
            OperationId = "Suppliers.GetSupplierById",
            Tags = new[] { Tags.Suppliers }
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SupplierResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async override Task<ActionResult<SupplierResponse>> HandleAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            Query query = new(id);

            var result = await _mediator.Send(query, cancellationToken);

            return this.Response(result);
        }
    }
}
