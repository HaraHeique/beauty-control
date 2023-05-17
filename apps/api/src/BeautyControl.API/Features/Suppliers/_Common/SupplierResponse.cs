#nullable disable
namespace BeautyControl.API.Features.Suppliers._Common
{
    public record SupplierResponse
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Observation { get; init; }
        public string Telephone { get; init; }
        public decimal AverageRating { get; init; }
    }
}
