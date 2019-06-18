using System.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;

namespace Sitecore.Feature.Reports.Scanners
{
	/// <summary>
	/// Scans for content items which use given presentation component which is 
	/// passed in as ‘renderingid’ parameter.
	/// </summary>
	[Obsolete("Consider removing currently not used by any report scanner.")]
	public class PresentationScanner : Sitecore.Feature.Reports.Interface.BaseScanner
    {
        public readonly string _textsearch = "renderingid";
        public readonly string[] FolderIDs = { 
                                             "{75CC5CE4-8979-4008-9D3C-806477D57619}", //LAYOUTS
                                             "{EB443C0B-F923-409E-85F3-E7893C8C30C2}"  //SUBLAYOUTS
                                             };
        private readonly string pathfieldname = "path";

        public string Text
        {
            get
            {
                return this.GetParameter(_textsearch);
            }
        }

        public Database DB
        {
            get
            {
                return Sitecore.Configuration.Factory.GetDatabase("master");
            }
        }

        public override ICollection Scan()
        {
            
            QueryScanner qs = new QueryScanner();
            qs.AddParameters(
                string.Format("query=/sitecore/content//*[contains(@__renderings,'{0}')]",Text));


            System.Collections.ArrayList list = new System.Collections.ArrayList(qs.Scan());

            foreach (var id in FolderIDs)
            {
                Item folder = DB.GetItem(id);
                if (folder != null)
                {
                    Item[] descendants = folder.Axes.GetDescendants();
                    foreach (var item in descendants)
                    {
                        if (searchItem(item)) list.Add(item);
                    }
                }
            }
            return list;
        }

        private bool searchItem(Item item)
        {
            string path = item[pathfieldname];
            if (string.Empty != path)
            {
                return searchFile(Sitecore.IO.FileUtil.MapPath(path));
            }
            return false;
        }

        private bool searchFile(string p)
        {
            bool result = false;
            System.IO.FileInfo file = new System.IO.FileInfo(p);
            if (file.Exists)
            {
                System.IO.StreamReader sReader = file.OpenText();
                result = searchFile(sReader);
                sReader.Close();
            }
            return result;
        }

        private bool searchFile(System.IO.StreamReader sReader)
        {
            string line;
            while ((line = sReader.ReadLine()) != null)
            {
                if (line.Contains(Text)) return true;
            }
            return false;
        }
    }
}
