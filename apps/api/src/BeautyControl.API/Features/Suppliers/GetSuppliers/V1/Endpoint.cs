using Ardalis.ApiEndpoints;
using BeautyControl.API.Features._Common.Endpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BeautyControl.API.Features._Common.Endpoints.SwaggerOperations;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using BeautyControl.API.Features.Suppliers._Common;

#pragma warning disable CS0809
namespace BeautyControl.API.Features.Suppliers.GetSuppliers.V1
{
    [Authorize]
    [ApiVersion("1")]
    [Route(Routes.SuppliersUri)]
    public class Endpoint : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<IEnumerable<SupplierResponse>>
    {
        private readonly IMediator _mediator;

        public Endpoint(IMediator mediator) => _mediator = mediator;

        [Obsolete("Use a versão V2 paginado desse método.")]
        [HttpGet]
        [SwaggerOperation(
            Summary = "Obter todos fornecedores",
            Description = "Este endpoint é responsável obter todos os fornedores da base de dados. " +
            "Uma observação que ainda não tem paginação, mas tudo bem porque a ideia é que o projeto seja de simples propósito.",
            OperationId = "Suppliers.GetSuppliers.V1",
            Tags = new[] { Tags.Suppliers }
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SupplierResponse>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async override Task<ActionResult<IEnumerable<SupplierResponse>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new Query(), cancellationToken);

            return Ok(response);
        }
    }
}
