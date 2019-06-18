using System;
using System.Collections;
using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Configuration;

namespace Sitecore.Feature.Reports.Scanners
{
	/// <summary>
	/// Scans for and returns last version of items which are descendants of 
	/// root item. Root item is passed in as parameter.
	/// </summary>
	class ItemScanner : Sitecore.Feature.Reports.Interface.BaseScanner
	{
		public const string ROOT_PARAMETER = "root";

		Database _database;
		/// <summary>
		/// Gets the database.
		/// </summary>
		/// <value>The database.</value>
		public Database Db
		{
			get
			{
				if (_database == null)
				{
					_database = Sitecore.Context.ContentDatabase ?? Factory.GetDatabase("master");
				}
				return _database;
			}
		}

		public override ICollection Scan()
		{
			string rootParameter = base.GetParameter(ROOT_PARAMETER);
			List<Item> itemList = new List<Item>();
			Item rootItem = null;
			if (!String.IsNullOrEmpty(rootParameter))
			{
				rootItem = Db.GetItem(rootParameter);
			}
			else
			{
				rootItem = Db.GetRootItem();
			}
			itemList.AddRange(rootItem.Versions.GetVersions());

			Item[] descendants = rootItem.Axes.GetDescendants();
			foreach (Item item in descendants)
			{
				itemList.AddRange(item.Versions.GetVersions());
			}

			return itemList;
		}
	}
}
