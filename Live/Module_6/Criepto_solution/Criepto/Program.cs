
using System.Globalization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Criepto;

internal class Program
{
    static void Main(string[] args)
    {
        //byte[] hash = Sender("Hello World");
        //Ontvanger("Hello World", hash);
        //byte[] hash = SenderSymmetric("Hello World");
        //OntvangerSymmetric("Hello World", hash);
        (string pub, byte[] sig) data = SenderAsymmetrisch("Hello World");
        OntvangerAsymmetrish("Hello World", data.sig, data.pub);

    }

    private static void Ontvanger(string v, byte[] hash)
    {
        SHA1 sha1 = SHA1.Create();
        byte[] hash2 = sha1.ComputeHash(Encoding.UTF8.GetBytes(v));
        Console.WriteLine(Convert.ToBase64String(hash2));
    }

    private static byte[] Sender(string v)
    {
        SHA1 sha1 = SHA1.Create();
        byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(v));
        Console.WriteLine(Convert.ToBase64String(hash));
        return hash;
    }

    private static byte[] SenderSymmetric(string v)
    {
        HMACSHA1 sha1 = new HMACSHA1();
        sha1.Key = Encoding.UTF8.GetBytes("MijnGeheim");
        byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(v));
        Console.WriteLine(Convert.ToBase64String(hash));
        return hash;
    }
    private static void OntvangerSymmetric(string v, byte[] hash)
    {
        HMACSHA1 sha1 = new HMACSHA1();
        sha1.Key = Encoding.UTF8.GetBytes("MijnGeheim");
        byte[] hash2 = sha1.ComputeHash(Encoding.UTF8.GetBytes(v));
        Console.WriteLine(Convert.ToBase64String(hash2));
    }

    private static void OntvangerAsymmetrish(string v, byte[] signature, string pubKey)
    {
        DSA dsa = DSA.Create();
        dsa.FromXmlString(pubKey);
        SHA1 sha1 = SHA1.Create();
        byte[] hash2 = sha1.ComputeHash(Encoding.UTF8.GetBytes(v));
        bool isOk = dsa.VerifyData(hash2, signature, HashAlgorithmName.SHA1);
        Console.WriteLine(isOk ? "Prima": "Noooooo");
    }

    private static (string pub, byte[] sign) SenderAsymmetrisch(string v)
    {
        DSA dsa = DSA.Create();
        string pubKey = dsa.ToXmlString(false);
        SHA1 sha1 = SHA1.Create();
        byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(v));
        byte[] sign = dsa.SignData(hash, HashAlgorithmName.SHA1);

        Console.WriteLine(Convert.ToBase64String(sign));
        return (pubKey, sign);
    }
}
