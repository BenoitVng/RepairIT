using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Data;

public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
    }

    public DbSet<Ordinateur> Ordinateurs => Set<Ordinateur>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var ordinateur = modelBuilder.Entity<Ordinateur>();
        ordinateur.ToTable("Ordinateurs");
        ordinateur.HasKey(o => o.Id);
        ordinateur.Property(o => o.Marque).IsRequired().HasMaxLength(50);
        ordinateur.Property(o => o.Modele).IsRequired().HasMaxLength(100);
        ordinateur.Property(o => o.Processeur).IsRequired().HasMaxLength(100);
        ordinateur.Property(o => o.Type).HasConversion<string>().HasMaxLength(20);
        ordinateur.Property(o => o.Etat).HasConversion<string>().HasMaxLength(20);
        ordinateur.Property(o => o.Prix).HasPrecision(10, 2);
    }
}
