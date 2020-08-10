using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.RequestModel
{
    public class UUserModel
    {
        [Required(ErrorMessage = "User Id is Required")]
        [RegularExpression(@"^[0-9]*$")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Firstname is Required")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Lastname is Required")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "EmailId is Required")]
        [RegularExpression("^[0-9a-zA-Z]+([._+-][0-9a-zA-Z]+)*@[0-9a-zA-Z]+.[a-zA-Z]{2,4}([.][a-zA-Z]{2,3})?$", ErrorMessage = "EmailId is not valid")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Role is Required")]
        [RegularExpression(@"^Admin|^Security$|^Police$|^Driver$|^Owner$|^Attendant$", ErrorMessage = "Roles are Admin, Security, Police, Driver, Attendant and Owner")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Local Address is Required")]
        public string LocalAddress { get; set; }

        [Required(ErrorMessage = "Mobile Number is Required")]
        [RegularExpression("([1-9]{1}[0-9]{9})$")]
        public string MobileNumber { get; set; }

        [Required]
        [RegularExpression("^(?:m|M|male|Male|f|F|female|Female)$", ErrorMessage = "Not valid Gender eg : Male Or Female")]
        public string Gender { get; set; }

    }
}
