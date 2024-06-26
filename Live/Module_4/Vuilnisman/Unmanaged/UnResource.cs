namespace Unmanaged;

internal class UnResource : IDisposable
{
    private static bool _isOpen = false;
    private FileStream? _stream = null;

    public void Open()
    {
        Console.WriteLine("Trying to open...");
        if (_isOpen)
        {
            Console.WriteLine("Already in use");
            return;
        }
        _stream = File.Open("bla.txt", FileMode.OpenOrCreate);
        _isOpen = true;
        Console.WriteLine("Is open");
    }

    public void Close()
    {
        Console.WriteLine("Closing...");
        _isOpen = false;
    }

    public void RuimOp(bool fromDispose)
    {
        Close();
        if (fromDispose)
        {
            _stream?.Dispose();
        }
    }
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        RuimOp(true);
    }

    ~UnResource()
    {
        RuimOp(false);
    }
}
