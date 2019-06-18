using Sitecore.Feature.Reports.Interface;
using Sitecore.Diagnostics;

namespace Sitecore.Feature.Reports.Sessions
{
	/// <summary>
	/// Implements viewer which allows display of current session details such 
	/// as user, time of session creation, last session request and session ID.
	/// </summary>
    public class SessionsViewer : Sitecore.Feature.Reports.Interface.BaseViewer
    {
        public override string[] AvailableColumns
        {
            get
            {
                return new string[]{"User","Created","LastRequest","ID"};
            }
        }
        public override void Display( DisplayElement dElement)
        {
            Assert.ArgumentNotNull(dElement, "element");
            Sitecore.Web.Authentication.DomainAccessGuard.Session session =
                dElement.Element as Sitecore.Web.Authentication.DomainAccessGuard.Session;
            if (session != null)
            {
                foreach (var column in Columns)
                {
                    switch (column.Name)
                    {
                        case "user":
                dElement.AddColumn("User", session.UserName);
                            break;
                        case "created":
                            dElement.AddColumn("Created", session.Created.ToString(GetDateFormat(null)));
                            break;
                        case "lastrequest":
                            dElement.AddColumn("Last request", session.LastRequest.ToString(GetDateFormat(null)));
                            break;
                        case "id":
                dElement.AddColumn("ID", session.SessionID);
                            break;
                    }
                }
                dElement.Value = session.SessionID;
            }

        
        }


    }
}
