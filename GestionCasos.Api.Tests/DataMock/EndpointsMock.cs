
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestionCasos.Api.Tests.DataMock;
public static class EndpointsMock
{
    public static void ConfigurarWireMockGuardar(BasePruebasIntegracion basePruebasIntegracion)
    {
        basePruebasIntegracion.MockServer
            .Given(Request.Create()
            .WithPath("/issue/bulk")
            .UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBodyAsJson(new
                {
                    issues = new[]
                    {
                        new { id = "917813", key = "SDSYF-190173", self = "https://gestiondecasosdev.humano.local/rest/api/2/issue/917813" }
                    },
                    errors = Array.Empty<object>()
                }));
    }
    public static void ConfigurarWireMockActualizar(BasePruebasIntegracion basePruebasIntegracion)
    {
        basePruebasIntegracion.MockServer
            .Given(Request.Create()
            .WithPath("/issue/917813")
            .UsingPut())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody(""
                ));
    }

    public static void ConfigurarWireMockConsultar(BasePruebasIntegracion basePruebasIntegracion)
    {
        basePruebasIntegracion.MockServer
       .Given(Request.Create().WithPath("/issue/917813").UsingGet())
       .RespondWith(Response.Create()
           .WithStatusCode(200)
           .WithBodyAsJson(new
           {
               id = "917813",
               key = "SDSYF-190173",
               self = "http://localhost:62225/issue/917813",
               fields = new
               {
                   summary = "Resumen de la solicitud",
                   description = "Descripción detallada de la solicitud",
                   issuetype = new
                   {
                       id = "12000",
                       name = "Test"
                   },
                   status = new
                   {
                       id = "10702",
                       name = "Nuevo"
                   },
                   assignee = new
                   {
                       key = "macristo",
                       name = "macristo"
                   },
                   creator = new
                   {
                       key = "user123",
                       name = "user123"
                   },
                   created = "2024-01-16T12:40:02.647-0400",
                   updated = "2024-01-17T12:40:02.647-0400",
                   project = new
                   {
                       id = "10701",
                       key = "SDSYF"
                   }
               }
           }));
    }

    public static void ConfigurarWireMockConsumirServicioNotificarPorEmail(BasePruebasIntegracion basePruebasIntegracion)
    {
        basePruebasIntegracion.MockServer
            .Given(Request.Create()
            .WithPath("/api/v1/gestion-casos/notificacion-email")
            .UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBodyAsJson(new
                {
                    success = true

                }));

    }

    public static void ConfigurarWireMockConsumirServicioNotificarPorSms(BasePruebasIntegracion basePruebasIntegracion)
    {
        basePruebasIntegracion.MockServer
            .Given(Request.Create()
            .WithPath("/api/v1/gestion-casos/notificacion-sms")
            .UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBodyAsJson(new
                {
                    success = true
                }));
    }

    public static void ConfigurarWireMockSmsNotificacion(BasePruebasIntegracion basePruebasIntegracion)
    {
        basePruebasIntegracion.MockServer
            .Given(Request.Create()
                .WithPath("/api/v1/notification/sms/send")
                .UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBodyAsJson(new
                {
                    response = new
                    {
                        success = true,
                        code = 0,
                        message = "SMS enviado correctamente",
                        date = "2024-02-23T12:06:06.0227951+00:00"
                    },
                    data = new
                    {
                        notificationId = "294f9d29-f66b-4a51-81f2-e63f3cb4a6ed"
                    }
                }));
    }

    public static void ConfigurarWireMockSmsPlantilla(BasePruebasIntegracion basePruebasIntegracion)
    {
        basePruebasIntegracion.MockServer
            .Given(Request.Create()
                .WithPath("/api/v1/notification/templates")
                .WithParam("name", "CambioEstatusSolicitudSms")
                .WithParam("platformName", "GestionCasosAPI")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBodyAsJson(new
                {
                    response = new
                    {
                        success = true,
                        code = 0,
                        message = "OK",
                        date = "2024-02-23T12:15:49.3193726+00:00"
                    },
                    pagination = new
                    {
                        page = 1,
                        pageSize = 50,
                        pageCount = 4,
                        totalPages = 1,
                        totalCount = 4
                    },
                    data = new[]
                    {
                    new
                    {
                        templateId = "5ffc5f29-cad4-451d-b6be-a1fbd4bca84f",
                        name = "CambioEstatusSolicitudSms",
                        subject = "Cambio de estado Solicitud - JIRA",
                        language = "Es",
                        notificationType = "SMS",
                        platformName = "GestionCasosAPI",
                        metadata = new[]
                        {
                            new { key = "nombreIntermediario", description = "Nombre del destinatario del SMS", isRequired = true },
                            new { key = "numeroSolicitud", description = "Número de la solicitud de JIRA", isRequired = true },
                            new { key = "estatusSolicitud", description = "Estatus al que pasó la solicitud", isRequired = true },
                            new { key = "enlace", description = "Enlace para ver el detalle de la solicitud", isRequired = true }
                        },
                        labels = Array.Empty<string>()
                    }
                }
                }));
    }

    public static void ConfigurarWireMockCorreoPlantilla(BasePruebasIntegracion basePruebasIntegracion)
    {
        basePruebasIntegracion.MockServer
                .Given(Request.Create()
                    .WithPath("/api/v1/notification/templates")
                    .WithParam("name", "CambioEstatusSolicitudCorreo")
                    .WithParam("platformName", "GestionCasosAPI")
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        response = new
                        {
                            success = true,
                            code = 0,
                            message = "OK",
                            date = "2024-02-23T12:15:49.3193726+00:00"
                        },
                        pagination = new
                        {
                            page = 1,
                            pageSize = 50,
                            pageCount = 4,
                            totalPages = 1,
                            totalCount = 4
                        },
                        data = new[]
                        {
                        new
                        {
                            templateId = "5ffc5f29-cad4-451d-b6be-a1fbd4bca84f",
                            name = "CambioEstatusSolicitudCorreo",
                            subject = "Cambio de estado Solicitud - JIRA",
                            language = "Es",
                            notificationType = "Email",
                            platformName = "GestionCasosAPI",
                            metadata = new[]
                            {
                                new { key = "nombreIntermediario", description = "Nombre del destinatario del correo", isRequired = true },
                                new { key = "numeroSolicitud", description = "Número de la solicitud de JIRA", isRequired = true },
                                new { key = "estatusSolicitud", description = "Estatus al que pasó la solicitud", isRequired = true },
                                new { key = "enlace", description = "Enlace para ver el detalle de la solicitud", isRequired = true }
                            },
                            labels = new string[] { }
                        }
                    }
                    }));
    }


    public static void ConfigurarWireMockEmailNotificacion(BasePruebasIntegracion basePruebasIntegracion)
    {
        basePruebasIntegracion.MockServer
            .Given(Request.Create()
                .WithPath("/api/v1/notification/email/send")
                .UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBodyAsJson(new
                {
                    response = new
                    {
                        success = true,
                        code = 0,
                        message = "Email enviado correctamente usando SendGrid",
                        date = "2024-02-23T12:06:06.0227951+00:00"
                    },
                    data = new
                    {
                        notificationId = "294f9d29-f66b-4a51-81f2-e63f3cb4a6ed"
                    }
                }));
    }
}

