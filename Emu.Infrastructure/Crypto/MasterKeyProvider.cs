using Application.Abstractions.Crypto;
using Microsoft.Extensions.Options;

namespace Infrastructure.Crypto;

public sealed class MasterKeyProvider : IMasterKeyProvider
{
    private readonly byte[] _masterKey;

    public MasterKeyProvider(IOptions<EncryptionOptions> options)
    {
        if (string.IsNullOrWhiteSpace(options.Value.MasterKey))
        {
            throw new InvalidOperationException("Master key is not configured.");
        }

        _masterKey = Convert.FromBase64String(options.Value.MasterKey);

        if (_masterKey.Length != 32)
        {
            throw new InvalidOperationException("Master key must be 32 bytes fro AES-256.");
        }
    }

    public byte[] GetMasterKey()
    {
        return _masterKey;
    }
}
