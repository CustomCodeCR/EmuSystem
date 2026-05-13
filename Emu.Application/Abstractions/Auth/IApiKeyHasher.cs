namespace Application.Abstractions.Auth;

public interface IApiKeyHasher
{
    string Hash(string apiKey);
    bool Verify(string apiKey, string hash);
}
