using BeautyControl.API.Domain._Common;
using BeautyControl.API.Domain.Employees;
using BeautyControl.API.Domain.Products;
using BeautyControl.API.Domain.Suppliers;

#nullable disable
namespace BeautyControl.API.Domain.StockMovements
{
    public class StockMovements : Entity
    {
        public int Quantity { get; private set; }
        public DateTime Date { get; private set; }
        public StockProcess Process { get; private set; }

        public Product Product { get; set; }
        public Supplier? Supplier { get; set; }
        public Employee Employee { get; set; }

        // EF Constructor
        private StockMovements() 
        {
            Date = DateTime.Now;
        }

        public static StockMovements InputStockMovement(int quantity, Product product, Supplier supplier, Employee employee) 
        {
            return new StockMovements()
            {
                Quantity = quantity,
                Product = product,
                Supplier = supplier,
                Employee = employee,
                Process = StockProcess.Input
            };
        }
        
        public static StockMovements OutputStockMovement(int quantity, Product product, Employee employee) 
        {
            return new StockMovements()
            {
                Quantity = quantity,
                Product = product,
                Supplier = null,
                Employee = employee,
                Process = StockProcess.Output
            };
        }
    }
}
