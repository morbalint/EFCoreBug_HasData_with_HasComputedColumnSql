using System.IO;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EfCore_Bug_HasDataWithHasComputedColumnSql
{
    public class DesignTimeUserDbContextFactory :
        IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile("appsettings.Development.json", true)
                .AddCommandLine(args)
                .Build();

            return new UserDbContext(new DbContextOptionsBuilder<UserDbContext>()
                .UseSqlServer(configuration.GetConnectionString("EFCoreBugTest"))
                .Options);
        }

        private static readonly Regex connectionStringReplace =
            new Regex("Database=([^;]*);",
            RegexOptions.Singleline | RegexOptions.Compiled);

        public static UserDbContext CreateInMemoryDbContext(string name) 
            => new UserDbContext(new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(name)
                .Options);

        public static UserDbContext CreateSqlServerDbContext(string name
            = "EFCoreBugTest")
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile("appsettings.Development.json", true)
                .Build();
            var connStr = configuration.GetConnectionString("EFCoreBugTest");
            connStr = connectionStringReplace.Replace(connStr,
                $"Database={name};");

            return new UserDbContext(new DbContextOptionsBuilder<UserDbContext>()
                .UseSqlServer(connStr)
                .Options);
        }

    }
}
