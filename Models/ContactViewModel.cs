using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
  public class ContactViewModel
  {
    [Required(ErrorMessage = "A subject is required")]
    public string subject { get; set; }
    [Required(ErrorMessage = "Your name is required")]
    public string name { get; set; }
    [Required(ErrorMessage = "The email address is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string email { get; set; }
    [Required(ErrorMessage = "The Message is required")]
    public string message { get; set; }

    public ContactViewModel()
    {

    }

  }
}