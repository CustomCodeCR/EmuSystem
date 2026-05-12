namespace Application.Abstractions.Crypto;

public interface ISecretEncryptionService
{
    EncryptionSecret Encrypt(string plainText);
    string Decrypt(EncryptionSecret encryptionSecret);
}

public sealed record EncryptionSecret(
    string EncryptionValue,
    string Nonce,
    string Tag,
    string Algorithm
);
