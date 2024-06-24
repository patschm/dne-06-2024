
namespace Shapes;

public interface IStorage
{
    string Name { get; }
    string TypeFilter { get; }
    void Save(List<Shape> shapes);
    void SaveAs(string path, List<Shape> shapes);
    List<Shape>? Open(string path);
}
