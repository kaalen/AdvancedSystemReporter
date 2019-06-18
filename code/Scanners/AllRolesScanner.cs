using System.Collections;
using System.Linq;
using Sitecore.Feature.Reports.Interface;
using Sitecore.Security.Domains;

namespace Sitecore.Feature.Reports.Roles
{
    public class AllRolesScanner : BaseScanner
    {
        public string DomainName { get; set; }

        public override ICollection Scan()
        {
            return Domain.GetDomain(DomainName).GetRoles().ToArray();
        }
    }
}