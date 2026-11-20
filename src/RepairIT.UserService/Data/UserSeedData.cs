using RepairIT.Shared.Enums;
using RepairIT.Shared.Models;

namespace RepairIT.UserService.Data;

public static class UserSeedData
{
    public static List<Utilisateur> Utilisateurs()
    {
        var maintenant = DateTime.UtcNow;
        return new List<Utilisateur>
        {
            Creer("Martin", "Alice", "alice.martin@repairit.fr", RoleUtilisateur.Administrateur, maintenant),
            Creer("Bernard", "Lucas", "lucas.bernard@repairit.fr", RoleUtilisateur.Gestionnaire, maintenant),
            Creer("Dubois", "Emma", "emma.dubois@repairit.fr", RoleUtilisateur.Gestionnaire, maintenant),
            Creer("Thomas", "Hugo", "hugo.thomas@repairit.fr", RoleUtilisateur.Gestionnaire, maintenant),
            Creer("Robert", "Chloe", "chloe.robert@repairit.fr", RoleUtilisateur.Gestionnaire, maintenant)
        };
    }

    private static Utilisateur Creer(string nom, string prenom, string email, RoleUtilisateur role, DateTime dateCreation)
    {
        return new Utilisateur
        {
            Nom = nom,
            Prenom = prenom,
            Email = email,
            Role = role,
            EstActif = true,
            DateCreation = dateCreation
        };
    }
}
