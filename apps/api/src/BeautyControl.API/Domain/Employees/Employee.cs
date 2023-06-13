using BeautyControl.API.Domain._Common;
using BeautyControl.API.Domain._Common.ValueObjects;

#nullable disable
namespace BeautyControl.API.Domain.Employees
{
    public class Employee : Entity
    {
        public string Name { get; set; }
        public Email Email { get; set; }
        public bool Active { get; private set; }
        public Position Position { get; set; }

        // EF Constructor
        private Employee() { }

        public Employee(int id, string name, Email email, Position position) : base(id)
        {
            Name = name;
            Email = email;
            Position = position;
            Active = true;
        }

        // Ad-hoc setters methods (não são necessários, mas OK ter também)

        public void ActiveEmployee() => Active = true;

        public void DeactiveEmployee() => Active = false;

        public void ChangeName(string newName) => Name = newName;

        public void ChangePosition(Position position) => Position = position;
    }
}
