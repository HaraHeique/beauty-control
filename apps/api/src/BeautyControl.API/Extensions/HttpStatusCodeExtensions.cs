using System.Net;

namespace BeautyControl.API.Extensions
{
    public static class HttpStatusCodeExtensions
    {
        public static int ToInt(this HttpStatusCode statusCode) => (int)statusCode;

        public static bool IsSuccessful(this HttpStatusCode statusCode)
        {
            var castedValue = statusCode.ToInt();

            return castedValue >= 200 && castedValue <= 299;
        }

        public static bool IsFailed(this HttpStatusCode statusCode)
            => statusCode.IsSuccessful() == false;
    }
}
