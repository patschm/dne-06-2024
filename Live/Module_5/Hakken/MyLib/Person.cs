namespace MyLib;

public class Person
{
    private int _age =0;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int Age 
    {
        get => _age;
        set
        { 
            if (value >= 0) _age = value; 
        }
    }

    public void Introduce()
    {
        Console.WriteLine($"{FirstName} {LastName} ({Age})");
    }
}
