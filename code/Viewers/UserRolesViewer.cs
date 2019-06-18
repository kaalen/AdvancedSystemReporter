using System;
using Sitecore.Feature.Reports.Interface;
using Sitecore.Security.Accounts;
using System.Linq;

namespace Sitecore.Feature.Reports.Users
{
    public class UserRolesViewer : BaseViewer
    {
        public override void Display(DisplayElement dElement)
        {
            var user = dElement.Element as User;
            if ((Account)user == (Account)null)
                return;

            foreach (var userRole in user.Roles)
            {
                if (Columns == null ||
                    Columns.Any(c => c.Name.Equals(userRole.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    dElement.AddColumn(userRole.Name, "yes");
                }
            }
        }
    }
}