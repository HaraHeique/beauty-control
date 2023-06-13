using BeautyControl.API.Domain._Common.Exceptions;
using BeautyControl.API.Domain._Common.ValueObjects;
using System.Text.RegularExpressions;

#pragma warning disable CS8618 
namespace BeautyControl.API.Domain.Suppliers
{
    public record class Telephone : IValueObject
    {
        public string RawNumber { get; private set; }

        public string FormattedNumber => GetFormattedNumber();

        // EF Constructor
        private Telephone() { }

        public Telephone(string rawNumber)
        {
            RawNumber = Validate(rawNumber) ? rawNumber : throw new DomainException("Número de telefone inválido");
        }

        public static IEnumerable<Telephone> Create(IEnumerable<string> telephones) 
            => telephones.Select(item => new Telephone(item));

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
