namespace Sitecore.Feature.Reports
{
  using Sitecore;

  public class Settings
  {
    private static Settings _instance;

    public static Settings Instance
    {
      get
      {
        if (_instance == null)
        {
          _instance = new Settings();
        }
        return _instance;
      }
    }

    protected Settings()
    {
    }

    /// <summary>
    /// Gets the configuration database.
    /// </summary>
    /// <value>The configuration database.</value>
    /// 
    public string ConfigurationDatabase
    {
      get
      {
        return Sitecore.Configuration.Settings.GetSetting("Sitecore.Feature.Reports.ConfigurationDatabase", "master");
      }
    }


    /// <summary>
    /// Gets the reports folder.
    /// </summary>
    /// <value>The reports folder.</value>
    public string ReportsFolder
    {
      get
      {
        return Sitecore.Configuration.Settings.GetSetting("Sitecore.Feature.Reports.ReportsFolder", "/sitecore/system/Modules/Reports/Reports");
      }
    }

    public string ParametersFolder
    {
      get
      {
        return Sitecore.Configuration.Settings.GetSetting("Sitecore.Feature.Reports.ParametersFolder", "/sitecore/system/Modules/Reports/Configuration/Parameters");
      }
    }

    public string EmailFrom
    {
      get
      {
        return Sitecore.Configuration.Settings.GetSetting("Sitecore.Feature.Reports.EmailFrom", Sitecore.Context.User.Profile.Email);
      }
    }

    public string ParameterRegex
    {
      get
      {
        return Sitecore.Configuration.Settings.GetSetting("Sitecore.Feature.Reports.ParameterRegex", @"\{(\w*)\}");
      }
    }

    /// <summary>
    /// Gets the size of the page.
    /// </summary>
    /// <value>The size of the page.</value>
    private int pageSize = int.MinValue;

    public int PageSize
    {
      get
      {
        if (pageSize < 0)
        {
          pageSize = int.Parse(Sitecore.Configuration.Settings.GetSetting("Sitecore.Feature.Reports.PageSize", "30"));
        }
        return pageSize;
      }
    }

    /// <summary>
    /// Gets the max number pages.
    /// </summary>
    /// <value>The max number pages.</value>
    private int maxNumberPages = int.MinValue;

    public int MaxNumberPages
    {
      get
      {
        if (maxNumberPages < 0)
        {
          maxNumberPages = int.Parse(Sitecore.Configuration.Settings.GetSetting("Sitecore.Feature.Reports.MaxNoPages", "40"));
        }
        return maxNumberPages;
      }
    }

    public bool AllowNonAdminDownloads
    {
      get
      {
        return "true" == Sitecore.Configuration.Settings.GetSetting("Sitecore.Feature.Reports.AllowNonAdminDownloads", "false");
      }
    }

    public string ReportExportPrefix
    {
      get
      {
        return Sitecore.Configuration.Settings.GetSetting("Sitecore.Feature.Reports.ReportExportPrefix", "Report - ");
      }
    }
  }
}