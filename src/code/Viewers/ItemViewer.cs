namespace Sitecore.Feature.Reports.Viewers
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Linq;
  using System.Text;
  using Sitecore;
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Feature.Reports.DisplayItems;
  using Sitecore.Feature.Reports.Extensions;
  using Sitecore.Feature.Reports.Interface;
  using Sitecore.Links;
  using Sitecore.Workflows;

  public class ItemViewer : BaseViewer
  {
    public static string COLUMNS_PARAMETER = "columns";
    public static string HEADERS_PARAMETER = "headers";
    public static string MAX_LENGHT_PARAMETER = "maxlength";
    private int maxLength = -1;


    public int MaxLength
    {
      get
      {
        if (this.maxLength < 0)
        {
          if (!int.TryParse(this.GetParameter(MAX_LENGHT_PARAMETER), out this.maxLength))
          {
            this.maxLength = 500;
          }
        }
        return this.maxLength;
      }
    }

    public override string[] AvailableColumns
    {
      get
      {
        return new string[]
        {
          "ChildrenCount",
          "Created",
          "CreatedBy",
          "DisplayName",
          "EmailAddress",
          "Name",
          "Language",
          "LockedBy",
          "Owner",
          "Path",
          "PhoneNumber",
          "Template",
          "Updated",
          "UpdatedBy",
          "Version",
          "Versions",
          "Workflow"
        };
      }
    }


    public override void Display(DisplayElement dElement)
    {
      var itemElement = dElement.Element as Item;

      if (itemElement == null)
      {
        if (dElement.Element is ID)
        {
          itemElement = Sitecore.Context.ContentDatabase.GetItem((ID)dElement.Element);
        }
        else if (dElement.Element is ItemWorkflowEvent)
        {
          itemElement = ((ItemWorkflowEvent)dElement.Element).Item;
        }
        else if (dElement.Element is AuditItem)
        {
          itemElement = Database.GetItem(((AuditItem)dElement.Element).ItemUri);
        }
      }


      if (itemElement == null)
      {
        return;
      }
      dElement.Value = itemElement.Uri.ToString();

      dElement.Header = itemElement.Name;

      foreach (var column in this.Columns)
      {
        if (!dElement.HasColumn(column.Header))
        {
          var text = this.GetColumnText(column.Name, itemElement);
          dElement.AddColumn(column.Header, string.IsNullOrEmpty(text) ? itemElement[column.Name] : text);
        }
      }
      dElement.Icon = itemElement.Appearance.Icon;
    }


    protected virtual string FormatDateField(Item item, ID fieldID)
    {
      DateField field = item.Fields[fieldID];
      if (field != null && !String.IsNullOrEmpty(field.Value))
      {
        var dateTimeFormatInfo = CultureInfo.CurrentUICulture.DateTimeFormat;

        var format = this.GetDateFormat(dateTimeFormatInfo.ShortDatePattern);

        if (field.InnerField.TypeKey == "datetime")
        {
          return field.DateTime.ToString(string.Concat(format, " ", dateTimeFormatInfo.ShortTimePattern));
        }
        else
        {
          return field.DateTime.ToString(format);
        }
      }
      return string.Empty;
    }

    protected virtual string GetColumnText(string name, Item itemElement)
    {
      switch (name)
      {
        case "name":
          return itemElement.Name;

        case "displayname":
          return itemElement.DisplayName;

        case "createdby":
          return itemElement[FieldIDs.CreatedBy];

        case "updated":
          return this.FormatDateField(itemElement, FieldIDs.Updated);

        case "updatedby":
          return itemElement[FieldIDs.UpdatedBy];

        case "created":
          return this.FormatDateField(itemElement, FieldIDs.Created);

        case "lockedby":
          LockField lf = itemElement.Fields[FieldIDs.Lock];
          var text = String.Empty;
          if (lf != null)
          {
            if (!string.IsNullOrEmpty(lf.Owner))
            {
              text = lf.Owner + " " + lf.Date.ToString("dd/MM/yy HH:mm");
            }
          }
          return text;
        case "template":
          return itemElement.Template.Name;

        case "path":
          return itemElement.Paths.FullPath;

        case "url":
          if (itemElement.Visualization.Layout != null)
          {
            return itemElement.Url(new UrlOptions { Site = itemElement.GetContextSite() });
          }
          return null;

        case "owner":
          return itemElement[FieldIDs.Owner];

        case "workflow":
          return GetWorkflowInfo(itemElement);

        case "childrencount":
          return itemElement.Children.Count.ToString();

        case "version":
          return itemElement.Version.ToString();

        case "versions":
          return itemElement.Versions.Count.ToString();

        case "language":
          return itemElement.Language.CultureInfo.DisplayName;
        default:
          return this.GetFriendlyFieldValue(name, itemElement);
      }
    }


    private static string GetWorkflowInfo(Item itemElement)
    {
      var sb = new StringBuilder();
      var iw = itemElement.State.GetWorkflow();
      if (iw != null)
      {
        sb.Append(iw.Appearance.DisplayName);
      }
      var ws = itemElement.State.GetWorkflowState();

      if (ws != null)
      {
        sb.AppendFormat(" ({0})", ws.DisplayName);
      }

      if (iw != null)
      {
        IEnumerable<WorkflowEvent> events = iw.GetHistory(itemElement).OrderByDescending(e => e.Date);
        var enumerator = events.GetEnumerator();
        if (enumerator.MoveNext())
        {
          var span = DateTime.Now.Subtract(enumerator.Current.Date);
          sb.AppendFormat(" for {0} days {1} hours {2} minutes", span.Days, span.Hours, span.Minutes);
        }
      }
      return sb.ToString();
    }

    protected virtual string GetFriendlyFieldValue(string name, Item itemElement)
    {
      // to allow forcing fields rather than properties, allow prepending the name with #
      name = name.TrimStart('@');
      var field = itemElement.Fields[name];
      if (field != null)
      {
        switch (field.TypeKey)
        {
          case "date":
          case "datetime":
            return this.FormatDateField(itemElement, field.ID);
          case "droplink":
          case "droptree":
          case "reference":
          case "grouped droplink":
            var lookupFld = (LookupField)field;
            if (lookupFld.TargetItem != null)
            {
              return lookupFld.TargetItem.Name;
            }
            break;
          case "checklist":
          case "multilist":
          case "treelist":
          case "treelistex":
            var multilistField = (MultilistField)field;
            var strBuilder = new StringBuilder();
            foreach (var item in multilistField.GetItems())
            {
              strBuilder.AppendFormat("{0}, ", item.Name);
            }
            return StringUtil.Clip(strBuilder.ToString().TrimEnd(',', ' '), this.MaxLength, true);
          case "link":
          case "general link":
            var lf = new LinkField(field);
            switch (lf.LinkType)
            {
              case "media":
              case "internal":
                if (lf.TargetItem != null)
                {
                  return lf.TargetItem.Paths.ContentPath;
                }
                return lf.Value == string.Empty ? "[undefined]" : "[broken link] " + lf.Value;
              case "anchor":
              case "mailto":
              case "external":
                return lf.Url;
              default:
                return lf.Text;
            }
          default:
            return StringUtil.Clip(StringUtil.RemoveTags(field.Value), this.MaxLength, true);
        }
      }
      return String.Empty;
    }
  }
}