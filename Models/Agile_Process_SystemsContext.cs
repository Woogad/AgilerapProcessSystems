using Microsoft.EntityFrameworkCore;

namespace AgilerapProcessSystems.Models
{
    public class Agile_Process_SystemsContext : DbContext
    {
        public DbSet<Work> Work { get; set; }
        public DbSet<WorkLog> WorkLog { get; set; }
        public DbSet<Provider> Provider { get; set; }
        public DbSet<ProviderLog> ProviderLog { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<User> User { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-RUUSDFN\SQLEXPRESS;Initial Catalog=Agilerap_Process_Systems;Integrated Security=True;Persist Security Info=False;Pooling=False;Multiple Active Result Sets=False;Encrypt=True;Trust Server Certificate=True;Command Timeout=0");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Provider>().HasOne(p => p.UpdateBy).WithMany().HasForeignKey(p => p.UpdateByID);
            modelBuilder.Entity<Provider>().HasOne(p => p.CreateBy).WithMany().HasForeignKey(p => p.CreateByID);
            modelBuilder.Entity<ProviderLog>().HasOne(p => p.UpdateBy).WithMany().HasForeignKey(p => p.UpdateByID);
            modelBuilder.Entity<ProviderLog>().HasOne(p => p.CreateBy).WithMany().HasForeignKey(p => p.CreateByID);
            modelBuilder.Entity<Work>().HasOne(p => p.UpdateBy).WithMany().HasForeignKey(p => p.UpdateByID);
            modelBuilder.Entity<Work>().HasOne(p => p.CreateBy).WithMany().HasForeignKey(p => p.CreateByID);
            modelBuilder.Entity<WorkLog>().HasOne(p => p.UpdateBy).WithMany().HasForeignKey(p => p.UpdateByID);
            modelBuilder.Entity<WorkLog>().HasOne(p => p.CreateBy).WithMany().HasForeignKey(p => p.CreateByID);
            base.OnModelCreating(modelBuilder);
        }
    }
}
