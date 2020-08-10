using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.ResponseModel
{
    public class RParkingModel
    {
    
        public int ReceiptNumber { get; set; }

        public string VehicalOwnerName { get; set; }

        public string VehicalOwnerEmail { get; set; }

        public string VehicalNumber { get; set; }

        public string Brand { get; set; }

        public string Color { get; set; }

        public string DriverName { get; set; }

        public string ParkingSlot { get; set; }

        public string Status { get; set; }

        public string ParkingDate { get; set; }

        public string UnparkDate { get; set; }

        public string TotalTime { get; set; }

        public string TotalAmount { get; set; }

        public string IsHandicap { get; set; }
    }
}
