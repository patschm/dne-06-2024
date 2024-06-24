using System.Xml.Serialization;

namespace SpecialeWriters;

[XmlRoot("Person")]
public class Person
{
    [XmlElement("name")]
    public string? Name { get; set; }
    [XmlElement("age")]
    public int Age { get; set; }


}
