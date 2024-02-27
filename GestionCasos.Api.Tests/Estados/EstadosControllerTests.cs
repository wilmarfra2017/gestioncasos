using GestionCasos.Api.Tests.DataMock;
using GestionCasos.Api.Tests.EstadosSolicitudes.DataBuilder;
using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Entities;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace GestionCasos.Api.Tests.EstadosSolicitudes
{
    public class EstadosControllerTests : IClassFixture<BasePruebasIntegracion>
    {
        private readonly BasePruebasIntegracion _basePruebas;
        private readonly string _recursoPostEstados = "/api/v1/gestion-casos/estados";

        public EstadosControllerTests(BasePruebasIntegracion basePruebas)
        {
            _basePruebas = basePruebas;
        }

        [Fact]
        public async Task ActualizarEstadosSolicitudDeberiaActualizarlaBaseDeDatos()
        {
            // Arrange
            string key = "TEST-190205";
            var payload = new WebHookSolicitudActualizadaRequestDtoBuilder()
                .WithKey(key)
                .WithFields(new JiraFieldsBuilder()
                    .WithStatus(new JiraStatus("123", "Pendiente de documentacion"))
                    .WithUpdated("2024-02-19T06:01:32.167+0000")
                    .WithComment(new JiraComment(["comentario test uno", "comentario test dos", "comentario test tres"]))
                    .Build())
                .Build();
            EndpointsMock.ConfigurarWireMockConsumirServicioNotificarPorEmail(_basePruebas);
            EndpointsMock.ConfigurarWireMockConsumirServicioNotificarPorSms(_basePruebas);

            //Act
            var response = await _basePruebas.Cliente.PostAsJsonAsync(_recursoPostEstados, payload);
            response.EnsureSuccessStatusCode();

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            var solicitudesResponse = JsonDocument.Parse(content).RootElement.GetProperty("data").ToString();
            Assert.NotNull(solicitudesResponse);
            Assert.NotEmpty(solicitudesResponse);
            var collection = _basePruebas._database.GetCollection<Solicitud>("Solicitud");
            var solicitudActualizada = await collection.Find(s => s.SolicitudKey == key).FirstOrDefaultAsync();
            Assert.NotNull(solicitudActualizada);
            Assert.Equal("Pendiente de documentacion", solicitudActualizada.Estatus.Nombre);
            Assert.Equal("comentario test tres", solicitudActualizada.Comentario);
            Assert.IsType<Estatus>(solicitudActualizada.Estatus);
            Assert.IsType<Solicitud>(solicitudActualizada);
        }


        [Fact]
        public async Task ActualizarEstadosSolicitudNoDeberiaActualizarlaBaseDeDatosPaylodkeyVacio()
        {
            // Arrange
            string key = string.Empty;
            var payload = new WebHookSolicitudActualizadaRequestDtoBuilder()
               .WithKey(key)
               .WithFields(new JiraFieldsBuilder()
                   .WithStatus(new JiraStatus("123", "Pendiente de documentacion"))
                   .WithUpdated("2024-02-19T06:01:32.167+0000")
                   .WithComment(new JiraComment(["comentario test uno", "comentario test dos", "comentario test tres"]))
                   .Build())
               .Build();

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                HttpResponseMessage response = await _basePruebas.Cliente.PostAsJsonAsync(_recursoPostEstados, payload);
                response.EnsureSuccessStatusCode();
            });
        }

        [Fact]
        public async Task ActualizarEstadosSolicitudConfechaInvalida()
        {
            // Arrange
            string key = "TEST-190205";
            var payload = new WebHookSolicitudActualizadaRequestDtoBuilder()
               .WithKey(key)
               .WithFields(new JiraFieldsBuilder()
                   .WithStatus(new JiraStatus("123", "Pendiente de documentacion"))
                   .WithUpdated("2024-02-19T06:01:32----")
                   .WithComment(new JiraComment(["comentario test uno", "comentario test dos", "comentario test tres"]))
                   .Build())
               .Build();

            // Act & Assert
            await Assert.ThrowsAsync<JsonReaderException>(async () =>
            {
                HttpResponseMessage response = await _basePruebas.Cliente.PostAsJsonAsync(_recursoPostEstados, payload);
                response.EnsureSuccessStatusCode();
            });
        }

        [Fact]
        public async Task ActualizarEstadosSolicitudComentariosNull()
        {
            // Arrange
            string key = "TEST-190205";
            var payload = new WebHookSolicitudActualizadaRequestDtoBuilder()
             .WithKey(key)
             .WithFields(new JiraFieldsBuilder()
                 .WithStatus(new JiraStatus("123", "Pendiente de documentacion"))
                 .WithUpdated("2024-02-19T06:01:32.167+0000")
                 .WithComment(new JiraComment(null))
                 .Build())
             .Build();
            EndpointsMock.ConfigurarWireMockConsumirServicioNotificarPorEmail(_basePruebas);
            EndpointsMock.ConfigurarWireMockConsumirServicioNotificarPorSms(_basePruebas);

            //Act
            var response = await _basePruebas.Cliente.PostAsJsonAsync(_recursoPostEstados, payload);
            response.EnsureSuccessStatusCode();

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            var solicitudesResponse = JsonDocument.Parse(content).RootElement.GetProperty("data").ToString();
            Assert.NotNull(solicitudesResponse);
            Assert.NotEmpty(solicitudesResponse);
            var collection = _basePruebas._database.GetCollection<Solicitud>("Solicitud");
            var solicitudActualizada = await collection.Find(s => s.SolicitudKey == key).FirstOrDefaultAsync();
            Assert.NotNull(solicitudActualizada);
            Assert.Equal("Pendiente de documentacion", solicitudActualizada.Estatus.Nombre);
            Assert.IsType<Estatus>(solicitudActualizada.Estatus);
            Assert.IsType<Solicitud>(solicitudActualizada);
        }
                           
    }
}
