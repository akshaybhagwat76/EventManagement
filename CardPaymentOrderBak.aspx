<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CardPaymentOrderBak.aspx.cs" Inherits="MiidWeb.IveriOrderBak" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="headTransaction">
    <title>iVeri Lite Example in ASP.NET 2.0 </title>
    <meta content="progid:DXImageTransform.Microsoft.Fade(duration=.25)" http-equiv="Page-Enter">
    <style type="text/css">
        body {
            font-family: sans-serif;
        }
        /*<![CDATA[*/
        P.clsHeading {
            color: #1C4231;
            font-family: arial;
            font-size: 17pt;
            font-weight: bold;
        }

        BODY.clsMain {
            background-color: #FFFFFF;
            background-image: url("/Images/background.gif");
            font-family: arial;
            font-size: 10pt;
        }

        TABLE.clsQuery {
            position: relative;
            background-color: #F0F0F0;
            border: 1px ridge black;
            border-collapse: collapse;
            margin-left: 12pt;
            margin-right: 12pt;
        }

        TD.clsQueryHeading {
            background-color: #1C4231;
            border: 1px solid #000000;
            color: #FFFFFF;
            font-family: arial;
            font-size: 8pt;
            font-weight: bold;
            padding: 2px;
            padding-right: 10px;
        }

        TD.clsQuery {
            background-color: #E0E0E0;
            border: 1px solid #F0F0F0;
            border-right: 1px solid black;
            color: #000000;
            font-family: arial;
            font-size: 8pt;
            padding: 2px;
            padding-left: 10px;
        }

        TD.clsInformation {
            background-color: #F0F0F0;
            border: 0px solid #385B38;
            color: #000000;
            font-family: arial;
            font-size: 8pt;
            padding: 15px;
        }

        INPUT.clsInputText {
            color: #000000;
            font-family: arial;
            font-size: 8pt;
            font-weight: normal;
        }

        INPUT.clsInputReadOnlyText {
            color: #000000;
            font-family: arial;
            font-size: 8pt;
            font-weight: normal;
            border: none;
            background-color: Transparent;
        }

        INPUT.clsInputButton {
            background-color: #FFF3D6;
            border-color: #FFF3D6;
            color: #1C4231;
            font-family: arial;
            font-size: 8pt;
            font-weight: bold;
        }

        INPUT.clsInputSubmit {
            background-color: #FFF3D6;
            border-color: #FFF3D6;
            color: #1C4231;
            font-family: arial;
            font-size: 8pt;
            font-weight: bold;
        }

        SELECT.clsInput {
            border-style: solid;
            color: #000000;
            font-family: arial;
            font-size: 8pt;
            font-weight: normal;
        }
        /*]]>*/
    </style>

