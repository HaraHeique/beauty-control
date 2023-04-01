using System.ComponentModel;

namespace BeautyControl.API.Extensions
{
    public static class DeclarionTypeExtensions
    {
        public static string GetDisplayName(this Type type, bool inherit = false)
        {
            return type.GetCustomAttributes(inherit)
                .OfType<DisplayNameAttribute>()
                .FirstOrDefault()?.DisplayName ?? type.Name;
        }
    }
}
