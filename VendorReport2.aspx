<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorReport2.aspx.cs" Inherits="MiidWeb.VendorReport2" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
     <div>
    <h1><asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        </h1>
    </div>
    <form id="form1" runat="server">
           <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <div>
      <asp:Panel runat="server" ID="pnlReport" Visible ="false">
          <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1273px"></rsweb:ReportViewer>
     
    </asp:Panel>
    </div>
    </form>
</body>
</html>
