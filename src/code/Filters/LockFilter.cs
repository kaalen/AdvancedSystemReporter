namespace Sitecore.Feature.Reports.Filters
{
  using System;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;

  /// <summary>
	/// Allows filtering of items based on whether they are locked. Filter 
	/// accepts age and lock owner parameter. Lock age is expressed in hours.
	/// </summary>
    public class LockFilter : Sitecore.Feature.Reports.Interface.BaseFilter
    {
        public string Owner { get; set; }

        public int Age
        {
            get; set;
        }

        public override bool Filter(object element)
        {
            Item item = element as Item;
            if ( item != null )
            {                
                LockField lField = item.Fields["__lock"];
                if ( lField != null )
                {
                    return this.checkDate(lField.Date, this.Age) && this.checkOwner(lField.Owner, this.Owner);
                }
            }
            return true;
        }

        private bool checkDate(DateTime lockingdate, int hours)
        {
            if (hours < 0) return true;

            return lockingdate.AddHours(hours).CompareTo(DateTime.Now) < 0;
        }

        private bool checkOwner(string ownername, string parameter)
        {
            if (string.IsNullOrEmpty(parameter)) return true;

            return ownername.Equals(parameter,StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
