namespace Sitecore.Feature.Reports.Scanners
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Feature.Reports.DisplayItems;

  public class AuditPerItem : Sitecore.Feature.Reports.Interface.BaseScanner
  {
    public enum Mode
    {
      Item = -1,
      Descendants = 1,
      Children = 0
    };

    public Mode Deep { get; set; }

    private Item _root;

    public Item Root
    {
      get
      {
        if (this._root == null)
        {
          this._root = this.Db.GetItem(this.GetParameter("root"));
        }
        return this._root;
      }
    }

    public string _allversions;

    public bool AllVersions
    {
      get
      {
        if (string.IsNullOrEmpty(this._allversions))
        {
          this._allversions = this.GetParameter("allversions");
        }
        return this._allversions == "1";
      }
    }

    private Database _db;

    /// <summary>
    /// Gets or sets the db.
    /// </summary>
    /// <value>The db.</value>
    protected Database Db
    {
      get
      {
        if (this._db == null)
        {
          this._db = Sitecore.Context.ContentDatabase == null ?
            Sitecore.Configuration.Factory.GetDatabase("master") : Sitecore.Context.ContentDatabase;
        }
        return this._db;
      }
    }

    public override ICollection Scan()
    {
      LogScanner scanner = new LogScanner();
      scanner.AddParameters(string.Format("{0}=audit", LogScanner.ENTRY_TYPES_PARAMETER));

      IEnumerable<AuditItem> auditItems = scanner.Scan().Cast<AuditItem>();

      Item[] items;
      ArrayList results = new ArrayList();
      switch (this.Deep)
      {
        case Mode.Descendants:
          items = this.Root.Axes.GetDescendants();
          break;
        case Mode.Children:
          items = this.Root.Axes.SelectItems("./*");
          break;
        default:
          items = new Item[] {this.Root};
          break;
      }

      foreach (var item in items)
      {
        if (this.AllVersions)
        {
          foreach (var version in item.Versions.GetVersions())
          {
            this.addItem(auditItems, results, version);
          }
        }
        else
        {
          this.addItem(auditItems, results, item);
        }
      }
      return results;
    }

    private void addItem(IEnumerable<AuditItem> auditItems, ArrayList results, Item item)
    {
      results.Add(item);
      foreach (AuditItem ai in auditItems.Where(a => this.compareUris(a.ItemUri, item.Uri)))
      {
        results.Add(ai);
      }
    }

    private bool compareUris(ItemUri a, ItemUri b)
    {
      if (a == null || b == null)
      {
        return false;
      }

      return a.DatabaseName == b.DatabaseName
             && a.ItemID == b.ItemID
             && a.Language == b.Language
             && a.Version == b.Version;
    }
  }
}