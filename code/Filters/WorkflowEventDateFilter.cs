using System;
using Sitecore.Feature.Reports.Interface;
using Sitecore.Feature.Reports.DisplayItems;

namespace Sitecore.Feature.Reports.Filters
{
	class WorkflowEventDateFilter : BaseFilter
	{
		internal ItemWorkflowEvent itemWorkflowEvent
		{
			get;
			set;
		}

		#region Parameters
		public DateTime FromDate
		{
			get
			{
				string value = base.GetParameter("FromDate");
				return Sitecore.DateUtil.ParseDateTime(value, DateTime.MinValue);
			}
		}
		public DateTime ToDate
		{
			get
			{
				string value = base.GetParameter("ToDate");
				return Sitecore.DateUtil.ParseDateTime(value, DateTime.MaxValue);
			}
		}
		#endregion

		public override bool Filter(object element)
		{
			if (element is ItemWorkflowEvent)
			{
				this.itemWorkflowEvent = element as ItemWorkflowEvent;
				return (FromDate <= this.itemWorkflowEvent.WorkflowEvent.Date && this.itemWorkflowEvent.WorkflowEvent.Date.Date <= ToDate);
			}
			return false;
		}
	}
}
