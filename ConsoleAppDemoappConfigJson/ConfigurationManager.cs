using System;
using System.IO;
using System.Text.Json;

namespace ConsoleAppDemoappConfigJson
{
  public static class ConfigurationManager
  {
    private static readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appConfig.json");

    public static ApplicationConfig Current { get; private set; }

    public static void Load()
    {
      if (!File.Exists(ConfigFilePath))
      {
        Current = new ApplicationConfig();
        Save();
        return;
      }

      string json = File.ReadAllText(ConfigFilePath);
      var options = new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      };

      Current = JsonSerializer.Deserialize<ApplicationConfig>(json, options) ?? new ApplicationConfig();
    }

    public static void Save()
    {
      var options = new JsonSerializerOptions
      {
        WriteIndented = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
      };

      string json = JsonSerializer.Serialize(Current, options);
      File.WriteAllText(ConfigFilePath, json);
    }
  }
}
