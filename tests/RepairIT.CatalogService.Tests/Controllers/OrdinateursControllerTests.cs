using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RepairIT.CatalogService.Controllers;
using RepairIT.CatalogService.Services;
using RepairIT.CatalogService.Tests.Outils;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Tests.Controllers;

public class OrdinateursControllerTests
{
    private readonly Mock<IOrdinateurService> _service = new();
    private readonly OrdinateursController _controller;

    public OrdinateursControllerTests()
    {
        _controller = new OrdinateursController(_service.Object, NullLogger<OrdinateursController>.Instance);
    }

    [Fact]
    public async Task Lister_RetourneOkAvecLaListe()
    {
        _service.Setup(s => s.ListerAsync()).ReturnsAsync(new List<Ordinateur> { new() { Id = 1 } });
        var resultat = await _controller.Lister();
        var ok = Assert.IsType<OkObjectResult>(resultat.Result);
        Assert.Single(Assert.IsType<List<Ordinateur>>(ok.Value));
    }

    [Fact]
    public async Task TrouverParId_Existant_RetourneOk()
    {
        _service.Setup(s => s.TrouverParIdAsync(1)).ReturnsAsync(new Ordinateur { Id = 1 });
        var resultat = await _controller.TrouverParId(1);
        Assert.IsType<OkObjectResult>(resultat.Result);
    }

    [Fact]
    public async Task TrouverParId_Inexistant_RetourneNotFound()
    {
        _service.Setup(s => s.TrouverParIdAsync(5)).ReturnsAsync((Ordinateur?)null);
        var resultat = await _controller.TrouverParId(5);
        Assert.IsType<NotFoundObjectResult>(resultat.Result);
    }

    [Fact]
    public async Task Ajouter_RetourneCreatedAtAction()
    {
        var request = ContexteDeTest.CreerRequest();
        _service.Setup(s => s.AjouterAsync(request)).ReturnsAsync(new Ordinateur { Id = 12 });
        var resultat = await _controller.Ajouter(request);
        var cree = Assert.IsType<CreatedAtActionResult>(resultat.Result);
        Assert.Equal(nameof(OrdinateursController.TrouverParId), cree.ActionName);
        Assert.Equal(12, cree.RouteValues!["id"]);
    }

    [Fact]
    public async Task Modifier_Existant_RetourneOk()
    {
        var request = ContexteDeTest.CreerRequest();
        _service.Setup(s => s.ModifierAsync(3, request)).ReturnsAsync(new Ordinateur { Id = 3 });
        var resultat = await _controller.Modifier(3, request);
        Assert.IsType<OkObjectResult>(resultat.Result);
    }

    [Fact]
    public async Task Modifier_Inexistant_RetourneNotFound()
    {
        _service.Setup(s => s.ModifierAsync(It.IsAny<int>(), It.IsAny<OrdinateurRequest>())).ReturnsAsync((Ordinateur?)null);
        var resultat = await _controller.Modifier(3, ContexteDeTest.CreerRequest());
        Assert.IsType<NotFoundObjectResult>(resultat.Result);
    }

    [Fact]
    public async Task Supprimer_Existant_RetourneNoContent()
    {
        _service.Setup(s => s.SupprimerAsync(4)).ReturnsAsync(true);
        Assert.IsType<NoContentResult>(await _controller.Supprimer(4));
    }

    [Fact]
    public async Task Supprimer_Inexistant_RetourneNotFound()
    {
        _service.Setup(s => s.SupprimerAsync(4)).ReturnsAsync(false);
        Assert.IsType<NotFoundObjectResult>(await _controller.Supprimer(4));
    }
}
