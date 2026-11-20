using RepairIT.Shared.Enums;
using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Data;

public static class CatalogSeedData
{
    public static List<Ordinateur> Ordinateurs()
    {
        var maintenant = DateTime.UtcNow;
        return new List<Ordinateur>
        {
            Creer("Dell", "XPS 13", "Intel Core i7-1360P", 16, 512, TypeOrdinateur.Portable, EtatOrdinateur.Neuf, 1299.00m, 5, maintenant),
            Creer("Lenovo", "ThinkPad T14", "AMD Ryzen 7 PRO", 32, 1000, TypeOrdinateur.Portable, EtatOrdinateur.Neuf, 1449.00m, 3, maintenant),
            Creer("HP", "EliteDesk 800 G6", "Intel Core i5-10500", 16, 512, TypeOrdinateur.Fixe, EtatOrdinateur.Reconditionne, 489.00m, 8, maintenant),
            Creer("Apple", "MacBook Air M2", "Apple M2", 8, 256, TypeOrdinateur.Portable, EtatOrdinateur.Neuf, 1199.00m, 4, maintenant),
            Creer("Asus", "ROG Strix G16", "Intel Core i9-13980HX", 32, 1000, TypeOrdinateur.Portable, EtatOrdinateur.Neuf, 2199.00m, 2, maintenant),
            Creer("Apple", "iMac 24", "Apple M3", 16, 512, TypeOrdinateur.AllInOne, EtatOrdinateur.Neuf, 1799.00m, 1, maintenant),
            Creer("Acer", "Aspire 5", "Intel Core i5-1235U", 8, 512, TypeOrdinateur.Portable, EtatOrdinateur.Occasion, 349.00m, 0, maintenant),
            Creer("Dell", "PowerEdge T150", "Intel Xeon E-2314", 32, 2000, TypeOrdinateur.Serveur, EtatOrdinateur.Neuf, 1890.00m, 2, maintenant)
        };
    }

    private static Ordinateur Creer(string marque, string modele, string processeur, int ramGo, int stockageGo, TypeOrdinateur type, EtatOrdinateur etat, decimal prix, int quantite, DateTime dateAjout)
    {
        return new Ordinateur
        {
            Marque = marque,
            Modele = modele,
            Processeur = processeur,
            RamGo = ramGo,
            StockageGo = stockageGo,
            Type = type,
            Etat = etat,
            Prix = prix,
            QuantiteStock = quantite,
            DateAjout = dateAjout
        };
    }
}
