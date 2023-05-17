namespace BeautyControl.API.Features._Common.Endpoints
{
    public static class Routes
    {
        private const string baseUri = "api/v{version:apiVersion}";

        public const string AccountUri = $"{baseUri}/account";
        public const string ProductsUri = $"{baseUri}/products";
        public const string SuppliersUri = $"{baseUri}/suppliers";
    }
}
