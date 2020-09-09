using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
    public class NFCTagViewModel
    {

        public int ID { get; set; }

        public int EndUserID { get; set; }

        public int StatusID { get; set; }
        public string StatusCode { get; set; }

        public DateTime ActivationDate { get; set; }
        public string IDNumber { get; set; }

        public string Email { get; set; }

        public string Firstname { get; set; }

        public string Surname { get; set; }


        public string NewIDNumber { get; set; }

        public string NewEmail { get; set; }

        public string TagNumber { get; set; }

        public string ActivationCode { get; set; }
        public int TagPin { get; set; }

        public int? NewTagPin { get; set; }
        public int? ConfirmTagPin { get; set; }

        public bool TermsAndConditions { get; set; }


        public NFCTagViewModel() { }

        public NFCTagViewModel(NFCTag nFCTag)
        {

            var user = EndUserRepository.GetByUserID(nFCTag.EndUserID ?? 0);

            ID = nFCTag.ID;
            EndUserID = (int)nFCTag.EndUserID;
            IDNumber = user.IDNumber;
            ActivationCode = nFCTag.ActivationCode;
            ActivationDate = (DateTime)nFCTag.ActivationDate;
            Email = user.Email;
            NewTagPin = null;
            StatusID = nFCTag.StatusID ?? 0;
            TagNumber = nFCTag.TagNumber;
            TagPin = nFCTag.TagPin ?? 0;
            Firstname = user.Firstname;
            Surname = user.Surname;

            StatusCode = nFCTag.Status.Code;
        }

    }
}