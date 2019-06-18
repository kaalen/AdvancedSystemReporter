using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Reports
{
  using Sitecore.Controls;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;
  using Sitecore.Security;
  using Sitecore.Security.Accounts;
  using Sitecore.Web.UI.Sheer;
  using Sitecore.Web.UI.WebControls;
  using Sitecore.Web.UI.XamlSharp.Xaml;

  public class SetContentOwnerPage : DialogPage
  {
    /// <summary>
    /// The change owner.
    /// </summary>
    protected System.Web.UI.WebControls.TextBox ChangeContentOwner;

    /// <summary>
    /// The current owner.
    /// </summary>
    protected System.Web.UI.WebControls.TextBox CurrentOwner;

    /// <summary>
    /// Browses this instance.
    /// </summary>
    protected static void Browse_Click()
    {
      ClientPipelineArgs clientPipelineArgs = ContinuationManager.Current.CurrentArgs as ClientPipelineArgs;
      Assert.IsNotNull(clientPipelineArgs, "args");
      if (clientPipelineArgs == null)
      {
        return;
      }
      if (!clientPipelineArgs.IsPostBack)
      {
        SheerResponse.ShowModalDialog(new ModalDialogOptions(new SelectAccountOptions
        {
          Multiple = false,
          ExcludeRoles = true
        }.ToUrlString().ToString())
        {
          Height = "350",
          Width = "700",
          Response = true
        });
        clientPipelineArgs.WaitForPostBack();
        return;
      }
      if (!clientPipelineArgs.HasResult)
      {
        return;
      }
      string text = clientPipelineArgs.Result;
      int num = text.IndexOf('^');
      if (num >= 0)
      {
        text = StringUtil.Left(text, num);
      }
      SheerResponse.Eval("$$('[id$=ChangeContentOwner]').first().value = '{0}';", new object[]
      {
        text.Replace("\\", "\\\\")
      });
    }

    /// <summary>
    /// Handles a click on the OK button.
    /// </summary>
    /// <remarks>
    /// When the user clicks OK, the dialog is closed by calling
    /// the <see cref="M:Sitecore.Web.UI.Sheer.ClientResponse.CloseWindow">CloseWindow</see> method.
    /// </remarks>
    protected override void OK_Click()
    {
      string text = this.ChangeContentOwner.Text;
      if (string.IsNullOrEmpty(text))
      {
        text = "-";
      }
      else if (!User.Exists(text))
      {
        SheerResponse.Alert(Translate.Text("The user \"{0}\" does not exist.", new object[]
        {
          text
        }), new string[0]);
        return;
      }
      text = text.Replace("\\", "|");
      SheerResponse.SetDialogValue(text);
      base.OK_Click();
    }

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Load"></see> event.
    /// </summary>
    /// <param name="e">
    /// The <see cref="T:System.EventArgs"></see> object that contains the event data.
    /// </param>
    protected override void OnLoad(EventArgs e)
    {
      Assert.ArgumentNotNull(e, "e");
      base.OnLoad(e);
      if (XamlControl.AjaxScriptManager.IsEvent)
      {
        return;
      }
      ItemUri itemUri = ItemUri.ParseQueryString();
      Assert.IsNotNull(itemUri, "Item not found");
      Item item = Database.GetItem(itemUri);
      Assert.IsNotNull(item, "Item \"{0}\" not found", new object[]
      {
        itemUri
      });
      string currentContentOwner = item["content owner"];
      this.CurrentOwner.Text = currentContentOwner;
      this.ChangeContentOwner.Text = Sitecore.Context.User.Name.Replace("\\", "|");
    }
  }
}