using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModelsDLL.Models;
using RepositoryDLL.RepoSvc;
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

        // AAA pattern is used below: Arrange / Act / Assert
        [Fact]
        public async Task x_TestGetStudentsDb()
        {
            // Arrange: start a disposable SQL Server container and prepare the database
            _container = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPassword("yourStrong(!)Password")
                //.WithAcceptLicenseAgreement(true)
                .Build();
            await _container.StartAsync();

            // DB setup helper uses the container connection string to create DB/tables
            DBSetup dbSetup = new DBSetup(_container.GetConnectionString());

            await dbSetup.InitializeDataBase(); // create the test database
            await dbSetup.CreateTablesDataBase(); // create tables
            await dbSetup.SeedDataBase(); // seed with initial data

            try
            {
                // Build a connection string that points to the test database instead of master
                var connectionString = (_container.GetConnectionString()).Replace("master", "xTEST_SGBD_C");

                // quick connection check (optional) — still part of Arrange
                await using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                // Act: create the repo using the test connection string so it targets the Docker DB only
                var logger = LoggerFactory.Create(builder => { /* optional config */ });
                var repo = new RepoSvc(logger, connectionString);

                // Call the method under test (Act)
                var students = repo.GetStudentsDb();

                // Assert: validate expected result(s)
                Assert.NotNull(students);   
                Assert.NotEmpty(students);
                Assert.Equal(5, students.Count());  

                // Cleanup: close connection and dispose container
                await connection.CloseAsync();
                await _container.DisposeAsync();
            }
            catch (Exception ex)
            {
                // Ensure container is disposed on failure and rethrow so the test fails
                await _container.DisposeAsync();
                Console.WriteLine($"Error connecting to the database: {ex.Message}");
                throw;
            }
        }


        [Theory]
        [InlineData("Robert", 2)]
        [InlineData("Paul", 1)]
        [InlineData("NonExistingName", 0)]
        public async Task x_TestFindStudentDb(string search, int expect_count)
        {
            _container = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPassword("yourStrong(!)Password")
                .Build();
            await _container.StartAsync();

            DBSetup dbSetup = new DBSetup(_container.GetConnectionString());

            await dbSetup.InitializeDataBase(); // create the test database
            await dbSetup.CreateTablesDataBase(); // create tables
            await dbSetup.SeedDataBase(); // seed with initial data

            try
            {
                var connectionString = (_container.GetConnectionString()).Replace("master", "xTEST_SGBD_C");

                await using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                var logger = LoggerFactory.Create(builder => { /* optional config */ });
                var repo = new RepoSvc(logger, connectionString);

                var students = repo.FindStudentDb(search); //need to pass search parameter

                Assert.Equal(expect_count, students.Count());

                await connection.CloseAsync();
                await _container.DisposeAsync();
            }
            catch (Exception ex)
            {
                await _container.DisposeAsync();
                Console.WriteLine($"Error connecting to the database: {ex.Message}");
                throw;
            }
        }

        [Fact]
        public async Task x_TestUpdateStudentDb()
        {
            //Arrange
            var id_toupdate = 002;
            var updatedStudent = new StudentUpdate
            {
                lname = "Mark"
            };


            _container = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPassword("yourStrong(!)Password")
                .Build();
            await _container.StartAsync();

            DBSetup dbSetup = new DBSetup(_container.GetConnectionString());

            await dbSetup.InitializeDataBase(); // create the test database
            await dbSetup.CreateTablesDataBase(); // create tables
            await dbSetup.SeedDataBase(); // seed with initial data

            try
            {
                var connectionString = (_container.GetConnectionString()).Replace("master", "xTEST_SGBD_C");

                await using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                var logger = LoggerFactory.Create(builder => { /* optional config */ });
                var repo = new RepoSvc(logger, connectionString);

                // Act: perform update on real repo
                repo.UpdateStudentDb(id_toupdate, updatedStudent);

                // Act: read back the updated student (FindStudentDb accepts string search)
                var updated = repo.FindStudentDb(id_toupdate.ToString());

                // Assert: stable checks by id, not by index
                Assert.Single(updated);
                Assert.Equal(id_toupdate, updated[0].id);
                Assert.Equal("Mark", updated[0].lname);

                await connection.CloseAsync();
                await _container.DisposeAsync();
            }
            catch (Exception ex)
            {
                await _container.DisposeAsync();
                Console.WriteLine($"Error connecting to the database: {ex.Message}");
                throw;
            }
        }

        [Theory]
        [InlineData("Alice", "Smith", "asmith@recov.org", "MB3", "+22918")]
        [InlineData("Nikiya", "De Kat", "ndk@cat.meow", "IT2", "")]
        public async Task x_InsertStudentDb(string fname, string lname, string email, string section, string phone)
        {
            var insert = new StudentsToInsert //Based on Model (in project) StudentsToInsert
            {
                fname = fname.Trim(),
                lname = lname.Trim(),
                email = email.Trim(),
                section = section.Trim(),
                phone = phone.Trim(),
                confirmed = DateTime.Now
            };


            _container = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPassword("yourStrong(!)Password")
                .Build();
            await _container.StartAsync();

            DBSetup dbSetup = new DBSetup(_container.GetConnectionString());

            await dbSetup.InitializeDataBase(); // create the test database
            await dbSetup.CreateTablesDataBase(); // create tables
            await dbSetup.SeedDataBase(); // seed with initial data

            try
            {
                var connectionString = (_container.GetConnectionString()).Replace("master", "xTEST_SGBD_C");

                await using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                var logger = LoggerFactory.Create(builder => { /* optional config */ });
                var repo = new RepoSvc(logger, connectionString);

                repo.InsertStudentDb(insert); //need to pass object to insert

                // Act: read back the updated student (FindStudentDb accepts string search)
                var verif = repo.FindStudentDb(insert.fname);

                // Assert: stable checks by id, not by index
                Assert.Single(verif);
                Assert.Equal(insert.fname, verif[0].fname);
                Assert.Equal(insert.email, verif[0].email);
                Assert.Equal(insert.lname, verif[0].lname);
                Assert.Equal(insert.phone, verif[0].phone);
            }
            catch (Exception ex)
            {
                await _container.DisposeAsync();
                Console.WriteLine($"Error connecting to the database: {ex.Message}");
                throw;
            }
        }

        [Theory]
        [InlineData(002)]
        [InlineData(001)]
        public async Task x_DeleteStudentDb(int id)
        {

            _container = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPassword("yourStrong(!)Password")
                .Build();
            await _container.StartAsync();

            DBSetup dbSetup = new DBSetup(_container.GetConnectionString());

            await dbSetup.InitializeDataBase(); // create the test database
            await dbSetup.CreateTablesDataBase(); // create tables
            await dbSetup.SeedDataBase(); // seed with initial data

            try
            {
                var connectionString = (_container.GetConnectionString()).Replace("master", "xTEST_SGBD_C");

                await using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                var logger = LoggerFactory.Create(builder => { /* optional config */ });
                var repo = new RepoSvc(logger, connectionString);

                repo.DeleteStudentDb(id); //need to pass id to delete

                var idstr = id.ToString(); //converting to string for search

                // Act: read back the updated student (FindStudentDb accepts string search)
                var verif = repo.FindStudentDb(idstr);

                // Assert: stable checks by id, not by index
                Assert.Empty(verif);
                Assert.Equal(0, verif.Count());

            }
            catch (Exception ex)
            {
                await _container.DisposeAsync();
                Console.WriteLine($"Error connecting to the database: {ex.Message}");
                throw;
            }
        }
    }
}
