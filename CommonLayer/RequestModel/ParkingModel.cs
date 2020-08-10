using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.RequestModel
{
    public class ParkingModel
    {

        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "First Letter Must Be Capital")]
        [Required(ErrorMessage = "Please Enter Owner Name")]
        public string VehicalOwnerName { get; set; }

        [EmailAddress]
        public string VehicalOwnerEmail { get; set; }

        [RegularExpression(@"^[A-Za-z]{2}\s[0-9]{2}\s[A-Za-z]{1,2}\s[0-9]{4}$", ErrorMessage = "Please Enter Vehical Number Like 'MH 01 AB 1111'")]
        [Required(ErrorMessage = "Please Enter The Vehical Number")]
        public string VehicalNumber { get; set; }

        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "First Letter Must Be Capital")]
        [Required(ErrorMessage = "Please Enter Vehical Brand")]
        public string Brand { get; set; }

        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "First Letter Must Be Capital")]
        [Required(ErrorMessage = "Please Enter Vehical Color")]
        public string Color { get; set; }

        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "First Letter Must Be Capital")]
        public string DriverName { get; set; }

        [Required]
        [RegularExpression("^(?:N|n|no|No|Y|y|yes|Yes)$", ErrorMessage = "Not valid Gender eg : Yes Or No")]
        public string IsHandicap { get; set; }

        [Required]
        [RegularExpression("^(?:A|B|C|D)$", ErrorMessage = "Not valid ParkingSlot eg : A | B | C| D ")]
        public string ParkingSlot { get; set; }
    }   
}
