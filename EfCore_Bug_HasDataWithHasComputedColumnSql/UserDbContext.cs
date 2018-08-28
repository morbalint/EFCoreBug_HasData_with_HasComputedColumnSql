using Microsoft.EntityFrameworkCore;

namespace EfCore_Bug_HasDataWithHasComputedColumnSql
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().Property(x => x.FullName).HasComputedColumnSql(
                $"[{nameof(User.FirstName)}] + ' ' + [{nameof(User.LastName)}]");

            builder.Entity<User>().HasData(new User
            {
                Id = long.MinValue,
                FirstName = "John",
                LastName = "Doe",
            });
        }
    }
}
