using CRM.Domain.Companies;
using CRM.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CRM.Web.Infrastructure
{
    public class CrmContext : DbContext
    {
        public CrmContext(DbContextOptions<CrmContext> options) : base(options)
        {
        }
        
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().ToTable("companies");
            modelBuilder.Entity<Company>().Property(c => c.Id).HasColumnName("id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Company>().Property(c => c.DomainName).HasColumnName("domain_name");
            modelBuilder.Entity<Company>().Property(c => c.Active).HasColumnName("active");
            modelBuilder.Entity<Company>().Property(c => c.NumberOfEmployees).HasColumnName("number_of_employees");
            
            modelBuilder.Entity<Company>(builder =>
            {
                builder.HasIndex(x => x.Id).IsUnique();
                builder.HasIndex(x => x.DomainName).IsUnique();
                builder.Ignore(x => x.PendingEvents);
            });
            

            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<User>().Property(u => u.Id).HasColumnName("id").ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(u => u.Email).HasColumnName("email");
            modelBuilder.Entity<User>().Property(u => u.Type).HasColumnName("type");
            modelBuilder.Entity<User>().Property(u => u.IsEmailConfirmed).HasColumnName("is_email_confirmed");
            
            modelBuilder.Entity<User>(builder =>
            {
                builder.HasIndex(x => x.Id).IsUnique();
                builder.Ignore(x => x.PendingEvents);
            });

            
            base.OnModelCreating(modelBuilder);
        }
    }
}