using System;
using Sitecore.Data.Templates;
using Sitecore.Data.Managers;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Feature.Reports.Filters
{
	/// <summary>
	/// Filters all items which inherit from a given base template.
	/// </summary>
	class InheritsFromTemplate : Sitecore.Feature.Reports.Interface.BaseFilter
	{
		/// <summary>
		/// Gets the name of the base template.
		/// </summary>
		/// <value>The name of the base template.</value>
		public string BaseTemplateName
		{
			get
			{
				return this.GetParameter("BaseTemplateName");
			}
		}

		public override bool Filter(object element)
		{
			if (element is Item)
			{
				Item item = element as Item;
				return HasBaseTemplate(item, BaseTemplateName);
			}
			return false;
		}

		private bool HasBaseTemplate(Item item, string baseTemplateName)
		{
			Template template = TemplateManager.GetTemplate(item);
			if (template != null)
			{
				ID id;
				if (ID.TryParse(baseTemplateName, out id))
				{
					return this.HasBaseTemplate(template, id);
				}
				if (template.FullName.Equals(baseTemplateName, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
				if (template.Name.Equals(baseTemplateName, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
				foreach (Template template2 in template.GetBaseTemplates())
				{
					if (template2.FullName.Equals(baseTemplateName, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
					if (template2.Name.Equals(baseTemplateName, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool HasBaseTemplate(Template template, ID baseTemplateId)
		{
			if (template.ID == baseTemplateId)
			{
				return true;
			}
			foreach (Template template2 in template.GetBaseTemplates())
			{
				if (template2.ID == baseTemplateId)
				{
					return true;
				}
			}
			return false;
		}
	}
}
