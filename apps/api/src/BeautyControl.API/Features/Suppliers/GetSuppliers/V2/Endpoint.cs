using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using BeautyControl.API.Features._Common.Contracts;
using BeautyControl.API.Features.Suppliers._Common;

namespace BeautyControl.API.Features.Suppliers.GetSuppliers.V2
{
    [Authorize]
    [ApiVersion("2")]
    [Route(Routes.SuppliersUri)]
    public class Endpoint : EndpointBaseAsync
        .WithRequest<Query>
        .WithActionResult<PaginatedResponse<SupplierResponse>>
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [SwaggerOperation(
            Summary = "Obter todos fornecedores paginados.",
            Description = "Este endpoint é responsável por obter os fornecedores de maneira paginada, tornando o processo mais eficiente na busca dos resultados. " +
            "Logo deve ser passado no mínimo o tamanho da página e a quantidade de registros desejados por página.",
            OperationId = "Suppliers.GetSuppliers.V2",
            Tags = new[] { Tags.Suppliers }
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResponse<SupplierResponse>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        public async override Task<ActionResult<PaginatedResponse<SupplierResponse>>> HandleAsync([FromQuery] Query request, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return this.Response(result);
        }
    }
}
