using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.RequestModel
{
    public class UserMode
    {

        [Required(ErrorMessage = "User Id is Required")]
        [RegularExpression(@"^[0-9]*$")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Role is Required")]
        [RegularExpression(@"^Admin|^Security$|^Police$|^Driver$|^Owner$|^Attendant$", ErrorMessage = "Roles are Admin, Security, Police, Driver, Attendant and Owner")]
        public string Role { get; set; }

    }
}
