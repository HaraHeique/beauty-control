using System.ComponentModel;

namespace BeautyControl.API.Features.Reports.GetBestSuppliersByRating
{
    [DisplayName("GetBestSuppliersByRatingRequest")]
    public record Query(decimal Rating = 0M);

    [DisplayName("BestSupplierResponse")]
    public record Response(int Id, string Name, decimal AverageRating);
}
