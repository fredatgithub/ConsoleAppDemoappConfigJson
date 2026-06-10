namespace ConsoleAppDemoappConfigJson
{
  public class ApplicationConfig
  {
    public string ApplicationName { get; set; } = "MonApplication";

    public string ApplicationConfigFileVersion { get; set; } = "2.0.0.1";

    public string LogLevel { get; set; } = "Info";

    public int MaxRetryCount { get; set; } = 3;

    public bool AutoSave { get; set; } = true;

    public string NewPropertyToTestVersionUpgrade { get; set; } = "new property value and version increased to 2.0.0.1";
  }
}
