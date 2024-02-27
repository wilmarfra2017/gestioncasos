using GestionCasos.Domain.Ports;
using System.Net.Http.Headers;
using System.Text;

namespace GestionCasos.Infrastructure.Helpers;
public class MicroHelper : IDisposable
{
    private readonly Uri BaseUrl;
    private readonly string Username;
    private readonly string Password;
    private readonly ILogMessageService _resourceManager;
    public MicroHelper(Uri baseUrl, string username, string password, ILogMessageService resourceManager)
    {
        BaseUrl = baseUrl;
        Username = username;
        Password = password;
        _resourceManager = resourceManager;
    }
    private HttpClient _client = default!;
    private bool _disposed;

    public HttpClient CreateHttpClient()
    {
        if (_client != null)
        {
            throw new InvalidOperationException(_resourceManager.ErrorValidacionHttp);
        }

        HttpClientHandler handler = new()
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        };

        _client = new HttpClient(handler)
        {
            BaseAddress = new Uri(BaseUrl.ToString())
        };
        string credentials = $"{Username}:{Password}";
        var base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);
        return _client;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _client?.Dispose();
            }

            _disposed = true;
        }
    }

    ~MicroHelper()
    {
        Dispose(false);
    }
}

