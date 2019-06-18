using System;
using System.Linq;
using Sitecore.Data.Items;
using Sitecore.Workflows;

namespace Sitecore.Feature.Reports.Filters
{
	/// <summary>
	/// Implements a filter which filters items which were rejected during 
	/// workflow. Rejection event occurs when item transitions from QA Review 
	/// or Awaiting Approval state to Draft.
	/// </summary>
	class Rejected : Sitecore.Feature.Reports.Interface.BaseFilter
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

		/// <summary>
		/// Filters the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns></returns>
		public override bool Filter(object element)
		{
			Item item = element as Item;
			if (item != null)
			{
				//if item in workflow
				IWorkflow workflow = item.State.GetWorkflow();
				if (workflow != null)
				{
					//retrieve workflow history for current version
					var rejections = from e in workflow.GetHistory(item)
									 where (e.OldState == "{B3D9B273-660C-4698-BEE7-145BCDD0C039}" || e.OldState == "{3FFDCB17-1512-4476-983E-CA998997C9ED}") && 
											e.NewState == "{E45963E2-C6EC-4002-AC41-82077E1ADAD0}" &&
											e.Date >= FromDate &&
											e.Date <= ToDate
									 select e;
					//if item was rejected, return false 
					if (rejections.Count() > 0)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
