namespace Sdk.Exceptions;

public sealed class VaultSecretApiException : Exception
{
    public int StatusCode { get; }

    public string? ResponseBody { get; }

    public VaultSecretApiException(int statusCode, string? responseBody)
        : base($"VaultSecret API request failed with status code {statusCode}.")
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }
}
