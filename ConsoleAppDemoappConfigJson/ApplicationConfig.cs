using System;

namespace ConsoleAppDemoappConfigJson
{
  public class ApplicationConfig
  {
    public string ApplicationName { get; set; } = "MonApplication";

    public Version ApplicationConfigFileVersion = new Version("2.0.0.0");

    public string LogLevel { get; set; } = "Info";

    public int MaxRetryCount { get; set; } = 3;

    public bool AutoSave { get; set; } = true;
  }
}
