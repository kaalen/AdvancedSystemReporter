using Sitecore.Feature.Reports.DisplayItems;

namespace Sitecore.Feature.Reports.Filters
{
  using Sitecore.Feature.Reports.Logs;

  /// <summary>
	/// Allows filtering of log entries based on their type.
	/// </summary>
	public class TypeFilter : Sitecore.Feature.Reports.Interface.BaseFilter
	{
		public override bool Filter(object element)
		{
			LogItem logElement = element as LogItem;
			if (logElement == null)
			{
				return false;
			}
			return logElement.Type == LogItem.LogType.Audit;
		}
	}
}
