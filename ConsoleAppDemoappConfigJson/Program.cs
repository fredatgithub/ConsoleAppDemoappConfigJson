using System;

namespace ConsoleAppDemoappConfigJson
{
  internal class Program
  {
    static void Main()
    {
      Action<string> Display = Console.WriteLine;
      
      ConfigurationManager.Load();
      Display($"Application Config Version: {ConfigurationManager.Current.ApplicationConfigFileVersion}");
      Display($"Application Name: {ConfigurationManager.Current.ApplicationName}");
      Display($"Log Level: {ConfigurationManager.Current.LogLevel}");
      Display($"Max Retry Count: {ConfigurationManager.Current.MaxRetryCount}");
      Display($"Auto Save: {ConfigurationManager.Current.AutoSave}");
      Display("");
      Display($"ApplicationConfigFileVersion: {ConfigurationManager.Current.ApplicationConfigFileVersion}");

      Display("Note: You can edit the appConfig.json file to change these values and see how the application handles different versions.");

      Display("Press any key to continue...");
      try
      {
        Console.ReadKey();
      }
      catch (InvalidOperationException)
      {
        // En cas d'entrée redirigée (ex: lors d'un test automatisé)
      }
    }
  }
}
