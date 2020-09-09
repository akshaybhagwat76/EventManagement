<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminRemoveUser.aspx.cs" Inherits="MiidWeb.AdminRemoveUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="Label1" runat="server" Text="UserName to Remove"></asp:Label>
    
    </div>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Click Me" />
    </form>
</body>
</html>
