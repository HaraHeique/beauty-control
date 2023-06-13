using BeautyControl.API.Domain._Common.Exceptions;
using BeautyControl.API.Domain._Common.ValueObjects;
using System.Text.RegularExpressions;

#pragma warning disable CS8618
namespace BeautyControl.API.Domain.Products
{
    public record class Image : IValueObject
    {
        public string Name { get; }
        public string Url { get; }

        private Image() { }

        public Image(string imageName, string urlAccess)
        {
            if (!Validate(imageName, urlAccess)) throw new DomainException("O nome da imagem e/ou URL de acesso está inválido.");

            Name = imageName;
            Url = urlAccess;
        }

        public static bool Validate(string? imageName, string? urlAccess)
        {
            if (string.IsNullOrEmpty(imageName) || string.IsNullOrEmpty(urlAccess))
                return false;

            var isValidName = Regex.IsMatch(imageName, @"^[a-zA-Z0-9-_()\s]+\.(png|jpg|gif)$");
            var isValidUrl = Regex.IsMatch(urlAccess, @"^(https?|ftp):\/\/[^\s/$.?#].*$");

            return isValidName && isValidUrl;
        }
    }
}
