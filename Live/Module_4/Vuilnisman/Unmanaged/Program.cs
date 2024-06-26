namespace Unmanaged;

internal class Program
{
    static UnResource r1 = new UnResource();
    static UnResource r2 = new UnResource();

    static void Main(string[] args)
    {
        using(r1)
        {
            r1 = new UnResource();
            r1.Open();
            //r1.Dispose();
            r1 = null;
        }

        using(UnResource r3 = new UnResource())
        {
            r3.Open();  
        }
        //GC.Collect();
        //GC.WaitForPendingFinalizers();

        r2 = new UnResource();
        r2.Open();
        //r2.Dispose();
        r2 = null;
        //Console.ReadLine();

        GC.Collect();
        GC.WaitForPendingFinalizers();


        Console.ReadLine();
    }
}
