using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sitecore.Feature.Reports.DomainObjects
{
	public class Column
	{
		private string _Name;
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				_Name = value.ToLower();
			}
		}
		public string Header;
    public string Parameters { get; set; }
  }
}
