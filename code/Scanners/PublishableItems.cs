﻿using System;
using System.Linq;
using Sitecore.Feature.Reports.Items;
using Sitecore.Data.Items;
using Sitecore;

namespace Sitecore.Feature.Reports.Scanners
{
  public class PublishableItems : QueryScanner
  {
    public DateTime Date { get; set; }

    public bool Approved { get; set; }

    public override System.Collections.ICollection Scan()
    {
      var results = base.Scan();
      return results.OfType<Item>().Select(i => i.Publishing.GetValidVersion(Date, Approved)).Where(i => i != null).ToArray();
    }
  }
}