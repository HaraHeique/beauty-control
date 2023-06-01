using BeautyControl.API.Domain._Common;
using BeautyControl.API.Domain._Common.Exceptions;

#nullable disable
namespace BeautyControl.API.Domain.Products
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Image? Image { get; set; }
        public int Quantity { get; private set; }
        public int RunningOutOfStock { get; set; }
        public StatusStock Status { get; private set; }
        public Category Category { get; set; }
        public DateTime CreationDate { get; private set; }

        // EF Constructor
        private Product() { }

        public Product(string name, string description, Image image, int runningOutOfStock, Category category)
        {
            Name = name;
            Description = description;
            Image = image;
            RunningOutOfStock = runningOutOfStock;
            Category = category;
            Quantity = 0;
            Status = StatusStock.OutOfStock;
            CreationDate = DateTime.Now;
        }

        public void UpdateBasicInfo(string name, string description, Image image, int runningOutOfStock, Category category)
        {
            Name = name;
            Description = description;
            Image = image;
            RunningOutOfStock = runningOutOfStock;
            Category = category;
        }

        public void DeleteImage() => Image = null;

        public void AddInStock(int quantity)
        {
            if (quantity <= 0) throw new DomainException("A quantidade deve ser maior que zero!");

            Quantity += quantity;

            if (Quantity <= RunningOutOfStock)
                Status = StatusStock.RunningOutOfStock;
            else
                Status = StatusStock.InStock;
        }
    }
}
