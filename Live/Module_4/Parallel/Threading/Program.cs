using System.Collections.Concurrent;

namespace Threading;

internal class Program
{
    static async Task Main(string[] args)
    {
        // Synchrone();
        // AsynchThreading();
        // ApmThreading();
        // ThreadPoolThread();
        //TaskAsyncThreading();
        //AsyncExceptions();
        //Cancelen();
        //await MooiAsync();
        //ParallelAsync();
        //MoreSyncing();
        //DeGarage();
        //SharingIsCaring();

        //BlockingCollection
        Console.WriteLine("Einde!");
        Console.ReadLine();
    }

    static object stokje = new object();
    private static void SharingIsCaring()
    {
        int counter = 0;
        //Mutex mutex = new Mutex();

        Parallel.For(0, 10, idx => {
            //Monitor.Enter(stokje);
            lock(stokje)
            {
                int tmp = counter;
                Task.Delay(50).Wait();
                counter = ++tmp;
                Console.WriteLine(counter);
            }
            
            //Monitor.Exit(stokje);
        });
    }

    private static void DeGarage()
    {
        Semaphore garage = new Semaphore(10, 10);
        Barrier barrier = new Barrier(50);
        Parallel.For(0, 50, id => {
            barrier.SignalAndWait();
            Console.WriteLine($"Auto {Thread.CurrentThread.ManagedThreadId} voor de poort");
            garage.WaitOne();
            Console.WriteLine($"Auto {Thread.CurrentThread.ManagedThreadId} rijdt naar binnen");
            Task.Delay(10000 + Random.Shared.Next(1000, 5000)).Wait();
            garage.Release();
            Console.WriteLine($"Auto {Thread.CurrentThread.ManagedThreadId} rijdt naar buiten");
        });
    }

    private static void MoreSyncing()
    {

        Barrier barrier = new Barrier(50);
        Parallel.For(0, 50, id => {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Aan de start...");
            barrier.SignalAndWait();
            Console.WriteLine($"Task {Thread.CurrentThread.ManagedThreadId} is started");
            Task.Delay(1000).Wait();
        });
        Console.WriteLine("Allemaal klaar");
        //Parallel.For(0, 50, id => {
        //    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Aan de start...");
        //    barrier.SignalAndWait();
        //    Console.WriteLine($"Task {Thread.CurrentThread.ManagedThreadId} is started");
        //    Task.Delay(1000).Wait();
        //});
    }

    private static async Task ParallelAsync()
    {
        var zl1 = new ManualResetEvent(false);
        var zl2 = new ManualResetEvent(false);

        int a = 0;
        int b = 0;

        var t1 = Task.Run(async () =>
        {
            await Task.Delay(2000);
            a = 20;
            //zl1.Set();
        });
        var t2 = Task.Run(async () =>
        {
            await Task.Delay(3000);
            b = 80;
            //zl2.Set();
        });

        //WaitHandle.WaitAny([zl1, zl2]);
        //Task.WaitAll(t1, t2);
        await Task.WhenAll(t1, t2);
        Console.WriteLine(a + b);
    }

    private static async Task MooiAsync()
    {
        var res = await LongAddAsync(6, 7);
        Console.WriteLine(res);


        Task<int> t1 = new Task<int>(() => LongAdd(6, 7));
        t1.ContinueWith(pt => Console.WriteLine(pt.Result));
        t1.Start();
        await t1;

        var t2 = Task.Run(() => LongAdd(8, 9));
        int result = await t2;
        Console.WriteLine(result);

        try
        {
            await Task.Run(() =>
            {
                Task.Delay(100).Wait();
                throw new Exception("oooops");
            });
        }
        catch (AggregateException ex) {
            await Console.Out.WriteLineAsync(ex.Message);
        }
    }

    private static void Cancelen()
    {
        CancellationTokenSource nikko = new CancellationTokenSource();

        CancellationToken bommetje = nikko.Token;
        Task.Run(() => {
            int counter = 0;
            do
            {
                Task.Delay(1500).Wait();
                Console.WriteLine(++counter);
                if(bommetje.IsCancellationRequested)
                {
                    return;
                }
                //bommetje.ThrowIfCancellationRequested();
            }
            while(true);
        } ).ContinueWith(pt=>Console.WriteLine(pt.Status));

        //Task.Delay(5000).Wait();
        nikko.CancelAfter(5000);
    }

    private static void AsyncExceptions()
    {
        //try
        //{
        //    Task.Run(() =>
        //    {
        //        Task.Delay(100).Wait();
        //        throw new Exception("oooops");
        //    });
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine(ex.Message);
        //}


        Task.Run(() =>
            {
                Task.Delay(100).Wait();
                throw new Exception("oooops");
            }).ContinueWith(pt =>
            {
                Console.WriteLine(pt.Status);
                if (pt.Exception != null)
                {
                    Console.WriteLine(pt.Exception.InnerException?.Message);
                }
            });

    }

    private static void TaskAsyncThreading()
    {
        Task<int> t1 = new Task<int>(() => LongAdd(6, 7));
        t1.ContinueWith(pt => Console.WriteLine(pt.Result))
            .ContinueWith(pt => Console.WriteLine(pt.Status));
        t1.ContinueWith(pt => Console.WriteLine(pt.Status));
        t1.Start();

        var t2 = Task.Run(() => LongAdd(8, 9));

        // .Result is Blocking!!!!!
        // .Wait();  Ook blocking
       // Console.WriteLine(t1.Result);
    }

    private static void ThreadPoolThread()
    {
        // Korte taakjes
        // Wordt impicite gebruikt door
        // 1) APM Delegates
        // 2) Tasks
        // 3) ???
        ThreadPool.QueueUserWorkItem(obj =>
        {
            //Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
            int result = LongAdd(6, 7);
            Console.WriteLine(result);
        });
    }

    private static void ApmThreading()
    {
        Func<int, int, int> f1 = LongAdd;
        //int result = f1.Invoke(2, 3);

        IAsyncResult ar = f1.BeginInvoke(2, 3, ar => {
            Func<int, int, int>? f2 = ar.AsyncState as Func<int, int, int>;
            int result = f2.EndInvoke(ar);
            Console.WriteLine(result);
        }, f1);

        //int result = f1.EndInvoke(ar);
        //Console.WriteLine(result);
    }

    private static void AsynchThreading()
    {
        Thread t1 = new Thread(() =>
        {
            int result = LongAdd(1, 2);
            Console.WriteLine(result);
        });
        t1.Start();
    }

    private static void Synchrone()
    {
        int result = LongAdd(1, 2);
        Console.WriteLine(result);
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
}
