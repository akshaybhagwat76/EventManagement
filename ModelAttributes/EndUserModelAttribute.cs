using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace MiidWeb
{

    [MetadataType(typeof(EndUserModelAttribute))]
    public partial class EndUser
    {
        // leave it empty.
        internal class FirstName
        {
            public FirstName()
            {
            }
        }
    }

    public class EndUserModelAttribute
    {
        // Your attribs will come here.
        [Display(Name = "ID Number")]
        //[Required(ErrorMessage = "Please enter ID / Passport Number.")]
        //[MaxLength(13, ErrorMessage = "ID number may only be 13 digits")]
        //[MinLength(13, ErrorMessage = "ID number may only be 13 digits")]
        //[RegularExpression(@"^\d+$", ErrorMessage = "Only numbers allowed")]
        //[Index(IsUnique = true)]
        public string IDNumber { get; set; }
        

        [Display(Name = "Cell")]
        [Required(ErrorMessage = "Please enter phone No.")]
       // [System.ComponentModel.DataAnnotations.MaxLength(10, ErrorMessage = "Cell number may only be 10 digits")]
        [System.ComponentModel.DataAnnotations.MinLength(9, ErrorMessage = "Phone number must be at least 9 digits")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only numbers allowed")]
        public string Cell { get; set; }

        [Display(Name = "Telephone")]
        //[Required(ErrorMessage = "Please enter Tel No.")]
        [System.ComponentModel.DataAnnotations.MaxLength(10, ErrorMessage = "Tel number may only be 10 digits")]
        [System.ComponentModel.DataAnnotations.MinLength(10, ErrorMessage = "Tel number may only be 10 digits")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only numbers allowed")]
        public string Telephone { get; set; }


        [Required(ErrorMessage = "Please enter First Name")]   
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Please enter Surname")]
           public string Surname { get; set; }

        //[Required(ErrorMessage = "Please enter Street Address")]
           public string StreetAddress { get; set; }

        //[Required(ErrorMessage = "Please enter Suburb")]
           public string Suburb { get; set; }

        //[Required(ErrorMessage = "Please enter City")]
           public string City { get; set; }

        //[Required(ErrorMessage = "Please enter Postal Code")]
        public string PostalCode { get; set; }

        //[Required(ErrorMessage = "Please select Gender")]
        public string Sex { get; set; }
        

        //[Required(ErrorMessage = "Please select Race")]
        public Nullable<int> RaceID { get; set; }


     


    }

}