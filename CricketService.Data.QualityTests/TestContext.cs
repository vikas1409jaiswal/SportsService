using CricketService.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CricketService.Data.QualityTests
{
    public class TestContext : IDisposable
    {
        private const string ConnectionString =
            "Host=localhost;Port=5433;Username=postgres;Password=admin;Database=cricket_database";

        public CricketServiceContext DbContext { get; private set; }

        public HttpClient TestHttpClient { get; private set; } 

        public TestContext()
        {
            var options = new DbContextOptionsBuilder<CricketServiceContext>()
                .UseNpgsql(ConnectionString)
                .Options;

            DbContext = new CricketServiceContext(options);

            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            DbContext?.Dispose();
            TestHttpClient?.Dispose();
        }
    }
}