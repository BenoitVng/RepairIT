namespace RepairIT.UserService.Configuration;

public class DatabaseOptions
{
    public const string Section = "Database";
    public bool SeedOnStartup { get; set; } = true;
}
