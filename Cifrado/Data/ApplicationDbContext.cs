using Cifrado.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cifrado.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Encriptado> Encriptados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Encriptado>()
                .HasKey(e => e.IdEncriptado);


            modelBuilder.Entity<Encriptado>()
                .Property(e => e.IdEncriptado)
                .ValueGeneratedOnAdd();


            modelBuilder.Entity<Encriptado>()
                .Property(e => e.btFecha)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Encriptado>()
                .HasIndex(e => e.KeyEncript)
                .IsUnique();
            modelBuilder.Entity<Encriptado>()
                .Property(e => e.TextEncript)
                .HasColumnType("nvarchar(MAX)");

            // Configurar KeyEncript como nvarchar(MAX)
            modelBuilder.Entity<Encriptado>()
                .Property(e => e.KeyEncript)
                .HasColumnType("nvarchar(MAX)");
        }
    }
}
