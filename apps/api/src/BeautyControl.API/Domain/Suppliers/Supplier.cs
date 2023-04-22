using BeautyControl.API.Domain._Common;

#nullable disable
namespace BeautyControl.API.Domain.Suppliers
{
    public class Supplier : Entity
    {
        public string Name { get; set; }
        public Telephone Telephone { get; set; }
        public string Observation { get; set; }
        public decimal AverageRating { get; set; }
        public DateTime CreationDate { get; set; }

        public IList<Supplier> SupplierRating { get; set; }
    }
}
