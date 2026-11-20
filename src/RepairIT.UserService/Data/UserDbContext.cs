using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Models;

namespace RepairIT.UserService.Data;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var utilisateur = modelBuilder.Entity<Utilisateur>();
        utilisateur.ToTable("Utilisateurs");
        utilisateur.HasKey(u => u.Id);
        utilisateur.Property(u => u.UserKeycloak).IsRequired().HasMaxLength(100);
        utilisateur.Property(u => u.Nom).IsRequired().HasMaxLength(50);
        utilisateur.Property(u => u.Prenom).IsRequired().HasMaxLength(50);
        utilisateur.Property(u => u.Email).IsRequired().HasMaxLength(150);
        utilisateur.HasIndex(u => u.Email).IsUnique();
        utilisateur.Property(u => u.Role).HasConversion<string>().HasMaxLength(20);
    }
}
