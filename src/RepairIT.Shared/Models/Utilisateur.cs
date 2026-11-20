using RepairIT.Shared.Enums;

namespace RepairIT.Shared.Models;

public class Utilisateur
{
    public const string UserKeycloakParDefaut = "non-configure";
    public int Id { get; set; }
    public string UserKeycloak { get; set; } = UserKeycloakParDefaut;
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public RoleUtilisateur Role { get; set; }
    public bool EstActif { get; set; } = true;
    public DateTime DateCreation { get; set; }
}
