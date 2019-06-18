using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Feature.Reports.DomainObjects;

namespace Sitecore.Feature.Reports.Viewers
{
  /// <summary>
  /// Implements a viewer which enables presentation of item links such as 
  /// Broken Links report. It implements display of:
  /// <list type="bullet">
  /// <item><description>Source item name</description></item>
  /// <item><description>Source item path</description></item>
  /// <item><description>Source field name</description></item>
  /// <item><description>Source field value</description></item>
  /// <item><description>Target item ID</description></item>
  /// <item><description>Target path</description></item>
  /// </list>
  /// </summary>
  public class ItemLinkViewer : Sitecore.Feature.Reports.Interface.BaseViewer
  {
    public override void Display(Sitecore.Feature.Reports.Interface.DisplayElement dElement)
    {
      if (dElement.Element is ItemLink)
      {
        ItemLink itemLink = (ItemLink)dElement.Element;
        Item sourceItem = itemLink.GetSourceItem();

        dElement.Icon = sourceItem.Appearance.Icon;
        dElement.Value = sourceItem.Uri.ToString();
        dElement.Header = sourceItem.Name;

        for (int i = 0; i < Columns.Count; i++)
        {
          Column column = Columns[i];

          string text = getColumnText(column.Name, column.Parameters, itemLink);

          if (string.IsNullOrEmpty(text))
          {
            dElement.AddColumn(column.Header, sourceItem[column.Name]);
          }
          else
          {
            dElement.AddColumn(column.Header, text);
          }
        }
      }
    }

    protected virtual string getColumnText(string name, string parameters, ItemLink itemLink)
    {
      Item sourceItem = itemLink.GetSourceItem();
      switch (name)
      {
        case "source item name":
          return sourceItem.Name;
        case "source item path":
          return sourceItem.Paths.FullPath;
        case "source field name":
          return sourceItem.Fields[itemLink.SourceFieldID].Name;
        case "source field value":
          return sourceItem.Fields[itemLink.SourceFieldID].Value;
        case "target item id":
          return itemLink.TargetItemID.ToString();
        case "target path":
          return itemLink.TargetPath;
        default:
          return string.Empty;
      }
    }
  }
}