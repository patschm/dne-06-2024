

using System.Data;
using System.IO.Compression;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SpecialeWriters;

internal class Program
{
    static List<Person> persons = new List<Person>();
    static void Main(string[] args)
    {
        FillList();
        //SchrijfAlsXml();
        //SchrijvenSimpel();
        LezenSimpel();
    }

    private static void LezenSimpel()
    {
        XmlSerializer seri = new XmlSerializer(typeof(Person));
        XmlReader reader = XmlReader.Create(@"E:\people.xml");
        //XmlDocument document = new XmlDocument ();
        //document.Load(reader);

        while (reader.ReadToFollowing("Person"))
        {
            Person? people = seri.Deserialize(reader.ReadSubtree()) as Person;
            Console.WriteLine(people.Name);
        }
        


        //foreach(Person p in people)
        //{
        //    Console.WriteLine(p.Name);
        //}



        reader.Close();
    }

    private static void SchrijvenSimpel()
    {
        XmlSerializer seri = new XmlSerializer(typeof(List<Person>));
        XmlWriter writer = XmlWriter.Create(@"E:\people.xml");
        seri.Serialize(writer, persons);
        writer.Close();
    }

    private static void SchrijfAlsXml()
    {
        FileStream fs = File.Create(@"E:\people.zzip");
        GZipStream zipper = new GZipStream(fs, CompressionMode.Compress);
        XmlWriter writer = XmlWriter.Create(zipper);
        writer.WriteProcessingInstruction("xml", "utf-8");
        writer.WriteStartElement("people");
        foreach (Person person in persons)
        {
            writer.WriteStartElement("person");
            writer.WriteStartElement("name");
            writer.WriteString(person.Name);
            writer.WriteEndElement();
            writer.WriteStartElement("age");
            writer.WriteString(person.Age.ToString());
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
        writer.Flush();
        writer.Close();
    }

    private static void FillList()
    {
        persons.Add(new Person { Name = "AAA", Age = 32 });
        persons.Add(new Person { Name = "BBB", Age = 12 });
        persons.Add(new Person { Name = "CCC", Age = 48 });
    }
}
