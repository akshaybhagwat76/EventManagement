<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CardPaymentOrder.aspx.cs" Inherits="MiidWeb.IveriOrder" %>

<!DOCTYPE html>
<!--
    Read Only by HTML5 UP
    html5up.net |
    Free for personal and commercial use under the CCA 3.0 license (html5up.net/license)
-->
<html>
<head>
    <title>Mii Funds options page</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!--[if lte IE 8]><script src="assets/js/ie/html5shiv.js"></script><![endif]-->
    <link rel="stylesheet" href="../../content/css/main.css" />
    <link rel="stylesheet" href="../../content/css/crop.css" type="text/css" />

    <script src="../../scripts/jquery.min.js"></script>
    <!--[if lte IE 8]><link rel="stylesheet" href="assets/css/ie8.css" /><![endif]-->
</head>
<body>

    <div class="header_banner_conainer">
        <a href="https://www.miid.co.za">
            <img src="../../content/images/logo_dark.svg" alt="" class="logo" /></a>
    </div>

    <!-- Header -->
    <section id="header">
        <header>
            <span class="logo">
                <img src="../../content/images/logo_dark.svg" alt="" class="small_logo" /></span>


        </header>
       
        <footer>
        </footer>

    </section>

    <!-- Wrapper -->
    <div id="wrapper">

        <!-- Main -->
        <div id="main">

            <!-- One -->
            <section>


                <div class="container">
                    <div class="underline_div">
                        <div class="two_columb">
                            <asp:Label ID="ScreenTitle" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="payment_div">
                        <img src="../../content/images/securepay.svg" class="payment_icons" /><img src="../../content/images/geotrust.svg" class="payment_icons" /><img src="../../content/images/iveri.svg" class="payment_icons" /><img src="../../content/images/visa.svg" class="payment_icons" /><img src="../../content/images/mastercard.svg" class="payment_icons" /></div>



                    <h3>Confirm Details</h3>





                    <form runat="server" id="Form1" action="https://portal.nedsecure.co.za/Lite/Authorise.aspx" method="post" name="Form1"> 

                    
                        <input id="Ecom_BillTo_Postal_Name_Prefix" type="hidden" style="width: 15px;" name="Ecom_BillTo_Postal_Name_Prefix" readonly="readonly" value="Mr.">

                        <div class="form_container">


                            <div class="row uniform">
                                <div class="6u 12u(xsmall)">
                                    First Name
                        <asp:TextBox runat="server" ID="Ecom_BillTo_Postal_Name_First" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="6u 12u(xsmall)">
                                    Surname
                            <input id="Ecom_BillTo_Postal_Name_Middle" type="hidden" name="Ecom_BillTo_Postal_Name_Middle">


                                    <asp:TextBox runat="server" ID="Ecom_BillTo_Postal_Name_Last" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row uniform">
                                <div class="6u 12u(xsmall)">
                                    Email

                            <asp:TextBox runat="server" ID="Ecom_BillTo_Online_Email" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="6u 12u(xsmall)">
                                    Telephone
                                <asp:TextBox runat="server" ID="Ecom_ShipTo_Telecom_Phone_Number" ReadOnly="true"></asp:TextBox>
                                </div>

                            </div>
                            <div class="row uniform">
                                <div class="6u 12u(xsmall)">
                                    Address Line 1
                            <asp:TextBox runat="server" ID="Ecom_ShipTo_Postal_Street_Line1" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="6u 12u(xsmall)">
                                    Address Line 2
                            <asp:TextBox runat="server" ID="Ecom_ShipTo_Postal_Street_Line2" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row uniform">
                                <div class="6u 12u(xsmall)">
                                    City
                            <asp:TextBox runat="server" ID="Ecom_ShipTo_Postal_City" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="6u 12u(xsmall)">
                                    Province
                            <asp:TextBox runat="server" ID="Ecom_ShipTo_Postal_StateProv" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row uniform">
                                <div class="6u 12u(xsmall)">
                                    Postal Code
                            <asp:TextBox runat="server" ID="Ecom_ShipTo_Postal_PostalCode" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="6u 12u(xsmall)">
                                    Unique Payment ID
                                <asp:TextBox runat="server" ID="Ecom_ConsumerOrderID" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>


                            <input id="Ecom_SchemaVersion" type="hidden" name="Ecom_SchemaVersion">
                            <input id="Ecom_TransactionComplete" type="hidden" value="false" name="Ecom_TransactionComplete">
                            <input id="Lite_Authorisation" type="hidden" value="false" name="Lite_Authorisation">
                            <input id="Lite_Version" type="hidden" value="2.0" name="Lite_Version">

                            <asp:HiddenField runat="server" ID="Lite_Order_LineItems_Product_1" Value="" />

                            <input id="Lite_Order_LineItems_Quantity_1" class="band_box" type="hidden" value="1" name="Lite_Order_LineItems_Quantity_1" />

                            <asp:HiddenField runat="server" ID="Lite_Order_LineItems_Amount_1"></asp:HiddenField>


                            <input id="Lite_Merchant_ApplicationID" type="hidden" value="{567B03C2-2FAC-4591-A94A-5684C9DE65F9}" name="Lite_Merchant_ApplicationID">

                            <asp:HiddenField runat="server" ID="Lite_Order_Amount" />
                            <div class="row uniform">
                                <div class="6u 12u(xsmall)">
                                    Amount
                            <asp:TextBox runat="server" ID="Lite_Order_Amount_Show" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="6u 12u(xsmall)">
                                    Included Fee
                            <asp:TextBox runat="server" ID="IncludedFee" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <ul class="actions">
                                <li>
                                    <input type="submit" class="button special" onclick="javascript: submitForm();" value="Submit" name="buttonSubmit" id="Submit1">
                                </li>
                            </ul>
                            <ul class="actions">
                                <li>
                                    <a href="..\MyMoneys\CancelTicket\0" class="cancel_button">Cancel</a>
                                </li>
                            </ul>
                            <input id="Ecom_Payment_Card_Protocols" type="hidden" value="iVeri" name="Ecom_Payment_Card_Protocols" />

                            <input id="Lite_Order_Terminal" type="hidden" value="77777001" name="Lite_Order_Terminal">
                            <input id="Lite_Order_AuthorisationCode" type="hidden" name="Lite_Order_AuthorisationCode">
                            <input id="Lite_Website_TextColor" type="hidden" value="#ffffff" name="Lite_Website_TextColor">
                            <input id="Lite_Website_BGColor" type="hidden" value="#1C4231" name="Lite_Website_BGColor">
                            <input id="Lite_ConsumerOrderID_PreFix" type="hidden" value="LITE" name="Lite_ConsumerOrderID_PreFix">
                            <input id="Lite_Website_Successful_Url" type="hidden" value="https://www.miid.co.za/IveriSuccess.aspx" name="Lite_Website_Successful_Url">
                            <input id="Lite_Website_Fail_Url" type="hidden" value="https://www.miid.co.za/IveriFail.aspx" name="Lite_Website_Fail_Url">
                            <input id="Lite_Website_Error_Url" type="hidden" value="https://www.miid.co.za/IveriError.aspx" name="Lite_Website_Error_Url">
                            <input id="Lite_Website_Trylater_Url" type="hidden" value="https://www.miid.co.za/IveriTrylater.aspx" name="Lite_Website_Trylater_Url">
                            <input id="Ecom_ShipTo_Postal_Name_Prefix" type="hidden" name="Ecom_ShipTo_Postal_Name_Prefix">
                            <input id="Ecom_ShipTo_Postal_Name_First" type="hidden" name="Ecom_ShipTo_Postal_Name_First">
                            <input id="Ecom_ShipTo_Postal_Name_Middle" type="hidden" name="Ecom_ShipTo_Postal_Name_Middle">
                            <input id="Ecom_ShipTo_Postal_Name_Last" type="hidden" name="Ecom_ShipTo_Postal_Name_Last">
                            <input id="Ecom_ShipTo_Postal_Name_Suffix" type="hidden" name="Ecom_ShipTo_Postal_Name_Suffix">
                            <input id="Ecom_ShipTo_Postal_Street_Line3" type="hidden" name="Ecom_ShipTo_Postal_Street_Line3">
                            <input id="Ecom_ShipTo_Postal_CountryCode" type="hidden" name="Ecom_ShipTo_Postal_CountryCode">

                            <input id="Ecom_ShipTo_Online_Email" type="hidden" name="Ecom_ShipTo_Online_Email">
                            <input id="Ecom_ReceiptTo_Postal_Name_Prefix" type="hidden" name="Ecom_ReceiptTo_Postal_Name_Prefix">
                            <input id="Ecom_ReceiptTo_Postal_Name_First" type="hidden" name="Ecom_ReceiptTo_Postal_Name_First">
                            <input id="Ecom_ReceiptTo_Postal_Name_Middle" type="hidden" name="Ecom_ReceiptTo_Postal_Name_Middle">
                            <input id="Ecom_ReceiptTo_Postal_Name_Last" type="hidden" name="Ecom_ReceiptTo_Postal_Name_Last">
                            <input id="Ecom_ReceiptTo_Postal_Name_Suffix" type="hidden" name="Ecom_ReceiptTo_Postal_Name_Suffix">
                            <input id="Ecom_ReceiptTo_Postal_Street_Line1" type="hidden" name="Ecom_ReceiptTo_Postal_Street_Line1">
                            <input id="Ecom_ReceiptTo_Postal_Street_Line2" type="hidden" name="Ecom_ReceiptTo_Postal_Street_Line2">
                            <input id="Ecom_ReceiptTo_Postal_Street_Line3" type="hidden" name="Ecom_ReceiptTo_Postal_Street_Line3">
                            <input id="Ecom_ReceiptTo_Postal_City" type="hidden" name="Ecom_ReceiptTo_Postal_City">
                            <input id="Ecom_ReceiptTo_Postal_StateProv" type="hidden" name="Ecom_ReceiptTo_Postal_StateProv">
                            <input id="Ecom_ReceiptTo_Postal_PostalCode" type="hidden" name="Ecom_ReceiptTo_Postal_PostalCode">
                            <input id="Ecom_ReceiptTo_Postal_CountryCode" type="hidden" name="Ecom_ReceiptTo_Postal_CountryCode">
                            <input id="Ecom_ReceiptTo_Telecom_Phone_Number" type="hidden" name="Ecom_ReceiptTo_Telecom_Phone_Number">
                            <input id="Ecom_ReceiptTo_Online_Email" type="hidden" name="Ecom_ReceiptTo_Online_Email">
                            <input id="Ecom_BillTo_Postal_Name_Suffix" type="hidden" name="Ecom_BillTo_Postal_Name_Suffix">
                            <input id="Ecom_BillTo_Postal_Street_Line1" type="hidden" name="Ecom_BillTo_Postal_Street_Line1">
                            <input id="Ecom_BillTo_Postal_Street_Line2" type="hidden" name="Ecom_BillTo_Postal_Street_Line2">
                            <input id="Ecom_BillTo_Postal_Street_Line3" type="hidden" name="Ecom_BillTo_Postal_Street_Line3">
                            <input id="Ecom_BillTo_Postal_City" type="hidden" name="Ecom_BillTo_Postal_City">
                            <input id="Ecom_BillTo_Postal_StateProv" type="hidden" name="Ecom_BillTo_Postal_StateProv">
                            <input id="Ecom_BillTo_Postal_PostalCode" type="hidden" name="Ecom_BillTo_Postal_PostalCode">
                            <input id="Ecom_BillTo_Postal_CountryCode" type="hidden" name="Ecom_BillTo_Postal_CountryCode">
                            <input id="Ecom_BillTo_Telecom_Phone_Number" type="hidden" name="Ecom_BillTo_Telecom_Phone_Number">
                            </div>
                    </form>
                </div>
            </section>
        </div>
    </div>









    <!-- Footer -->
    <!-- Footer -->
    <section id="footer">
        <div class="container">
            <ul class="icons">
                <li><a href="https://twitter.com/MiiDEvents" class="icon fa-twitter"><span class="label">Twitter</span></a></li>
                <li><a href="https://www.facebook.com/Fullsummerfest-1707827266111866/?fref=ts" class="icon fa-facebook"><span class="label">Facebook</span></a></li>
                <li><a href="https://www.instagram.com/miid_events/" class="icon fa-instagram"><span class="label">Instagram</span></a></li>
            </ul>
            <ul class="copyright">
                <li><a href="~/Home/Contact">Contact us</a></li>
                <li><a href="~/Home/TermsConditions">Terms & Conditions</a></li>
                <li><a href="~/Home/IndexEO">Event Organiser info</a></li>
            </ul>


            <ul class="copyright">
                <li>&copy; Copyright of MiiD (PTY) ltd. All rights reserved.</li>
                <li>MiiD online ticketing and cashless solutions
                        16 Franssen Street, Bothasig, Cape Town, South Africa, 7441
                </li>
            </ul>


        </div>
    </section>


    </div>

    <!-- Scripts -->


    <script src="../../scripts/jquery.scrollzer.min.js"></script>
    <script src="../../scripts/jquery.scrolly.min.js"></script>
    <script src="../../scripts/skel.min.js"></script>
    <script src="../../scripts/util.js"></script>
    <!--[if lte IE 8]><script src="assets/js/ie/respond.min.js"></script><![endif]-->
    <script src="../../scripts/main.js"></script>

</body>
</html>
