using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProxmoxManager.Services;

public class ConnectionService
{
    private const string FileName = "connections.json";
    private readonly string _filePath;

    public ConnectionService()
    {
        _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ProxmoxManager", FileName);
        var dir = Path.GetDirectoryName(_filePath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    public async Task<List<string>> LoadConnectionsAsync()
    {
        if (!File.Exists(_filePath)) return new List<string>();
        try
        {
            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }

    public async Task SaveConnectionAsync(string url)
    {
        var list = await LoadConnectionsAsync();
        if (!list.Contains(url))
        {
            list.Add(url);
            var json = JsonSerializer.Serialize(list);
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
    
    public async Task RemoveConnectionAsync(string url)
    {
        var list = await LoadConnectionsAsync();
        if (list.Contains(url))
        {
            list.Remove(url);
            var json = JsonSerializer.Serialize(list);
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
