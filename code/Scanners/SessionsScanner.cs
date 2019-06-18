using System.Collections;
using Sitecore.Web.Authentication;

namespace Sitecore.Feature.Reports.Sessions
{
	/// <summary>
	/// Scans for and returns all currently active user sessions.
	/// </summary>
    public class SessionsScanner : Sitecore.Feature.Reports.Interface.BaseScanner
    {
        public override ICollection Scan()
        {
            return DomainAccessGuard.Sessions;
        }
    }
}
