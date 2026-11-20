using Microsoft.Extensions.Logging.Abstractions;
using RepairIT.Shared.Enums;
using RepairIT.Shared.Models;
using RepairIT.UserService.Data;
using RepairIT.UserService.Services;
using RepairIT.UserService.Tests.Outils;

namespace RepairIT.UserService.Tests.Services;

public class UtilisateurServiceTests
{
    private static UtilisateurService CreerService(UserDbContext contexte)
    {
        return new UtilisateurService(contexte, NullLogger<UtilisateurService>.Instance);
    }

    [Fact]
    public async Task CreerAsync_CreeUnUtilisateurActifAvecUserKeycloakParDefaut()
    {
        using var contexte = ContexteDeTest.CreerContexte();
        var service = CreerService(contexte);
        var utilisateur = await service.CreerAsync(ContexteDeTest.CreerRequest(email: " Jean.Dupont@RepairIT.fr "));
        Assert.NotNull(utilisateur);
        Assert.True(utilisateur.Id > 0);
        Assert.True(utilisateur.EstActif);
        Assert.Equal(Utilisateur.UserKeycloakParDefaut, utilisateur.UserKeycloak);
        Assert.Equal("jean.dupont@repairit.fr", utilisateur.Email);
    }

    [Fact]
    public async Task CreerAsync_ConserveLeRoleDemande()
    {
        using var contexte = ContexteDeTest.CreerContexte();
        var service = CreerService(contexte);
        var utilisateur = await service.CreerAsync(ContexteDeTest.CreerRequest(role: RoleUtilisateur.Administrateur));
        Assert.Equal(RoleUtilisateur.Administrateur, utilisateur!.Role);
    }

    [Fact]
    public async Task CreerAsync_EmailDejaUtilise_RetourneNull()
    {
        using var contexte = ContexteDeTest.CreerContexte();
        var service = CreerService(contexte);
        await service.CreerAsync(ContexteDeTest.CreerRequest());
        var doublon = await service.CreerAsync(ContexteDeTest.CreerRequest(email: "JEAN.DUPONT@repairit.fr"));
        Assert.Null(doublon);
        Assert.Single(contexte.Utilisateurs);
    }

    [Fact]
    public async Task ListerAsync_RetourneLesUtilisateursTriesParNom()
    {
        using var contexte = ContexteDeTest.CreerContexte();
        contexte.Utilisateurs.AddRange(UserSeedData.Utilisateurs());
        contexte.SaveChanges();
        var service = CreerService(contexte);
        var utilisateurs = await service.ListerAsync();
        Assert.Equal(5, utilisateurs.Count);
        Assert.Equal("Bernard", utilisateurs.First().Nom);
    }

    [Fact]
    public async Task TrouverParIdAsync_RetourneLUtilisateurOuNull()
    {
        using var contexte = ContexteDeTest.CreerContexte();
        var service = CreerService(contexte);
        var utilisateur = await service.CreerAsync(ContexteDeTest.CreerRequest());
        Assert.NotNull(await service.TrouverParIdAsync(utilisateur!.Id));
        Assert.Null(await service.TrouverParIdAsync(999));
    }
}
