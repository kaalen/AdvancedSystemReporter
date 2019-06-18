using System.Collections;
using System.Linq;

namespace Sitecore.Feature.Reports.Scanners
{
	/// <summary>
	/// Scans for and returns all users in the Sitecore domain.
	/// </summary>
    public class AllUsersScanner : Sitecore.Feature.Reports.Interface.BaseScanner
    {
        public string DomainName { 
            get
            {
                return "sitecore";
            }
        }
        public override ICollection Scan()
        {
            var domain = Sitecore.Security.Domains.Domain.GetDomain(DomainName);

            return domain.GetUsers().ToArray<Sitecore.Security.Accounts.User>();
        }
    }
}
