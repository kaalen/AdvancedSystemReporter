using System.Configuration;
using System.Data.SqlClient;
using Sitecore.Shell.Framework.Commands;

namespace Sitecore.Feature.Reports.Commands
{
	/// <summary>
	/// Command resets item’s version number to 1 if item only has 1 version 
	/// and it’s version number is higher than 1.
	/// </summary>
	class ResetVersionNumber : Command
	{
		public override void Execute(CommandContext context)
		{
			foreach (var item in context.Items)
			{
				SqlConnection connection = null;
				SqlCommand command = null;
				try
				{
					if (item.Versions.IsLatestVersion() && item.Versions.Count == 1 && item.Version.Number > 1)
					{
						connection = new SqlConnection(ConfigurationManager.ConnectionStrings["master"].ConnectionString);
						connection.Open();

						string cmdText = string.Format(@"UPDATE VersionedFields SET Version = 1 WHERE ItemID='{0}'", item.ID);
						command = new SqlCommand(cmdText, connection);
					}
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
				}
				
			}
		}
	}
}
