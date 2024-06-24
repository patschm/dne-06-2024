using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StromingsLeer;

internal class Program
{
    static void Main(string[] args)
    {
        //SchrijvenOuderwets();
        //ArchaischLezen();
        //SchrijvenNieuwerwets();
        //ModernLezen();
        //CompressedSchrijven();
       // CompressedLezen();

    }

    private static void CompressedLezen()
    {
        FileInfo file = new FileInfo(@"E:\text.zip");
        FileStream fs = file.Open(FileMode.Open);
        GZipStream zipper = new GZipStream(fs, CompressionMode.Decompress);
        StreamReader sr = new StreamReader(zipper);
        string? line = null;
 
        while ((line = sr.ReadLine()) != null)
        {
            Console.WriteLine(line);
        }
    }
    private static void CompressedSchrijven()
    {
        FileInfo file = new FileInfo(@"E:\text.zip");
        if (file.Exists)
        {
            file.Delete();
            return;
        }
        FileStream stream = file.Create();
        GZipStream zipper = new GZipStream(stream, CompressionMode.Compress);

        
        StreamWriter writer = new StreamWriter(zipper);
        for (int i = 0; i < 1000; i++)
        {
            writer.WriteLine($"Hello World {i}");
        }
        writer.Flush();
        stream.Close();
    }

    private static void ModernLezen()
    {
        FileInfo file = new FileInfo(@"E:\text.txt");
        FileStream fs = file.Open(FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        string? line = null;
        //Console.SetIn(sr);
        while((line = sr.ReadLine()) != null)
        {
            Console.WriteLine(line);
        }
    }

    private static void SchrijvenNieuwerwets()
    {
        FileInfo file = new FileInfo(@"E:\text2.txt");
        if (file.Exists)
        {
            file.Delete();
            return;
        }
        FileStream stream = file.Create();

        StreamWriter writer = new StreamWriter(stream);
        for (int i = 0; i < 1000; i++)
        {
            writer.WriteLine($"Hello World {i}");
        }
        stream.Close();
    }

    private static void ArchaischLezen()
    {
        FileInfo file = new FileInfo(@"E:\text.txt");
        FileStream fs = file.Open(FileMode.Open);

        byte[] buffer = new byte[4];
        int nrRead = 0;
        while ((nrRead = fs.Read(buffer)) > 0)
        {
            string s = Encoding.UTF8.GetString(buffer);
            Console.Write(s);
            Array.Clear(buffer, 0, nrRead);
        }
    }

    private static void SchrijvenOuderwets()
    {
        FileInfo file = new FileInfo(@"E:\text.txt");
        if (file.Exists)
        {
            file.Delete();
            return;
        }
        FileStream stream = file.Create();
        
        string data = "Hello World";
        for(int i = 0; i < 1000; i++) 
        {
            byte[] buffer = Encoding.UTF8.GetBytes($"{data} {i}\r\n");
         //stream.Seek(10, SeekOrigin.Begin);
            stream.Write(buffer);   
            
        }
        stream.Close();

    }
}
