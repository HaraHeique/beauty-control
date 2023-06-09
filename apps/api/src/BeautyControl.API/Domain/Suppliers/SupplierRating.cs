using BeautyControl.API.Domain._Common;
using BeautyControl.API.Domain.Employees;

#nullable disable
namespace BeautyControl.API.Domain.Suppliers
{
    public class SupplierRating : Entity
    {
        public decimal Rating { get; set; }
        public DateTime FirstRatingAt { get; private set; }
        public DateTime LastRatingAt { get; private set; }

        public Supplier Supplier { get; set; }
        public Employee Employee { get; set; }

        // EF Constructor
        private SupplierRating() { }

        public SupplierRating(Supplier supplier, Employee employee, decimal rating) 
        {
            Supplier = supplier;
            Employee = employee;
            Rating = rating;
            FirstRatingAt = LastRatingAt = DateTime.Now;
        }

        // Ad-hoc setter
        public void NewRating(decimal rating)
        {
            LastRatingAt = DateTime.Now;
            Rating = rating;
        }
    }
}
