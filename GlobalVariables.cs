using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb
{


    public static class GlobalVariables
    {

        public static string Company
        {
            get
            {
                return HttpContext.Current.Application["Company"] as string;
            }
            set
            {
                HttpContext.Current.Application["Company"] = value;
            }
        }


        public static string LogoFile
        {
            get
            {
                return HttpContext.Current.Application["LogoFile"] as string;
            }
            set
            {
                HttpContext.Current.Application["LogoFile"] = value;
            }
        }
        public static string CompanyTagLine
        {
            get
            {
                return HttpContext.Current.Application["CompanyTagLine"] as string;
            }
            set
            {
                HttpContext.Current.Application["CompanyTagLine"] = value;
            }
        }
        public static string Layout
        {
            get
            {
                return HttpContext.Current.Application["Layout"] as string;
            }
            set
            {
                HttpContext.Current.Application["Layout"] = value;
            }
        }

        public static string LayoutLogin
        {
            get
            {
                return HttpContext.Current.Application["LayoutLogin"] as string;
            }
            set
            {
                HttpContext.Current.Application["LayoutLogin"] = value;
            }
        }
        public static string LayoutAdmin
        {
            get
            {
                return HttpContext.Current.Application["LayoutAdmin"] as string;
            }
            set
            {
                HttpContext.Current.Application["LayoutAdmin"] = value;
            }
        }

        public static string Text1
        {
            get
            {
                return HttpContext.Current.Application["Text1"] as string;
            }
            set
            {
                HttpContext.Current.Application["Text1"] = value;
            }
        }
        public static string Text2
        {
            get
            {
                return HttpContext.Current.Application["Text2"] as string;
            }
            set
            {
                HttpContext.Current.Application["Text2"] = value;
            }
        }
        public static string Text3
        {
            get
            {
                return HttpContext.Current.Application["Text3"] as string;
            }
            set
            {
                HttpContext.Current.Application["Text3"] = value;
            }
        }
        public static string Text4
        {
            get
            {
                return HttpContext.Current.Application["Text4"] as string;
            }
            set
            {
                HttpContext.Current.Application["Text4"] = value;
            }
        }
        public static string Text5
        {
            get
            {
                return HttpContext.Current.Application["Text5"] as string;
            }
            set
            {
                HttpContext.Current.Application["Text5"] = value;
            }
        }
        public static string Text6
        {
            get
            {
                return HttpContext.Current.Application["Text6"] as string;
            }
            set
            {
                HttpContext.Current.Application["Text6"] = value;
            }
        }

		public static string HomeParaOne
		{
			get
			{
				return HttpContext.Current.Application["HomeParaOne"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeParaOne"] = value;
			}
		}

		public static string HomeParaTwo
		{
			get
			{
				return HttpContext.Current.Application["HomeParaTwo"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeParaTwo"] = value;
			}
		}

		public static string HomeImageOne
		{
			get
			{
				return HttpContext.Current.Application["HomeImageOne"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeImageOne"] = value;
			}
		}

		public static string HomeImagetwo
		{
			get
			{
				return HttpContext.Current.Application["HomeImagetwo"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeImagetwo"] = value;
			}
		}

		public static string HomeImagethree
		{
			get
			{
				return HttpContext.Current.Application["HomeImagethree"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeImagethree"] = value;
			}
		}

		public static string HomeImageFour
		{
			get
			{
				return HttpContext.Current.Application["HomeImageFour"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeImageFour"] = value;
			}
		}

		public static string HomeImageOneLink
		{
			get
			{
				return HttpContext.Current.Application["HomeImageOneLink"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeImageOneLink"] = value;
			}
		}

		public static string HomeImageTwoLink
		{
			get
			{
				return HttpContext.Current.Application["HomeImageTwoLink"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeImageTwoLink"] = value;
			}
		}

		public static string HomeImageThreeLink
		{
			get
			{
				return HttpContext.Current.Application["HomeImageThreeLink"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeImageThreeLink"] = value;
			}
		}

		public static string HomeImageFourLink
		{
			get
			{
				return HttpContext.Current.Application["HomeImageFourLink"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeImageFourLink"] = value;
			}
		}

		public static string HomeImageOneText
		{
			get
			{
				return HttpContext.Current.Application["HomeImageOneText"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeImageOneText"] = value;
			}
		}

		public static string HomeImageTwoText
		{
			get
			{
				return HttpContext.Current.Application["HomeImageTwoText"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeImageTwoText"] = value;
			}
		}

		public static string HomeImageThreeText
		{
			get
			{
				return HttpContext.Current.Application["HomeImageThreeText"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeImageThreeText"] = value;
			}
		}

		public static string HomeImageFourText
		{
			get
			{
				return HttpContext.Current.Application["HomeImageFourText"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeImageFourText"] = value;
			}
		}

		public static string HomeCardOneHeading
		{
			get
			{
				return HttpContext.Current.Application["HomeCardOneHeading"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeCardOneHeading"] = value;
			}
		}

		public static string HomeCardTwoHeading
		{
			get
			{
				return HttpContext.Current.Application["HomeCardTwoHeading"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeCardTwoHeading"] = value;
			}
		}

		public static string HomeCardThreeHeading
		{
			get
			{
				return HttpContext.Current.Application["HomeCardThreeHeading"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeCardThreeHeading"] = value;
			}
		}
		public static string HomeCardOnePara
		{
			get
			{
				return HttpContext.Current.Application["HomeCardOnePara"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeCardOnePara"] = value;
			}
		}

		public static string HomeCardTwoPara
		{
			get
			{
				return HttpContext.Current.Application["HomeCardTwoPara"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeCardTwoPara"] = value;
			}
		}

		public static string HomeCardThreePara
		{
			get
			{
				return HttpContext.Current.Application["HomeCardThreePara"] as string;
			}
			set
			{
				HttpContext.Current.Application["HomeCardThreePara"] = value;
			}
		}

		public static string LoginParaOne
		{
			get
			{
				return HttpContext.Current.Application["LoginParaOne"] as string;
			}
			set
			{
				HttpContext.Current.Application["LoginParaOne"] = value;
			}
		}

		public static string LoginParatwo
		{
			get
			{
				return HttpContext.Current.Application["LoginParatwo"] as string;
			}
			set
			{
				HttpContext.Current.Application["LoginParatwo"] = value;
			}
		}

		public static string CallToActionOne
		{
			get
			{
				return HttpContext.Current.Application["CallToActionOne"] as string;
			}
			set
			{
				HttpContext.Current.Application["CallToActionOne"] = value;
			}
		}

		public static string CallToActionTwo
		{
			get
			{
				return HttpContext.Current.Application["CallToActionTwo"] as string;
			}
			set
			{
				HttpContext.Current.Application["CallToActionTwo"] = value;
			}
		}

		public static string CallToActionThree
		{
			get
			{
				return HttpContext.Current.Application["CallToActionThree"] as string;
			}
			set
			{
				HttpContext.Current.Application["CallToActionThree"] = value;
			}
		}

	



		public static string SubdomainID
        {
            get
            {
                return HttpContext.Current.Application["SubdomainID"] as string;
            }
            set
            {
                HttpContext.Current.Application["SubdomainID"] = value;
            }
        }

		public static string CallToActionAll
		{
			get
			{
				return HttpContext.Current.Application["CallToActionAll"] as string;
			}
			set
			{
				HttpContext.Current.Application["CallToActionAll"] = value;
			}
		}

		public static string PurchaseContainerHeading
		{
			get
			{
				return HttpContext.Current.Application["PurchaseContainerHeading"] as string;
			}
			set
			{
				HttpContext.Current.Application["PurchaseContainerHeading"] = value;
			}
		}

		public static string PurchaseInfo
		{
			get
			{
				return HttpContext.Current.Application["PurchaseInfo"] as string;
			}
			set
			{
				HttpContext.Current.Application["PurchaseInfo"] = value;
			}
		}

		public static string PurchaseExtra
		{
			get
			{
				return HttpContext.Current.Application["PurchaseExtra"] as string;
			}
			set
			{
				HttpContext.Current.Application["PurchaseExtra"] = value;
			}

		}

		public static string PurchaseDetails
		{
			get
			{
				return HttpContext.Current.Application["PurchaseDetails"] as string;
			}
			set
			{
				HttpContext.Current.Application["PurchaseDetails"] = value;
			}

		}

		public static string CallToActionPurchase
		{
			get
			{
				return HttpContext.Current.Application["CallToActionPurchase"] as string;
			}
			set
			{
				HttpContext.Current.Application["CallToActionPurchase"] = value;
			}

		}

		public static string PurchaseTermsOne
		{
			get
			{
				return HttpContext.Current.Application["PurchaseTermsOne"] as string;
			}
			set
			{
				HttpContext.Current.Application["PurchaseTermsOne"] = value;
			}

		}

        public static string CustomTerms
        {
            get
            {
                return HttpContext.Current.Application["CustomTerms"] as string;
            }
            set
            {
                HttpContext.Current.Application["CustomTerms"] = value;
            }

        }

        public static string PurchaseTermsTwo
		{
			get
			{
				return HttpContext.Current.Application["PurchaseTermsTwo"] as string;
			}
			set
			{
				HttpContext.Current.Application["PurchaseTermsTwo"] = value;
			}

		}


        public static string PurchaseTermsThree
        {
            get
            {
                return HttpContext.Current.Application["PurchaseTermsThree"] as string;
            }
            set
            {
                HttpContext.Current.Application["PurchaseTermsThree"] = value;
            }

        }

        public static string PurchaseSoldOut
		{
			get
			{
				return HttpContext.Current.Application["PurchaseSoldOut"] as string;
			}
			set
			{
				HttpContext.Current.Application["PurchaseSoldOut"] = value;
			}

		}

		public static string Currency
		{
			get
			{
				return HttpContext.Current.Application["Currency"] as string;
			}
			set
			{
				HttpContext.Current.Application["Currency"] = value;
			}

		}

		public static string PurchaseNoAvailiblity
		{
			get
			{
				return HttpContext.Current.Application["PurchaseNoAvailiblity"] as string;
			}
			set
			{
				HttpContext.Current.Application["PurchaseNoAvailiblity"] = value;
			}

		}

		public static string PurchaseFilter
		{
			get
			{
				return HttpContext.Current.Application["PurchaseFilter"] as string;
			}
			set
			{
				HttpContext.Current.Application["PurchaseFilter"] = value;
			}

		}


		public static string PurchaseFindTickets
		{
			get
			{
				return HttpContext.Current.Application["PurchaseFindTickets"] as string;
			}
			set
			{
				HttpContext.Current.Application["PurchaseFindTickets"] = value;
			}

		}

		public static string PurchaseRowNumber
		{
			get
			{
				return HttpContext.Current.Application["PurchaseRowNumber"] as string;
			}
			set
			{
				HttpContext.Current.Application["PurchaseRowNumber"] = value;
			}

		}

		public static string PurchaseSeatNumber
		{
			get
			{
				return HttpContext.Current.Application["PurchaseSeatNumber"] as string;
			}
			set
			{
				HttpContext.Current.Application["PurchaseSeatNumber"] = value;
			}

		}

		public static string PurchaseTotalQunatity
		{
			get
			{
				return HttpContext.Current.Application["PurchaseTotalQunatity"] as string;
			}
			set
			{
				HttpContext.Current.Application["PurchaseTotalQunatity"] = value;
			}

		}

		public static string PurchaseSubTotal
		{
			get
			{
				return HttpContext.Current.Application["PurchaseSubTotal"] as string;
			}
			set
			{
				HttpContext.Current.Application["PurchaseSubTotal"] = value;
			}

		}

		public static string PurchaseTotal
		{
			get
			{
				return HttpContext.Current.Application["PurchaseTotal"] as string;
			}
			set
			{
				HttpContext.Current.Application["PurchaseTotal"] = value;
			}

		}

		public static string MyEventsHeading
		{
			get
			{
				return HttpContext.Current.Application["MyEventsHeading"] as string;
			}
			set
			{
				HttpContext.Current.Application["MyEventsHeading"] = value;
			}

		}


		public static string MyEventsParaOne
		{
			get
			{
				return HttpContext.Current.Application["MyEventsParaOne"] as string;
			}
			set
			{
				HttpContext.Current.Application["MyEventsParaOne"] = value;
			}

		}

		public static string MyEventsParaTwo
		{
			get
			{
				return HttpContext.Current.Application["MyEventsParaTwo"] as string;
			}
			set
			{
				HttpContext.Current.Application["MyEventsParaTwo"] = value;
			}

		}

		public static string MyEventsParaHelpHeading
		{
			get
			{
				return HttpContext.Current.Application["MyEventsParaHelpHeading"] as string;
			}
			set
			{
				HttpContext.Current.Application["MyEventsParaHelpHeading"] = value;
			}

		}

		public static string MyEventsHelpOne
		{
			get
			{
				return HttpContext.Current.Application["MyEventsHelpOne"] as string;
			}
			set
			{
				HttpContext.Current.Application["MyEventsHelpOne"] = value;
			}

		}

		public static string MyEventsHelpTwo
		{
			get
			{
				return HttpContext.Current.Application["MyEventsHelpTwo"] as string;
			}
			set
			{
				HttpContext.Current.Application["MyEventsHelpTwo"] = value;
			}

		}

		public static string MyEventsHelpThree
		{
			get
			{
				return HttpContext.Current.Application["MyEventsHelpThree"] as string;
			}
			set
			{
				HttpContext.Current.Application["MyEventsHelpThree"] = value;
			}

		}

		public static string MyEventsDownLoadHeading
		{
			get
			{
				return HttpContext.Current.Application["MyEventsDownLoadHeading"] as string;
			}
			set
			{
				HttpContext.Current.Application["MyEventsDownLoadHeading"] = value;
			}

		}

		public static string MyEventsHelpHeadingOne
		{
			get
			{
				return HttpContext.Current.Application["MyEventsHelpHeadingOne"] as string;
			}
			set
			{
				HttpContext.Current.Application["MyEventsHelpHeadingOne"] = value;
			}

		}

		public static string MyEventsHelpHeadingTwo
		{
			get
			{
				return HttpContext.Current.Application["MyEventsHelpHeadingTwo"] as string;
			}
			set
			{
				HttpContext.Current.Application["MyEventsHelpHeadingTwo"] = value;
			}

		}

		public static string MyEventsHelpHeadingThree
		{
			get
			{
				return HttpContext.Current.Application["MyEventsHelpHeadingThree"] as string;
			}
			set
			{
				HttpContext.Current.Application["MyEventsHelpHeadingThree"] = value;
			}

		}

		public static string ButtonDownload
		{
			get
			{
				return HttpContext.Current.Application["ButtonDownload"] as string;
			}
			set
			{
				HttpContext.Current.Application["ButtonDownload"] = value;
			}

		}

		public static string ButtonPending
		{
			get
			{
				return HttpContext.Current.Application["ButtonPending"] as string;
			}
			set
			{
				HttpContext.Current.Application["ButtonPending"] = value;
			}

		}

		public static string CreateFirstHeading
		{
			get
			{
				return HttpContext.Current.Application["CreateFirstHeading"] as string;
			}
			set
			{
				HttpContext.Current.Application["CreateFirstHeading"] = value;
			}

		}

		public static string CreateFirstParaOne
		{
			get
			{
				return HttpContext.Current.Application["CreateFirstParaOne"] as string;
			}
			set
			{
				HttpContext.Current.Application["CreateFirstParaOne"] = value;
			}

		}

		public static string CreateFirstFirstName
		{
			get
			{
				return HttpContext.Current.Application["CreateFirstFirstName"] as string;
			}
			set
			{
				HttpContext.Current.Application["CreateFirstFirstName"] = value;
			}

		}

		public static string CreateFirstFirstNameSub
		{
			get
			{
				return HttpContext.Current.Application["CreateFirstFirstNameSub"] as string;
			}
			set
			{
				HttpContext.Current.Application["CreateFirstFirstNameSub"] = value;
			}

		}

		public static string CreateFirstLastName
		{
			get
			{
				return HttpContext.Current.Application["CreateFirstLastName"] as string;
			}
			set
			{
				HttpContext.Current.Application["CreateFirstLastName"] = value;
			}

		}


		public static string CreateFirstLastNameSub
		{
			get
			{
				return HttpContext.Current.Application["CreateFirstLastNameSub"] as string;
			}
			set
			{
				HttpContext.Current.Application["CreateFirstLastNameSub"] = value;
			}

		}


		public static string CreateFirstCell
		{
			get
			{
				return HttpContext.Current.Application["CreateFirstCell"] as string;
			}
			set
			{
				HttpContext.Current.Application["CreateFirstCell"] = value;
			}

		}


		public static string CreateFirstCellSub
		{
			get
			{
				return HttpContext.Current.Application["CreateFirstCellSub"] as string;
			}
			set
			{
				HttpContext.Current.Application["CreateFirstCellSub"] = value;
			}

		}

		public static string CreateFirstID
		{
			get
			{
				return HttpContext.Current.Application["CreateFirstID"] as string;
			}
			set
			{
				HttpContext.Current.Application["CreateFirstID"] = value;
			}

		}

		public static string CreateFirstIDSub
		{
			get
			{
				return HttpContext.Current.Application["CreateFirstIDSub"] as string;
			}
			set
			{
				HttpContext.Current.Application["CreateFirstIDSub"] = value;
			}

		}

		public static string CreateFirstEmail
		{
			get
			{
				return HttpContext.Current.Application["CreateFirstEmail"] as string;
			}
			set
			{
				HttpContext.Current.Application["CreateFirstEmail"] = value;
			}

		}

		public static string CreateFirstSeucrePurchase
		{
			get
			{
				return HttpContext.Current.Application["CreateFirstSeucrePurchase"] as string;
			}
			set
			{
				HttpContext.Current.Application["CreateFirstSeucrePurchase"] = value;
			}

		}

		public static string Terms
		{
			get
			{
				return HttpContext.Current.Application["Terms"] as string;
			}
			set
			{
				HttpContext.Current.Application["Terms"] = value;
			}

		}

		public static string ChooseTenderHeadingOne
		{
			get
			{
				return HttpContext.Current.Application["ChooseTenderHeadingOne"] as string;
			}
			set
			{
				HttpContext.Current.Application["ChooseTenderHeadingOne"] = value;
			}

		}

		public static string ChooseTenderHeadingTwo
		{
			get
			{
				return HttpContext.Current.Application["ChooseTenderHeadingTwo"] as string;
			}
			set
			{
				HttpContext.Current.Application["ChooseTenderHeadingTwo"] = value;
			}

		}



		public static string ChooseTenderParaOne
		{
			get
			{
				return HttpContext.Current.Application["ChooseTenderParaOne"] as string;
			}
			set
			{
				HttpContext.Current.Application["ChooseTenderParaOne"] = value;
			}

		}

		public static string ChooseTenderParaTwo
		{
			get
			{
				return HttpContext.Current.Application["ChooseTenderParaTwo"] as string;
			}
			set
			{
				HttpContext.Current.Application["ChooseTenderParaTwo"] = value;
			}

		}

		public static string ChooseTenderButtonTwo
		{
			get
			{
				return HttpContext.Current.Application["ChooseTenderButtonTwo"] as string;
			}
			set
			{
				HttpContext.Current.Application["ChooseTenderButtonTwo"] = value;
			}

		}

		public static string ChooseTenderParaSubOne
		{
			get
			{
				return HttpContext.Current.Application["ChooseTenderParaSubOne"] as string;
			}
			set
			{
				HttpContext.Current.Application["ChooseTenderParaSubOne"] = value;
			}

		}

		public static string ChooseTenderParaSubTwo
		{
			get
			{
				return HttpContext.Current.Application["ChooseTenderParaSubTwo"] as string;
			}
			set
			{
				HttpContext.Current.Application["ChooseTenderParaSubTwo"] = value;
			}

		}

		public static string PurchaseArea
		{
			get
			{
				return HttpContext.Current.Application["PurchaseArea"] as string;
			}
			set
			{
				HttpContext.Current.Application["PurchaseArea"] = value;
			}

		}

		public static string SeatingPlanImage
		{
			get
			{
				return HttpContext.Current.Application["SeatingPlanImage"] as string;
			}
			set
			{
				HttpContext.Current.Application["SeatingPlanImage"] = value;
			}

		}

		public static string ButtonAllEvents
		{
			get
			{
				return HttpContext.Current.Application["ButtonAllEvents"] as string;
			}
			set
			{
				HttpContext.Current.Application["ButtonAllEvents"] = value;
			}

		}

		public static string ShowPassword
		{
			get
			{
				return HttpContext.Current.Application["ShowPassword"] as string;
			}
			set
			{
				HttpContext.Current.Application["ShowPassword"] = value;
			}

		}

		public static string ForgotpasswordText
		{
			get
			{
				return HttpContext.Current.Application["ForgotpasswordText"] as string;
			}
			set
			{
				HttpContext.Current.Application["ForgotpasswordText"] = value;
			}

		}


		public static string RegisterParaOne
		{
			get
			{
				return HttpContext.Current.Application["RegisterParaOne"] as string;
			}
			set
			{
				HttpContext.Current.Application["RegisterParaOne"] = value;
			}

		}

		public static string RegisterPasswordLength
		{
			get
			{
				return HttpContext.Current.Application["RegisterPasswordLength"] as string;
			}
			set
			{
				HttpContext.Current.Application["RegisterPasswordLength"] = value;
			}

		}

		public static string VoucherHeading
		{
			get
			{
				return HttpContext.Current.Application["VoucherHeading"] as string;
			}
			set
			{
				HttpContext.Current.Application["VoucherHeading"] = value;
			}

		}

		public static string VoucherPara
		{
			get
			{
				return HttpContext.Current.Application["VoucherPara"] as string;
			}
			set
			{
				HttpContext.Current.Application["VoucherPara"] = value;
			}

		}

		public static string CardPaymentHeading
		{
			get
			{
				return HttpContext.Current.Application["CardPaymentHeading"] as string;
			}
			set
			{
				HttpContext.Current.Application["CardPaymentHeading"] = value;
			}

		}

		public static string ButtonBack
		{
			get
			{
				return HttpContext.Current.Application["ButtonBack"] as string;
			}
			set
			{
				HttpContext.Current.Application["ButtonBack"] = value;
			}

		}

		public static string ButtonConfirm
		{
			get
			{
				return HttpContext.Current.Application["ButtonConfirm"] as string;
			}
			set
			{
				HttpContext.Current.Application["ButtonConfirm"] = value;
			}

		}

		public static string CardPaymentPara
		{
			get
			{
				return HttpContext.Current.Application["CardPaymentPara"] as string;
			}
			set
			{
				HttpContext.Current.Application["CardPaymentPara"] = value;
			}

		}








		// readonly variable
		public static string Foo
        {
            get
            {
                return "foo";
            }
        }

        // read-write variable
        public static string Bar
        {
            get
            {
                return HttpContext.Current.Application["Bar"] as string;
            }
            set
            {
                HttpContext.Current.Application["Bar"] = value;
            }
        }
    }


}