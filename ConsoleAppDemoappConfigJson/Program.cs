using System;

namespace ConsoleAppDemoappConfigJson
{
  internal class Program
  {
    static void Main()
    {
      Action<string> Display = Console.WriteLine;
      
      ConfigurationManager.Load();
      Display($"Application Name: {ConfigurationManager.Current.ApplicationName}");
      Display($"Log Level: {ConfigurationManager.Current.LogLevel}");
      Display($"Max Retry Count: {ConfigurationManager.Current.MaxRetryCount}");
      Display($"Auto Save: {ConfigurationManager.Current.AutoSave}");

      Display("Press any key to continue...");
      Console.ReadKey();
    }
  }
}
