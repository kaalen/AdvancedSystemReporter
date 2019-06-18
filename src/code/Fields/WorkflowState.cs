using System;
using System.Linq;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Workflows;

namespace Sitecore.Feature.Reports.Fields
{
	/// <summary>
	/// Implements a custom reference field which is used to represent 
	/// workflow state.
	/// </summary>
	class WorkflowState : Sitecore.Data.Fields.ReferenceField
	{
		public WorkflowState(Field innerField)
			: base(innerField)
		{ }

		/// <summary>
		/// Gets the number of days in item has spent in the current workflow 
		/// state.
		/// </summary>
		/// <value>The number of days in workflow state.</value>
		public string DaysInWorkflowState
		{
			get
			{
				Item item = InnerField.Item;
				//check if item in workflow
				if (item.State.GetWorkflow() != null)
				{
					WorkflowEvent[] events = item.State.GetWorkflow().GetHistory(item);
					DateTime startDate = DateTime.Today;
					if (events.Length > 0)
					{
						startDate = events.Last().Date;
					}
					else
					{
						startDate = item.Statistics.Created;
					}
					return DateTime.Today.Subtract(startDate).Days.ToString();
				}
				return string.Empty;
			}
		}
	}
}
