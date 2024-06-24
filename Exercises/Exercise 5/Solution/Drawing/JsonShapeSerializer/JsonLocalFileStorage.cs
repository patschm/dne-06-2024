using Newtonsoft.Json;
using Shapes;

namespace JsonShapeSerializer;

public class JsonLocalFileStorage : IStorage
{
    private string? _currentFile = null;

    public string Name => "JSON";

    public string TypeFilter => "Json files (*.json)|*.json";

    public List<Shape>? Open(string path)
    {
        JsonSerializer serializer = new JsonSerializer();
        serializer.TypeNameHandling = TypeNameHandling.All;
        using (var stream = File.OpenRead(path))
        using (var reader = new StreamReader(stream))
            return serializer.Deserialize(reader, typeof(List<Shape>)) as List<Shape>;
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
        JsonSerializer serializer = new JsonSerializer();
        serializer.TypeNameHandling = TypeNameHandling.All;
        using (var stream = File.OpenWrite(_currentFile))
        using (var writer = new StreamWriter(stream))
            serializer.Serialize(writer, shapes);
    }
}
