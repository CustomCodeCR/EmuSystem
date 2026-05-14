namespace Application.Abstractions.Auth;

public interface IApiKeyGenerator
{
    GeneratedApiKey Generate();
}

public sealed record GeneratedApiKey(string Prefix, string PlainTextKey);
