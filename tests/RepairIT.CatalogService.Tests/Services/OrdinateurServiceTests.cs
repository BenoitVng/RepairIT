using Microsoft.Extensions.Logging.Abstractions;
using RepairIT.CatalogService.Data;
using RepairIT.CatalogService.Services;
using RepairIT.CatalogService.Tests.Outils;

namespace RepairIT.CatalogService.Tests.Services;

public class OrdinateurServiceTests
{
    private static OrdinateurService CreerService(CatalogDbContext contexte)
    {
        return new OrdinateurService(contexte, NullLogger<OrdinateurService>.Instance);
    }

    [Fact]
    public async Task ListerAsync_RetourneTousLesOrdinateursTriesParMarque()
    {
        using var contexte = ContexteDeTest.CreerContexteAvecDonnees();
        var service = CreerService(contexte);
        var resultat = await service.ListerAsync();
        Assert.Equal(CatalogSeedData.Ordinateurs().Count, resultat.Count);
        Assert.Equal("Acer", resultat.First().Marque);
    }

    [Fact]
    public async Task ListerAsync_BaseVide_RetourneUneListeVide()
    {
        using var contexte = ContexteDeTest.CreerContexte();
        var service = CreerService(contexte);
        Assert.Empty(await service.ListerAsync());
    }

    [Fact]
    public async Task AjouterAsync_EnregistreLOrdinateurAvecUneDateDAjout()
    {
        using var contexte = ContexteDeTest.CreerContexte();
        var service = CreerService(contexte);
        var ordinateur = await service.AjouterAsync(ContexteDeTest.CreerRequest(marque: "  HP  "));
        Assert.True(ordinateur.Id > 0);
        Assert.Equal("HP", ordinateur.Marque);
        Assert.NotEqual(default, ordinateur.DateAjout);
        Assert.Single(contexte.Ordinateurs);
    }

    [Fact]
    public async Task TrouverParIdAsync_IdExistant_RetourneLOrdinateur()
    {
        using var contexte = ContexteDeTest.CreerContexte();
        var service = CreerService(contexte);
        var ordinateur = await service.AjouterAsync(ContexteDeTest.CreerRequest());
        var resultat = await service.TrouverParIdAsync(ordinateur.Id);
        Assert.NotNull(resultat);
        Assert.Equal("Latitude 5440", resultat.Modele);
    }

    [Fact]
    public async Task TrouverParIdAsync_IdInconnu_RetourneNull()
    {
        using var contexte = ContexteDeTest.CreerContexte();
        var service = CreerService(contexte);
        Assert.Null(await service.TrouverParIdAsync(42));
    }

    [Fact]
    public async Task ModifierAsync_OrdinateurExistant_MetAJourLesInformations()
    {
        using var contexte = ContexteDeTest.CreerContexte();
        var service = CreerService(contexte);
        var ordinateur = await service.AjouterAsync(ContexteDeTest.CreerRequest());
        var resultat = await service.ModifierAsync(ordinateur.Id, ContexteDeTest.CreerRequest(modele: "Latitude 7440", quantite: 10, prix: 1500m));
        Assert.NotNull(resultat);
        Assert.Equal("Latitude 7440", resultat.Modele);
        Assert.Equal(10, resultat.QuantiteStock);
        Assert.Equal(1500m, resultat.Prix);
    }

    [Fact]
    public async Task ModifierAsync_OrdinateurInexistant_RetourneNull()
    {
        using var contexte = ContexteDeTest.CreerContexte();
        var service = CreerService(contexte);
        Assert.Null(await service.ModifierAsync(99, ContexteDeTest.CreerRequest()));
    }

    [Fact]
    public async Task SupprimerAsync_OrdinateurExistant_LeRetireDuCatalogue()
    {
        using var contexte = ContexteDeTest.CreerContexte();
        var service = CreerService(contexte);
        var ordinateur = await service.AjouterAsync(ContexteDeTest.CreerRequest());
        Assert.True(await service.SupprimerAsync(ordinateur.Id));
        Assert.Empty(contexte.Ordinateurs);
    }

    [Fact]
    public async Task SupprimerAsync_OrdinateurInexistant_RetourneFalse()
    {
        using var contexte = ContexteDeTest.CreerContexte();
        var service = CreerService(contexte);
        Assert.False(await service.SupprimerAsync(777));
    }
}
