namespace Sitecore.Feature.Reports
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Web;
  using System.Web.UI;
  using System.Web.UI.WebControls;
  using System.Text;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Templates;
  using Sitecore.Feature.Reports.Extensions;

  public partial class VerifyModuleConfig : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Database db = Factory.GetDatabase("master");
      var configurationFolder = db.GetItem("/sitecore/system/Modules/Reports/Configuration");
      if (configurationFolder == null)
      {
        Response.Write("Configuration folder does not exist. Sitecore Report module most likely isn't installed");
        return;
      }

      List<string> errorMsg = new List<string>();
      List<string> successMsg = new List<string>();
      foreach (var item in configurationFolder.Axes.GetDescendants())
      {
        if (item.IsDerived(Templates.Reference.ID))
        {
          string typeName = String.Format("{0},{1}", item["class"], item["assembly"]);
          Type type = Type.GetType(typeName);
          if (type == null)
          {
            errorMsg.Add($"Type '{typeName}' referenced by item '{item.Paths.FullPath}' doesn't exist.\n");
          }
          else
          {
            successMsg.Add($"Item '{item.Paths.FullPath}' references '{typeName}'");
          }
        }
        else if (item.IsDerived(Templates.Command.ID))
        {
          //item is a command
          string commandName = item["command"];
          if (!commandName.Contains(":"))
          {
            if (Type.GetType(commandName) == null)
            {
              errorMsg.Add(String.Format("Type '{0}' referenced by item '{1}' doesn't exist.\n",
                commandName,
                item.Paths.FullPath));
            }
            else
            {
              successMsg.Add($"Item '{item.Paths.FullPath}' references command '{commandName}'");
            }
          }
        }
      }

      ctlErrorList.DataSource = errorMsg;
      ctlErrorList.DataBind();

      ctlStatus.DataSource = successMsg;
      ctlStatus.DataBind();
    }
  }
}