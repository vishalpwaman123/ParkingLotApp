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

        [AllowAnonymous]
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



    }
}
