using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ConsoleAppDemoappConfigJson
{
  public static class ConfigurationManager
  {
    private static readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appConfig.json");

    // Version cible attendue par cette build de l'application
    public const string TargetVersion = "2.0.0.1";

    public static ApplicationConfig Current { get; private set; }

    public static void Load()
    {
      if (!File.Exists(ConfigFilePath))
      {
        Current = new ApplicationConfig { ApplicationConfigFileVersion = TargetVersion };
        Save();
        return;
      }

      string json = File.ReadAllText(ConfigFilePath);
      JsonNode rootNode;

      try
      {
        rootNode = JsonNode.Parse(json);
      }
      catch (JsonException)
      {
        // En cas de corruption du fichier JSON, on repart à zéro
        Current = new ApplicationConfig { ApplicationConfigFileVersion = TargetVersion };
        Save();
        return;
      }

      if (rootNode == null)
      {
        Current = new ApplicationConfig { ApplicationConfigFileVersion = TargetVersion };
        Save();
        return;
      }

      // Extraction de la version. On tente en respectant la casse ou non.
      string fileVersionStr = rootNode["ApplicationConfigFileVersion"]?.GetValue<string>()
                              ?? rootNode["applicationConfigFileVersion"]?.GetValue<string>()
                              ?? "2.0.0.0"; // Par défaut 2.0.0.0 si absent

      if (Version.TryParse(fileVersionStr, out Version fileVersion) && Version.TryParse(TargetVersion, out Version targetVersion))
      {
        if (fileVersion < targetVersion)
        {
          // Exécuter le pipeline de migration sur le JSON DOM
          rootNode = Migrate(rootNode, fileVersion, targetVersion);
          
          // Mettre à jour la version dans le fichier physique immédiatement
          var saveOptions = new JsonSerializerOptions { WriteIndented = true };
          File.WriteAllText(ConfigFilePath, rootNode.ToJsonString(saveOptions));
        }
      }

      // Désérialisation vers l'objet C# final
      var deserializeOptions = new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      };

      Current = JsonSerializer.Deserialize<ApplicationConfig>(rootNode.ToJsonString(), deserializeOptions) ?? new ApplicationConfig();
    }

    private static JsonNode Migrate(JsonNode root, Version fromVersion, Version toVersion)
    {
      var currentVersion = fromVersion;

      // Étape 1 : Passage de 2.0.0.0 à 2.0.0.1
      if (currentVersion <= new Version("2.0.0.0"))
      {
        root = Migrate_2_0_to_2_0_0_1(root);
        currentVersion = new Version("2.0.0.1");
      }

      // Ajouter d'autres étapes ici si nécessaire, ex:
      // if (currentVersion <= new Version("2.0.0.1")) { ... }

      // Assurer que le champ de version est mis à jour dans l'arbre JSON
      if (root is JsonObject obj)
      {
        obj["ApplicationConfigFileVersion"] = TargetVersion;
      }

      return root;
    }

    private static JsonNode Migrate_2_0_to_2_0_0_1(JsonNode root)
    {
      if (root is JsonObject obj)
      {
        // Migration de 2.0.0.0 à 2.0.0.1 : 
        // Initialisation de la nouvelle propriété "NewPropertyToTestVersionUpgrade" si elle n'existe pas.
        if (!obj.ContainsKey("NewPropertyToTestVersionUpgrade"))
        {
          obj["NewPropertyToTestVersionUpgrade"] = "Valeur initialisée lors de la migration vers 2.0.0.1";
        }
      }
      return root;
    }

    public static void Save()
    {
      var options = new JsonSerializerOptions
      {
        WriteIndented = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
      };

      // Toujours s'assurer que la version sauvegardée est la version cible
      Current.ApplicationConfigFileVersion = TargetVersion;

      string json = JsonSerializer.Serialize(Current, options);
      File.WriteAllText(ConfigFilePath, json);
    }
  }
}
