using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RepairIT.AuthentificationService.Configuration;

namespace RepairIT.AuthentificationService.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    private readonly ServiceInfoOptions _serviceInfo;
    private readonly ILogger<HealthController> _logger;

    public HealthController(IOptions<ServiceInfoOptions> serviceInfo, ILogger<HealthController> logger)
    {
        _serviceInfo = serviceInfo.Value;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<ServiceInfoOptions> Verifier()
    {
        _logger.LogInformation("Vérification de l'état du service {Nom}", _serviceInfo.Nom);
        return Ok(_serviceInfo);
    }
}