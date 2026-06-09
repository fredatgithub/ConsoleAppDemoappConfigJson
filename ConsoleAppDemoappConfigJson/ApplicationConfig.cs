namespace ConsoleAppDemoappConfigJson
{
  public class ApplicationConfig
  {
    public string ApplicationName { get; set; } = "MonApplication";

    public string LogLevel { get; set; } = "Info";

    public int MaxRetryCount { get; set; } = 3;

    public bool AutoSave { get; set; } = true;
  }
}
