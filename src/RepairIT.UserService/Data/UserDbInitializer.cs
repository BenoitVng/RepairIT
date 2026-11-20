using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RepairIT.UserService.Configuration;

namespace RepairIT.UserService.Data;

public class UserDbInitializer
{
    private readonly UserDbContext _context;
    private readonly DatabaseOptions _options;
    private readonly ILogger<UserDbInitializer> _logger;

    public UserDbInitializer(UserDbContext context, IOptions<DatabaseOptions> options, ILogger<UserDbInitializer> logger)
    {
        _context = context;
        _options = options.Value;
        _logger = logger;
    }

    public void Initialiser()
    {
        CreerBase();
        if (!_options.SeedOnStartup)
        {
            _logger.LogInformation("Initialisation des données désactivée par la configuration.");
            return;
        }
        if (_context.Utilisateurs.Any())
        {
            _logger.LogInformation("La base contient déjà des utilisateurs, aucune donnée ajoutée.");
            return;
        }
        _context.Utilisateurs.AddRange(UserSeedData.Utilisateurs());
        _context.SaveChanges();
        _logger.LogInformation("Utilisateurs de démonstration créés (1 administrateur, 4 gestionnaires).");
    }

    private void CreerBase()
    {
        if (_context.Database.IsRelational())
        {
            _context.Database.Migrate();
            return;
        }
        _context.Database.EnsureCreated();
    }
}
