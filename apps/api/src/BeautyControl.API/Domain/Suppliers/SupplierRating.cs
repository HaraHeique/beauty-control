using BeautyControl.API.Domain._Common;
using BeautyControl.API.Domain.Employees;

#nullable disable
namespace BeautyControl.API.Domain.Suppliers
{
    public class SupplierRating : Entity
    {
        public decimal Rating { get; set; }
        public DateTime Date { get; set; }

        public Supplier Supplier { get; set; }
        public Employee Employee { get; set; }

        // EF Constructor
        private SupplierRating() { }
    }
}
