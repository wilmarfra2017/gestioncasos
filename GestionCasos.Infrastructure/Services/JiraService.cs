using GestionCasos.Domain.Dtos;
using GestionCasos.Domain.Exceptions;
using GestionCasos.Domain.Ports;
using GestionCasos.Infrastructure.Adapters;
using GestionCasos.Infrastructure.Config;
using GestionCasos.Infrastructure.Helpers;
using GestionCasos.Infrastructure.Ports;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GestionCasos.Infrastructure.Services;
[Repository]
public class JiraService : IJiraService
{
    private readonly JiraConfig _config;
    private readonly Uri JIRA_ENDPOINT;
    private readonly Uri BASE_URL;
    private readonly ILogMessageService _resourceManager;
    private readonly JsonSerializerOptions serializeOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true,
    };

    public JiraService(JiraConfig config, ILogMessageService resourceManager)
    {
        _config = config;
        BASE_URL = _config.Url;
        JIRA_ENDPOINT = new Uri($"{_config.Url}{_config.Base_Endpoint}");
        _resourceManager = resourceManager;
    }

    public async Task<string> CreateIssueAsync(RequestSolicitudDto issue)
    {
        try
        {
            using var httpClient = new MicroHelper(BASE_URL, _config.Username, _config.Password, _resourceManager);
            using var client = httpClient.CreateHttpClient();
            var stringContent = new StringContent(JsonSerializer.Serialize(issue, serializeOptions), Encoding.UTF8, MediaTypeNames.Application.Json);
            var requestUri = JiraEndpointsHelper.CreateIssueUrl(JIRA_ENDPOINT);
            var response = await client.PostAsync(requestUri, stringContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (System.Exception ex)
        {
            throw new ReporteSolicitudException($"{_resourceManager.ErrorApiJira} {ex.Message}");
        }
    }
    public async Task<string> CreateIssueAsync(StringContent jsonString)
    {
        try
        {
            using var httpClient = new MicroHelper(BASE_URL, _config.Username, _config.Password, _resourceManager);
            using var client = httpClient.CreateHttpClient();
            var requestUri = JiraEndpointsHelper.CreateIssueUrl(JIRA_ENDPOINT);
            var response = await client.PostAsync(requestUri, jsonString);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (System.Exception ex)
        {
            throw new ReporteSolicitudException($"{_resourceManager.ErrorApiJira} {ex.Message}");
        }
    }

    public async Task<string> UpdateIssueAsync(string keyorid, StringContent jsonString)
    {
        try
        {
            using var httpClient = new MicroHelper(BASE_URL, _config.Username, _config.Password, _resourceManager);
            using var client = httpClient.CreateHttpClient();
            var requestUri = JiraEndpointsHelper.GetIssueUrl(JIRA_ENDPOINT, keyorid);
            var response = await client.PutAsync(requestUri, jsonString);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (System.Exception ex)
        {
            throw new ReporteSolicitudException($"{_resourceManager.ErrorApiJira}  {ex.Message}");
        }
    }
    public async Task<string> UpdateIssueAsync(string keyorid, RequestSolicitudDto issue)
    {
        try
        {
            using var httpClient = new MicroHelper(BASE_URL, _config.Username, _config.Password, _resourceManager);
            using var client = httpClient.CreateHttpClient();
            var requestUri = JiraEndpointsHelper.GetIssueUrl(JIRA_ENDPOINT, keyorid);
            var stringContent = new StringContent(JsonSerializer.Serialize(issue, serializeOptions), Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PutAsync(requestUri, stringContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (System.Exception ex)
        {
            throw new ReporteSolicitudException($"{_resourceManager.ErrorApiJira} {ex.Message}");
        }
    }

    public async Task<string> AddAttachmentAsync(string keyorid, string documentName, string documentPath)
    {
        try
        {
            var attachmentUrl = JiraEndpointsHelper.AddAttachmentUrl(JIRA_ENDPOINT, keyorid);
            var response = await PostDocumentAsync(attachmentUrl, documentName, documentPath);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (System.Exception ex)
        {
            throw new ReporteSolicitudException($"{_resourceManager.ErrorApiJira}  {ex.Message}");
        }
    }

    public async Task<string> GetIssueAsync(string keyorid)
    {
        try
        {
            using var httpClient = new MicroHelper(BASE_URL, _config.Username, _config.Password, _resourceManager);
            using var client = httpClient.CreateHttpClient();
            var requestUri = JiraEndpointsHelper.GetIssueUrl(JIRA_ENDPOINT, keyorid);
            var response = await client.GetAsync(requestUri).ConfigureAwait(false);
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return content;
        }
        catch (System.Exception ex)
        {
            throw new ReporteSolicitudException($"{_resourceManager.ErrorApiJira} {ex.Message}");
        }
    }


    public async Task<string> GetProjectAsync(string keyorid)
    {
        try
        {
            using var httpClient = new MicroHelper(BASE_URL, _config.Username, _config.Password, _resourceManager);
            using var client = httpClient.CreateHttpClient();
            var requestUri = JiraEndpointsHelper.GetProjectInfoUrl(JIRA_ENDPOINT, keyorid);
            var response = await client.GetAsync(requestUri).ConfigureAwait(false);
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return content;
        }
        catch (System.Exception ex)
        {
            throw new ReporteSolicitudException($"{_resourceManager.ErrorApiJira}  {ex.Message}");
        }
    }

    private async Task<HttpResponseMessage> PostDocumentAsync(Uri url, string documentName, string documentPath)
    {
        using var httpClient = new MicroHelper(BASE_URL, _config.Username, _config.Password, _resourceManager);
        using var client = httpClient.CreateHttpClient();
        using (var formData = new MultipartFormDataContent())
        {
            using (var fileStream = new FileStream(documentPath, FileMode.Open))
            {
                var fileContent = new StreamContent(fileStream);
                formData.Add(fileContent, documentName, Path.GetFileName(documentPath));
            }
            var response = await client.PostAsync(url, formData);
            return response;
        }
    }
}

