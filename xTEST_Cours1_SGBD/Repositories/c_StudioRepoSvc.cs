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
    public class c_StudioRepoSvc
    {
        private readonly DatabaseFixture _fixture;

        public c_StudioRepoSvc(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task x_TestGetStudioDb()
        {
            await _fixture.SeedStudio(); // seed with initial data

            // quick connection check (optional) — still part of Arrange
            await using var connection = new SqlConnection(_fixture.ConnectionString);
            await connection.OpenAsync();

            // Act: create the repo using the test connection string so it targets the Docker DB only
            var logger = LoggerFactory.Create(builder => { /* optional config */ });
            var repo = new StudioRepoSvc(logger, _fixture.ConnectionString);

            // Call the method under test (Act)
            var studio = repo.GetStudioDb();

            // Assert: validate expected result(s)
            Assert.NotNull(studio);   
            Assert.NotEmpty(studio);
            Assert.Equal(4, studio.Count());  //4 Because only four students are mapped to a studio !

        }
    }
}
