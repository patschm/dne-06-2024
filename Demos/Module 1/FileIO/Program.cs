using System.IO;

namespace FileIO;

internal class Program
{
    static void Main(string[] args)
    {
        //FileManipulation();
        DirectoryManipulation();
    }

    private static void FileManipulation()
    {
        // Static group
        var file = @$"{Environment.CurrentDirectory}\data.txt";
        if (!File.Exists(file))
        {
            var fstream = File.Create(file);
            fstream.Dispose();
            Console.WriteLine($"File {file} created at {File.GetCreationTime(file)}");
            Console.WriteLine($"Attributes: {File.GetAttributes(file)}");
        }
        else
        {
            File.Delete(file);
            Console.WriteLine($"{file} is deleted");
        }

        // Instance group
        file = @$"{Environment.CurrentDirectory}\data2.txt";
        FileInfo fileInf = new FileInfo(file);
        if (!fileInf.Exists)
        {
            var fstream = fileInf.Create();
            fstream.Dispose();
            Console.WriteLine($"File {fileInf.Name} created at {fileInf.CreationTime}");
            Console.WriteLine($"Attributes: {fileInf.Attributes}");
        }
        else
        {
            fileInf.Delete();
            Console.WriteLine($"{fileInf.Name} is deleted");
        }

    }

    private static void DirectoryManipulation()
    {
        // Static group
        var dirname = $@"{Environment.CurrentDirectory}\SomeDir";
        if (!Directory.Exists(dirname))
        {
            Directory.CreateDirectory(dirname);
            Console.WriteLine($"Directory {dirname} created");
        }
        else
        { 
            Directory.Delete(dirname);
            Console.WriteLine($"Directory {dirname} deleted");
        }

        // Instance group
        dirname = $@"{Environment.CurrentDirectory}\SomeDir2";
        DirectoryInfo dirInf = new DirectoryInfo(dirname);
        if (!dirInf.Exists)
        {
            dirInf.Create();
            Console.WriteLine($"Directory {dirInf.FullName} created");
        }
        else
        {
            dirInf.Delete();
            Console.WriteLine($"Directory {dirInf.FullName} deleted");
        }
    }

}