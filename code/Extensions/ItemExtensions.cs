using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Reports.Extensions
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.Links;
  using Sitecore.Resources.Media;
  using Sitecore.Sites;

  public static class ItemExtensions
  {
    public static SiteContext GetContextSite(this Item item)
    {
      var site = SiteContextFactory.Sites
        .Where(s => s.RootPath != "" && item.Paths.Path.ToLower().StartsWith(s.RootPath.ToLower()))
        .OrderByDescending(s => s.RootPath.Length)
        .FirstOrDefault();

      if (site != null)
        return Sitecore.Configuration.Factory.GetSite(site.Name);
      else
        return null;
    }

    public static string Url(this Item item, UrlOptions options = null)
    {
      if (item == null)
      {
        throw new ArgumentNullException(nameof(item));
      }

      if (options != null)
        return LinkManager.GetItemUrl(item, options);
      return !item.Paths.IsMediaItem ? LinkManager.GetItemUrl(item) : MediaManager.GetMediaUrl(item);
    }

    public static bool IsDerived(this Item item, ID templateId)
    {
      if (item == null)
      {
        return false;
      }

      return !templateId.IsNull && item.IsDerived(item.Database.Templates[templateId]);
    }

    private static bool IsDerived(this Item item, Item templateItem)
    {
      if (item == null)
      {
        return false;
      }

      if (templateItem == null)
      {
        return false;
      }

      var itemTemplate = TemplateManager.GetTemplate(item);
      return itemTemplate != null && (itemTemplate.ID == templateItem.ID || itemTemplate.DescendsFrom(templateItem.ID));
    }
  }
}