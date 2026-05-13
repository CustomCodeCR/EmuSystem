namespace Application.Abstractions.Auth;

public sealed class ApiKeyOptions
{
    public string Pepper { get; set; } = default!;
}
