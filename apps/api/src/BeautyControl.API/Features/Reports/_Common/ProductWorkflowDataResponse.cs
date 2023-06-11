using BeautyControl.API.Domain.Products;

#nullable disable
namespace BeautyControl.API.Features.Reports._Common
{
    public record ProductWorkflowDataResponse
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public Category Category { get; init; }
        public int Quantity { get; set; }
    }
}
