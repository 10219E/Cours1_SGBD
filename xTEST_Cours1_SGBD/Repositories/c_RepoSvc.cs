using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging.Abstractions;
using ModelsDLL.Models;
using RepositoryDLL.RepoSvc;
using Xunit;
using xTEST_Cours1_SGBD.Shared;
using Microsoft.Extensions.Logging;

namespace xTEST_Cours1_SGBD.Repositories
{
    [Collection("DbCollection")]
    public class c_RepoSvc
    {
        private readonly DatabaseFixture _fixture;

        public c_RepoSvc(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task x_TestGetStudentsDb()
        {
            await _fixture.SeedDataBase(); // seed with initial data

            // quick connection check (optional) — still part of Arrange
            await using var connection = new SqlConnection(_fixture.ConnectionString);
            await connection.OpenAsync();

            // Act: create the repo using the test connection string so it targets the Docker DB only
            var logger = LoggerFactory.Create(builder => { /* optional config */ });
            var repo = new RepoSvc(logger, _fixture.ConnectionString);

            // Call the method under test (Act)
            var students = repo.GetStudentsDb();

            // Assert: validate expected result(s)
            Assert.NotNull(students);   
            Assert.NotEmpty(students);
            Assert.Equal(5, students.Count());  

        }


        [Theory]
        [InlineData("Robert", 2)]
        [InlineData("Paul", 1)]
        [InlineData("NonExistingName", 0)]
        public async Task x_TestFindStudentDb(string search, int expect_count)
        {
            await _fixture.SeedDataBase(); // seed with initial data

            // quick connection check (optional) — still part of Arrange
            await using var connection = new SqlConnection(_fixture.ConnectionString);
            await connection.OpenAsync();

            var logger = LoggerFactory.Create(builder => { /* optional config */ });
            var repo = new RepoSvc(logger, _fixture.ConnectionString);

            var students = repo.FindStudentDb(search); //need to pass search parameter

            Assert.Equal(expect_count, students.Count());
            
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

            await _fixture.SeedDataBase(); // seed with initial data

            // quick connection check (optional) — still part of Arrange
            await using var connection = new SqlConnection(_fixture.ConnectionString);
            await connection.OpenAsync();

            var logger = LoggerFactory.Create(builder => { /* optional config */ });
            var repo = new RepoSvc(logger, _fixture.ConnectionString);

            // Act: perform update on real repo
            repo.UpdateStudentDb(id_toupdate, updatedStudent);

            // Act: read back the updated student (FindStudentDb accepts string search)
            var updated = repo.FindStudentDb(id_toupdate.ToString());

            // Assert: stable checks by id, not by index
            Assert.Single(updated);
            Assert.Equal(id_toupdate, updated[0].id);
            Assert.Equal("Mark", updated[0].lname);

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

            await _fixture.SeedDataBase(); // seed with initial data

            // quick connection check (optional) — still part of Arrange
            await using var connection = new SqlConnection(_fixture.ConnectionString);
            await connection.OpenAsync();

            var logger = LoggerFactory.Create(builder => { /* optional config */ });
            var repo = new RepoSvc(logger, _fixture.ConnectionString);

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

        [Theory]
        [InlineData(002)]
        [InlineData(001)]
        public async Task x_DeleteStudentDb(int id)
        {

            await _fixture.SeedDataBase(); // seed with initial data

            // quick connection check (optional) — still part of Arrange
            await using var connection = new SqlConnection(_fixture.ConnectionString);
            await connection.OpenAsync();

            var logger = LoggerFactory.Create(builder => { /* optional config */ });
            var repo = new RepoSvc(logger, _fixture.ConnectionString);

            repo.DeleteStudentDb(id); //need to pass id to delete

            var idstr = id.ToString(); //converting to string for search

            // Act: read back the updated student (FindStudentDb accepts string search)
            var verif = repo.FindStudentDb(idstr);

            // Assert: stable checks by id, not by index
            Assert.Empty(verif);
            Assert.Equal(0, verif.Count());


        }
    }
}
