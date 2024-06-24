namespace GarbageMan;

internal class Program
{
    static UnmanagedResource resource1 = new UnmanagedResource();
    static UnmanagedResource resource2 = new UnmanagedResource();

    static void Main(string[] args)
    {
        resource1.Open();
        resource1 = null;

        GC.Collect();
       // GC.WaitForPendingFinalizers();
        resource2.Open();
        resource2.Close();

        using (resource1 = new UnmanagedResource())
        {
            resource1.Open();
        }
        using (resource2 = new UnmanagedResource())
        {
            resource2.Open();
        }

    }
}