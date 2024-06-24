namespace InfraStructure;

public class Counter : ICounter
{
    private readonly IConfiguration _configuration;

    public Counter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private int _counter = 0;
    public void Increment()
    {
        _counter++;
    }

    public void Show()
    {
        var txt = _configuration.GetSection("Mijn").Value;
        Console.WriteLine($"{txt} Counter value is {_counter}");
    }
}
