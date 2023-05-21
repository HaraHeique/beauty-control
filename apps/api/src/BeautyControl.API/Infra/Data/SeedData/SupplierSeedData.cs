namespace BeautyControl.API.Infra.Data.SeedData
{
    public static class SupplierSeedData
    {
        public static string GetScriptSQL()
        {
            return @"
                INSERT INTO [Business].[Suppliers] (Name, Observation, Telephone, AverageRating, CreationDate)
                VALUES
                    ('Natura', 'Natura é uma empresa brasileira de cosméticos, perfumaria e produtos de higiene pessoal. Com um compromisso com a sustentabilidade e o uso de ingredientes naturais, a Natura oferece uma ampla gama de produtos de beleza que atendem às necessidades dos consumidores.', '11123456789', 0, GETDATE()),
                    ('O Boticário', 'O Boticário é uma renomada marca brasileira de perfumaria e cosméticos. Com uma ampla variedade de produtos para cuidados pessoais e fragrâncias, O Boticário destaca-se pela qualidade e inovação, conquistando a confiança dos consumidores.', '22987654321', 0, GETDATE()),
                    ('Embelleze', 'Embelleze é uma empresa brasileira especializada em produtos para cabelos. Com uma linha diversificada de shampoos, condicionadores, tinturas e tratamentos capilares, a Embelleze oferece soluções para diversos tipos de cabelo e necessidades.', '33456789012', 0, GETDATE()),
                    ('Granado', 'Granado é uma marca brasileira conhecida por seus produtos tradicionais de cuidados pessoais. Com mais de um século de história, a Granado oferece sabonetes, cremes e outros itens de higiene com fórmulas suaves e ingredientes de alta qualidade.', '44890123456', 0, GETDATE()),
                    ('Salon Line', 'Salon Line é uma marca brasileira especializada em produtos para cabelos, principalmente para cabelos cacheados e afro. Com uma linha completa de produtos para tratamento, estilização e finalização, a Salon Line busca atender às necessidades específicas desse público.', '55567890123', 0, GETDATE());
            ";
        }
    }
}
