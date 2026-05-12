namespace Application.Abstractions.Crypto;

public interface IMasterKeyProvider
{
    byte[] GetMasterKey();
}
