using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.RequestModel
{
    public class UserLoginModel
    {

        /// <summary>
        /// define employee id variable
        /// </summary>
        [Required]
        [RegularExpression("^[0-9a-zA-Z]+([._+-][0-9a-zA-Z]+)*@[0-9a-zA-Z]+.[a-zA-Z]{2,4}([.][a-zA-Z]{2,3})?$", ErrorMessage = "EmailId is not valid")]
        public string EmailId { get; set; }

        /// <summary>
        /// define user password variable
        /// </summary>
        [Required]
        //[RegularExpression("^.{3,30}$", ErrorMessage = "Password Length should be between 8 to 15")]
        public string Password { get; set; }


    }
}
