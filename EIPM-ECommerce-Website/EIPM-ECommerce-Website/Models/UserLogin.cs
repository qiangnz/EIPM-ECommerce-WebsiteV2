using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EIPM_ECommerce_Website.Models
{
    public class UserLogin
    {
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.RegularExpression("^[a-z]+$", ErrorMessage = "Please enter lowercase characters")]
        [StringLength(5, ErrorMessage = "Please enter no more than 5 characters")]
        public string username { get; set; }

        [Required]
        [RegularExpression("^[a-z]+$", ErrorMessage = "Please enter lowercase characters")]
        [StringLength(5, ErrorMessage = "Please enter no more than 5 characters")]
        public string password { get; set; }
    }
}