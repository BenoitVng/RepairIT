using Microsoft.AspNetCore.Mvc;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Models;
using RepairIT.UserService.Services;

namespace RepairIT.UserService.Controllers;

[ApiController]
[Route("api/utilisateurs")]
public class UtilisateursController : ControllerBase
{
    private readonly IUtilisateurService _service;
    private readonly ILogger<UtilisateursController> _logger;

    public UtilisateursController(IUtilisateurService service, ILogger<UtilisateursController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<Utilisateur>>> Lister()
    {
        _logger.LogInformation("Liste des utilisateurs demandée");
        return Ok(await _service.ListerAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Utilisateur>> TrouverParId(int id)
    {
        var utilisateur = await _service.TrouverParIdAsync(id);
        if (utilisateur == null)
        {
            return NotFound($"Aucun utilisateur avec l'identifiant {id}.");
        }
        return Ok(utilisateur);
    }

    [HttpPost]
    public async Task<ActionResult<Utilisateur>> Creer([FromBody] UtilisateurRequest request)
    {
        var utilisateur = await _service.CreerAsync(request);
        if (utilisateur == null)
        {
            return Conflict($"L'email {request.Email} est déjà utilisé.");
        }
        return CreatedAtAction(nameof(TrouverParId), new { id = utilisateur.Id }, utilisateur);
    }
}
