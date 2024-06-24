using System.IO.Compression;
using System.Text;

namespace Streaming;

internal class Program
{
    static void Main(string[] args)
    {
        //DifficultStreamWrite();
        //DifficultStreamRead();
        //EasyStreamWrite();
        //EasyStreamRead();
        WriteCompression();
        ReadCompression();
    }

    private static void DifficultStreamWrite()
    {
        File.Delete("file1.txt");
        var stream = File.Create("file1.txt");
        for (var i = 0; i < 1000; i++)
        {
            var buffer = Encoding.UTF8.GetBytes($"Hello World {i}\r\n");
            stream.Write(buffer, 0, buffer.Length);
        }
        stream.Flush();
        stream.Close();
    }

    private static void DifficultStreamRead()
    {
        var stream = File.OpenRead("file1.txt");
        var buffer = new byte[4];
        while (stream.Read(buffer, 0, buffer.Length) > 0)
        {
            var txt = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            Console.Write(txt);
            Array.Clear(buffer, 0, buffer.Length);
        }
        stream.Close();
    }

    private static void EasyStreamWrite()
    {
        File.Delete("file2.txt");
        var stream = File.Create("file2.txt");
        var writer = new StreamWriter(stream);
        for (var i = 0; i < 1000; i++)
        {
            writer.WriteLine($"Hello World {i}");
        }
        writer.Flush();
        writer.Close();
    }

    private static void EasyStreamRead()
    {
        var stream = File.OpenRead("file2.txt");
        var reader = new StreamReader(stream);
        var line = String.Empty;
        while ((line = reader.ReadLine()) != null)
        {
            Console.WriteLine(line);
        }
        reader.Close();
    }

    private static void WriteCompression()
    {
        File.Delete("file.zip");
        var stream = File.Create("file.zip");
        var zipper = new GZipStream(stream, CompressionMode.Compress);
        var writer = new StreamWriter(zipper);
        
        for (var i = 0; i < 1000; i++)
        {
            writer.WriteLine($"Hello World {i}");
        }
        writer.Flush();
        writer.Close();
    }
    private static void ReadCompression()
    {
        var stream = File.OpenRead("file.zip");
        var zipper = new GZipStream(stream, CompressionMode.Decompress);
        var reader = new StreamReader(zipper);
        var line = String.Empty;
        while ((line = reader.ReadLine()) != null)
        {
            Console.WriteLine(line);
        }
        reader.Close();
    }
}