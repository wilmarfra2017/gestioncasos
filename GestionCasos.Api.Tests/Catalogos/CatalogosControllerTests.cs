using GestionCasos.Api.Tests.Catalogos.DataBuilder;
using GestionCasos.Api.Tests.Utils;
using GestionCasos.Application.Dtos;

namespace GestionCasos.Api.Tests.Catalogos;

public class CatalogosControllerTests(BasePruebasIntegracion _basePruebas) : IClassFixture<BasePruebasIntegracion>
{
    [Fact]
    public async Task ObtenerCatalogos_DebeRetornarDataCuandoSeIngreseInformacionEnElQueryString()
    {
        var elemento1 = new ElementoDtoBuilder().WithClave("clave1").WithValor("valor1").Build();
        var elemento2 = new ElementoDtoBuilder().WithClave("clave2").WithValor("valor2").Build();
        _ = new CatalogoRequestDtoBuilder()
            .WithNombre("Seguros")
            .WithDescripcion("Descripción de Seguros")
            .AddElemento(elemento1)
            .AddElemento(elemento2)
            .Build();


        var url = "/api/v1/gestion-casos/catalogos?pagina=1&tamanoPagina=10&nombre=Seguros&contieneClave=&contieneValor=";
        var response = await _basePruebas.Cliente.GetAsync(url);
        var totalRegistrosEsperados = 1;

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var catalogosResponse = JsonDocument.Parse(content).RootElement.GetProperty("data").ToString();
        var paginatedCatalogos = JsonSerializer.Deserialize<ResponsePaginadoDto<CatalogoDto>>(catalogosResponse, JsonOptions.DefaultJsonSerializerOptions);

        Assert.Equal(totalRegistrosEsperados, paginatedCatalogos!.TotalRegistros);
        Assert.NotNull(paginatedCatalogos);
        Assert.IsType<ResponsePaginadoDto<CatalogoDto>>(paginatedCatalogos);
        Assert.True(paginatedCatalogos.Registros.Any());
    }

    [Fact]
    public async Task ObtenerCatalogos_NoDebeRetornarDataCuandoIngreseInformacionEnElQueryStringYNoTengaCoincidenciaConLoGuardado()
    {
        var url = "/api/v1/gestion-casos/catalogos?pagina=1&tamanoPagina=10&nombre=Humano&contieneClave=&contieneValor=";
        var response = await _basePruebas.Cliente.GetAsync(url);
        var totalRegistrosEsperados = 0;

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var catalogosResponse = JsonDocument.Parse(content).RootElement.GetProperty("data").ToString();
        var paginatedCatalogos = JsonSerializer.Deserialize<ResponsePaginadoDto<CatalogoDto>>(catalogosResponse, JsonOptions.DefaultJsonSerializerOptions);

        Assert.Equal(totalRegistrosEsperados, paginatedCatalogos!.TotalRegistros);
        Assert.IsType<ResponsePaginadoDto<CatalogoDto>>(paginatedCatalogos);
        Assert.False(paginatedCatalogos.Registros.Any());
    }

    [Fact]
    public async Task ObtenerCatalogos_CuandoNoIngreseInformacionEnElQueryString_RetornaBadRequest()
    {
        var url = "/api/v1/gestion-casos/catalogos?pagina=1&tamanoPagina=10&nombre=&contieneClave=&contieneValor=";
        var response = await _basePruebas.Cliente.GetAsync(url);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

