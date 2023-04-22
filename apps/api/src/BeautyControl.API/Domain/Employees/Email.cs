using BeautyControl.API.Domain._Common;
using BeautyControl.API.Domain._Common.Exceptions;
using System.Text.RegularExpressions;

namespace BeautyControl.API.Domain.Employees
{
    public record class Email : IValueObject
    {
        public const int MaxLength = 254;
        public const int MinLength = 5;

        public string Address { get; }

        public Email(string address)
        {
            if (!Validate(address)) throw new DomainException("E-mail fornecido inválido");
            Address = address;
        }

        public static bool Validate(string address)
        {
            var regexEmail = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
            return regexEmail.IsMatch(address);
        }
    }
}
