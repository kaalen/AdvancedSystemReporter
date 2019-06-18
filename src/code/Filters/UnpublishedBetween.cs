using System;
using Sitecore.Common;
using Sitecore.Data.Items;

namespace Sitecore.Feature.Reports.Filters
{
	/// <summary>
	/// Filters items which have been unpublished between a certain period. 
	/// Accepts parameters ‘FromDate’ and ‘ToDate’ which should be in Sitecore 
	/// date format. Unpublished date is mainly determined from item’s ‘__valid 
	/// to’ and ‘__unpublish’ fields.
	/// </summary>
	public class UnpublishedBetween : Sitecore.Feature.Reports.Interface.BaseFilter
	{
		public const string FROM_DATE_PARAMETER = "FromDate";
		public const string TO_DATE_PARAMETER = "ToDate";

		/// <summary>
		/// Gets from date.
		/// </summary>
		/// <value>From date.</value>
		public DateTime FromDate
		{
			get
			{
				string value = base.GetParameter(FROM_DATE_PARAMETER);
				return Sitecore.DateUtil.ParseDateTime(value, DateTime.MinValue);
			}
		}

		/// <summary>
		/// Gets to date.
		/// </summary>
		/// <value>To date.</value>
		public DateTime ToDate
		{
			get
			{
				string value = base.GetParameter(TO_DATE_PARAMETER);
				return Sitecore.DateUtil.ParseDateTime(value, DateTime.MaxValue);
			}
		}

		public override bool Filter(object element)
		{
			Item item = element as Item;
			if (item != null)
			{
				DateTimeRange visibleRange = GetRange(item);
				if (visibleRange.To.InRange(FromDate, ToDate))
				{
					return true;
				}
			}
			return false;
		}

		public static DateTimeRange GetRange(Item item)
		{
			DateTime from = item.Publishing.ValidFrom;
			DateTime to = item.Publishing.ValidTo;

			if (item.Publishing.HideVersion || item.Publishing.NeverPublish)
			{
				return DateTimeRange.Empty;
			}

			Item[] laterVersions = item.Versions.GetLaterVersions();
			for (int i = laterVersions.Length - 1; i >= 0; i--)
			{
				if (!laterVersions[i].Publishing.HideVersion && laterVersions[i].Publishing.ValidFrom < to)
				{
					to = laterVersions[i].Publishing.ValidFrom;
				}
			}

			if (item.Publishing.PublishDate > from)
			{
				from = item.Publishing.PublishDate;
			}
			if (item.Publishing.UnpublishDate < to)
			{
				to = item.Publishing.UnpublishDate;
			}

			return new DateTimeRange(from, to);
		}
	}
}
