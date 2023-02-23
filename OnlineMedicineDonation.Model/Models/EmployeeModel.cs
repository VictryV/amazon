using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMedicineDonation.Model.Models
{
    public class EmployeeModel
    {
        [Key]
       
        public int Id { get; set; }
        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(49, ErrorMessage = "Name should not be more then 49 char!")]
        [RegularExpression(@"^[^<>{}\[\]]+$", ErrorMessage = "Please Enter Valid Name")]

        public string EmpName { get; set; }

        [Required(ErrorMessage = "Address is required!")]
        [StringLength(150, ErrorMessage = "Name should not be more then 150 char!")]
        public string EmpAddress { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number Required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string EmpContact { get; set; }
    }
}
