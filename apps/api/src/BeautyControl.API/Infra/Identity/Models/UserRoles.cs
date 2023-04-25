using BeautyControl.API.Domain._Common;

namespace BeautyControl.API.Infra.Identity.Models
{
    public class UserRoles : Enumeration
    {
        public const string AdminDisplayName = "Admin";
        public const string EmployeeDisplayName = "Employee";

        public static readonly UserRoles Admin = new(0, AdminDisplayName);
        public static readonly UserRoles Employee = new(1, EmployeeDisplayName);

        private UserRoles(int value, string name) : base(value, name) { }
    }
}
