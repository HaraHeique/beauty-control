﻿using BeautyControl.API.Features._Common.Contracts;
using BeautyControl.API.Infra.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace BeautyControl.API.Features.Suppliers.UpdateSupplier
{
    public class DataAccess : IInjectableService
    {
        private readonly AppDataContext _context;

        public DataAccess(AppDataContext context) => _context = context;

        //public virtual async Task<bool> ExistsSupplier(int id)
        //{
        //    using var dbConnection = _context.Database.GetDbConnection();

        //    var @params = new { Id = id };

        //    return await dbConnection.QueryFirstAsync<int>(@"
        //        SELECT COUNT(*) FROM [BeautyControl].[Business].[Suppliers] WHERE Id = @Id;
        //    ", @params) > 0;
        //}

        public virtual async Task<int> UpdateSupplier(Command request)
        {
            using var dbConnection = _context.Database.GetDbConnection();

            var @params = new
            {
                request.Id,
                request.Name,
                request.Observation,
                request.Telephone
            };

            return await dbConnection.ExecuteAsync(@"
                UPDATE [BeautyControl].[Business].[Suppliers]
                SET [Name] = @Name,
                    [Observation] = @Observation,
                    [Telephone] = @Telephone
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
