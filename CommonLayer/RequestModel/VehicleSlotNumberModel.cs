using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.RequestModel
{
    public class VehicleSlotNumberModel
    {

        [Required]
        [RegularExpression("^(?:A|B|C|D)$", ErrorMessage = "Not valid ParkingSlot eg : A | B | C| D ")]
        public string ParkingSlot { get; set; }

    }
}
