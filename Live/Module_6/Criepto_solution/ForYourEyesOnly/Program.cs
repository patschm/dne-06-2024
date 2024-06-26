using System.Security.Cryptography;
using System.Text;

namespace ForYourEyesOnly;

internal class Program
{
    static byte[] key;
    static byte[] iv;
    static string publicKeyOntvanger;
    static string privateKeyOntvanger;

    static void Main(string[] args)
    {
        // Voordat de hele zaak gaat lopen, maakt de ontvanger een publ/priv keypaar aan.
        var rsa = RSA.Create();
        publicKeyOntvanger = rsa.ToXmlString(false);
        privateKeyOntvanger = rsa.ToXmlString(true);
        //Console.WriteLine(privateKeyOntvanger);

        //byte[] secrets = SenderSymmetric();
        //OntvangerSymmetric(secrets);
        byte[] secrets = SenderASymmetric();
        Console.WriteLine(Convert.ToBase64String(secrets));
        OntvangerASymmetric(secrets);
    }

    private static byte[] SenderASymmetric()
    {
        var rsa = RSA.Create();
        rsa.FromXmlString(publicKeyOntvanger);
       return  rsa.Encrypt(Encoding.UTF8.GetBytes("Hello World"), RSAEncryptionPadding.Pkcs1);
    }

    private static void OntvangerASymmetric(byte[] secrets)
    {
        var rsa = RSA.Create();
        rsa.FromXmlString(privateKeyOntvanger);
        byte[] origineel = rsa.Decrypt(secrets, RSAEncryptionPadding.Pkcs1);
        Console.WriteLine(Encoding.UTF8.GetString(origineel));
    }

    private static void OntvangerSymmetric(byte[] secrets)
    {
        Aes alg = Aes.Create();
        alg.Key = key;
        alg.IV = iv;
        alg.Mode = CipherMode.CBC;

        using MemoryStream mem = new MemoryStream(secrets);
        using CryptoStream crypt = new CryptoStream(mem, alg.CreateDecryptor(), CryptoStreamMode.Read);
        using (StreamReader rdr = new StreamReader(crypt))
        {
            Console.WriteLine(rdr.ReadToEnd());
        }
    }

    private static byte[] SenderSymmetric()
    {
        Aes alg = Aes.Create();
        key = alg.Key;
        iv = alg.IV;
        alg.Mode = CipherMode.CBC;
        //alg.KeySize = 32;
        //alg.Key = Encoding.UTF8.GetBytes("Test_12345");

        using MemoryStream mem = new MemoryStream();
        using CryptoStream crypt = new CryptoStream(mem, alg.CreateEncryptor(), CryptoStreamMode.Write);
        using (StreamWriter writer = new StreamWriter(crypt))
        {
            writer.WriteLine("Hello World");
        }

        //Console.WriteLine(Encoding.UTF8.GetString(mem.ToArray()));

        return mem.ToArray();
    }
}
