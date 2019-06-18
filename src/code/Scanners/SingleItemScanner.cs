using System.Collections;
using System.Linq;
using Sitecore.Feature.Reports.Exceptions;
using Sitecore.Data.Items;

namespace Sitecore.Feature.Reports.Scanners
{
  /// <summary>
  /// Scans a database according to a Sitecore Query. Accepts as parameters:
  /// Query: the sitcore Query,
  /// db: the database to use. By default "master".
  /// root: the root item (an ID or a path). By default runs the Query against the database object.
  /// </summary>
  public class SingleItemScanner : DatabaseScanner
  {
    public override ICollection Scan()
    {
      Item[] results;

      Item rootItem = GetRootItem();
      if (rootItem == null)
      {
        throw new RootItemNotFoundException("Can't find selected item " + Root);
      }

      results = rootItem.Versions.GetVersions();

      return results;
    }
  }
}