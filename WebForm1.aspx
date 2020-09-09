<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="MiidWeb.WebForm1"  MasterPageFile="~/Site1.Master" %>



 <asp:Content ID="Content1" ContentPlaceHolderID ="ContentPlaceHolder1" runat="server" >
    <form id="form1" runat="server">

   <%-- <asp:Button ID="Button2" runat="server" onclick="Button2_Click" 
        Text="Execute NonQuery" /> --%>
		 <div class="col-md-9">

				  <div class="row">
                     <div class="col-md-12">
						  <div class="content-box-header">					
							  <div class="panel-title">Run Query</div>
							  </div>

							  <div class="content-box-large box-with-header">
							  <div class="table-responsive" style="overflow-x:auto;">
								  	
    <asp:TextBox ID="TextBox1" runat="server" Height="136px" TextMode="MultiLine" 
        Width="100%"></asp:TextBox>

		<div style="margin-top:20px; margin-bottom:20px">
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
        Text="Select Query" />

			</div>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:MiidConnectionString %>" 
        SelectCommand="SELECT * FROM [EndUser]"></asp:SqlDataSource>


    <%-- <asp:Button ID="Button3" runat="server" onclick="Button3_Click" Text="Tables" /> --%>
   
		
		<asp:Label ID="Label1" runat="server" ForeColor="#FF3300"></asp:Label>
    
		
		
		<asp:GridView ID="GridView1" runat="server">
		<alternatingrowstyle backcolor="#efefef"  
          forecolor="DarkBlue"
          />
    </asp:GridView>


    <asp:Button ID="Button4" runat="server" onclick="Button4_Click" 
        Text="Thumbnail Maker" />
    <asp:CheckBox ID="CheckBox1" runat="server" Text="Membership" />
								  </div>
								  </div>
						     
                   
					</div>
				</div>
		</div>

	
    </form>

 </asp:Content>