using BeautyControl.API.Domain.Products;

namespace BeautyControl.API.Features.Products._Common
{
    public record ProductResponse(
        int Id,
        string Name,
        string Description,
        string? UrlImage,
        int Quantity,
        int RunningOutOfStock,
        Category Category,
        StatusStock StatusStock
    );
}
