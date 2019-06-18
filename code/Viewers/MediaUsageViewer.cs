using Sitecore.Feature.Reports.Interface;
using Sitecore.Feature.Reports.DisplayItems;
using Sitecore.Feature.Reports.DomainObjects;

namespace Sitecore.Feature.Reports.Viewers
{
  using Sitecore.Exceptions;

  /// <summary>
	/// Implements a report viewer which enables presentation of media item 
	/// usage i.e. it enables presentation of content item details and 
	/// referenced media item details.
	/// </summary>
	class MediaUsageViewer : DetailedViewer
  {
    public override void Display(DisplayElement dElement)
    {
      MediaUsageItem usageItem = dElement.Element as MediaUsageItem;
      dElement.Value = usageItem.Item.Uri.ToString();
      dElement.Header = usageItem.Item.Name;
      dElement.Icon = usageItem.Item.Appearance.Icon;

      if (usageItem != null)
      {
        for (int i = 0; i < Columns.Count; i++)
        {
          Column column = Columns[i];
          string prefix = column.Name.Substring(0, 2).ToLower();
          string lcColumn = column.Name.ToLower().Remove(0, 2);

          string text = string.Empty;
          switch (prefix)
          {
            case "c_":
              //todo: refactor this
              //text = GetColumnText(lcColumn, column.Parameters, usageItem.Item);
              text = GetColumnText(lcColumn, usageItem.Item);
              break;
            case "m_":
              //todo: refactor this
              //text = GetColumnText(lcColumn, column.Parameters, usageItem.Media);
              text = GetColumnText(lcColumn, usageItem.Media);
              break;
          }

          dElement.AddColumn(column.Header, text);
        }
        dElement.Icon = usageItem.Item.Appearance.Icon;
      }
    }
  }
}
