using BeautyControl.API.Domain._Common;
using BeautyControl.API.Domain._Common.Exceptions;
using System.Text.RegularExpressions;

namespace BeautyControl.API.Domain.Suppliers
{
    public record class Telephone : IValueObject
    {
        public string RawNumber { get; }

        public string FormattedNumber => GetFormattedNumber();

        // EF Constructor
        private Telephone() { }

        public Telephone(string phoneNumber)
        {
            RawNumber = Validate(phoneNumber) ? phoneNumber : throw new DomainException("Número de telefone inválido");
        }

        public static bool Validate(string phoneNumber)
        {
            var pattern = @"^\(?\d{2}\)?\d{8,9}$";

            return Regex.IsMatch(phoneNumber, pattern);
        }

        private string GetFormattedNumber()
        {
            string pattern = @"(\d{2})(\d{4,5})(\d{4})";

            return Regex.Replace(RawNumber, pattern, "($1) $2-$3");
        }
    }
}
