<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerifyModuleConfig.aspx.cs" Inherits="Sitecore.Feature.Reports.VerifyModuleConfig" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
</head>
<body>
  <form id="form1" runat="server">
    <div>
      <h1>Report Module Verification</h1>
      <h2>Error Messages</h2>
      <asp:BulletedList ID="ctlErrorList" runat="server" />
      <h2>Status Messages</h2>
      <asp:BulletedList ID="ctlStatus" runat="server"/>
    </div>
  </form>
</body>
</html>

