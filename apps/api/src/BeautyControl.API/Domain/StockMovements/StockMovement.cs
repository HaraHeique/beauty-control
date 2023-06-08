using BeautyControl.API.Domain._Common;
using BeautyControl.API.Domain.Employees;
using BeautyControl.API.Domain.Products;
using BeautyControl.API.Domain.Suppliers;

#nullable disable
namespace BeautyControl.API.Domain.StockMovements
{
    public class StockMovement : Entity
    {
        public int Quantity { get; private set; }
        public DateTime Date { get; private set; }
        public StockProcess Process { get; private set; }
        public int ProductId { get; private set; }
        public int? SupplierId { get; private set; }
        public int EmployeeId { get; private set; }

        // EF Relations
        public Product Product { get; set; }
        public Supplier? Supplier { get; set; }
        public Employee Employee { get; set; }

        // EF Constructor
        private StockMovement() 
        {
            Date = DateTime.Now;
        }

        public static StockMovement InputStockMovement(int quantity, int productId, int supplierId, int employeeId) 
        {
            return new StockMovement()
            {
                Quantity = quantity,
                ProductId = productId,
                SupplierId = supplierId,
                EmployeeId = employeeId,
                Process = StockProcess.Input
            };
        }
        
        public static StockMovement OutputStockMovement(int quantity, int productId, int employeeId) 
        {
            return new StockMovement()
            {
                Quantity = quantity,
                ProductId = productId,
                SupplierId = null,
                EmployeeId = employeeId,
                Process = StockProcess.Output
            };
        }
    }
}
