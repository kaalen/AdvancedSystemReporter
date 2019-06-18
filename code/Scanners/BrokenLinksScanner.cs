using System;
using System.Collections.Generic;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Jobs;
using Sitecore.Links;

namespace Sitecore.Feature.Reports.Scanners
{
	/// <summary>
	/// Implements a scanner that searches for and returns a list of broken 
	/// item links which are descendants of the given root item.
	/// </summary>
	class BrokenLinksScanner : Sitecore.Feature.Reports.Interface.BaseScanner
	{
		private Database _db;
		/// <summary>
		/// Gets the db.
		/// </summary>
		/// <value>The db.</value>
		protected Database Db
		{
			get
			{
				if (_db == null)
				{
					_db = Sitecore.Context.ContentDatabase ?? Factory.GetDatabase("master");
				}
				return _db;
			}
		}

		private Item _rootItem = null;
		public Item RootItem
		{
			get
			{
				if (_rootItem == null)
				{
					string rootParameter = this.GetParameter("root");
					if (!String.IsNullOrEmpty(rootParameter))
					{
						_rootItem = Db.GetItem(rootParameter);
					}
					else
					{
						_rootItem = Db.GetRootItem();
					}
				}
				return _rootItem;
			}
		}

		public override System.Collections.ICollection Scan()
		{
			Job job = Sitecore.Context.Job;
			List<ItemLink> list = new List<ItemLink>();
			try
			{
				foreach (ItemLink link in Globals.LinkDatabase.GetBrokenLinks(Db))
				{
					if (link.GetSourceItem().Axes.IsDescendantOf(RootItem))
					{
						list.Add(link);
					}
				}
				JobStatus status = job.Status;
				status.Processed++;
			}
			catch (Exception exception)
			{
				job.Status.Failed = true;
				job.Status.Messages.Add(exception.ToString());
			}
			job.Status.Result = list;
			job.Status.State = JobState.Finished;
			return list;
		}
	}
}
