namespace Sitecore.Feature.Reports
{
  using Sitecore.Data;

  public struct Templates
  {
    public struct Command
    {
      public static readonly ID ID = new ID("{A2F37FA0-B644-4FA1-8DBE-6DD346F48ABB}");

      public struct Fields
      {
        public static readonly ID Title = new ID("{4754B397-F055-4D4A-AC74-FD09390BC8F3}");
        public static readonly ID Icon = new ID("{F48567F6-CC4C-4AA9-B722-EBA7B878B40C}");
        public static readonly ID Command = new ID("{B026DCD1-C3E9-4061-9F7A-7A61F200D383}");
        public static readonly ID SingleItemContext = new ID("{A0C02C83-23E3-4A3D-95F4-037C79678314}");
        public static string Title_FieldName = "Title";
        public static string Icon_FieldName = "Icon";
        public static string Command_FieldName = "Command";
        public static string SingleItemContext_FieldName = "SingleItemContext";
      }
    }

    public struct Filter
    {
      public static readonly ID ID = new ID("{1383EBC6-BCC4-451D-8E33-7F74921F6664}");
    }

    public struct Report
    {
      public static readonly ID ID = new ID("{55ECC766-BF85-4E85-92CB-53AE306F0414}");

      public struct Fields
      {
        public static readonly ID Description = new ID("{E9C5B093-0D53-4F39-9383-F5C839441562}");
        public static readonly ID Scanners = new ID("{F4033239-E21D-488D-990D-78E45760FCE3}");
        public static readonly ID Viewers = new ID("{9DE630AE-6A85-4821-9842-E90D900769B0}");
        public static readonly ID Filters = new ID("{B5EBDC93-C3AB-4438-89FB-A8D7F5552110}");
        public static readonly ID Commands = new ID("{E6B9CD5F-58E8-407D-9D45-4FE36544EA86}");
      }
    }

    public struct Reference
    {
      public static readonly ID ID = new ID("{CB65CD27-96FB-4E9A-8610-73A9B0138B7A}");

      public struct Fields
      {
        public static readonly ID Assembly = new ID("{6FB5BEBA-ECD8-4B56-B20A-BF292193D491}");
        public static readonly ID Class = new ID("{49EB5881-0AEA-4D84-9850-1F8A9A2183FE}");
        public static readonly ID Attributes = new ID("{60ACFB3B-D34D-43D8-8B57-86D26B30DD5A}");
      }
    }

    public struct ReportEmailTask
    {
      public static readonly ID ID = new ID("{93E0E6C6-D8AD-4CE1-95EE-D8C07C2F596D}");

      public struct Fields
      {
        public static readonly ID Active = new ID("{CEA5F89D-99A7-4ED2-BE12-5D879AEC5698}");
        public static readonly ID From = new ID("{DF6402D0-D2C4-48FA-A01D-9DF6D12ACED8}");
        public static readonly ID To = new ID("{03F9AF45-FDCA-4496-BC3D-7E80D919A53D}");
        public static readonly ID Subject = new ID("{7829DE7C-9234-45BE-931D-F8C0D925F29D}");
        public static readonly ID Text = new ID("{0F4D0401-1140-446E-B605-7462783C601A}");
        public static readonly ID SendEmpty = new ID("{65C03AD1-FA58-4E3F-9BFA-662E0CCED2CB}");
        public static readonly ID Reports = new ID("{D1492B7B-0912-4C49-9F47-1A6824936CCE}");
        public static readonly ID Format = new ID("{3210A8A4-8850-4F34-A2D6-46662EBF044D}");
      }
    }

    public struct SavedReport
    {
      public static readonly ID ID = new ID("{7F01AAE6-90D8-4F48-A9DA-7B48E28A56B3}");
    }
  }
}