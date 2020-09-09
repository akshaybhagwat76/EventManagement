<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PayfastConfirm.aspx.cs" Inherits="MiidWeb.PayfastConfirm" %>

<!DOCTYPE html>

<head>
    <title>Payment Confimred</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="apple-touch-icon" sizes="57x57" href="/apple-icon-57x57.png">
    <link rel="apple-touch-icon" sizes="60x60" href="/apple-icon-60x60.png">
    <link rel="apple-touch-icon" sizes="72x72" href="/apple-icon-72x72.png">
    <link rel="apple-touch-icon" sizes="76x76" href="/apple-icon-76x76.png">
    <link rel="apple-touch-icon" sizes="114x114" href="/apple-icon-114x114.png">
    <link rel="apple-touch-icon" sizes="120x120" href="/apple-icon-120x120.png">
    <link rel="apple-touch-icon" sizes="144x144" href="/apple-icon-144x144.png">
    <link rel="apple-touch-icon" sizes="152x152" href="/apple-icon-152x152.png">
    <link rel="apple-touch-icon" sizes="180x180" href="/apple-icon-180x180.png">
    <link rel="icon" type="image/png" sizes="192x192" href="/android-icon-192x192.png">
    <link rel="icon" type="image/png" sizes="32x32" href="/favicon-32x32.png">
    <link rel="icon" type="image/png" sizes="96x96" href="/favicon-96x96.png">
    <link rel="icon" type="image/png" sizes="16x16" href="/favicon-16x16.png">
    <link rel="manifest" href="/manifest.json">
    <meta name="msapplication-TileColor" content="#ffffff">
    <meta name="msapplication-TileImage" content="/ms-icon-144x144.png">
    <meta name="theme-color" content="#ffffff">


    <link rel="stylesheet" href="~/Scripts/growl.css" type="text/css">
    <!-- Google Tag Manager -->
    <script>
(function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':
new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0],
j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;j.src=
'https://www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);
})(window,document,'script','dataLayer','GTM-TKCDRPP');</script>
    <!-- End Google Tag Manager -->

    <link href="content/css/css2018/bootstrap.css" rel="stylesheet">
        <link href="content/css/css2018/main.css" rel="stylesheet">
    <link href="content/css/newcss/font-awesome.css" rel="stylesheet">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.css" rel="stylesheet">
    <script src="content/js/jquery-1-12-4-min.js"></script>
     <script src="content/js/jquery-touchswipe-min.jss"></script>
 
   
   
    

   

    <link href="https://fonts.googleapis.com/css?family=Roboto" rel="stylesheet">
    <Script>
  function openNav() {
    document.getElementById("mySidenav").style.width = "70%";
    // document.getElementById("flipkart-navbar").style.width = "50%";
    document.body.style.backgroundColor = "rgba(0,0,0,0.4)";
}

function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
    document.body.style.backgroundColor = "rgba(0,0,0,0)";
}
    </Script>

    <!--[if lte IE 8]><script src="assets/js/ie/html5shiv.js"></script><![endif]-->
    <!--[if lte IE 8]><link rel="stylesheet" href="assets/css/ie8.css" /><![endif]-->


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
                        <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                            <span class="sr-only">Toggle navigation</span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                        </button>

                    </div>


                </div>

                <div class="collapse navbar-collapse">
                    <ul class="nav navbar-nav pull-right navbar-padding-top hidden-sm hidden-xs" style="padding-top:0px;">
                      

                            <li style="padding-top:27px; color:#ffffff;">
                             Powered by Mi-id Online Ticketing & Cashless Services
                            </li>
                            
                          
                           




                        </ul>

                </div>
            </div>
      <div class="navbar navbar-inverse navbar-static-top" role="navigatio
    <div class="collapse navbar-collapse">
        <ul class="nav navbar-nav">




                <li class=""><a href="../Home/Index">Home</a></li>
                 <li class=""><a href="../Events/Search">All Events</a></li>
              <li class=""><a href="../Events/MiiEvents">Your Tickets</a></li>
          






  </ul>
</div>
          </div>

       


    
    
        <form id="form1" runat="server">

          <asp:Panel runat="server" ID="pnlTicketPurchase" Visible="false">
      <div class="container bg-grey">
        <div class="row vertical-offset-40px">


            <div class="col-md-8 col-md-offset-2">

                <div class="panel panel-default ">

                    <div class="panel-body text-center">
         

                   <h3> Transaction Successful</h3>
              <br><br>
               <i class="fa fa-check fa-5x" aria-hidden="true" style="color:#139242"></i>
               <br><br>
                 <p >Ticket(s) purchased successfully. You will also receive an email confirming the transaction.</p>

               <a href="../Events/MiiEvents" class="btn btn-lg btn-default btn-block">View your tickets</a>

         
        
              </div>
            </div>
            </div>
          </div>
        </div>
           
          </asp:Panel>






          <asp:Panel runat="server" ID="pnlMiiFundsTopup" Visible="false">

