namespace RepairIT.CatalogService.Configuration;

public class DatabaseOptions
{
    public const string Section = "Database";
    public bool SeedOnStartup { get; set; } = true;
}
