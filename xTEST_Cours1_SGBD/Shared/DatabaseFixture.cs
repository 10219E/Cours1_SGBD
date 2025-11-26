using System.Threading.Tasks;
using Testcontainers.MsSql;
using Dapper;
using Xunit;

namespace xTEST_Cours1_SGBD.Shared
{
    /// <summary>
    /// Shared database fixture using a single Testcontainers SQL Server instance.
    /// Tests that belong to the same xUnit collection will reuse this container.
    /// </summary>
    public class DatabaseFixture : IAsyncLifetime
    {
        private MsSqlContainer _container;
        private DBSetup _dbSetup;
        public string ConnectionString { get; private set; } = string.Empty;

        // Optional direct access if needed (internal to avoid accessibility mismatch)
        internal DBSetup Setup => _dbSetup;

        public async Task InitializeAsync()
        {
            _container = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPassword("yourStrong(!)Password")
                .Build();
            await _container.StartAsync();

            _dbSetup = new DBSetup(_container.GetConnectionString());
            await _dbSetup.InitializeDataBase();
            await _dbSetup.CreateTablesDataBase();
            ConnectionString = _container.GetConnectionString().Replace("master", "xTEST_SGBD_C");
            // initial seed so read tests can run immediately
            await _dbSetup.SeedStudents();
        }

        public async Task DisposeAsync()
        {
            if (_container != null)
            {
                await _container.StopAsync();
                await _container.DisposeAsync();
            }
        }

        // Public convenience method for tests.
        public Task SeedStudents() => _dbSetup.SeedStudents();
        public Task SeedStudio() => _dbSetup.SeedStudio();
    }

    /// <summary>
    /// Collection definition so all tests referencing the name share <see cref="DatabaseFixture"/>.
    /// xUnit runs tests in the same collection sequentially, avoiding concurrent mutations.
    /// </summary>
    [CollectionDefinition("DbCollection")]
    public class DbCollection : ICollectionFixture<DatabaseFixture> { }
}
