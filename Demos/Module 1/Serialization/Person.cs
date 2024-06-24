using System.Xml.Serialization;

namespace Serialization;

[XmlRoot("person")]
public class Person
{
    [XmlAttribute("id")]
    public int Id { get; set; }
    [XmlElement("first-name")]
    public string? FirstName { get; set; }
    [XmlElement("last-name")]
    public string? LastName { get; set; }
    [XmlElement("age")]
    public int Age { get; set; }

    public void Introduce()
    {
        Console.WriteLine($"Hello I'm {FirstName} {LastName} and I'm {Age} years old");
    }
}
