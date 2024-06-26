using System.Diagnostics;
using System.Text;

namespace Vullis;

internal class Program
{
    static void Main(string[] args)
    {
        Console.ReadLine();
        string s = "";
        //StringBuilder s = new StringBuilder();

        Stopwatch sw = new Stopwatch();
        sw.Start();
        for (int i = 0;i < 100000;i++)
        {
            s += i;//.ToString();
            //.Append(i.ToString());
        }
        sw.Stop();

        Console.WriteLine(sw.Elapsed);

        Console.ReadLine();
    }
}
