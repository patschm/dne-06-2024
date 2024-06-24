using Shapes;
using System.Xml.Serialization;

namespace XmlShapeSerializer;

public class XmlLocalFileStorage : IStorage
{
    private string? _currentFile = null;

    public string Name => "XML";

    public string TypeFilter => "XML files (*.xml)|*.xml";

    public List<Shape>? Open(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Shape>));
        
        using (var stream = File.OpenRead(path))
        using (var reader = new StreamReader(stream))
            return serializer.Deserialize(reader) as List<Shape>;
    }

    public void Save(List<Shape> shapes)
    { 
        if (_currentFile == null)
        {
            throw new FileNotFoundException();
        }
       SaveAs(_currentFile, shapes); 
    }

    public void SaveAs(string path, List<Shape> shapes)
    {
        _currentFile = path;
        XmlSerializer serializer = new XmlSerializer(typeof(List<Shape>));
        using (var stream = File.OpenWrite(_currentFile))
        using (var writer = new StreamWriter(stream))
            serializer.Serialize(writer, shapes);
    }
}
