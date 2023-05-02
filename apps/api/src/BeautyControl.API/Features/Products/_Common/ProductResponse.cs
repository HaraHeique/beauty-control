using BeautyControl.API.Domain.Products;

#nullable disable
namespace BeautyControl.API.Features.Products._Common
{
    public record ProductResponse
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string? ImageUrl { get; init; }
        public int Quantity { get; init; }
        public int RunningOutOfStock { get; init; }
        public Category Category { get; init; }
        public StatusStock StatusStock { get; init; }
    }
}
