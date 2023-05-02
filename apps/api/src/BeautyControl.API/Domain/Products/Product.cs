using BeautyControl.API.Domain._Common;

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
    }
}
