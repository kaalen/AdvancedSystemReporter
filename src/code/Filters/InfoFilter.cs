using Sitecore.Feature.Reports.Interface;
using Sitecore.Feature.Reports.DisplayItems;

namespace Sitecore.Feature.Reports.Filters
{
  using Sitecore.Feature.Reports.Logs;

  /// <summary>
	/// Filters log entries based on verb.
	/// </summary>
	class InfoFilter : BaseFilter
	{
		public static string VERB_PARAMETER = "Verb";

		/// <summary>
		/// Gets the verb.
		/// </summary>
		/// <value>The verb.</value>
		public string Verb { get { return this.GetParameter(VERB_PARAMETER); } }

		public override bool Filter(object element)
		{
			LogItem logItem = element as LogItem;

			if (logItem == null)
			{
				return true;
			}

			return (logItem.Message == null || logItem.Message.Contains(Verb));
		}
	}
}
