using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstepTechnologies.Models
{
    public class ContactModel
    {

        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }
      
        [Required(ErrorMessage = "Please enter email")]

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".com)+))$",
                            ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
         [Required(ErrorMessage = "Please select Topics")]
        public string Topics { get; set; }
        [Required]
        public string Message { get; set; }
    }

 }