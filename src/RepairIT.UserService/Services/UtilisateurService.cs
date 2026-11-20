using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Models;
using RepairIT.UserService.Data;

namespace RepairIT.UserService.Services;

public class UtilisateurService : IUtilisateurService
{
    private readonly UserDbContext _context;
    private readonly ILogger<UtilisateurService> _logger;

    public UtilisateurService(UserDbContext context, ILogger<UtilisateurService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Utilisateur>> ListerAsync()
    {
        return await _context.Utilisateurs.AsNoTracking().OrderBy(u => u.Nom).ThenBy(u => u.Prenom).ToListAsync();
    }

    public async Task<Utilisateur?> TrouverParIdAsync(int id)
    {
        return await _context.Utilisateurs.FindAsync(id);
    }

    public async Task<Utilisateur?> CreerAsync(UtilisateurRequest request)
    {
        var email = request.Email.Trim().ToLower();
        if (await _context.Utilisateurs.AnyAsync(u => u.Email == email))
        {
            _logger.LogWarning("Création refusée : l'email {Email} est déjà utilisé", email);
            return null;
        }
        var utilisateur = new Utilisateur
        {
            Nom = request.Nom.Trim(),
            Prenom = request.Prenom.Trim(),
            Email = email,
            Role = request.Role,
            EstActif = true,
            UserKeycloak = Utilisateur.UserKeycloakParDefaut,
            DateCreation = DateTime.UtcNow
        };
        _context.Utilisateurs.Add(utilisateur);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Utilisateur {Id} créé avec le rôle {Role}", utilisateur.Id, utilisateur.Role);
        return utilisateur;
    }
}
