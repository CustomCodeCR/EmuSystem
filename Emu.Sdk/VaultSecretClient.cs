using System.Net.Http.Headers;
using System.Net.Http.Json;
using Sdk.Exceptions;
using Sdk.Requests;
using Sdk.Responses;

namespace Sdk;

public sealed class VaultSecretClient
{
    private readonly HttpClient _httpClient;

    public VaultSecretClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public void SetApiKey(string apiKey)
    {
        _httpClient.DefaultRequestHeaders.Remove("X-Api-Key");
        _httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
    }

    public void SetBearerToken(string accessToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            accessToken
        );
    }

    public Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default) =>
        PostAsync<LoginRequest, LoginResponse>("/api/auth/login", request, ct);

    public Task<CreateUserResponse> CreateUserAsync(
        CreateUserRequest request,
        CancellationToken ct = default
    ) => PostAsync<CreateUserRequest, CreateUserResponse>("/api/users", request, ct);

    public Task<IReadOnlyList<UserListItemResponse>> ListUsersAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) => GetAsync<IReadOnlyList<UserListItemResponse>>($"/api/users?tenantId={tenantId}", ct);

    public Task<CreateTenantResponse> CreateTenantAsync(
        CreateTenantRequest request,
        CancellationToken ct = default
    ) => PostAsync<CreateTenantRequest, CreateTenantResponse>("/api/tenants", request, ct);

    public Task<GetTenantResponse> GetTenantAsync(Guid id, CancellationToken ct = default) =>
        GetAsync<GetTenantResponse>($"/api/tenants/{id}", ct);

    public Task<IReadOnlyList<TenantListItemResponse>> ListTenantsAsync(
        CancellationToken ct = default
    ) => GetAsync<IReadOnlyList<TenantListItemResponse>>("/api/tenants", ct);

    public Task<CreateProjectResponse> CreateProjectAsync(
        CreateProjectRequest request,
        CancellationToken ct = default
    ) => PostAsync<CreateProjectRequest, CreateProjectResponse>("/api/projects", request, ct);

    public Task<IReadOnlyList<ProjectListItemResponse>> ListProjectsAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) => GetAsync<IReadOnlyList<ProjectListItemResponse>>($"/api/projects?tenantId={tenantId}", ct);

    public Task<CreateEnvironmentResponse> CreateEnvironmentAsync(
        CreateEnvironmentRequest request,
        CancellationToken ct = default
    ) =>
        PostAsync<CreateEnvironmentRequest, CreateEnvironmentResponse>(
            "/api/environments",
            request,
            ct
        );

    public Task<IReadOnlyList<EnvironmentListItemResponse>> ListEnvironmentsAsync(
        Guid projectId,
        CancellationToken ct = default
    ) =>
        GetAsync<IReadOnlyList<EnvironmentListItemResponse>>(
            $"/api/environments?projectId={projectId}",
            ct
        );

    public Task<CreateApiKeyResponse> CreateApiKeyAsync(
        CreateApiKeyRequest request,
        CancellationToken ct = default
    ) => PostAsync<CreateApiKeyRequest, CreateApiKeyResponse>("/api/api-keys", request, ct);

    public Task<IReadOnlyList<ApiKeyListItemResponse>> ListApiKeysAsync(
        Guid tenantId,
        CancellationToken ct = default
    ) => GetAsync<IReadOnlyList<ApiKeyListItemResponse>>($"/api/api-keys?tenantId={tenantId}", ct);

    public Task<DisableApiKeyResponse> DisableApiKeyAsync(
        Guid id,
        CancellationToken ct = default
    ) => PostAsync<object, DisableApiKeyResponse>($"/api/api-keys/{id}/disable", new { }, ct);

    public Task<CreateAccessPolicyResponse> CreateAccessPolicyAsync(
        CreateAccessPolicyRequest request,
        CancellationToken ct = default
    ) =>
        PostAsync<CreateAccessPolicyRequest, CreateAccessPolicyResponse>(
            "/api/policies",
            request,
            ct
        );

    public Task<IReadOnlyList<AccessPolicyListItemResponse>> ListAccessPoliciesByApiKeyAsync(
        Guid apiKeyId,
        CancellationToken ct = default
    ) =>
        GetAsync<IReadOnlyList<AccessPolicyListItemResponse>>(
            $"/api/policies/by-api-key/{apiKeyId}",
            ct
        );

    public Task<CreateSecretResponse> CreateSecretAsync(
        CreateSecretRequest request,
        CancellationToken ct = default
    ) => PostAsync<CreateSecretRequest, CreateSecretResponse>("/api/secrets", request, ct);

    public Task<IReadOnlyList<SecretListItemResponse>> ListSecretsAsync(
        Guid environmentId,
        CancellationToken ct = default
    ) =>
        GetAsync<IReadOnlyList<SecretListItemResponse>>(
            $"/api/secrets?environmentId={environmentId}",
            ct
        );

    public Task<GetSecretResponse> GetSecretByPathAsync(
        Guid environmentId,
        string path,
        CancellationToken ct = default
    )
    {
        var url =
            $"/api/secrets/by-path?environmentId={environmentId}&path={Uri.EscapeDataString(path)}";
        return GetAsync<GetSecretResponse>(url, ct);
    }

    public Task<RotateSecretResponse> RotateSecretAsync(
        Guid id,
        RotateSecretRequest request,
        CancellationToken ct = default
    ) =>
        PostAsync<RotateSecretRequest, RotateSecretResponse>(
            $"/api/secrets/{id}/rotate",
            request,
            ct
        );

    public Task<DeleteSecretResponse> DeleteSecretAsync(Guid id, CancellationToken ct = default) =>
        DeleteAsync<DeleteSecretResponse>($"/api/secrets/{id}", ct);

    public Task<IReadOnlyList<AuditLogListItemResponse>> ListAuditLogsAsync(
        Guid tenantId,
        int page = 1,
        int pageSize = 50,
        CancellationToken ct = default
    )
    {
        return GetAsync<IReadOnlyList<AuditLogListItemResponse>>(
            $"/api/audit-logs?tenantId={tenantId}&page={page}&pageSize={pageSize}",
            ct
        );
    }

    private async Task<TResponse> GetAsync<TResponse>(string url, CancellationToken ct)
    {
        using var response = await _httpClient.GetAsync(url, ct);
        return await ReadAsync<TResponse>(response, ct);
    }

    private async Task<TResponse> PostAsync<TRequest, TResponse>(
        string url,
        TRequest request,
        CancellationToken ct
    )
    {
        using var response = await _httpClient.PostAsJsonAsync(url, request, ct);
        return await ReadAsync<TResponse>(response, ct);
    }

    private async Task<TResponse> DeleteAsync<TResponse>(string url, CancellationToken ct)
    {
        using var response = await _httpClient.DeleteAsync(url, ct);
        return await ReadAsync<TResponse>(response, ct);
    }

    private static async Task<TResponse> ReadAsync<TResponse>(
        HttpResponseMessage response,
        CancellationToken ct
    )
    {
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(ct);
            throw new VaultSecretApiException((int)response.StatusCode, body);
        }

        var result = await response.Content.ReadFromJsonAsync<TResponse>(ct);

        return result
            ?? throw new InvalidOperationException("VaultSecret API returned an empty response.");
    }
}
