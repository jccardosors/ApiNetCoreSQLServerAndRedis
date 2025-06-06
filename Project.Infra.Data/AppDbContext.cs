using Microsoft.EntityFrameworkCore;
using Project.Domain.Entities;
using Project.Infra.Data.Mappers;

namespace Project.Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Entry> entries { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Logs> logs { get; set; }
        public DbSet<ControlUserAccess> controlUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var userIntial = new User
            {
                Id = 1,
                Email = "teste@gmail.com",
                Password = "senha123",
            };

            modelBuilder.Entity<User>().HasData(userIntial);

            modelBuilder.ApplyConfiguration(new EntryMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new LogsMap());
            modelBuilder.ApplyConfiguration(new ControlUserAccessMap());          

            base.OnModelCreating(modelBuilder);
        }
    }
}
