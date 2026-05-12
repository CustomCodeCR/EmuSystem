using System.Security.Cryptography;
using System.Text;
using Application.Abstractions.Crypto;

namespace Infrastructure.Crypto;

public sealed class SecretEncryptionService : ISecretEncryptionService
{
    private const int NonceSize = 12;
    private const int TageSize = 16;
    private const string Algorithm = "AES-256-GCM";

    private readonly IMasterKeyProvider _masterKeyProvider;

    public SecretEncryptionService(IMasterKeyProvider masterKeyProvider)
    {
        _masterKeyProvider = masterKeyProvider;
    }

    public EncryptionSecret Encrypt(string plainText)
    {
        var key = _masterKeyProvider.GetMasterKey();

        byte[] nonce = RandomNumberGenerator.GetBytes(NonceSize);
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] cipherBytes = new byte[plainBytes.Length];
        byte[] tag = new byte[TageSize];

        using var aes = new AesGcm(key, TageSize);
        aes.Encrypt(nonce, plainBytes, cipherBytes, tag);

        return new EncryptionSecret(
            Convert.ToBase64String(cipherBytes),
            Convert.ToBase64String(nonce),
            Convert.ToBase64String(tag),
            Algorithm
        );
    }

    public string Decrypt(EncryptionSecret encryptionSecret)
    {
        var key = _masterKeyProvider.GetMasterKey();

        byte[] cipherBytes = Convert.FromBase64String(encryptionSecret.EncryptionValue);
        byte[] nonce = Convert.FromBase64String(encryptionSecret.Nonce);
        byte[] tag = Convert.FromBase64String(encryptionSecret.Tag);

        byte[] plainBytes = new byte[cipherBytes.Length];

        using var aes = new AesGcm(key, TageSize);
        aes.Decrypt(nonce, cipherBytes, tag, plainBytes);

        return Encoding.UTF8.GetString(plainBytes);
    }
}
