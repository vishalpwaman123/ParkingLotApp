using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using CommonLayer.Exceptions;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ParkingLotAPI.MSMQSender;

namespace ParkingLotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private IParkingBL parkingLotBL;

        private readonly IDistributedCache distributedCache;

        /// <summary>
        /// Declare Sender object
        /// </summary>
        Sender senderObject = new Sender();

        public ParkingController(IParkingBL parkingLotBL, IDistributedCache distributedCache)
        {
            this.parkingLotBL = parkingLotBL;
            this.distributedCache = distributedCache;
        }

        //[Authorize(Roles = "Admin, Driver, Attendant ,Owner")]
        [HttpPost]
        [Route("Park")]
        public IActionResult Park([FromBody] ParkingModel parkingDetails)
        {
            try
            {

                string cacheKey = "Parking";

                distributedCache.Remove(cacheKey);


                if (parkingDetails.VehicalOwnerName == null || parkingDetails.VehicalNumber == null || parkingDetails.VehicalOwnerEmail == null ||
                     parkingDetails.Brand == null || parkingDetails.Color == null || parkingDetails.DriverName == null)
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.NULL_EXCEPTION.ToString());
                }

                if (parkingDetails.VehicalOwnerName == "" || parkingDetails.VehicalNumber == "" || parkingDetails.VehicalOwnerEmail == "" ||
                    parkingDetails.Brand == "" || parkingDetails.Color == "" || parkingDetails.DriverName == "")
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.EMPTY_EXCEPTION.ToString());
                }

                RParkingModel parkResponse = parkingLotBL.Park(parkingDetails);

                if (parkResponse != null )
                {

                    //Message For MSMQ.
                    string message = "  Hello " + Convert.ToString(parkResponse.VehicalOwnerName) +
                  "\n Your " + " Vehicle Park Succesfully" +
                  "\n Driver Name :" + Convert.ToString(parkResponse.DriverName) +
                  "\n Email :" + Convert.ToString(parkResponse.VehicalOwnerEmail) +
                  "\n Vehicle Number: " + Convert.ToString(parkResponse.VehicalNumber) +
                  "\n Vehicle Brand :  " + Convert.ToString(parkResponse.Brand) +
                  "\n Vehicle Color : " + Convert.ToString(parkResponse.Color) +
                  "\n Parking Slot : " + Convert.ToString(parkResponse.ParkingSlot) +
                  "\n Parking Date : " + Convert.ToString(parkResponse.ParkingDate) +
                  "\n Handicapped : " + Convert.ToString(parkResponse.IsHandicap) + "\n\n\n";

                    string status = "parked";
                    senderObject.Send(message, status);

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


        //[Authorize(Roles = " Admin, Driver ,Owner")]
        [HttpDelete("UnPark/{VehicalNumber}")]
        public IActionResult UnPark([FromRoute] string VehicalNumber)
        {
            try
            {

                var parkResponse = parkingLotBL.UnPark(VehicalNumber);

            if (parkResponse != null)
            {

                                //Message For MSMQ.
                                string message = "Hello " + Convert.ToString(parkResponse.VehicalOwnerName) +
                              "\n Your " + " Vehicle UnPark Succesfully" +
                              "\n Driver Name :" + Convert.ToString(parkResponse.DriverName) +
                              "\n Email :" + Convert.ToString(parkResponse.VehicalOwnerEmail) +
                              "\n Vehicle Number: " + Convert.ToString(parkResponse.VehicalNumber) +
                              "\n vehicle Brand :  " + Convert.ToString(parkResponse.Brand) +
                              "\n Vehicle Color : " + Convert.ToString(parkResponse.Color) +
                              "\n Parking Slot : " + Convert.ToString(parkResponse.ParkingSlot) +
                              "\n Parking Date : " + Convert.ToString(parkResponse.ParkingDate) +
                              "\n Handicapped : " + Convert.ToString(parkResponse.IsHandicap) + "\n\n\n";

                                string status = "Unparked";
                                senderObject.Send(message, status);

                 bool Success = true;
                var Message = "Vehical UnParked";
                return Ok(new { Success, Message, Data = parkResponse });
            }
            else if (parkResponse == null)
            {
                bool Success = false;
                var Message = "Vehical Is Already UnParked";
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


        //[Authorize(Roles = " Admin, Driver ,Owner")]
        [HttpDelete("UnPark/{ParkingId}")]
        public IActionResult DeleteVehicalById([FromRoute] int ParkingId)
        {
            try
            {

                var parkResponse = parkingLotBL.DeleteVehicalById(ParkingId);

                if (parkResponse != null)
                {
                    bool Success = true;
                    var Message = "Vehical UnParked Using By Id";
                    return Ok(new { Success, Message, Data = parkResponse });
                }
                else if (parkResponse == null)
                {
                    bool Success = false;
                    var Message = "Vehical Is Already UnParked";
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

        [Authorize(Roles = "Admin, Driver ,Owner")]
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

        [Authorize(Roles = "Admin, Police, Security ,Owner")]
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

        [Authorize(Roles = "Admin, Police, Security ,Owner")]
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
        [Route("Vehicals")]
        public IActionResult CheckAllVehical()
        {
            try
            {
                var Response = this.parkingLotBL.CheckAllVehical();

                if (Response != null)
                {
                    bool Success = true;
                    var Message = "Vehical Found";
                    return Ok(new { Success , Message , Data = Response });
                }
                else
                {
                    bool Success = false;
                    var Message = "Vehical Not Found";
                    return NotFound(new { Success , Message });
                }
            }
            catch (Exception exception)
            {
                bool Success = false;
                return BadRequest(new { Success , Message = exception.Message });
            }
        }


        /// <summary>
        /// Function For Checking Parking Lot Status.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Owner, Security")]
        [HttpGet]
        [Route("Vehicals/Park")]
        public IActionResult CheckParkVehical()
        {
            try
            {
                    var Response = this.parkingLotBL.CheckParkVehical();

                if (Response != null)
                {
                        bool Success = true;
                        var Message = "Park Vehical Is Available";  
                        return Ok(new { Success, Message , Data = Response });
                }
                else
                {
                        bool Success = false;
                        var Message = "Park Not Vehical Is Full";
                        return NotFound(new { Success , Message  });
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
        [Route("Vehicals/UnPark")]
        public IActionResult CheckUnParkVehical()
        {
            try
            {
                    var Response = this.parkingLotBL.CheckUnParkVehical();

                if (Response != null)
                {
                        bool Success = true;
                        var Message = "UnPark Vehical Is Available";
                        return Ok(new { Success, Message , Data = Response });
                }
                else
                {
                        bool Success = false;
                        var Message = "UnPark Not Vehical Is Full";
                        return NotFound(new { Success , Message  });
                }
            }
            catch (Exception exception)
            {
                    return BadRequest(new { Success = false, Message = exception.Message });
            }
        }
    }
}
