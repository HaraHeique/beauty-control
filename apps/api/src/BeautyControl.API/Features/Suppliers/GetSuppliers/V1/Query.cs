using BeautyControl.API.Features.Suppliers._Common;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Suppliers.GetSuppliers.V1
{
    [DisplayName("GetSuppliersRequest")]
    public record Query : IRequest<IEnumerable<SupplierResponse>>;
}
