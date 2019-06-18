using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Reports.Commands
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Shell.Framework.Commands;

  /// <summary>
  /// Opens item in Content Editor.
  /// </summary>
  public class OpenItem : Command
  {
    public override void Execute(CommandContext context)
    {

      var id = context.Parameters["id"];
      if (String.IsNullOrEmpty(id))
      {
        return;
      }
      var item = Sitecore.Context.ContentDatabase.GetItem(ID.Parse(id));
      Util.OpenItem(item);
    }
  }
}