// See https://aka.ms/new-console-template for more information

class Program
{
    static void Main(string[] args)
    {
        foreach(int nr in Numbers())
        {
            Console.WriteLine(nr);
        }
    }

    static IEnumerable<int> Numbers()
    {
        Console.WriteLine("Eerste");
        yield return 1;
        Console.WriteLine("Tweede");
        yield return 2;
        Console.WriteLine("Derde");
        yield return 3;
        Console.WriteLine("Vierde");
        yield return 4;
    }
}

