using System.Collections.Concurrent;

namespace Tasks;

internal class Program
{
    static async Task Main(string[] args)
    {
        //Synchronous();
        //AsynchronousOld();
        //AsynchronousNew();
        //AsynchronousErrorHandling();
        //AsynchronousCancelling();
        //await AsynchronousAwaitAsync();
        //Locking();
        //Locking();
        //Semaphoring();
        //Blocking();
        //CountDown();
        GreatBarrier();
        Console.WriteLine("Back to main!!!");
        Console.ReadLine();
    }

    private static void Synchronous()
    {
        int result = LongAdd(2, 3);
        Console.WriteLine($"The answer is {result}");
    }
    private static void AsynchronousOld()
    {
        // Doesn't work anymore in .NET core
        var f1 = new Func<int, int, int>(LongAdd);
        var asyncResult = f1.BeginInvoke(4, 5, Callback, f1);
        Console.WriteLine("Continue...");

        void Callback(IAsyncResult ar)
        {
            var f2 = ar.AsyncState as Func<int, int, int>;
            var result = f2?.EndInvoke(ar);
            Console.WriteLine($"The answer is {result}");
        }
    }
    private static void AsynchronousNew()
    {
        Task<int> t1 = new Task<int>(()=>LongAdd(7,8));
        t1.Start();
        var result = t1.Result; // Blocking!!!
        Console.WriteLine($"The answer is {result}");

        Task.Run(() => LongAdd(9, 10))
            .ContinueWith(t => Console.WriteLine($"The answer is {t.Result}"));
    }
    private static void AsynchronousErrorHandling()
    {
        Task.Run(() => {
            Task.Delay(1000).Wait();
            throw new Exception("Ooops");
        }).ContinueWith(t => { 
                if (t.Exception != null)
                 {
                     foreach(var e in t.Exception.InnerExceptions)
                     {
                         Console.WriteLine(e.Message);
                     }
                 }
             });
    }
    private static void AsynchronousCancelling()
    {
        var nikko = new CancellationTokenSource();
        var token = nikko.Token;
        Task.Run(() => { 
            for(var x = 0; x < 100; x++)
            {
                Task.Delay(1000).Wait();
                Console.WriteLine($"Iteration {x}"); 
                if (token.IsCancellationRequested) 
                {
                    Console.WriteLine("Cancelling...");
                    return;
                }
            }
        });

        nikko.CancelAfter(15000);
        Console.WriteLine("Done!");
    }
    private static async Task AsynchronousAwaitAsync()
    {
        var t1 = Task.Run(() => LongAdd(11, 12));
        int result=await t1;
        Console.WriteLine($"The answer is {result}");

        result = await LongAddAsync(13,14);
        Console.WriteLine($"The answer is {result}");
    }

    static int LongAdd(int a, int b)
    {
        Task.Delay(5000).Wait();
        return a + b; 
    }
    static Task<int> LongAddAsync(int a, int b)
    {
        return Task.Run(() => LongAdd(a, b));
    }

    static int counter = 0;
    static object stick = new object();
    private static void Locking()
    {      
        Parallel.For(0, 10, x => {
            Task.Delay(1000).Wait();
            lock (stick)
            {
                int tmp = counter;
                tmp++;
                Task.Delay(300).Wait();
                counter = tmp;
            }
            Console.WriteLine(counter);
        });
    }
    private static void Semaphoring()
    {
        var rnd = new Random();
        var garage = new SemaphoreSlim(5, 5);
        
        Parallel.For(0, 20, x => Car(x));
        
        void Car(int id)
        {
            Console.WriteLine($"Car {id} is waiting for the garage...");
            garage.Wait();
            Console.WriteLine($"Car {id} is entering the garage");
            Task.Delay(5000 + rnd.Next(1000, 5000)).Wait();
            Console.WriteLine($"Car {id} is leaving the garage");
            garage.Release();
        }
    }
    private static async void Blocking()
    {
        //AutoResetEvent zaklamp = new AutoResetEvent(false);
        int a = 0;
        int b = 0;

        var t1 = Task.Run(() => {
            Task.Delay(2000).Wait();
            a = 10;
           //zaklamp.Set();
        });
        var t2 = Task.Run(() => {
            Task.Delay(3000).Wait();
            b = 20;
            //zaklamp.Set();
        });
        // zaklamp.WaitOne();
        // zaklamp.WaitOne();
        Task.WaitAll(t1, t2);
        //await Task.WhenAll(t1, t2); 
        var result = a + b;
        Console.WriteLine($"The answer is {result}");
    }
    private static void CountDown()
    {
        var cde = new CountdownEvent(10);

        for(int i = 0; i < 10; i++) {
            Task.Run(() => {
                Task.Delay(1000).Wait();
                Console.WriteLine($"Task {Thread.CurrentThread.ManagedThreadId} has finished.");
                cde.Signal();
            });
        }

        cde.Wait();
        Console.WriteLine("And we continue");     
    }
    private static void GreatBarrier()
    {
        var rnd = new Random();
        var barrier = new Barrier(10);

        for (int i = 0; i < 10; i++)
        {
            Task.Run(() => {
                Task.Delay(1000 + rnd.Next(1000, 5000)).Wait();
                Console.WriteLine($"Task {Thread.CurrentThread.ManagedThreadId} has finished. Waiting for other tasks.");
                barrier.SignalAndWait();
                Console.WriteLine("Yoohoo!!!");
            });
        }

        Console.WriteLine("And we continue");
    }
}