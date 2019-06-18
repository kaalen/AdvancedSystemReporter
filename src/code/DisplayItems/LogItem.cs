namespace Sitecore.Feature.Reports.Logs
{
  using System;
  using Sitecore.Feature.Reports.DisplayItems;

  /// <summary>
  /// Represents log item record.
  /// </summary>
  public class LogItem
  {
    public enum LogType
    {
      Info,
      Error,
      Audit,
      Warning
    };

    public LogType Type { get; private set; }
    public DateTime DateTime { get; private set; }
    public string Process { get; private set; }
    public string Message { get; protected set; }


    protected LogItem()
    {
    }

    public static LogItem Make(DateTime date, string pid, string type, string text)
    {
      LogItem logItem;
      if (type == "INFO  AUDIT")
      {
        AuditItem auditItem = new AuditItem();
        auditItem.Initialize(text);
        logItem = auditItem;
      }
      else
      {
        logItem = new LogItem();
      }
      logItem.DateTime = date;
      logItem.Process = pid;
      logItem.Type = parse(type);
      logItem.Message = text;

      return logItem;
    }

    private static LogType parse(string type)
    {
      switch (type)
      {
        case "INFO":
          return LogType.Info;
        case "WARN":
          return LogType.Warning;
        case "ERROR":
          return LogType.Error;
        default:
          return LogType.Audit;
      }
    }

    internal void SetDate(DateTime filedate)
    {
      DateTime = DateTime.Parse(
        string.Format("{0} {1}:{2}:{3}", filedate.ToShortDateString(), DateTime.Hour, DateTime.Minute, DateTime.Second)
        );
    }
  }
}