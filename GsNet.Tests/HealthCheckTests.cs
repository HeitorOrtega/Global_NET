using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using GsNetApi.Data;
using GsNetApi.Services;
using GsNetApi.Models;

namespace GsNet.Tests;

public class MensagemServiceTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task DeveCriarMensagem()
    {
        var context = GetDbContext();
        var service = new MensagemService(context);

        var mensagem = new Mensagem
        {
            TextoMensagem = "Mensagem de teste",
            NivelEstresse = 5,
            UsuarioId = 10
        };

        var criada = await service.CreateAsync(mensagem);

        criada.Should().NotBeNull();
        criada.Id.Should().BeGreaterThan(0);
        criada.TextoMensagem.Should().Be("Mensagem de teste");
        criada.NivelEstresse.Should().Be(5);
        criada.UsuarioId.Should().Be(10);
    }
}