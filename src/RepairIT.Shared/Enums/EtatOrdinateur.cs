using System.Text.Json.Serialization;

namespace RepairIT.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EtatOrdinateur
{
    Neuf,
    Reconditionne,
    Occasion
}
