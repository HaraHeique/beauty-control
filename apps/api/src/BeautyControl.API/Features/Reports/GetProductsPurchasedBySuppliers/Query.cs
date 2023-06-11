using System.ComponentModel;
using BeautyControl.API.Features.Reports._Common;

namespace BeautyControl.API.Features.Reports.GetProductsPurchasedBySuppliers
{
    [DisplayName("GetProductsPurchasedBySuppliersRequest")]
    public record Query(DateTime? Start, DateTime? End)
    {
        // Mesmo que seja um DTO, tudo bem este método aqui por ser por uma questão bem específica dele não extrapolando nenhum compartamento ou "boa prática"
        public bool HasAnyParams() => Start.HasValue || End.HasValue;
    };

    [DisplayName("ProductsPurchasedBySuppliersResponse")]
    public record Response(
        int Id,
        string Name,
        IEnumerable<ProductWorkflowDataResponse> Products
    );
}
