using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RepairIT.CatalogService.Configuration;

namespace RepairIT.CatalogService.Data;

public class CatalogDbInitializer
{
    private readonly CatalogDbContext _context;
    private readonly DatabaseOptions _options;
    private readonly ILogger<CatalogDbInitializer> _logger;

    public CatalogDbInitializer(CatalogDbContext context, IOptions<DatabaseOptions> options, ILogger<CatalogDbInitializer> logger)
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
        if (_context.Ordinateurs.Any())
        {
            _logger.LogInformation("Le catalogue contient déjà des ordinateurs, aucune donnée ajoutée.");
            return;
        }
        _context.Ordinateurs.AddRange(CatalogSeedData.Ordinateurs());
        _context.SaveChanges();
        _logger.LogInformation("Catalogue initialisé avec des ordinateurs de démonstration.");
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
