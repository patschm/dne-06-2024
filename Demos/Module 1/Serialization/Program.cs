using System.Xml.Serialization;
using Newtonsoft.Json;

// Aliases
using TextSerializer = System.Text.Json.JsonSerializer;
using NewtonSerializer = Newtonsoft.Json.JsonSerializer;
using Newtonsoft.Json.Serialization;
using System.Text.Json;

namespace Serialization;

internal class Program
{
    static void Main(string[] args)
    {
        //XmlSerializers();
        JsonSerializers();
    }

    private static void XmlSerializers()
    {
        var p1 = new Person { Id = 1, FirstName = "Peter", LastName = "Hendriks", Age = 42 };
        p1.Introduce();

        File.Delete("people.xml");
        var stream = File.Create("people.xml");
        var serializer = new XmlSerializer(typeof(Person));
        serializer.Serialize(stream, p1);
        stream.Flush();
        stream.Close();

        Console.WriteLine("Press Enter to deserialize...");
        Console.ReadLine();
        stream = File.OpenRead("people.xml");
        var p2 = serializer.Deserialize(stream) as Person;
        p2?.Introduce();

    }

    private static void JsonSerializers()
    {
        var p1 = new Person { Id = 1, FirstName = "Peter", LastName = "Hendriks", Age = 42 };
        p1.Introduce();
        File.Delete("people2.json");
        File.Delete("people3.json");

        // using System.Text.Json.JsonSerializer aliased as TextSerializer
        var stream = File.Create("people2.json");
        var options = new JsonSerializerOptions();
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        
        TextSerializer.Serialize(stream, p1, options);
        stream.Flush();
        stream.Close();

        Console.WriteLine("Press Enter to deserialize...");
        Console.ReadLine();
        stream = File.OpenRead("people2.json");
        var p2=TextSerializer.Deserialize<Person>(stream, options);
        p2?.Introduce();

        // using System.Text.Json.JsonSerializer aliased as TextSerializer
        stream = File.Create("people3.json");
        var serializer = NewtonSerializer.Create();
        serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
        var wrt=new StreamWriter(stream);
        var jwrt = new JsonTextWriter(wrt);
        serializer.Serialize(jwrt, p1);
        jwrt.Flush();
        jwrt.Close();

        Console.WriteLine("Press Enter to deserialize...");
        Console.ReadLine();
        stream = File.OpenRead("people3.json");
        var rdr = new StreamReader(stream);
        var jrdr = new JsonTextReader(rdr);

        p2 = serializer.Deserialize<Person>(jrdr);
        p2?.Introduce();

    }
}