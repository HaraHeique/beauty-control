using BeautyControl.API.Domain._Common;

#nullable disable
namespace BeautyControl.API.Domain.Employees
{
    public class Employee : Entity
    {
        public string Name { get; set; }
        public Email Email { get; set; }
        public bool Active { get; set; }
    }

    public enum Position
    {
        Salesman = 1,
        Manager
    }
}
