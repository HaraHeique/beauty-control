using BeautyControl.API.Domain._Common;
using BeautyControl.API.Domain._Common.Exceptions;
using BeautyControl.API.Domain.Employees;

#nullable disable
namespace BeautyControl.API.Domain.Suppliers
{
    public class Supplier : Entity
    {
        public string Name { get; set; }
        public string Observation { get; set; }
        public Telephone Telephone { get; set; }
        public decimal AverageRating { get; private set; }
        public DateTime CreationDate { get; private set; }

        public IReadOnlyCollection<SupplierRating> SupplierRatings => _supplierRatings;
        private readonly List<SupplierRating> _supplierRatings;

        // EF Constructor
        private Supplier()
        {
            _supplierRatings = new List<SupplierRating>();
        }

        public Supplier(string name, string observation, Telephone telephone) : this()
        {
            Name = name;
            Observation = observation;
            Telephone = telephone;
            AverageRating = 0M;
            CreationDate = DateTime.Now;
        }

        public void Evaluate(decimal rating, Employee employee)
        {
            ValidateConsistence(rating, employee);

            var existingRating = _supplierRatings.SingleOrDefault(sr => sr.Employee == employee);

            if (existingRating is null)
            {
                var newSupplierRating = new SupplierRating(this, employee, rating);
                _supplierRatings.Add(newSupplierRating);
            }
            else
            {
                existingRating.NewRating(rating);
            }

            AverageRating = SupplierRatings.Select(sr => sr.Rating).Sum() / SupplierRatings.Count;

            static void ValidateConsistence(decimal rating, Employee employee)
            {
                if (rating <= 0M)
                    throw new DomainException("A avaliação do fornecedor não pode ser maior que zero!");

                if (employee is null)
                    throw new DomainException("O funcionário não pode ser nulo!");
            }
        }
    }
}