<div class="container bg-grey">
        <div class="row vertical-offset-40px">


            <div class="col-md-8 col-md-offset-2">

                <div class="panel panel-default ">

                    <div class="panel-body text-center">
         

                   <h3> Transaction Successful</h3>
              <br><br>
               <i class="fa fa-check fa-5x" aria-hidden="true" style="color:#139242"></i>
               <br><br>
                 <p> You will also receive an email confirming the transaction.</p>
               <a  href="../MyMoneys/IndexByUserID" class="btn btn-lg btn-default btn-block">View Pre-Paid Funds</a>

              

         
        
              </div>
            </div>
           </div>
            </div>
    </div>
          </asp:Panel>
        </form>
      
  
     <script src="~/Scripts/jsnew/jquery.js"></script>
        <!-- Bootstrap Core JavaScript -->
        <script src="~/Scripts/jsnew/bootstrap.min.js"></script>
         <script src="~/content/js/jquery1.1.2/jquery.min.js"></script>
         <script src="~/content/js/jquery2.1.3/jquery.min.js"></script>
    <script src="~/content/js/bootstrap.min.js"></script>

       
     
    <footer>
                <div class="footer" id="footer">
                    <div class="container">
                        <div class="row">
                            <div class="col-lg-3  col-md-3 col-sm-12 col-xs-12">
                                <h3> General </h3>
                                <ul>
                                    <li><a href="../Home/Help">Help</a></li>
                                    <li><a href="../Home/Contact">Contact</a></li>
                                    <li><a href="../Home/TermsConditions">Terms &amp; Conditions</a></li>
                                    <li><a href="../Home/FAQ">T&C's</a></li>

									<li><a href="../Home/RefundsPolicy">Refund Policy</a></li>
                                   

                                </ul>
                            </div>
							<div class="col-lg-3  col-md-3 col-sm-12 col-xs-12">
								<h3>Event Organisers</h3>
								<ul>
									<li> <a href="../Home/HowItWorks"> How it works </a> </li>
									<li> <a href="../Home/ProductSummary">Services &amp; Products </a> </li>
									<li> <a href="../Home/HelpEventOrganiser">Event Organiser's FAQ's </a> </li>
									<li> <a href="../Home/RegisterDoc"> Terms Of Service </a> </li>
								</ul>
							</div>
							<div class="col-lg-3  col-md-3 col-sm-12 col-xs-12">
								<h3>Products &Aacute; Services</h3>
								<ul>
									<li> <a href="../Home/TicketingExplanation">Online Ticketing </a> </li>
									<li> <a href="../Home/BoxOffice">Box Office </a> </li>
									<li> <a href="../Home/NFCTickets">NFC Ticketing</a> </li>
									<li> <a href="../Home/MiiFundsExplanation">Cashless </a> </li>
									<li> <a href="../Home/WhiteLabel">White Label</a> </li>
									
								</ul>
							</div>
                            
                           
                            
							<div class="col-lg-3  col-md-3 col-sm-12 col-xs-12">
								<h3>News, updates &amp; Ticket Give-Aways</h3>
								<ul>
									<li>
										<div class="input-append newsletter-box text-center">

											<link href="//cdn-images.mailchimp.com/embedcode/slim-10_7.css" rel="stylesheet" type="text/css">
											<style type="text/css">
												#mc_embed_signup {
													clear: left;
													font: 14px Helvetica,Arial,sans-serif;
												}
												/* Add your own MailChimp form style overrides in your site stylesheet or in this style block.
													We recommend moving this block and the preceding CSS link to the HEAD of your HTML file. */
											</style>
											<div id="mc_embed_signup class="form-control">
												<form action="https://Miid.us11.list-manage.com/subscribe/post?u=ed8a48e7c98e079fec3fc9d69&amp;id=7be4714437" method="post" id="mc-embedded-subscribe-form" name="mc-embedded-subscribe-form" class="validate" target="_blank" novalidate>
													<div id="mc_embed_signup_scroll">

														<input type="email" value="" name="EMAIL" class="form-control input-lg" id="mce-EMAIL" placeholder="email address" required>
														<!-- real people should not fill this in and expect good things - do not remove this or risk form bot signups-->
														<div style="position: absolute; left: -5000px;" aria-hidden="true"><input type="text" name="b_ed8a48e7c98e079fec3fc9d69_7be4714437" tabindex="-1" value=""></div>
														<br>
														<input type="submit" value="Subscribe" name="subscribe" class="btn btn-primary btn-lg btn-block" id="mc-embedded-subscribe">
													</div>
												</form>
											</div>


										</div>
									</li>
								</ul>
								<ul class="social">
									<li> <a href="https://www.facebook.com/miidevents/"> <i class="fa fa-facebook">   </i> </a> </li>
									<li> <a href="https://api.whatsapp.com/send?phone=27614512633"> <i class="fa fa-whatsapp">   </i> </a> </li>

								</ul>
								<br><br><br>
							</div>

                        </div>
                        <!--/.row-->
                    </div>
                    <!--/.container-->
                </div>
                <!--/.footer-->

                <div class="footer-bottom">
                    <div class="container">
                        <p class="pull-left"> Copyright © Mii-D PTY ltd </p>
                        <div class="pull-right">
                            <ul class="nav nav-pills payments">
                                <li><i class="fa fa-cc-visa fa-lg"></i></li>
                                <li><i class="fa fa-cc-mastercard fa-lg"></i></li>

                            </ul>
                        </div>
                    </div>
                </div>
                <!--/.footer-bottom-->
            </footer>

     
    </body>
