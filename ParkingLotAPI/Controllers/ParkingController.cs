using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using CommonLayer.Exceptions;
using CommonLayer.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ParkingLotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private IParkingBL parkingLotBL;

        public ParkingController(IParkingBL parkingLotBL)
        {
            this.parkingLotBL = parkingLotBL;
        }

        [Authorize(Roles = "Admin, Driver, Attendant ,Owner")]
        [HttpPost]
        [Route("Park")]
        public IActionResult Park([FromBody] ParkingModel parkingDetails)
        {
            try
            {

                var parkResponse = parkingLotBL.Park(parkingDetails);

                if (parkResponse != null )
                {
                    bool Success = true;
                    var Message = "Vehical Parked";
                    return Ok(new { Success , Message , Data = parkResponse });
                }
                else if (parkResponse == null)
                {
                    bool Success = false;
                    var Message = "Vehical Is Already Parked";
                    return Conflict(new { Success , Message });
                }
                else
                {
                    bool Success = false;
                    var Message = "Lot Is Full";
                    return NotFound(new { Success , Message  });
                }

            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }


        [Authorize(Roles = " Admin, Driver ")]
        [HttpDelete("UnPark/{VehicalNumber}")]
        public IActionResult UnPark([FromRoute] string VehicalNumber)
        {
            try
            {

                var parkResponse = parkingLotBL.UnPark(VehicalNumber);

                if (parkResponse != null)
                {
                    bool Success = true;
                    var Message = "Vehical Parked";
                    return Ok(new { Success, Message, Data = parkResponse });
                }
                else if (parkResponse == null)
                {
                    bool Success = false;
                    var Message = "Vehical Is Already Parked";
                    return Conflict(new { Success, Message });
                }
                else
                {
                    bool Success = false;
                    var Message = "Lot Is Full";
                    return NotFound(new { Success, Message });
                }

            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }


        /// <summary>
        /// Function For Checking Parking Lot Status.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Owner, Security")]
        [HttpGet]
        [Route("Slot/Availability")]
        public IActionResult CheckLotStatus()
        {
            try
            {
                bool status = this.parkingLotBL.CheckLotStatus();

                if (status)
                {
                    return Ok(new { Success = true, Message = "Lot Is Available" });
                }
                else
                {
                    return NotFound(new { Success = false, Message = "Lot Is Full" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }

        [Authorize(Roles = "Admin, Driver ")]
        [HttpGet]
        [Route("Vehical/{VehicalNumber}")]
        public IActionResult GetVehicalByNumber([FromRoute] string VehicalNumber)
        {
            try
            {
                //Throws Custom Exception If VehicalNumber Is Not in Valid Format.
                if (!Regex.IsMatch(VehicalNumber, @"^[A-Z]{2}\s[0-9]{2}\s[A-Z]{1,2}\s[0-9]{4}$"))
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.INVALID_VEHICAL_NUMBER.ToString() + " Please Enter Vehical In 'MH 01 AZ 2005' This Format.");
                }

                var parkResponse = parkingLotBL.GetVehicalByNumber(VehicalNumber);

                if (parkResponse != null)
                {
                    bool Success = true;
                    var Message = "Vehical Available";
                    return Ok(new { Success, Message, Data = parkResponse });
                }
                else 
                {
                    bool Success = false;
                    var Message = "Vehical Not Available";
                    return NotFound(new { Success, Message });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }

        [Authorize(Roles = "Admin, Police, Security, Owner")]
        [HttpGet]
        [Route("Vehicals/{Color}")]
        public IActionResult GetVehicalDetailsByColor([FromRoute] string Color)
        {
            try
            {
                if (!Regex.IsMatch(Color, @"^[A-Z][a-zA-Z]*$"))
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.INVALID_COLOR.ToString() + " Please Enter Vehical Color.");
                }

                var parkResponse = parkingLotBL.GetVehicalDetailsByColor(Color);

                if (parkResponse != null)
                {
                    bool Success = true;
                    var Message = "Vehical Color Available";
                    return Ok(new { Success, Message, Data = parkResponse });
                }
                else 
                {
                    bool Success = false;
                    var Message = "Vehical Color Not Available";
                    return NotFound(new { Success, Message });
                }
                

            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }

        [Authorize(Roles = "Admin, Police, Security, Owner ")]
        [HttpGet("{Brand}")]
        public IActionResult GetVehicalDetailsByBrand([FromRoute] string Brand)
        {
            try
            {
                //Throws Custom Exception If VehicalNumber Is Not in Valid Format.
                if (!Regex.IsMatch(Brand, @"^[A-Z][a-zA-Z]*$"))
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.INVALID_COLOR.ToString() + " Please Enter Vehical Color.");
                }

                var parkResponse = parkingLotBL.GetVehicalDetailsByBrand(Brand);

                if (parkResponse != null)
                {
                    bool Success = true;
                    var Message = "Vehical Brand Available";
                    return Ok(new { Success, Message, Data = parkResponse });
                }
                else 
                {
                    bool Success = false;
                    var Message = "Vehical Brand Not Available";
                    return NotFound(new { Success, Message });
                }

            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }

        [Authorize(Roles = "Admin, Police, Security ")]
        [HttpGet("{Brand}/{Color}")]
        public IActionResult GetVehicalDetailsByBrandAndColor([FromRoute] string Brand , [FromRoute] string Color)
        {
            try
            {

                //Throws Custom Exception If VehicalNumber Is Not in Valid Format.
                if (!Regex.IsMatch(Brand, @"^[A-Z][a-zA-Z]*$"))
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.INVALID_COLOR.ToString() + " Please Enter Valied Vehical Color.");
                }

                var parkResponse = parkingLotBL.GetVehicalDetailsByBrandAndColor(Brand, Color);

                if (parkResponse != null)
                {
                    bool Success = true;
                    var Message = "Vehical Brand And Color Available";
                    return Ok(new { Success, Message, Data = parkResponse });
                }
                else 
                {
                    bool Success = false;
                    var Message = "Vehical Brand And Color Not Available";
                    return NotFound(new { Success, Message });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }

        [Authorize(Roles = "Admin, Police, Security ")]
        [HttpGet]
        [Route("vehicle/{ParkingSlot}")]
        public IActionResult GetVehicalDetailsByParkingSlot([FromRoute] string ParkingSlot)
        {
            try
            {

                //Throws Custom Exception If VehicalNumber Is Not in Valid Format.
                if (!Regex.IsMatch(ParkingSlot, @"^(?:A|B|C|D)$"))
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.INVALID_PARKING_SLOT.ToString() + " Please Enter valid slot Number eg. A,B,C,D.");
                }

                var parkResponse = parkingLotBL.GetVehicalDetailsByParkingSlot(ParkingSlot);

                if (parkResponse != null)
                {
                    bool Success = true;
                    var Message = " Vahicals Found In a Slot ";
                    return Ok(new { Success, Message, Data = parkResponse });
                }
                else 
                {
                    bool Success = false;
                    var Message = " Vahicals Not Found In a Slot ";
                    return NotFound(new { Success, Message });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }

        /// <summary>
        /// Function For Checking Parking Lot Status.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Owner, Security")]
        [HttpGet]
        [Route("Vehicals/All")]
        public IActionResult CheckAllVehical()
        {
            try
            {
                var Response = this.parkingLotBL.CheckAllVehical();

                if (Response != null)
                {
                    return Ok(new { Success = true, Message = "Vehical Found" , Data = Response });
                }
                else
                {
                    return NotFound(new { Success = false, Message = "Vehical Not Found" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }


        /// <summary>
        /// Function For Checking Parking Lot Status.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Owner, Security")]
        [HttpGet]
        [Route("Vehicals/AllPark")]
        public IActionResult CheckParkVehical()
        {
            try
            {
                var Response = this.parkingLotBL.CheckParkVehical();

                if (Response != null)
                {
                    return Ok(new { Success = true, Message = "Park Vehical Is Available", Data = Response });
                }
                else
                {
                    return NotFound(new { Success = false, Message = "Park Not Vehical Is Full" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }


        /// <summary>
        /// Function For Checking Parking Lot Status.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Owner, Security")]
        [HttpGet]
        [Route("Vehicals/AllUnPark")]
        public IActionResult CheckUnParkVehical()
        {
            try
            {
                var Response = this.parkingLotBL.CheckUnParkVehical();

                if (Response != null)
                {
                    return Ok(new { Success = true, Message = "UnPark Vehical Is Available", Data = Response });
                }
                else
                {
                    return NotFound(new { Success = false, Message = "UnPark Not Vehical Is Full" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { Success = false, Message = exception.Message });
            }
        }


    }
}
