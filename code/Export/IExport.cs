using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Feature.Reports.Export
{
  interface IExport
  {
    string SaveFile(string prefix, string extension);
  }
}
