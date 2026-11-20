using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RepairIT.CatalogService.Configuration;
using RepairIT.CatalogService.Data;
using RepairIT.CatalogService.Tests.Outils;

namespace RepairIT.CatalogService.Tests.Data;

public class CatalogDbInitializerTests
{
    private static CatalogDbInitializer CreerInitialiseur(CatalogDbContext contexte, bool seed)
    {
        var options = Options.Create(new DatabaseOptions { SeedOnStartup = seed });
        return new CatalogDbInitializer(contexte, options, NullLogger<CatalogDbInitializer>.Instance);
    }

    [Fact]
    public void Initialiser_BaseVide_AjouteLesOrdinateursDeDemonstration()
    {
        using var contexte = ContexteDeTest.CreerContexte();
        CreerInitialiseur(contexte, true).Initialiser();
        Assert.Equal(CatalogSeedData.Ordinateurs().Count, contexte.Ordinateurs.Count());
    }

    [Fact]
    public void Initialiser_AppeleDeuxFois_NeDupliquePasLesDonnees()
    {
        using var contexte = ContexteDeTest.CreerContexte();
        var initialiseur = CreerInitialiseur(contexte, true);
        initialiseur.Initialiser();
        initialiseur.Initialiser();
        Assert.Equal(CatalogSeedData.Ordinateurs().Count, contexte.Ordinateurs.Count());
    }

    [Fact]
    public void Initialiser_SeedDesactive_LaisseLaBaseVide()
    {
        using var contexte = ContexteDeTest.CreerContexte();
        CreerInitialiseur(contexte, false).Initialiser();
        Assert.Empty(contexte.Ordinateurs);
    }
}
