using BeautyControl.API.Domain._Common;
using BeautyControl.API.Domain.Employees;

namespace BeautyControl.API.Infra.Identity.Models
{
    public class UserRoles : Enumeration
    {
        public const string AdminName = "Admin";
        public const string EmployeeName = "Employee";

        public static readonly UserRoles Admin = new(0, AdminName, Position.Manager);
        public static readonly UserRoles Employee = new(1, EmployeeName, Position.Salesman);

        public Position Position { get; }

        private UserRoles(int value, string name, Position position) : base(value, name) 
        {
            Position = position;
        }

        public static IEnumerable<UserRoles> List => new[] { Admin, Employee };

        public static UserRoles From(Position position)
        {
            if (IsValidPosition(position) == false)
                throw new ArgumentException($"Possíveis valores para UserRoles: {string.Join(",", List.Select(s => s.Name))}");

            return List.Single(x => x.Position == position);
        }

        public static bool IsValidValue(int value) => List.Any(x => x.Value == value);

        public static bool IsValidPosition(Position position) => List.Any(x => x.Position == position);
    }
}
