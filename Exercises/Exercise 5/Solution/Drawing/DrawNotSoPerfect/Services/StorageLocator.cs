using Shapes;
using System.Reflection;

namespace DrawNotSoPerfect.Services;

internal class StorageLocator
{
    private readonly string _pluginPath;
    public event EventHandler Changed;

    public StorageLocator(string pluginPath)
    {
        _pluginPath = pluginPath;
    }
    public List<IStorage> StorageOptions { get; set; } = new List<IStorage>();

    public void ScanFolder()
    {
        LoadAssembles();
        var watcher = new FileSystemWatcher();
        watcher.Path = _pluginPath;
        watcher.Filter = "*.dll";
        watcher.Created += Watcher_Changed;
        watcher.Deleted += Watcher_Changed;
        watcher.EnableRaisingEvents = true;     
    }

    private void Watcher_Changed(object sender, FileSystemEventArgs e)
    {
        LoadAssembles();       
    }

    private void LoadAssembles()
    {
        StorageOptions.Clear();
        var files = Directory.GetFiles(_pluginPath);
        // TODO 1
        // Run the application and check under the menu item 
        // Options->Save Format. There should be at least a JSON option.

        // TODO 2
        // Read all dll's (assemblies) in the folder _pluginPath
        // Search for types that implement the interface IStorage
        // If found create an instance of that type and add the
        // instance to the list StorageOptions.
        foreach (var file in files)
        {
            var asm = Assembly.LoadFile(file);
            if (asm != null)
            {
                foreach (var type in asm.GetTypes())
                {
                    if (type.GetInterface(nameof(IStorage)) != null)
                    {
                        var obj = Activator.CreateInstance(type) as IStorage;
                        if (obj != null)
                            StorageOptions.Add(obj);
                    }
                }
            }
        }

        // TODO 3
        // Test the application by copy the XmlShapeSerializer.dll (from project XmlShapeSerializer)
        // to the plugin folder in the execution folder of DrawNotSoPerfect.exe
        // Check in the menubar under the Options->Save Format if the XML format option appears.
        Changed?.Invoke(this, EventArgs.Empty);
    }
}
