using RepairIT.Shared.Dtos;
using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Services;

public interface IOrdinateurService
{
    Task<List<Ordinateur>> ListerAsync();
    Task<Ordinateur?> TrouverParIdAsync(int id);
    Task<Ordinateur> AjouterAsync(OrdinateurRequest request);
    Task<Ordinateur?> ModifierAsync(int id, OrdinateurRequest request);
    Task<bool> SupprimerAsync(int id);
}
