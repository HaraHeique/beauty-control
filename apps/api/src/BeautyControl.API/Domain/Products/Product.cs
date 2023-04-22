using BeautyControl.API.Domain._Common;

#nullable disable
namespace BeautyControl.API.Domain.Products
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public int RunningOutOfStock { get; set; }
        public StatusStock StatusStock { get; set; }
        public Category Category { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
