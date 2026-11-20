namespace RepairIT.AuthentificationService.Configuration;

public class ServiceInfoOptions
{
    public const string Section = "ServiceInfo";
    public string Nom { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}