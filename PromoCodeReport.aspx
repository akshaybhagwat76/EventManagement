<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PromoCodeReport.aspx.cs" Inherits="MiidWeb.PromoCodeReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">


    <title>Promo Code Report</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="apple-touch-icon" sizes="57x57" href="apple-icon-57x57.png"/>
    <link rel="apple-touch-icon" sizes="60x60" href="apple-icon-60x60.png"/>
    <link rel="apple-touch-icon" sizes="72x72" href="apple-icon-72x72.png"/>
    <link rel="apple-touch-icon" sizes="76x76" href="apple-icon-76x76.png"/>
    <link rel="apple-touch-icon" sizes="114x114" href="apple-icon-114x114.png"/>
    <link rel="apple-touch-icon" sizes="120x120" href="apple-icon-120x120.png"/>
    <link rel="apple-touch-icon" sizes="144x144" href="apple-icon-144x144.png"/>
    <link rel="apple-touch-icon" sizes="152x152" href="apple-icon-152x152.png"/>
    <link rel="apple-touch-icon" sizes="180x180" href="apple-icon-180x180.png"/>
    <link rel="icon" type="image/png" sizes="192x192" href="android-icon-192x192.png"/>
    <link rel="icon" type="image/png" sizes="32x32" href="favicon-32x32.png"/>
    <link rel="icon" type="image/png" sizes="96x96" href="favicon-96x96.png"/>
    <link rel="icon" type="image/png" sizes="16x16" href="favicon-16x16.png"/>
    <link rel="manifest" href="/manifest.json"/>
    <meta name="msapplication-TileColor" content="#ffffff"/>
    <meta name="msapplication-TileImage" content="ms-icon-144x144.png"/>
    <meta name="theme-color" content="#ffffff"/>


    <link rel="stylesheet" href="~/Scripts/growl.css" type="text/css"/>
    <!-- Google Tag Manager -->
    <script>
(function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':
new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0],
j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;j.src=
'https://www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);
})(window,document,'script','dataLayer','GTM-TKCDRPP');</script>
    <!-- End Google Tag Manager -->

    <link href="content/css/css2018/bootstrap.css" rel="stylesheet"/>
        <link href="content/css/css2018/main.css" rel="stylesheet"/>
    <link href="content/css/newcss/font-awesome.css" rel="stylesheet"/>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.css" rel="stylesheet"/>
    <script src="content/js/jquery-1-12-4-min.js"></script>
     <script src="content/js/jquery-touchswipe-min.jss"></script>
 
   
   
    

   

    <link href="https://fonts.googleapis.com/css?family=Roboto" rel="stylesheet"/>
  



    <link rel="shortcut icon" href="~/favicon.ico" type="image/x-icon" />



</head>
<body>
	  <div class="navbar navbar-inverse  nav-backround-color " role="navigation">
            <div class="navbar-header ">
                <div class="navbar-header">
                    <a class="navbar-brand" href="~/Home/Index">
                        <img src="https://www.miid.co.za/Content/images2018/logo.svg" class="logo hidden-md hidden-lg" />
                        <img src="https://www.miid.co.za/Content/images2018/logo.svg" class="logo hidden-sm hidden-xs" />
                    </a>
                    <div class="new-tag-line "><div class=" hidden-xs hidden-sm">Safe Secure Online ticketing &amp; Cashless Events</div></div>        

                </div>
            </div>     
        </div>
  


	 <div class="container">
        <div class="row vertical-offset-40px" style="margin-top:100px;">
			 <div class="col-md-10 col-md-offset-1">

    <form id="form1" runat="server">
		
    <div>
    
    </div>
        <asp:GridView ID="GridView1" runat="server" Width="100%" CellPadding="4">
			    <AlternatingRowStyle BackColor="#efefef"  />
           
        </asp:GridView>
    </form>
	</div>
	</div>
	</div>
</body>
</html>
