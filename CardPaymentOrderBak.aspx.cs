using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiidWeb
{
    public partial class IveriOrderBak : System.Web.UI.Page
    {
        public BankTransaction model;
        public string Amount;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BankTransaction model = new BankTransaction();

                //model.SaveAsBankTransaction();

                model = BankTransactionRepository.GetByUniquePaymentID(Request.QueryString["on"]);

                Amount = ((int)model.Amount*100).ToString();//convert to cents
                Lite_Order_Amount.Value = Amount;
                Lite_Order_LineItems_Amount_1.Value = Amount;
                Lite_Order_Amount_Show.Text = "R " + model.Amount.ToString();
                Ecom_ConsumerOrderID.Text = model.Description;
                Ecom_BillTo_Postal_Name_First.Text = model.EndUser.Firstname;
                Ecom_BillTo_Online_Email.Text = model.EndUser.Email;
                Ecom_BillTo_Postal_Name_Last.Text = model.EndUser.Surname;
                Ecom_ShipTo_Postal_Street_Line1.Text = model.EndUser.StreetAddress;
                Ecom_ShipTo_Postal_Street_Line2.Text = model.EndUser.Suburb;
                Ecom_ShipTo_Postal_City.Text = model.EndUser.City;
                Ecom_ShipTo_Postal_StateProv.Text = model.EndUser.Province;
                Ecom_ShipTo_Postal_PostalCode.Text = model.EndUser.PostalCode;

               
            }

         
        }

      

    }
}