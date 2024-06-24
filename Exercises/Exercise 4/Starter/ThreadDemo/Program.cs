using System.Threading.Channels;

namespace ThreadDemo;

internal class Program
{
    static void Main(string[] args)
    {
        //CallSynchronous();
        //CallAsynchronousMetPuntjes();
        //CallAsynchronous();
        //HetWordtMooier();
        //Fouten();
        //CancellationTokenSource nikko = new CancellationTokenSource();
        //Cancellen(nikko.Token);
        //Task.Delay(5000).Wait();    
        //nikko.Cancel();
        //nikko.CancelAfter(6000);
        //EchtAsync();
        Ooops();
        //foreach(int x in MyCollection()) { Console.WriteLine(x); }

        Console.WriteLine("End Program");
        Console.ReadLine();
    }

    static object stokje = new object();

    private static void Ooops()
    {
        int counter = 0;

        Parallel.For(0, 10, idx => {
            lock (stokje)
            {
                int sub = counter;
                sub++;
                Task.Delay(200).Wait();
                counter = sub;
                Console.WriteLine(counter);
            }
        });
    }

    private async static void EchtAsync()
    {
        int a = 0;
        int b = 0;

        //var lamp1 = new ManualResetEvent(false);
        //var lamp2 = new ManualResetEvent(false);

        var t1 = Task.Run(() =>
        {
            Task.Delay(1000).Wait();
            a = 42;
            //lamp1.Set();
        });
        var t2 = Task.Run(() =>
        {
            Task.Delay(2000).Wait();
            b = 10;
            //lamp2.Set();
        });

        //WaitHandle.WaitAny(new WaitHandle[] { lamp1, lamp2 });
        await Task.WhenAll(t1, t2);
        int result = a + b;
        Console.WriteLine(result);


    }

    private static void Cancellen(CancellationToken token)
    {
        CancellationToken bommetje = token;
        Task.Run(() =>
        {
            for (int i = 0; i < 1000; i++)
            {
                Task.Delay(1000).Wait();
                Console.WriteLine(i);
                if (bommetje.IsCancellationRequested)
                {
                    Console.WriteLine("Doei!!");
                    break;
                }
            }
        });
    }

    private async static void Fouten()
    {
        try
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Iets");
                Task.Delay(1000).Wait();
                throw new Exception("Ooops!");
            });//.ContinueWith(pt=> { 
            //    if (pt.Exception != null) Console.WriteLine(pt.Exception.Message);
            //    Console.WriteLine(pt.Status);
            //});
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static IEnumerable<int> MyCollection()
    {
        Console.WriteLine("Eerste");
        yield return 1;
        Console.WriteLine("Volgende");
        yield return 2;
        Console.WriteLine("Volgende");
        yield return 3;
    }

    private static async void HetWordtMooier()
    {
        Console.WriteLine("We beginnen");
        var t1 = new Task<int>(() =>LongAdd(4, 5));
        t1.Start();
        int result = await t1;
        Console.WriteLine(result);
        Console.WriteLine("En de volgende");

        var t2 = new Task<int>(() => LongAdd(41, 15));
        t2.Start();
        result = await t2;
        Console.WriteLine(result);

        await Task.Delay(2000);
        Console.WriteLine("Klaar");
    }

    static void CallAsynchronous()
    {
        Task<int> t1 = new Task<int>(() =>
        {
            return LongAdd(4, 5);
        });

        t1.ContinueWith(pt => {
            int result = pt.Result;
            Console.WriteLine(result);
        } ).ContinueWith(pt=>Console.WriteLine($"{pt.Status}"));

        t1.ContinueWith(pt => { Console.WriteLine("Doe Iets"); });


        t1.Start();
    }
    static void CallAsynchronousMetPuntjes()
    {
        // Ouwe meuk
        //Func<int, int, int> del= LongAdd;
        //del.BeginInvoke(2, 3, ar =>
        //{
        //    int result = del.EndInvoke(ar);
        //    Console.WriteLine(result);
        //}, null);

        
        Task<int> t1 = new Task<int>(() => { 
            return LongAdd(4, 5);
        });
        t1.Start();
        while (!t1.IsCompleted)
        {
            Console.Write(".");
            Task.Delay(100).Wait();
        }
        Console.WriteLine();

        int result = t1.Result;
        Console.WriteLine(result);
    }
    static void CallSynchronous()
    {
        int result = LongAdd(1, 2);
        Console.WriteLine(result);
    }

    static int LongAdd(int a, int b)
    {
        Task.Delay(10000).Wait();
        return a + b;
    }
}