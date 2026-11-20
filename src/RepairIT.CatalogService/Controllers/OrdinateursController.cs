using Microsoft.AspNetCore.Mvc;
using RepairIT.CatalogService.Services;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Controllers;

[ApiController]
[Route("api/ordinateurs")]
public class OrdinateursController : ControllerBase
{
    private readonly IOrdinateurService _service;
    private readonly ILogger<OrdinateursController> _logger;

    public OrdinateursController(IOrdinateurService service, ILogger<OrdinateursController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<Ordinateur>>> Lister()
    {
        _logger.LogInformation("Liste des ordinateurs demandée");
        return Ok(await _service.ListerAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Ordinateur>> TrouverParId(int id)
    {
        var ordinateur = await _service.TrouverParIdAsync(id);
        if (ordinateur == null)
        {
            return NotFound($"Aucun ordinateur avec l'identifiant {id}.");
        }
        return Ok(ordinateur);
    }

    [HttpPost]
    public async Task<ActionResult<Ordinateur>> Ajouter([FromBody] OrdinateurRequest request)
    {
        var ordinateur = await _service.AjouterAsync(request);
        return CreatedAtAction(nameof(TrouverParId), new { id = ordinateur.Id }, ordinateur);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Ordinateur>> Modifier(int id, [FromBody] OrdinateurRequest request)
    {
        var ordinateur = await _service.ModifierAsync(id, request);
        if (ordinateur == null)
        {
            return NotFound($"Aucun ordinateur avec l'identifiant {id}.");
        }
        return Ok(ordinateur);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Supprimer(int id)
    {
        var supprime = await _service.SupprimerAsync(id);
        if (!supprime)
        {
            return NotFound($"Aucun ordinateur avec l'identifiant {id}.");
        }
        return NoContent();
    }
}
