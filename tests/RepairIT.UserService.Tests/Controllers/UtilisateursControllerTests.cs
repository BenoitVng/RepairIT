using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RepairIT.Shared.Dtos;
using RepairIT.Shared.Models;
using RepairIT.UserService.Controllers;
using RepairIT.UserService.Services;
using RepairIT.UserService.Tests.Outils;

namespace RepairIT.UserService.Tests.Controllers;

public class UtilisateursControllerTests
{
    private readonly Mock<IUtilisateurService> _service = new();
    private readonly UtilisateursController _controller;

    public UtilisateursControllerTests()
    {
        _controller = new UtilisateursController(_service.Object, NullLogger<UtilisateursController>.Instance);
    }

    [Fact]
    public async Task Lister_RetourneOk()
    {
        _service.Setup(s => s.ListerAsync()).ReturnsAsync(new List<Utilisateur> { new(), new() });
        var resultat = await _controller.Lister();
        var ok = Assert.IsType<OkObjectResult>(resultat.Result);
        Assert.Equal(2, Assert.IsType<List<Utilisateur>>(ok.Value).Count);
    }

    [Fact]
    public async Task TrouverParId_Existant_RetourneOk()
    {
        _service.Setup(s => s.TrouverParIdAsync(3)).ReturnsAsync(new Utilisateur { Id = 3 });
        var resultat = await _controller.TrouverParId(3);
        Assert.IsType<OkObjectResult>(resultat.Result);
    }

    [Fact]
    public async Task TrouverParId_Inexistant_RetourneNotFound()
    {
        _service.Setup(s => s.TrouverParIdAsync(3)).ReturnsAsync((Utilisateur?)null);
        var resultat = await _controller.TrouverParId(3);
        Assert.IsType<NotFoundObjectResult>(resultat.Result);
    }

    [Fact]
    public async Task Creer_Succes_RetourneCreatedAtAction()
    {
        var request = ContexteDeTest.CreerRequest();
        _service.Setup(s => s.CreerAsync(request)).ReturnsAsync(new Utilisateur { Id = 6 });
        var resultat = await _controller.Creer(request);
        var cree = Assert.IsType<CreatedAtActionResult>(resultat.Result);
        Assert.Equal(6, cree.RouteValues!["id"]);
    }

    [Fact]
    public async Task Creer_EmailDejaUtilise_RetourneConflict()
    {
        _service.Setup(s => s.CreerAsync(It.IsAny<UtilisateurRequest>())).ReturnsAsync((Utilisateur?)null);
        var resultat = await _controller.Creer(ContexteDeTest.CreerRequest());
        Assert.IsType<ConflictObjectResult>(resultat.Result);
    }
}
