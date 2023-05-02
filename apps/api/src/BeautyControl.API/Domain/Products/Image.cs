using BeautyControl.API.Domain._Common;
using BeautyControl.API.Domain._Common.Exceptions;
using System.Text.RegularExpressions;

namespace BeautyControl.API.Domain.Products
{
    public record class Image : IValueObject
    {
        public string Name { get; set; }
        public string Url { get; set; }

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
