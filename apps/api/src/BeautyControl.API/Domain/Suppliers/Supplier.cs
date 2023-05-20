using BeautyControl.API.Domain._Common;

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

        public IList<SupplierRating> SupplierRatings { get; set; }

        // EF Constructor
        private Supplier() { }

        public Supplier(string name, string observation, Telephone telephone)
        {
            Name = name;
            Observation = observation;
            Telephone = telephone;
            AverageRating = 0;
            CreationDate = DateTime.Now;
        }
    }
}
