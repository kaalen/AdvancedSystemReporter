using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Sitecore.Caching;
using Sitecore.Data.Items;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.Feature.Reports.Commands
{
	/// <summary>
	/// Command removes all but the last item version and resets its version 
	/// number.
	/// </summary>
	public class RemoveVersions : Command
	{
		public override void Execute(CommandContext context)
		{
			SqlConnection connection = null;
			SqlCommand command = null;
			try
			{
				foreach (Item item in context.Items)
				{
					//remove old versions
					Item[] versions = item.Versions.GetVersions().OrderBy(v => v.Version.Number).ToArray();
					for (int i = 0; i < versions.Length - 1; i++)
					{
						versions[i].Versions.RemoveVersion();
					}

					//reset version number
					if (item.Versions.IsLatestVersion() && item.Versions.Count == 1 && item.Version.Number > 1)
					{
						connection = new SqlConnection(ConfigurationManager.ConnectionStrings["master"].ConnectionString);
						connection.Open();

						string cmdText = string.Format(@"UPDATE VersionedFields SET Version = 1 WHERE ItemID='{0}'", item.ID);
						command = new SqlCommand(cmdText, connection);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception)
			{
				SheerResponse.Alert("Unable to remove and reset version for at least one item");
			}
			finally
			{
				if (connection != null)
				{
					connection.Dispose();
				}
				if (command != null)
				{
					command.Dispose();
				}
				CacheManager.ClearAllCaches();
			}
		}
	}
}

