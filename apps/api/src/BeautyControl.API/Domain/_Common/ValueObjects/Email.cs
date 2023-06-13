using BeautyControl.API.Domain._Common.Exceptions;
using BeautyControl.API.Domain.Suppliers;
using System.Text.RegularExpressions;

#pragma warning disable CS8618
namespace BeautyControl.API.Domain._Common.ValueObjects
{
    public record class Email : IValueObject
    {
        public const int MaxLength = 254;
        public const int MinLength = 5;

        public string Address { get; private set; }

        // EF Constructor
        private Email() { }

        public Email(string address)
        {
            if (!Validate(address))
                throw new DomainException("E-mail fornecido inválido");

            Address = address;
        }

        public static IEnumerable<Email> Create(IEnumerable<string> emails)
            => emails.Select(item => new Email(item));

        public static bool Validate(string address)
        {
            var regexEmail = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
            return regexEmail.IsMatch(address);
        }
    }
}
