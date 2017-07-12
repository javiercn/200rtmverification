using EntityFrameworkVerificationApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EntityFrameworkVerificationApp.Tests
{
    public class BasicTests
    {
        [Fact]
        public async Task CanCreateDbAsync()
        {
            var context = CreateDbContext<CatalogContext>();
            await context.Database.EnsureCreatedAsync();
        }

        public TContext CreateDbContext<TContext>(string dataBaseName = null)
            where TContext : DbContext
        {
            var services = new ServiceCollection();
            services.AddDbContext<TContext>(options => 
                options.UseSqlServer(GenerateConnectionString(dataBaseName)));
            var provider = services.BuildServiceProvider();
            return provider.GetRequiredService<TContext>();
        }

        private string GenerateConnectionString(string databaseName) => $"Server=(localdb)\\mssqllocaldb;Database={databaseName ?? "EntityFrameworkVerificationApp-" + Guid.NewGuid()};Trusted_Connection=True;MultipleActiveResultSets=true";
    }
}
