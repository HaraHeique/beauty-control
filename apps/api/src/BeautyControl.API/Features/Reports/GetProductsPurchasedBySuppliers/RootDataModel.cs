using BeautyControl.API.Domain.Products;

#nullable disable
namespace BeautyControl.API.Features.Reports.GetProductsPurchasedBySuppliers
{
    public record RootDataModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public SupplierDataModel Supplier { get; set; }
        public ProductDataModel Product { get; set; }
    }

    public record SupplierDataModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
    
    public record ProductDataModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public Category Category { get; init; }
    }
}
