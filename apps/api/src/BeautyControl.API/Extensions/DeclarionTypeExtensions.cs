using System.ComponentModel;

namespace BeautyControl.API.Extensions
{
    public static class DeclarionTypeExtensions
    {
        //private static readonly Dictionary<string, int> _schemaNameRepetition = new();

        public static string GetDisplayName(this Type type, bool inherit = false)
        {
            string id = DefaultSchemaIdSelector(type, inherit);

            //if (!_schemaNameRepetition.ContainsKey(id))
            //    _schemaNameRepetition.Add(id, 0);

            //int count = _schemaNameRepetition[id] + 1;
            //_schemaNameRepetition[id] = count;

            //return id + (count > 1 ? count.ToString() : string.Empty);

            return id;
        }

        private static string DefaultSchemaIdSelector(Type type, bool inherit)
        {
            string id = type.GetCustomAttributes(inherit)
                .OfType<DisplayNameAttribute>()
                .FirstOrDefault()?.DisplayName ?? type.Name;

            if (!type.IsConstructedGenericType) 
                return id.Replace("[]", "Array");

            var prefix = type.GetGenericArguments()
                .Select(genericArg => DefaultSchemaIdSelector(genericArg, inherit))
                .Aggregate((previous, current) => previous + current);

            return prefix + type.Name.Split('`').First();
        }
    }
}
