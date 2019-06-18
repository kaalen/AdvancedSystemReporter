using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Feature.Reports.Interface;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Feature.Reports.Scanners
{
  using Sitecore.Feature.Reports.Exceptions;

  public abstract class DatabaseScanner : BaseScanner
  {
    public string Root { get; set; }
    public string Db { get; set; }

    Database _db = null;
    public Database Database
    {
      get
      {
        if (_db == null)
        {
          _db = !string.IsNullOrEmpty(Db) ?
              Sitecore.Configuration.Factory.GetDatabase(Db)
              : Sitecore.Context.ContentDatabase;

          if (_db == null)
          {
            //assume default database is master. This may occur in scheduled tasks which lack context
            _db = Sitecore.Configuration.Factory.GetDatabase("master");
          }
        }
        return _db;
      }
    }
    protected Item GetRootItem()
    {
      var root = string.IsNullOrEmpty(Root) ? "/sitecore" : Root;
      return Database.GetItem(root);
    }
  }
}
