using Microsoft.EntityFrameworkCore;
using RepairIT.CatalogService.Data;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Enums;

namespace RepairIT.CatalogService.Tests.Outils;

public static class ContexteDeTest
{
    public static CatalogDbContext CreerContexte()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        return new CatalogDbContext(options);
    }

    public static CatalogDbContext CreerContexteAvecDonnees()
    {
        var contexte = CreerContexte();
        contexte.Ordinateurs.AddRange(CatalogSeedData.Ordinateurs());
        contexte.SaveChanges();
        return contexte;
    }

    public static OrdinateurRequest CreerRequest(string marque = "Dell", string modele = "Latitude 5440", int quantite = 3, decimal prix = 999m)
    {
        return new OrdinateurRequest
        {
            Marque = marque,
            Modele = modele,
            Processeur = "Intel Core i5",
            RamGo = 16,
            StockageGo = 512,
            Type = TypeOrdinateur.Portable,
            Etat = EtatOrdinateur.Neuf,
            Prix = prix,
            QuantiteStock = quantite
        };
    }
}
