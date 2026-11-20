using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RepairIT.Shared.Enums;
using RepairIT.UserService.Configuration;
using RepairIT.UserService.Data;
using RepairIT.UserService.Tests.Outils;

namespace RepairIT.UserService.Tests.Data;

public class UserDbInitializerTests
{
    private static UserDbInitializer CreerInitialiseur(UserDbContext contexte, bool seed)
    {
        var options = Options.Create(new DatabaseOptions { SeedOnStartup = seed });
        return new UserDbInitializer(contexte, options, NullLogger<UserDbInitializer>.Instance);
    }

    [Fact]
    public void Initialiser_BaseVide_CreeUnAdministrateurEtQuatreGestionnaires()
    {
        using var contexte = ContexteDeTest.CreerContexte();
        CreerInitialiseur(contexte, true).Initialiser();
        Assert.Equal(1, contexte.Utilisateurs.Count(u => u.Role == RoleUtilisateur.Administrateur));
        Assert.Equal(4, contexte.Utilisateurs.Count(u => u.Role == RoleUtilisateur.Gestionnaire));
    }

    [Fact]
    public void Initialiser_AppeleDeuxFois_NeDupliquePasLesDonnees()
    {
        using var contexte = ContexteDeTest.CreerContexte();
        var initialiseur = CreerInitialiseur(contexte, true);
        initialiseur.Initialiser();
        initialiseur.Initialiser();
        Assert.Equal(5, contexte.Utilisateurs.Count());
    }

    [Fact]
    public void Initialiser_SeedDesactive_LaisseLaBaseVide()
    {
        using var contexte = ContexteDeTest.CreerContexte();
        CreerInitialiseur(contexte, false).Initialiser();
        Assert.Empty(contexte.Utilisateurs);
    }
}
