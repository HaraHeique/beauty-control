using BeautyControl.API.Domain._Common;
using BeautyControl.API.Domain.Employees;
using BeautyControl.API.Domain.Products;
using BeautyControl.API.Domain.Suppliers;

#nullable disable
namespace BeautyControl.API.Domain.StockMovements
{
    public class StockMovements : Entity
    {
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public StockProcess Process { get; set; }

        public Product Product { get; set; }
        public Supplier Supplier { get; set; }
        public Employee Employee { get; set; }
    }
}
