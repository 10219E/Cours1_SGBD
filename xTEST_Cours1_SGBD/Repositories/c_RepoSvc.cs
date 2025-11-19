using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;
using xTEST_Cours1_SGBD.Shared;

namespace xTEST_Cours1_SGBD.Repositories
{
    public class c_RepoSvc //: IAsyncLifetime
    {
        private MsSqlContainer _container;

        public Task DisposeAsync()
        {
            return _container.DisposeAsync().AsTask();
        }

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }


        [Fact]
        public async Task x_TestDBSetup()
            {
            _container = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPassword("yourStrong(!)Password")
                //.WithAcceptLicenseAgreement(true)
                .Build();
            await _container.StartAsync();

            DBSetup dbSetup = new DBSetup(_container.GetConnectionString());

            await dbSetup.InitializeDataBase();

            await dbSetup.CreateTablesDataBase();

            try
            {
                await using var connection = new SqlConnection(_container.GetConnectionString());
                await connection.OpenAsync();
            }
            catch (Exception ex)
            {
                await _container.DisposeAsync();    
                Console.WriteLine($"Error connecting to the database: {ex.Message}");
            }

        }



    }
}
