using System.Web;

namespace Sitecore.Feature.Reports
{
    internal static class Current
    {
        internal static Context Context
        {
            get
            {
                if (HttpContext.Current.Session["Sitecore.Feature.Reports.Context"] == null)
                {
                    Context c = new Context();
                    HttpContext.Current.Session["Sitecore.Feature.Reports.Context"] = c;
                }
                return (Context)HttpContext.Current.Session["Sitecore.Feature.Reports.Context"];
            }

            private set
            {
                HttpContext.Current.Session["Sitecore.Feature.Reports.Context"] = value;
            }

        }
        internal static void ClearContext()
        {
            Context = null;
        }

    }
}
