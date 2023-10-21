using System.Text.Json;

namespace VSDFCore;

public class Metadata
{
    public List<VSDFFile> Files { get; set; }
    public DateTime CreationTime { get; set; }
    public string OriginalExtension { get; set; }
    public string OriginalFileName { get; set; }

    public Metadata()
    {
        Files = new List<VSDFFile>();
        OriginalExtension = string.Empty;
        OriginalFileName = string.Empty;
    }

    public Metadata(List<VSDFFile> files, string originalExtension, string originalFileName)
    {
        Files = files;
        CreationTime = DateTime.UtcNow;
        OriginalExtension = originalExtension;
        OriginalFileName = originalFileName;
    }

    public static Metadata? Load(string path)
    {
        return JsonSerializer.Deserialize<Metadata>(File.ReadAllText($"{path}\\metadata.json"));
    }

    public bool Save(string path)
    {
        if (!Directory.Exists(path)) return false;

        var serializedMetadata = JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText($"{path}\\metadata.json", serializedMetadata);

        return true;
    }
}