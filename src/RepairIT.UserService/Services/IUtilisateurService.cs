using RepairIT.Shared.Dtos;
using RepairIT.Shared.Models;

namespace RepairIT.UserService.Services;

public interface IUtilisateurService
{
    Task<List<Utilisateur>> ListerAsync();
    Task<Utilisateur?> TrouverParIdAsync(int id);
    Task<Utilisateur?> CreerAsync(UtilisateurRequest request);
}
