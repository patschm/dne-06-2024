using Newtonsoft.Json;
using Shapes;
using System.Security.Cryptography;

namespace DrawNotSoPerfect;

public class LocalFileStorage : IStorage
{
    private string? _currentFile = null;
    private byte[] KEY = new byte[] { 139, 176, 225, 182, 242, 4, 158, 72, 116, 220, 138, 55, 195, 165, 37, 90, 7, 39, 186, 16, 248, 209, 106, 231, 244, 229, 98, 56, 82, 152, 246, 220 };
    private byte[] IV = new byte[] { 85, 42, 134, 74, 83, 112, 9, 54, 240, 42, 168, 44, 241, 175, 52, 35 };

    public List<Shape>? Open(string path)
    {
        // TODO 2: Change this code to read the encrypted data.
        // Test the code
        var aes = Aes.Create();
        aes.Key = KEY;
        aes.IV = IV;
        JsonSerializer serializer = new JsonSerializer();
        serializer.TypeNameHandling = TypeNameHandling.All;
        using var stream = File.OpenRead(path);
        using var crypt = new CryptoStream(stream, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using var reader = new StreamReader(crypt);
            return serializer.Deserialize(reader, typeof(List<Shape>)) as List<Shape>;
    }

    public void Save(List<Shape> shapes)
    { 
        if (_currentFile == null)
        {
            throw new FileNotFoundException();
        }
       SaveAs(_currentFile, shapes); 
    }

    public void SaveAs(string path, List<Shape> shapes)
    {
        // TODO 1: Change this code to store the data encrypted using a symmetric algorithm.
        // Test the code and check that the data is encrypted.
        var aes = Aes.Create();
        aes.Key = KEY;
        aes.IV = IV;
        _currentFile = path;
        JsonSerializer serializer = new JsonSerializer();
        serializer.TypeNameHandling = TypeNameHandling.All;
        using var stream = File.OpenWrite(_currentFile);
        using var crypt = new CryptoStream(stream, aes.CreateEncryptor(), CryptoStreamMode.Write);
        using var writer = new StreamWriter(crypt);
            serializer.Serialize(writer, shapes);
    }
}
