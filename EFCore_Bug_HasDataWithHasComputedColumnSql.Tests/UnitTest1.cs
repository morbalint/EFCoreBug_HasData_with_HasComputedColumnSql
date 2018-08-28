using System;
using System.Threading.Tasks;

using EfCore_Bug_HasDataWithHasComputedColumnSql;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EFCore_Bug_HasDataWithHasComputedColumnSql.Tests
{
    public class InMemoryDbTests : IAsyncLifetime 
    {
        private UserDbContext context;

        public async Task InitializeAsync()
        {
            context = DesignTimeUserDbContextFactory.CreateInMemoryDbContext(
                Guid.NewGuid().ToString());
            await context.Database.EnsureCreatedAsync().ConfigureAwait(false);
        }

        public async Task DisposeAsync() => await (context?.Database.EnsureDeletedAsync() ?? Task.CompletedTask).ConfigureAwait(false);

        [Fact]
        public async Task EnsureCreated()
        {
            
        }

        [Fact]
        public async Task ContainsTestData()
        {
            // Act
            var result = await context.Users.FindAsync(long.MinValue)
                .ConfigureAwait(false);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task FullNameIsComputed()
        {
            // Act
            var result = await context.Users.FindAsync(long.MinValue);

            // Assert
            Assert.Equal($"{result.FirstName} {result.LastName}", result.FullName);
        }
    }

    public class SqlServerDbTests : IAsyncLifetime
    {
        private UserDbContext context;

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync() => 
            await (context?.Database.EnsureDeletedAsync() ??            
                    Task.CompletedTask).ConfigureAwait(false);

        [Fact]
        public async Task EnsureCreated()
        {
            // Arrange
            context = 
                DesignTimeUserDbContextFactory.CreateSqlServerDbContext("create_test");
            await context.Database.EnsureDeletedAsync().ConfigureAwait(false);

            // Act
            await context.Database.EnsureCreatedAsync().ConfigureAwait(false);
        }

        [Fact]
        public async Task Migrate()
        {
            // Arrange
            context = 
                DesignTimeUserDbContextFactory.CreateSqlServerDbContext("migrate_test");

            // Act
            await context.Database.MigrateAsync().ConfigureAwait(false);
        }

    }

}
