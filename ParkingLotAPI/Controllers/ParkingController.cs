using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
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

        [Authorize(Roles = "Admin, Driver, Attendant ")]
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
        [Route("Park/IsSlotAvailble")]
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
        [Route("Vehicals/{VehicalNumber}")]
        public IActionResult GetVehicalByNumber([FromRoute] string VehicalNumber)
        {
            try
            {

                var parkResponse = parkingLotBL.GetVehicalByNumber(VehicalNumber);

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

        [Authorize(Roles = "Admin, Police, Security, Owner")]
        [HttpGet]
        [Route("Vehical/{Color}")]
        public IActionResult GetVehicalDetailsByColor([FromRoute] string Color)
        {
            try
            {

                var parkResponse = parkingLotBL.GetVehicalDetailsByColor(Color);

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

        [Authorize(Roles = "Admin, Police, Security, Owner ")]
        [HttpGet("{Brand}")]
        public IActionResult GetVehicalDetailsByBrand([FromRoute] string Brand)
        {
            try
            {

                var parkResponse = parkingLotBL.GetVehicalDetailsByBrand(Brand);

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

        [Authorize(Roles = "Admin, Police, Security ")]
        [HttpGet("{Brand}/{Color}")]
        public IActionResult GetVehicalDetailsByBrandAndColor([FromRoute] string Brand , [FromRoute] string Color)
        {
            try
            {

                var parkResponse = parkingLotBL.GetVehicalDetailsByBrandAndColor(Brand, Color);

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

        [Authorize(Roles = "Admin, Police, Security ")]
        [HttpGet]
        [Route("vehicle/{ParkingSlot}")]
        public IActionResult GetVehicalDetailsByParkingSlot([FromRoute] string ParkingSlot)
        {
            try
            {

                var parkResponse = parkingLotBL.GetVehicalDetailsByParkingSlot(ParkingSlot);

                if (parkResponse != null)
                {
                    bool Success = true;
                    var Message = "In a Slot Vahical Found ";
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

    }
}
