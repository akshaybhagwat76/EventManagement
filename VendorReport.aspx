<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorReport.aspx.cs" Inherits="MiidWeb.VendorReport" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vendor Report</title>
     
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1><asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        </h1>
    </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" /> 
        <asp:GridView ID="GridView1" runat="server">
		<alternatingrowstyle backcolor="#efefef"  
          forecolor="DarkBlue"
          />
        </asp:GridView>

        <asp:Panel runat="server" ID="pnlReport" Visible ="false">
       <%-- <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1273px" Height="752px">
        </rsweb:ReportViewer>--%>
           
        </asp:Panel>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="987px">
        </rsweb:ReportViewer>
    </form>
</body>
</html>
