//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MiidWeb
{
    using System;
    using System.Collections.Generic;
    
    public partial class Subdomain
    {
        public Subdomain()
        {
            this.Configurations = new HashSet<Configuration>();
        }
    
        public int ID { get; set; }
        public string SubdomainName { get; set; }
        public string CompanyName { get; set; }
        public Nullable<bool> Active { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Text4 { get; set; }
        public string Text5 { get; set; }
        public string Text6 { get; set; }
        public string EmailFile { get; set; }
        public string EmailTicketPurchase { get; set; }
        public string EmailTicketPurchaseEFT { get; set; }
        public string EmailTicketPurchaseMiiFunds { get; set; }
        public string EmailRegistration { get; set; }
        public string EmailUserUpdate { get; set; }
        public string EmailAccountActive { get; set; }
        public string EmailMiifundsTopup { get; set; }
        public string EmailMiifundsTopupEFT { get; set; }
        public string EmailTicketPurchaseEFTExpired { get; set; }
        public string LoginParaOne { get; set; }
        public string LoginParatwo { get; set; }
        public string HomeParaOne { get; set; }
        public string HomeParatwo { get; set; }
        public string HomeImageOne { get; set; }
        public string HomeImagetwo { get; set; }
        public string HomeImagethree { get; set; }
        public string HomeImageFour { get; set; }
        public string HomeImageOneLink { get; set; }
        public string HomeImageTwoLink { get; set; }
        public string HomeImageThreeLink { get; set; }
        public string HomeImageFourLink { get; set; }
        public string HomeImageOneText { get; set; }
        public string HomeImageTwoText { get; set; }
        public string HomeImageThreeText { get; set; }
        public string HomeImageFourText { get; set; }
        public string HomeCardOneHeading { get; set; }
        public string HomeCardTwoHeading { get; set; }
        public string HomeCardThreeHeading { get; set; }
        public string HomeCardOnePara { get; set; }
        public string HomeCardTwoPara { get; set; }
        public string HomeCardThreePara { get; set; }
        public string CallToActionOne { get; set; }
        public string CallToActionTwo { get; set; }
        public string CallToActionThree { get; set; }
        public string CallToActionAll { get; set; }
        public string PurchaseContainerHeading { get; set; }
        public string PurchaseInfo { get; set; }
        public string PurchaseDetails { get; set; }
        public string PurchaseExtra { get; set; }
        public string CallToActionPurchase { get; set; }
        public string PurchaseTermsOne { get; set; }
        public string PurchaseTermsTwo { get; set; }
        public string PurchaseSoldOut { get; set; }
        public string Currency { get; set; }
        public string PurchaseNoAvailiblity { get; set; }
        public string PurchaseFilter { get; set; }
        public string PurchaseFindTickets { get; set; }
        public string PurchaseRowNumber { get; set; }
        public string PurchaseSeatNumber { get; set; }
        public string PurchaseTotalQunatity { get; set; }
        public string PurchaseSubTotal { get; set; }
        public string PurchaseTotal { get; set; }
        public string MyEventsHeading { get; set; }
        public string MyEventsParaOne { get; set; }
        public string MyEventsParaTwo { get; set; }
        public string MyEventsParaHelpHeading { get; set; }
        public string MyEventsHelpOne { get; set; }
        public string MyEventsHelpTwo { get; set; }
        public string MyEventsHelpThree { get; set; }
        public string MyEventsDownLoadHeading { get; set; }
        public string ButtonDownload { get; set; }
        public string ButtonPending { get; set; }
        public string MyEventsHelpHeadingOne { get; set; }
        public string MyEventsHelpHeadingTwo { get; set; }
        public string MyEventsHelpHeadingThree { get; set; }
        public string CreateFirstHeading { get; set; }
        public string CreateFirstParaOne { get; set; }
        public string CreateFirstFirstName { get; set; }
        public string CreateFirstFirstNameSub { get; set; }
        public string CreateFirstLastName { get; set; }
        public string CreateFirstLastNameSub { get; set; }
        public string CreateFirstCell { get; set; }
        public string CreateFirstCellSub { get; set; }
        public string CreateFirstID { get; set; }
        public string CreateFirstIDSub { get; set; }
        public string CreateFirstEmail { get; set; }
        public string CreateFirstSeucrePurchase { get; set; }
        public string Terms { get; set; }
        public string ChooseTenderHeadingOne { get; set; }
        public string ChooseTenderHeadingTwo { get; set; }
        public string ChooseTenderParaOne { get; set; }
        public string ChooseTenderParaTwo { get; set; }
        public string ChooseTenderButtonOne { get; set; }
        public string ChooseTenderButtonTwo { get; set; }
        public string ChooseTenderParaSubOne { get; set; }
        public string ChooseTenderParaSubTwo { get; set; }
        public string PurchaseArea { get; set; }
        public string SeatingPlanImage { get; set; }
        public string ButtonAllEvents { get; set; }
        public string ShowPassword { get; set; }
        public string ForgotpasswordText { get; set; }
        public string RegisterParaOne { get; set; }
        public string VoucherHeading { get; set; }
        public string CardPaymentHeading { get; set; }
        public string CardPaymentPara { get; set; }
        public string ButtonBack { get; set; }
        public string ButtonConfirm { get; set; }
        public string VoucherPara { get; set; }
        public string RegisterPasswordLength { get; set; }
        public string KeyWordSearchText { get; set; }
        public string ManualEftProRata { get; set; }
        public string CustomTerms { get; set; }
        public string PurchaseTermsThree { get; set; }
        public string ManauEFTtopUpRequest { get; set; }
        public string ManauEFTtopUpConfirmed { get; set; }
    
        public virtual ICollection<Configuration> Configurations { get; set; }
    }
}
