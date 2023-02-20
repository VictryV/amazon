using OnlineMedicineDonation.Model.Filter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMedicineDonation.Model.APIModel
{
    public class UserCredential
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter UserName")]
        [ValidateAllowAll("UserName", 100)]
        public string? Username { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter Password")]
        [ValidateAllowAll("Password", 100)]
        public string? Password { get; set; }
    }
}
