using GestionCasos.Domain.Enums;
using GestionCasos.Domain.Exceptions;
using System.Collections.Immutable;
using System.Globalization;

namespace GestionCasos.Domain.Entities;

public class Adjunto
{
    private readonly ImmutableDictionary<FormatoAdjuntoValido, string> FormatoAdjuntoValido;
    private const string applicationPdf = "application/pdf";
    private const string applicationDoc = "application/msword";
    private const string applicationDocx = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
    private const string applicationJpeg = "image/jpeg";
    private const string applicationPng = "image/png";
    private readonly CultureInfo culture = CultureInfo.GetCultureInfo("en-ES");

    public Adjunto()
    {
        FormatoAdjuntoValido = ImmutableDictionary.CreateRange(new Dictionary<FormatoAdjuntoValido, string>
    {
        { Enums.FormatoAdjuntoValido.PDF, applicationPdf },
        { Enums.FormatoAdjuntoValido.DOC, applicationDoc },
        { Enums.FormatoAdjuntoValido.DOCX, applicationDocx },
        { Enums.FormatoAdjuntoValido.JPEG, applicationJpeg },
        { Enums.FormatoAdjuntoValido.PNG, applicationPng }
    });
    }

    public static void VerificarStreamData(Stream streamData)
    {
        _ = streamData ?? throw new AdjuntoInvalidoException(string.Format(CultureInfo.CurrentCulture, "{0} inválido", nameof(streamData)));
    }

    public void VerificarFormatoValido(string formato)
    {
        if (string.IsNullOrEmpty(formato) || !FormatoAdjuntoValido.ContainsValue(formato.ToLower(culture)))
        {
            throw new AdjuntoInvalidoException(string.Format(CultureInfo.InvariantCulture, "{0} inválido", nameof(formato)));
        }
    }

    public void SetUrl(Uri url)
    {
        _ = url ?? throw new ArgumentNullException(nameof(url));
        Url = url;
    }

    public Uri? Url { get; set; }
}
