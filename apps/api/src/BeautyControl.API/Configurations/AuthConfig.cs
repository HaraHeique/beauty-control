namespace BeautyControl.API.Configurations
{
    public static class AuthConfig
    {
        public static void AddAuthConfiguration(this WebApplicationBuilder builder)
        {
            // COLOCAR A CONFIGURAÇÃO DO JWT COM CHAVE SIMÉTRICA AQUI USANDO O PACOTE:
            // Mais simples: https://github.com/NetDevPack/Security.Identity
            // Mais completo e complexo: https://github.com/NetDevPack/Security.Jwt
        }

        public static void UseAuthConfiguration(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
