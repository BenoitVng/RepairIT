using System.ComponentModel.DataAnnotations;
using RepairIT.Shared.Enums;

namespace RepairIT.Shared.Dtos;

public class UtilisateurRequest
{
    [Required(ErrorMessage = "Le nom est obligatoire.")]
    [StringLength(50)]
    public string Nom { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le prénom est obligatoire.")]
    [StringLength(50)]
    public string Prenom { get; set; } = string.Empty;

    [Required(ErrorMessage = "L'email est obligatoire.")]
    [EmailAddress(ErrorMessage = "L'email n'est pas valide.")]
    public string Email { get; set; } = string.Empty;

    public RoleUtilisateur Role { get; set; }
}
