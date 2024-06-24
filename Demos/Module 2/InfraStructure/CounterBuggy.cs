namespace InfraStructure;

public class CounterBuggy : ICounter
{
    private int _counter = 0;
    public void Increment()
    {
        _counter--;
    }

    public void Show()
    {
        Console.WriteLine($"Counter value is {_counter}");
    }
}
