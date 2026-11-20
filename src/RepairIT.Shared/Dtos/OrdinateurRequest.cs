using System.ComponentModel.DataAnnotations;
using RepairIT.Shared.Enums;

namespace RepairIT.Shared.Dtos;

public class OrdinateurRequest
{
    [Required(ErrorMessage = "La marque est obligatoire.")]
    [StringLength(50)]
    public string Marque { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le modèle est obligatoire.")]
    [StringLength(100)]
    public string Modele { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le processeur est obligatoire.")]
    [StringLength(100)]
    public string Processeur { get; set; } = string.Empty;

    [Range(1, 1024, ErrorMessage = "La RAM doit être comprise entre 1 et 1024 Go.")]
    public int RamGo { get; set; }

    [Range(1, 100000, ErrorMessage = "Le stockage doit être compris entre 1 et 100000 Go.")]
    public int StockageGo { get; set; }

    public TypeOrdinateur Type { get; set; }

    public EtatOrdinateur Etat { get; set; }

    [Range(0, 1000000, ErrorMessage = "Le prix doit être positif.")]
    public decimal Prix { get; set; }

    [Range(0, 100000, ErrorMessage = "La quantité en stock doit être positive.")]
    public int QuantiteStock { get; set; }
}
