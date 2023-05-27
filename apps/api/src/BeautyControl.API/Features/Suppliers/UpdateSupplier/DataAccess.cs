using Dapper;
using System.Data;

namespace BeautyControl.API.Features.Suppliers.UpdateSupplier
{
    public class DataAccess
    {
        public static async Task UpdateSupplier(IDbConnection dbConnection, Command request)
        {
            var @params = new
            {
                request.Id,
                request.Name,
                request.Observation,
                request.Telephone
            };

            await dbConnection.ExecuteAsync(@"
                UPDATE [BeautyControl].[Business].[Suppliers]
                SET [Name] = @Name,
                    [Observation] = @Observation,
                    [Telephone] = @Telephone
                 WHERE Id = @Id
            ", @params);
        }
    }
}
