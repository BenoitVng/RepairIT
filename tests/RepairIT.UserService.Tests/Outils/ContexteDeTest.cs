using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Enums;
using RepairIT.UserService.Data;

namespace RepairIT.UserService.Tests.Outils;

public static class ContexteDeTest
{
    public static UserDbContext CreerContexte()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        return new UserDbContext(options);
    }

    public static UtilisateurRequest CreerRequest(string email = "jean.dupont@repairit.fr", RoleUtilisateur role = RoleUtilisateur.Gestionnaire)
    {
        return new UtilisateurRequest
        {
            Nom = "Dupont",
            Prenom = "Jean",
            Email = email,
            Role = role
        };
    }
}
