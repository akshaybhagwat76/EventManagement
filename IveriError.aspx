<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IveriError.aspx.cs" Inherits="MiidWeb.IveriError" %>

<!DOCTYPE html>
<!--
    Read Only by HTML5 UP
    html5up.net |
    Free for personal and commercial use under the CCA 3.0 license (html5up.net/license)
-->
<html>
<head>
  <title>MiiD cashless payment sytems for music festivals. concerts & Events in Cape Town  |   Home,</title>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  <meta name="description" content="Use your festival wristband to make purchases without cash Easy, safe, quick & secure cashless purchases.">
  <!--[if lte IE 8]><script src="assets/js/ie/html5shiv.js"></script><![endif]-->
  <link rel="stylesheet" href="../../content/css/main.css" />

  <!--[if lte IE 8]><link rel="stylesheet" href="assets/css/ie8.css" /><![endif]-->
  <script src="../../scripts/js/modernizr-2.6.2.min.js"></script>

  <link rel="stylesheet" href="~/Content/css/main.css" type="text/css">
  <link rel="stylesheet" href="~/Scripts/growl.css" type="text/css">

  <link rel="shortcut icon" href="~/favicon.ico" type="image/x-icon" />


  <!--[if lte IE 8]><link rel="stylesheet" href="assets/css/ie8.css" /><![endif]-->
  <script src="../../scripts/js/modernizr-2.6.2.min.js"></script>
</head>
<body>
  <div class="header_banner_conainer">
    <a href="https://www.miid.co.za"><img src="../../content/images/logo_dark.svg" alt="" class="logo" /></a>
  </div>
  <!-- Header -->
  <section id="header">
    <header>
      <span class="logo">
        <img src="../../content/images/logo_dark.svg" alt="" /></span>


    </header>
    <nav id="nav">
      <ul>
        <li><a href="https://www.miid.co.za">Home</a></li>
        <li><a href="Home/Help">Help</a></li>
        <li><a href="Home/Contact">Contact</a></li>
      </ul>
    </nav>
    <footer>
      <ul class="icons">
        <li><a href="https://twitter.com/MiiDEvents" class="icon fa-twitter"><span class="label">Twitter</span></a></li>
        <li><a href="https://www.facebook.com/Fullsummerfest-1707827266111866/?fref=ts" class="icon fa-facebook"><span class="label">Facebook</span></a></li>
        <li><a href="https://www.instagram.com/miid_events/" class="icon fa-instagram"><span class="label">Instagram</span></a></li>
      </ul>
    </footer>
  </section>

  <!--  first section picture & form -->

     <div id="wrapper">
    <!-- Wrapper -->
    
      <!-- Main -->
      <div id="main">

    <!-- opaque wrapper -->
    <div class="wrapper">

		<section>

              <div class="container">



      <div class="form_wrapper">
        <form name="Form1" method="post" id="Form1" runat="server">


          <div class="form_container">

            <div class="register_heading">MiiD Card Payment Result</div>


            <asp:Panel runat="server" ID="pnlSuccess" Visible="false">
              <div class="register_text">Your transaction was unsuccessful.</div>
              <div class="right_container_form_two">


                <div class="ticket_total_text_two">
                  Transaction Type
                    
                        <asp:Label runat="server" ID="lbltransactionType" />

                </div>

                <div class="ticket_total_text_two">
                  Transaction Amount
                    
                    <asp:Label runat="server" ID="lblamount" />
                </div>

                <div class="ticket_total_text_two">
                  Authorisation Code
                    
                    <asp:Label runat="server" ID="lblauthorisationCode" />
                </div>

                <div class="ticket_total_text_two">
                  Merchant Reference
                    
                    <asp:Label runat="server" ID="lblmerchantReference" />
                </div>
              </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlFailed" Visible="false">
              <div class="register_text">Your transaction failed.</div>
              <div class="right_container_form_two">
                <div class="ticket_total_text_two">
                  Result Status

                                    <asp:Label runat="server" ID="lblstatus" />
                </div>

                <div class="ticket_total_text_two">
                  Result Description

                                    <asp:Label runat="server" ID="lbldescription" />
                </div>
              </div>
            </asp:Panel>

            <div class="tender_ticket_small">
              <div class="ticket_check">
                <a href="../Home/Index" class="button special">
                  <input type="button" class="band_input_cancel" value="Continue" />
                </a>
              </div>
            </div>
          </div>
        </form>
      </div>
    </div>

		</section>
			</div>

			
		  </div>
  
  <!-- Footer -->
  <section id="footer">
    <div class="container">
      <ul class="icons">
        <li><a href="https://twitter.com/MiiDEvents" class="icon fa-twitter"><span class="label">Twitter</span></a></li>
        <li><a href="https://www.facebook.com/Mii-D-Event-Solutions-1707827266111866/?fref=ts" class="icon fa-facebook"><span class="label">Facebook</span></a></li>
        <li><a href="https://www.instagram.com/miid_events/" class="icon fa-instagram"><span class="label">Instagram</span></a></li>
      </ul>
      <ul class="copyright">
        <li><a href="~/Home/Contact">Contact us</a></li>
        <li><a href="~/Home/TermsConditions">Terms & Conditions</a></li>
        <li><a href="~/Home/IndexEO">Event Organiser info</a></li>
      </ul>
      <img src="../../content/images/geotrust.svg" class="scale_small" />
      <img src="../../content/images/payfast.svg" class="scale_small" />
      <img src="../../content/images/visa.svg" class="scale_small" />
      <img src="../../content/images/mastercard.svg" class="scale_small" />
      <img src="../../content/images/bankinglogos.svg" class="scale_small" />

      <ul class="copyright">
        <li>&copy; Copyright of MiiD (PTY) ltd. All rights reserved.</li>
      </ul>
      <ul class="copyright">
        <li>MiiD online ticketing & cashless solutions & payments  |
              16 Franssen Street, Bothasig, Cape Town, South Africa, 7441
        </li>
      </ul>
    </div>
  </section>
</body>
</html>
