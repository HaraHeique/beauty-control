using BeautyControl.API.Domain._Common.ValueObjects;
using BeautyControl.API.Domain.Suppliers;
using BeautyControl.API.Features._Common.Contracts;
using BeautyControl.API.Infra.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BeautyControl.API.Features.Suppliers.UpdateSupplier
{
    public class DataAccess : IInjectableService
    {
        private readonly AppDataContext _context;

        public DataAccess(AppDataContext context) => _context = context;

        public virtual async Task<int> UpdateSupplier(Command request)
        {
            using var dbConnection = _context.Database.GetDbConnection();

            var @params = new
            {
                request.Id,
                request.Name,
                request.Observation,
                Telephones = JsonConvert.SerializeObject(Telephone.Create(request.Telephones)),
                Emails = JsonConvert.SerializeObject(Email.Create(request.Emails))
            };

            return await dbConnection.ExecuteAsync(@"
                UPDATE [BeautyControl].[Business].[Suppliers]
                SET [Name] = @Name,
                    [Observation] = @Observation,
                    [Telephones] = @Telephones,
                    [Emails] = @Emails
                WHERE Id = @Id
            ", @params);
        }

        //public static async Task UpdateSupplier(IDbConnection dbConnection, Command request)
        //{
        //    var @params = new
        //    {
        //        request.Id,
        //        request.Name,
        //        request.Observation,
        //        request.Telephone
        //    };

        //    await dbConnection.ExecuteAsync(@"
        //        UPDATE [BeautyControl].[Business].[Suppliers]
        //        SET [Name] = @Name,
        //            [Observation] = @Observation,
        //            [Telephone] = @Telephone
        //         WHERE Id = @Id
        //    ", @params);
        //}
    }
}
