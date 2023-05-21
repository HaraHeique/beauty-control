namespace BeautyControl.API.Infra.Data.SeedData
{
    public static class ProductSeedData
    {
        public static string GetScriptSQL()
        {
            return @"
                INSERT INTO [Business].[Products] ([Name], [Description], [ImageName], [ImageUrl], [Quantity], [RunningOutOfStock], [StatusStock], [Category], [CreationDate])
                VALUES
                    ('Lápis de Sobrancelha', 'Perfeito para preencher e definir suas sobrancelhas, criando um visual natural e elegante.', NULL, NULL, 0, 20, 0, 1, GETDATE()),
                    ('Máscara de Cílios', 'Dê um destaque incrível aos seus cílios com nossa máscara de volume, proporcionando um olhar deslumbrante.', NULL, NULL, 0, 30, 0, 1, GETDATE()),
                    ('Batom Matte', 'Tenha lábios perfeitos com nosso batom matte de longa duração e cores vibrantes.', NULL, NULL, 0, 40, 0, 1, GETDATE()),
                    ('Paleta de Sombras', 'Crie looks deslumbrantes com nossa paleta de sombras de alta pigmentação e diversas tonalidades.', NULL, NULL, 0, 50, 0, 1, GETDATE()),
                    ('Base Líquida', 'Obtenha uma pele impecável com nossa base líquida de cobertura média, deixando uma aparência natural.', NULL, NULL, 0, 60, 0, 2, GETDATE()),
                    ('Blush Compacto', 'Realce suas bochechas com nosso blush compacto, proporcionando um brilho saudável e radiante.', NULL, NULL, 0, 70, 0, 1, GETDATE()),
                    ('Pó Translúcido', 'Fixe sua maquiagem com nosso pó translúcido, controlando o brilho e deixando a pele aveludada.', NULL, NULL, 0, 80, 0, 2, GETDATE()),
                    ('Esmalte', 'Tenha unhas deslumbrantes com nossos esmaltes de alta qualidade e cores variadas.', NULL, NULL, 0, 90, 0, 4, GETDATE()),
                    ('Pincéis de Maquiagem', 'Aplique sua maquiagem com perfeição usando nossos pincéis de maquiagem macios e duráveis.', NULL, NULL, 0, 100, 0, 1, GETDATE()),
                    ('Batom Líquido Matte', 'Tenha lábios irresistíveis com nosso batom líquido matte de longa duração e acabamento impecável.', NULL, NULL, 0, 110, 0, 1, GETDATE());
            ";
        }
    }
}
