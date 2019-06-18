using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Workflows;

namespace Sitecore.Feature.Reports.Filters
{
    /// <summary>
    /// Implements a filter which filters items which are currently in a user defined state
    /// </summary>
    class IsInState : Sitecore.Feature.Reports.Interface.BaseFilter
    {
        /// <summary>
        /// The stateID(s) to check for
        /// </summary>
        public List<string> StateID
        {
            get
            {
                string value = base.GetParameter("StateID");
                string[] items = value.Split(',');
                List<string> values = new List<string>();
                values.AddRange(items);
                return values;
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
				IWorkflow workflow = item.State.GetWorkflow();
                if (workflow != null)
                {
                    WorkflowState state = item.State.GetWorkflowState();
                    List<string> states = StateID;
                    return StateID.Contains(state.StateID);
                }
            }
            return false;
        }
    }
}
