namespace GarbageMan;

internal class UnmanagedResource : IDisposable
{
    private static bool _isOpen = false;

    public void Open()
    {
        if (!_isOpen)
        {
            _isOpen = true;
            Console.WriteLine("Resource is open");
        }
        else
        {
            Console.WriteLine("Resource already in use");
        }    
    }
    public void Close()
    {
        Console.WriteLine("Closing Resource");
        _isOpen = false;
    }
    public void Dispose()
    {
        Console.WriteLine("Disposing Resource");
        _isOpen = false;
    }
    ~UnmanagedResource()
    {
        Console.WriteLine("Finalizing Resource");
        _isOpen = false;
    }
}
