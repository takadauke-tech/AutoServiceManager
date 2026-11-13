using System.IO;
using System.Text.Json;
using AutoServiceManager.Models;

namespace AutoServiceManager.Services;

public class JsonDataService
{
    private readonly string _folder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "AutoServiceManager");
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public JsonDataService()
    {
        Directory.CreateDirectory(_folder);
    }

    private string PathFor(string name) => Path.Combine(_folder, name + ".json");

    public List<T> Load<T>(string name)
    {
        string path = PathFor(name);
        if (!File.Exists(path)) return new List<T>();
        try
        {
            return JsonSerializer.Deserialize<List<T>>(File.ReadAllText(path), _options) ?? new List<T>();
        }
        catch
        {
            return new List<T>();
        }
    }

    public void Save<T>(string name, IEnumerable<T> items)
    {
        File.WriteAllText(PathFor(name), JsonSerializer.Serialize(items, _options));
    }
}
