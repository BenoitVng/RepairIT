using Microsoft.EntityFrameworkCore;
using RepairIT.CatalogService.Data;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Services;

public class OrdinateurService : IOrdinateurService
{
    private readonly CatalogDbContext _context;
    private readonly ILogger<OrdinateurService> _logger;

    public OrdinateurService(CatalogDbContext context, ILogger<OrdinateurService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Ordinateur>> ListerAsync()
    {
        return await _context.Ordinateurs.AsNoTracking().OrderBy(o => o.Marque).ThenBy(o => o.Modele).ToListAsync();
    }

    public async Task<Ordinateur?> TrouverParIdAsync(int id)
    {
        return await _context.Ordinateurs.FindAsync(id);
    }

    public async Task<Ordinateur> AjouterAsync(OrdinateurRequest request)
    {
        var ordinateur = new Ordinateur { DateAjout = DateTime.UtcNow };
        AppliquerRequest(ordinateur, request);
        _context.Ordinateurs.Add(ordinateur);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Ordinateur {Id} ajouté : {Marque} {Modele}", ordinateur.Id, ordinateur.Marque, ordinateur.Modele);
        return ordinateur;
    }

    public async Task<Ordinateur?> ModifierAsync(int id, OrdinateurRequest request)
    {
        var ordinateur = await TrouverParIdAsync(id);
        if (ordinateur == null)
        {
            _logger.LogWarning("Modification impossible : ordinateur {Id} introuvable", id);
            return null;
        }
        AppliquerRequest(ordinateur, request);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Ordinateur {Id} modifié", id);
        return ordinateur;
    }

    public async Task<bool> SupprimerAsync(int id)
    {
        var ordinateur = await TrouverParIdAsync(id);
        if (ordinateur == null)
        {
            _logger.LogWarning("Suppression impossible : ordinateur {Id} introuvable", id);
            return false;
        }
        _context.Ordinateurs.Remove(ordinateur);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Ordinateur {Id} supprimé", id);
        return true;
    }

    private static void AppliquerRequest(Ordinateur ordinateur, OrdinateurRequest request)
    {
        ordinateur.Marque = request.Marque.Trim();
        ordinateur.Modele = request.Modele.Trim();
        ordinateur.Processeur = request.Processeur.Trim();
        ordinateur.RamGo = request.RamGo;
        ordinateur.StockageGo = request.StockageGo;
        ordinateur.Type = request.Type;
        ordinateur.Etat = request.Etat;
        ordinateur.Prix = request.Prix;
        ordinateur.QuantiteStock = request.QuantiteStock;
    }
}