</head>
<body class="clsMain">
    <div id="divSwipeUserPrompt" style="display: none; position: absolute; z-index: 14; width: 101%; height: 103%;">
        <div style="margin-left: 0px; position: absolute; left: -10px; top: -15px; width: 100%; height: 100%; text-align: center; background-color: black; opacity: 0.7; filter: alpha(opacity=70);"></div>
        <div style="position: absolute; text-align: center; padding-top: 50px; z-index: 15; top: 80px; background-color: #ececec; left: 170px; width: 400px; height: 100px; border: 2px ridge black;">
            <img style="position: relative; top: 10px;" alt="" src="../Img/NPaySmall.gif">
            <label id="labelNpayInstructions" style="font: 14px Dejavu Condensed; font-weight: bold; position: absolute; top: 10px; left: 50px;">Please Follow the instructions on the terminal...</label>
        </div>
    </div>
    <div id="divContent" class="clsContent" align="center">
        <br />
        <form runat="server" id="Form1" action="https://backoffice.nedsecure.co.za/Lite/Transactions/New/EasyAuthorise.aspx" method="post" name="Form1">
            <table class="clsQuery" cellspacing="0" border="0" align="center">
                <tbody>
                    <tr>
                        <td class="clsInformation" align="center" colspan="3">
                            <b>Confirm Details</b>
                        </td>
                    </tr>
                    <tr>
                        <td class="clsQueryHeading" align="left">Name: </td>
                        <td class="clsQuery" align="left" colspan="2" style="border-top: 1px solid black;">
                            <input id="Ecom_BillTo_Postal_Name_Prefix" class="clsInputReadOnlyText" type="hidden" style="width: 15px;" name="Ecom_BillTo_Postal_Name_Prefix" readonly="readonly" value="Mr.">


                            <asp:TextBox runat="server" ID="Ecom_BillTo_Postal_Name_First" ReadOnly="true"></asp:TextBox>

                            <input id="Ecom_BillTo_Postal_Name_Middle" type="hidden" name="Ecom_BillTo_Postal_Name_Middle">


                            <asp:TextBox runat="server" ID="Ecom_BillTo_Postal_Name_Last" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="clsQueryHeading" align="left">Email: </td>
                        <td class="clsQuery" align="left" colspan="2">

                            <asp:TextBox runat="server" ID="Ecom_BillTo_Online_Email" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="clsQueryHeading" align="left">Ship To Street (Line 1): </td>
                        <td class="clsQuery" align="left" colspan="2">

                            <asp:TextBox runat="server" ID="Ecom_ShipTo_Postal_Street_Line1" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="clsQueryHeading" align="left">Ship To Street (Line 2): </td>
                        <td class="clsQuery" align="left" colspan="2">

                            <asp:TextBox runat="server" ID="Ecom_ShipTo_Postal_Street_Line2" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="clsQueryHeading" align="left">Ship To City: </td>
                        <td class="clsQuery" align="left" colspan="2">


                            <asp:TextBox runat="server" ID="Ecom_ShipTo_Postal_City" ReadOnly="true"></asp:TextBox>

                        </td>
                    </tr>
                    <tr>
                        <td class="clsQueryHeading" align="left">Ship To Province: </td>
                        <td class="clsQuery" align="left" colspan="2">

                            <asp:TextBox runat="server" ID="Ecom_ShipTo_Postal_StateProv" ReadOnly="true"></asp:TextBox>

                        </td>
                    </tr>
                    <tr>
                        <td class="clsQueryHeading" align="left">Ship To Postal Code: </td>
                        <td class="clsQuery" align="left" colspan="2">

                            <asp:TextBox runat="server" ID="Ecom_ShipTo_Postal_PostalCode" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="clsQueryHeading" align="left">Merchant Reference: </td>
                        <td class="clsQuery" align="left" colspan="2" style="border-bottom: 1px solid black;">
                            <asp:TextBox runat="server" ID="Ecom_ConsumerOrderID" ReadOnly="true"></asp:TextBox>
                            <%--<input id="Ecom_ConsumerOrderID" class="clsInputReadOnlyText" type="text" maxlength="20" value="AUTOGENERATE" readonly="readonly" name="Ecom_ConsumerOrderID">--%>
                            <input id="Ecom_SchemaVersion" type="hidden" name="Ecom_SchemaVersion">
                            <input id="Ecom_TransactionComplete" type="hidden" value="false" name="Ecom_TransactionComplete">
                            <input id="Lite_Authorisation" type="hidden" value="false" name="Lite_Authorisation">
                            <input id="Lite_Version" type="hidden" value="2.0" name="Lite_Version">
                        </td>
                    </tr>
                   

                    <tr>
                        <td>
                            <input id="Lite_Order_LineItems_Product_1" class="clsInputReadOnlyText" type="hidden" value="MiiFunds Credit Card Topup" name="Lite_Order_LineItems_Product_1" />
                        </td>
                        <td>
                            <input id="Lite_Order_LineItems_Quantity_1" class="clsInputReadOnlyText" type="hidden" value="1" name="Lite_Order_LineItems_Quantity_1" />
                        </td>
                        <td>
                            <asp:HiddenField  runat="server" ID="Lite_Order_LineItems_Amount_1" ></asp:HiddenField>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--<input id="Lite_Order_LineItems_Product_2" class="clsInputReadOnlyText" type="text" value="Sample Product#2" name="Lite_Order_LineItems_Product_2" readonly="readonly">--%>
                        </td>
                        <td>
                            <%--<input id="Lite_Order_LineItems_Quantity_2" class="clsInputReadOnlyText" type="text" value="3" name="Lite_Order_LineItems_Quantity_2" readonly="readonly">--%>
                        </td>
                        <td>
                            <%--<input id="Lite_Order_LineItems_Amount_2" class="clsInputReadOnlyText" type="text" value="1000" name="Lite_Order_LineItems_Amount_2" readonly="readonly">--%>
                        </td>
                    </tr>



                    <%--<input id="Lite_Order_DiscountAmount" class="clsInputReadOnlyText" type="text" style="font-weight: bold; font-size: 12px;" maxlength="10" value="0" readonly="readonly" name="Lite_Order_DiscountAmount">--%>


                    <tr>
                        <td class="clsQueryHeading" align="left">Total Order Amount: </td>
                        <td class="clsQuery" align="left" colspan="2" style="border: 1px solid black;">
                            <input id="Lite_Merchant_ApplicationID" type="hidden" value="{2c22d31d-5d73-432c-9997-61bfef46f1fc}" name="Lite_Merchant_ApplicationID">
                            <%--<input id="Lite_Order_Amount" class="clsInputReadOnlyText" type="text" style="font-weight: bold; font-size: 12px;" maxlength="10" value="<% =Request.QueryString["am"] %>" readonly="readonly" name="Lite_Order_Amount">--%>
                            <asp:HiddenField runat="server" ID="Lite_Order_Amount" />
                            <asp:TextBox runat="server" ID="Lite_Order_Amount_Show" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="clsInformation" align="center" colspan="3" style="border-bottom: 1px solid black;">
                            <input id="Submit1" class="clsInputSubmit" type="submit" style="width: 75px;" onclick="javascript: submitForm();" value="Submit" name="buttonSubmit" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <table id="tblCardInformation" class="clsQuery" style="top: 10px; visibility: hidden">
                <tbody>
                    <tr>
                        <td class="clsQueryHeading" align="left"></td>
                        <td class="clsQuery" align="left">
                            <input id="Ecom_Payment_Card_Protocols" type="hidden" value="iVeri" name="Ecom_Payment_Card_Protocols" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <input id="Lite_Order_Terminal" type="hidden" value="77777001" name="Lite_Order_Terminal">
            <input id="Lite_Order_AuthorisationCode" type="hidden" name="Lite_Order_AuthorisationCode">
            <input id="Lite_Website_TextColor" type="hidden" value="#ffffff" name="Lite_Website_TextColor">
            <input id="Lite_Website_BGColor" type="hidden" value="#1C4231" name="Lite_Website_BGColor">
            <input id="Lite_ConsumerOrderID_PreFix" type="hidden" value="LITE" name="Lite_ConsumerOrderID_PreFix">
            <input id="Lite_Website_Successful_Url" type="hidden" value="http://examples.iveri.net/Lite/Result.asp" name="Lite_Website_Successful_Url">
            <input id="Lite_Website_Fail_Url" type="hidden" value="http://examples.iveri.net/Lite/Result.asp" name="Lite_Website_Fail_Url">
            <input id="Lite_Website_Error_Url" type="hidden" value="http://examples.iveri.net/Lite/Result.asp" name="Lite_Website_Error_Url">
            <input id="Lite_Website_Trylater_Url" type="hidden" value="http://examples.iveri.net/Lite/Result.asp" name="Lite_Website_Trylater_Url">
            <input id="Ecom_ShipTo_Postal_Name_Prefix" type="hidden" name="Ecom_ShipTo_Postal_Name_Prefix">
            <input id="Ecom_ShipTo_Postal_Name_First" type="hidden" name="Ecom_ShipTo_Postal_Name_First">
            <input id="Ecom_ShipTo_Postal_Name_Middle" type="hidden" name="Ecom_ShipTo_Postal_Name_Middle">
            <input id="Ecom_ShipTo_Postal_Name_Last" type="hidden" name="Ecom_ShipTo_Postal_Name_Last">
            <input id="Ecom_ShipTo_Postal_Name_Suffix" type="hidden" name="Ecom_ShipTo_Postal_Name_Suffix">
            <input id="Ecom_ShipTo_Postal_Street_Line3" type="hidden" name="Ecom_ShipTo_Postal_Street_Line3">
            <input id="Ecom_ShipTo_Postal_CountryCode" type="hidden" name="Ecom_ShipTo_Postal_CountryCode">
            <input id="Ecom_ShipTo_Telecom_Phone_Number" class="clsInputReadOnlyText" type="text" name="Ecom_ShipTo_Telecom_Phone_Number" readonly="readonly">
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
        </form>
    </div>
</body>
</html>
