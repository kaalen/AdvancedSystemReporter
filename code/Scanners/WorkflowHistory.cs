using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Sitecore.Feature.Reports.Interface;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Workflows;
using Sitecore.Workflows.Simple;

namespace Sitecore.Feature.Reports.Scanners
{
  using Sitecore.Feature.Reports.DisplayItems;

  public class WorkflowHistory : BaseScanner
  {
    public const string AGE_PARAMETER = "Age";
    private int _days = int.MinValue;
    private Database _db;
    private Item _root;

    public int Days
    {
      get
      {
        if (_days == int.MinValue && !int.TryParse(this.GetParameter("Age"), out _days))
          _days = 0;
        return _days;
      }
    }

    public Item RootItem
    {
      get { return _root ?? (_root = Db.GetItem(this.GetParameter("root"))); }
    }

    protected Database Db
    {
      get
      {
        if (_db == null)
          _db = Sitecore.Context.ContentDatabase ?? Factory.GetDatabase("master");
        return _db;
      }
    }

    public override ICollection Scan()
    {
      var arrayList = new ArrayList();
      DateTime dt = DateTime.Now.AddDays(-Days);
      var workflowProvider = Db.WorkflowProvider as WorkflowProvider;
      if (workflowProvider == null)
        return arrayList;
      string connectionString = Sitecore.Configuration.Settings.GetConnectionString(Db.Name);
      SqlConnection sqlConnection = null;
      SqlDataReader sqlDataReader = null;
      try
      {
        sqlConnection = new SqlConnection(connectionString);
        SqlCommand command = sqlConnection.CreateCommand();
        command.CommandText = "SELECT DISTINCT ItemID, Language FROM WorkflowHistory WHERE Date > @date";
        command.Parameters.Add(new SqlParameter("@date", SqlDbType.DateTime)).Value = dt;
        sqlConnection.Open();
        sqlDataReader = command.ExecuteReader();
        while (sqlDataReader.Read())
        {
          var itemId = new ID(sqlDataReader.GetGuid(0));
          var language = Language.Parse(sqlDataReader.GetString(1));
          Item item = Db.GetItem(itemId, language);
          if (item == null)
          {
            //skip if item no longer exists
            continue;
          }
          if (!item.Paths.Path.StartsWith(RootItem.Paths.Path))
            continue;

          foreach (WorkflowEvent wEvent in (workflowProvider.HistoryStore.GetHistory(item)).Where((hi =>
          {
            if (hi.Date > dt)
              return hi.NewState != hi.OldState;
            return
                          false;
          })))
          {
            var workflowEventCustom = new ItemWorkflowEvent(item, wEvent);
            arrayList.Add(workflowEventCustom);
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex.Message, this);
      }
      finally
      {
        if (sqlDataReader != null)
          sqlDataReader.Close();
        if (sqlConnection != null)
          sqlConnection.Close();
      }
      return arrayList;
    }
  }
}
