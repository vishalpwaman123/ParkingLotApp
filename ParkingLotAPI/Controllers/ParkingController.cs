using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using NuGet.Configuration;
using ParkingLotAPI.MSMQSender;
using StackExchange.Redis;

namespace ParkingLotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private IParkingBL parkingLotBL;

        private readonly IDistributedCache distributedCache;

        private  IDatabase _cache;
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
                DeleteCacheData();
                
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
                    string message = "\n Hello " + Convert.ToString(parkResponse.VehicalOwnerName) +
                  "\n Your " + " Vehicle Park Succesfully" +
                  "\n Driver Name :" + Convert.ToString(parkResponse.DriverName) +
                  "\n Email :" + Convert.ToString(parkResponse.VehicalOwnerEmail) +
                  "\n Vehicle Number: " + Convert.ToString(parkResponse.VehicalNumber) +
                  "\n Vehicle Brand :  " + Convert.ToString(parkResponse.Brand) +
                  "\n Vehicle Color : " + Convert.ToString(parkResponse.Color) +
                  "\n Parking Slot : " + Convert.ToString(parkResponse.ParkingSlot) +
                  "\n Parking Date : " + Convert.ToString(parkResponse.ParkingDate) +
                  "\n Handicapped : " + Convert.ToString(parkResponse.IsHandicap) + "\n";

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
        [HttpDelete("{VehicalNumber}")]
        public IActionResult UnPark([FromRoute] string VehicalNumber)
        {
            try
            {

            DeleteCacheData();

            var parkResponse = parkingLotBL.UnPark(VehicalNumber);

            if (parkResponse != null)
            {

                                //Message For MSMQ.
                                string message = "\n Hello " + Convert.ToString(parkResponse.VehicalOwnerName) +
                              "\n Your " + " Vehicle UnPark Succesfully" +
                              "\n Driver Name :" + Convert.ToString(parkResponse.DriverName) +
                              "\n Email :" + Convert.ToString(parkResponse.VehicalOwnerEmail) +
                              "\n Vehicle Number: " + Convert.ToString(parkResponse.VehicalNumber) +
                              "\n vehicle Brand :  " + Convert.ToString(parkResponse.Brand) +
                              "\n Vehicle Color : " + Convert.ToString(parkResponse.Color) +
                              "\n Parking Slot : " + Convert.ToString(parkResponse.ParkingSlot) +
                              "\n Parking Date : " + Convert.ToString(parkResponse.ParkingDate) +
                              "\n Handicapped : " + Convert.ToString(parkResponse.IsHandicap) + "\n";

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
        [HttpDelete("{ParkingId}")]
        public IActionResult DeleteVehicalById([FromRoute] int ParkingId)
        {
            try
            {
                DeleteCacheData();

                ParkDetailedModel employee = null;

                employee = parkingLotBL.DeleteVehicalById(ParkingId);

                if (!employee.Equals(null))
                {

                    //Message For MSMQ.
                    string message = "\n Hello " + Convert.ToString(employee.VehicalOwnerName) +
                  "\n Your " + " Vehicle UnPark Succesfully" +
                  "\n Driver Name :" + Convert.ToString(employee.DriverName) +
                  "\n Email :" + Convert.ToString(employee.VehicalOwnerEmail) +
                  "\n Vehicle Number: " + Convert.ToString(employee.VehicalNumber) +
                  "\n vehicle Brand :  " + Convert.ToString(employee.Brand) +
                  "\n Vehicle Color : " + Convert.ToString(employee.Color) +
                  "\n Parking Slot : " + Convert.ToString(employee.ParkingSlot) +
                  "\n Parking Date : " + Convert.ToString(employee.ParkingDate) +
                  "\n Handicapped : " + Convert.ToString(employee.IsHandicap) + "\n";

                    string status = "Unparked";
                    senderObject.Send(message, status);

                    bool Success = true;
                    var Message = "Vehical UnParked Using By Id";
                    return Ok(new { Success, Message, Data = employee });
                }
                else if (employee == null)
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
        //[Authorize(Roles = "Admin, Owner, Security")]
        [HttpGet]
        [Route("Slot/Availability")]
        public IActionResult CheckLotStatus()
        {
            try
            {
                bool status ;

                string cacheKey = "Status";
                string serializedEmployee;

                var Vehicledetail = distributedCache.Get(cacheKey);

                //If Redis has employee detail then it will fetch from Redis else it will fetch from Database.
                if (Vehicledetail != null)
                {
                    serializedEmployee = Encoding.UTF8.GetString(Vehicledetail);
                    status = JsonConvert.DeserializeObject<bool>(serializedEmployee);
                }
                else
                {
                    status = this.parkingLotBL.CheckLotStatus();
                    serializedEmployee = JsonConvert.SerializeObject(status);
                    Vehicledetail = Encoding.UTF8.GetBytes(serializedEmployee);
                    var options = new DistributedCacheEntryOptions()
                                     .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                                     .SetAbsoluteExpiration(DateTime.Now.AddHours(6));
                    distributedCache.Set(cacheKey, Vehicledetail, options);
                }

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

        //[Authorize(Roles = "Admin, Driver ,Owner")]
        [HttpGet]
        [Route("Vehical/{VehicalNumber}")]
        public IActionResult GetVehicalByNumber([FromRoute] string VehicalNumber)
        {
            try
            {

                int Flag = 0, i , Count = 0 ;
            //Throws Custom Exception If VehicalNumber Is Not in Valid Format.
                if (!Regex.IsMatch(VehicalNumber, @"^[A-Z]{2}\s[0-9]{2}\s[A-Z]{1,2}\s[0-9]{4}$"))
                {
                        throw new Exception(ParkingLotExceptions.ExceptionType.INVALID_VEHICAL_NUMBER.ToString() + " Please Enter Vehical In 'MH 01 AZ 2005' This Format.");
                }

                RParkingModel employee = null;

                string cacheKey = VehicalNumber;
                string serializedEmployee;

                var Vehicledetail = distributedCache.Get(cacheKey);

                //If Redis has employee detail then it will fetch from Redis else it will fetch from Database.
                if (Vehicledetail != null)
                {
                    serializedEmployee = Encoding.UTF8.GetString(Vehicledetail);
                    employee = JsonConvert.DeserializeObject<RParkingModel>(serializedEmployee);
                }
                else
                {
                    employee = parkingLotBL.GetVehicalByNumber(VehicalNumber);
                    serializedEmployee = JsonConvert.SerializeObject(employee);
                    Vehicledetail = Encoding.UTF8.GetBytes(serializedEmployee);
                    var options = new DistributedCacheEntryOptions()
                                     .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                                     .SetAbsoluteExpiration(DateTime.Now.AddHours(6));
                    distributedCache.Set(cacheKey, Vehicledetail, options);
                }

                if (!employee.Equals(null))
                {
                        bool Success = true;
                        var Message = "Vehical Available";
                        return Ok(new { Success, Message, Data = employee });
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

        //[Authorize(Roles = "Admin, Police, Security, Owner")]
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

                List<RParkingModel> employee = null;

                string cacheKey = Color;
                string serializedEmployee;

                var Vehicledetail = distributedCache.Get(cacheKey);

                //If Redis has employee detail then it will fetch from Redis else it will fetch from Database.
                if (Vehicledetail != null)
                {
                    serializedEmployee = Encoding.UTF8.GetString(Vehicledetail);
                    employee = JsonConvert.DeserializeObject<List<RParkingModel>>(serializedEmployee);
                }
                else
                {
                    employee = parkingLotBL.GetVehicalDetailsByColor(Color);
                    serializedEmployee = JsonConvert.SerializeObject(employee);
                    Vehicledetail = Encoding.UTF8.GetBytes(serializedEmployee);
                    var options = new DistributedCacheEntryOptions()
                                     .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                                     .SetAbsoluteExpiration(DateTime.Now.AddHours(6));
                    distributedCache.Set(cacheKey, Vehicledetail, options);
                }

                if (!employee.Equals(null))
                {
                        bool Success = true;
                        var Message = "Vehical Color Available";
                        return Ok(new { Success, Message, Data = employee });
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

        //[Authorize(Roles = "Admin, Police, Security, Owner ")]
        [HttpGet("{Brand}")]
        public IActionResult GetVehicalDetailsByBrand([FromRoute] string Brand)
        {
            try
            {
                int Flag = 0, i;
            //Throws Custom Exception If VehicalNumber Is Not in Valid Format.
                if (!Regex.IsMatch(Brand, @"^[A-Z][a-zA-Z]*$"))
                {
                        throw new Exception(ParkingLotExceptions.ExceptionType.INVALID_COLOR.ToString() + " Please Enter Vehical Color.");
                }

                List<RParkingModel> employee = null;

                string cacheKey = Brand;
                string serializedEmployee;

                var Vehicledetail = distributedCache.Get(cacheKey);

                //If Redis has employee detail then it will fetch from Redis else it will fetch from Database.
                if (Vehicledetail != null)
                {
                    serializedEmployee = Encoding.UTF8.GetString(Vehicledetail);
                    employee = JsonConvert.DeserializeObject<List<RParkingModel>>(serializedEmployee);
                }
                else
                {
                    employee = parkingLotBL.GetVehicalDetailsByBrand(Brand); ;
                    serializedEmployee = JsonConvert.SerializeObject(employee);
                    Vehicledetail = Encoding.UTF8.GetBytes(serializedEmployee);
                    var options = new DistributedCacheEntryOptions()
                                     .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                                     .SetAbsoluteExpiration(DateTime.Now.AddHours(6));
                    distributedCache.Set(cacheKey, Vehicledetail, options);
                }

               
        
                if (!employee.Equals(null))
                {
                        bool Success = true;
                        var Message = "Vehical Brand Available";
                        return Ok(new { Success, Message, Data = employee });
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

        //[Authorize(Roles = "Admin, Police, Security ,Owner")]
        [HttpGet("{Brand}/{Color}")]
        public IActionResult GetVehicalDetailsByBrandAndColor([FromRoute] string Brand , [FromRoute] string Color)
        {
            try
            {

                int Flag = 0 , i ;

            //Throws Custom Exception If VehicalNumber Is Not in Valid Format.
                if (!Regex.IsMatch(Brand, @"^[A-Z][a-zA-Z]*$"))
                {
                        throw new Exception(ParkingLotExceptions.ExceptionType.INVALID_COLOR.ToString() + " Please Enter Valied Vehical Color.");
                }

                List<RParkingModel> employee = null;

                string cacheKey = Brand+Color;
                string serializedEmployee;

                var Vehicledetail = distributedCache.Get(cacheKey);

                //If Redis has employee detail then it will fetch from Redis else it will fetch from Database.
                if (Vehicledetail != null)
                {
                    serializedEmployee = Encoding.UTF8.GetString(Vehicledetail);
                    employee = JsonConvert.DeserializeObject<List<RParkingModel>>(serializedEmployee);
                }
                else
                {
                    employee = parkingLotBL.GetVehicalDetailsByBrandAndColor(Brand, Color); 
                    serializedEmployee = JsonConvert.SerializeObject(employee);
                    Vehicledetail = Encoding.UTF8.GetBytes(serializedEmployee);
                    var options = new DistributedCacheEntryOptions()
                                     .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                                     .SetAbsoluteExpiration(DateTime.Now.AddHours(6));
                    distributedCache.Set(cacheKey, Vehicledetail, options);
                }

                if (!employee.Equals(null))
                {
                        bool Success = true;
                        var Message = "Vehical Brand And Color Available";
                        return Ok(new { Success, Message, Data = employee });
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

        //[Authorize(Roles = "Admin, Police, Security ,Owner")]
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

                string cacheKey;

                if (ParkingSlot == "A")
                {
                    cacheKey = ParkingSlot;
                }
                else if (ParkingSlot == "B")
                {
                   cacheKey = ParkingSlot;
                }
                else if (ParkingSlot == "C")
                {
                    cacheKey = ParkingSlot;
                }
                else
                {
                    cacheKey = ParkingSlot;
                }

                List<RParkingModel> employee = null;

                
                string serializedEmployee;

                var Vehicledetail = distributedCache.Get(cacheKey);

                //If Redis has employee detail then it will fetch from Redis else it will fetch from Database.
                if (Vehicledetail != null)
                {
                    serializedEmployee = Encoding.UTF8.GetString(Vehicledetail);
                    employee = JsonConvert.DeserializeObject<List<RParkingModel>>(serializedEmployee);
                }
                else
                {
                    employee = parkingLotBL.GetVehicalDetailsByParkingSlot(ParkingSlot);
                    serializedEmployee = JsonConvert.SerializeObject(employee);
                    Vehicledetail = Encoding.UTF8.GetBytes(serializedEmployee);
                    var options = new DistributedCacheEntryOptions()
                                     .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                                     .SetAbsoluteExpiration(DateTime.Now.AddHours(6));
                    distributedCache.Set(cacheKey, Vehicledetail, options);
                }

                if (!employee.Equals(null))
                {
                        bool Success = true;
                        var Message = " Vahicals Found In a Slot ";
                        return Ok(new { Success, Message, Data = employee });
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
        //[Authorize(Roles = "Admin, Owner, Security")]
        [HttpGet]
        [Route("Vehicals")]
        public IActionResult CheckAllVehical()
        {
            try
            {
                List<RParkingModel> employee = null;

                string cacheKey = "AllVehical";
                string serializedEmployee;

                var Vehicledetail = distributedCache.Get(cacheKey);

                //If Redis has employee detail then it will fetch from Redis else it will fetch from Database.
                if (Vehicledetail != null)
                {
                    serializedEmployee = Encoding.UTF8.GetString(Vehicledetail);
                    employee = JsonConvert.DeserializeObject<List<RParkingModel>>(serializedEmployee);
                }
                else
                {
                    employee = this.parkingLotBL.CheckAllVehical();
                    serializedEmployee = JsonConvert.SerializeObject(employee);
                    Vehicledetail = Encoding.UTF8.GetBytes(serializedEmployee);
                    var options = new DistributedCacheEntryOptions()
                                     .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                                     .SetAbsoluteExpiration(DateTime.Now.AddHours(6));
                    distributedCache.Set(cacheKey, Vehicledetail, options);
                }

                if (employee != null)
                {
                    bool Success = true;
                    var Message = "Vehical Found";
                    return Ok(new { Success , Message , Data = employee });
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
        //[Authorize(Roles = "Admin, Owner, Security")]
        [HttpGet]
        [Route("Vehicals/Park")]
        public IActionResult CheckParkVehical()
        {
            try
            {

                List<RParkingModel> employee = null;

                string cacheKey = "ParkVehical";
                string serializedEmployee;

                var Vehicledetail = distributedCache.Get(cacheKey);

                //If Redis has employee detail then it will fetch from Redis else it will fetch from Database.
                if (Vehicledetail != null)
                {
                    serializedEmployee = Encoding.UTF8.GetString(Vehicledetail);
                    employee = JsonConvert.DeserializeObject<List<RParkingModel>>(serializedEmployee);
                }
                else
                {
                    employee = this.parkingLotBL.CheckParkVehical();
                    serializedEmployee = JsonConvert.SerializeObject(employee);
                    Vehicledetail = Encoding.UTF8.GetBytes(serializedEmployee);
                    var options = new DistributedCacheEntryOptions()
                                     .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                                     .SetAbsoluteExpiration(DateTime.Now.AddHours(6));
                    distributedCache.Set(cacheKey, Vehicledetail, options);
                }

                if (employee != null)
                {
                        bool Success = true;
                        var Message = "Park Vehical Is Available";  
                        return Ok(new { Success, Message , Data = employee });
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
        //[Authorize(Roles = "Admin, Owner, Security")]
        [HttpGet]
        [Route("Vehicals/UnPark")]
        public IActionResult CheckUnParkVehical()
        {
            try
            {

                List<RParkingModel> employee = null;

                string cacheKey = "UnParkVehical";
                string serializedEmployee;

                var Vehicledetail = distributedCache.Get(cacheKey);

                //If Redis has employee detail then it will fetch from Redis else it will fetch from Database.
                if (Vehicledetail != null)
                {
                    serializedEmployee = Encoding.UTF8.GetString(Vehicledetail);
                    employee = JsonConvert.DeserializeObject<List<RParkingModel>>(serializedEmployee);
                }
                else
                {
                    employee = this.parkingLotBL.CheckUnParkVehical();
                    serializedEmployee = JsonConvert.SerializeObject(employee);
                    Vehicledetail = Encoding.UTF8.GetBytes(serializedEmployee);
                    var options = new DistributedCacheEntryOptions()
                                     .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                                     .SetAbsoluteExpiration(DateTime.Now.AddHours(6));
                    distributedCache.Set(cacheKey, Vehicledetail, options);
                }

                if (employee != null)
                {
                        bool Success = true;
                        var Message = "UnPark Vehical Is Available";
                        return Ok(new { Success, Message , Data = employee });
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

        void DeleteCacheData()
        {
            try
            {
                string cacheKey = "Parking";
                string cacheKey1 = "Status";
                string cacheKey2 = "UnParkVehical";
                string cacheKey3 = "ParkVehical";
                string cacheKey4 = "AllVehical";
                string cacheKey5 = "A";
                string cacheKey6 = "B";
                string cacheKey7 = "C";
                string cacheKey8 = "D";

                distributedCache.Remove(cacheKey);
                distributedCache.Remove(cacheKey1);
                distributedCache.Remove(cacheKey2);
                distributedCache.Remove(cacheKey3);
                distributedCache.Remove(cacheKey4);
                distributedCache.Remove(cacheKey5);
                distributedCache.Remove(cacheKey6);
                distributedCache.Remove(cacheKey7);
                distributedCache.Remove(cacheKey8);

            }
            catch(Exception exception)
            {
                throw exception;
            }
        }
    }
}

/*{
  "vehicalOwnerName": "Mahesh",
  "vehicalOwnerEmail": "mahesh@gmail.com",
  "vehicalNumber": "MH 34 KI 8456",
  "brand": "Honda",
  "color": "Black",
  "driverName": "Mahesh",
  "isHandicap": "No"
}*/