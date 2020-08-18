//-------------------------------------------------------------------------
// <copyright file="ParkingTestCase.cs" company="BridgeLab">
//      Copyright (c) Company. All rights reserved.
// </copyright>
// <author>Vishal Waman</author>
//-------------------------------------------------------------------------

namespace XUnitTestProject
{
    using BusinessLayer.Interface;
    using BusinessLayer.Service;
    using CommonLayer.RequestModel;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Distributed;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using ParkingLotApi.Controllers;
    using RepositoryLayer.Interface;
    using RepositoryLayer.Service;
    using Xunit;
    public class ParkingTestCase
    {
        /// <summary>
        /// Declare Parking Inteface BL Variable
        /// </summary>
        private readonly IParkingBL parkingLotBL;

        /// <summary>
        /// Declare Parking Interface RL Variable
        /// </summary>
        private readonly IParkingRL parkingLotRL;

        /// <summary>
        /// Declare Parking Controller Variable
        /// </summary>
        ParkingController parkingController;

        private readonly IDistributedCache distributedCache;

        /// <summary>
        /// Declare Constructor
        /// </summary>
        public ParkingTestCase()
        {

            parkingLotRL = new ParkingRL();
            parkingLotBL = new ParkingBL(parkingLotRL);
            parkingController = new ParkingController(parkingLotBL, distributedCache);

        }

        /// <summary>
        /// Declare Parking model instance
        /// </summary>
        ParkingModel park = new ParkingModel();

        private const bool SuccessFalse = false;
        private const bool SuccessTrue = true;
        private const string Message_NullException = "NULL_EXCEPTION";
        private const string Message_EmptyException = "EMPTY_EXCEPTION";

        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenAllNullFields_ShouldReturnBadRequestObjectResult()
        {
            //Setting Values.
            park.Brand = null;
            park.Color = null;
            park.DriverName = null;
            park.IsHandicap = null;
            park.VehicalNumber = null;
            park.VehicalOwnerEmail = null;
            park.VehicalOwnerName = null;
            

            //Act
            var response = parkingController.Park(park) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);
        }


        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenBrandNullField_ShouldReturnBadRequestObjectResult()
        {
            //Setting Values.
            park.Brand = null;
            park.Color = "black";
            park.DriverName = "Rahul";
            park.IsHandicap = "Yes";
            park.VehicalNumber = "MH 12 HK 6584";
            park.VehicalOwnerEmail = "vishal@gmail.com";
            park.VehicalOwnerName = "Rahul";


            //Act
            var response = parkingController.Park(park) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);
        }


        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenAllEmptyField_ShouldReturnBadRequestObjectResult()
        {
            //Setting Values.
            park.Brand = "";
            park.Color = "";
            park.DriverName = "";
            park.IsHandicap = "";
            park.VehicalNumber = "";
            park.VehicalOwnerEmail = "";
            park.VehicalOwnerName = "";

            //Act
            var response = parkingController.Park(park) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_EmptyException, responseMessage);
        }

        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenCheckLotField_ShouldReturnOkObjectResultt()
        {
            //Act
            var response = parkingController.CheckLotStatus() as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "Lot Is Available";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenPassVehicleField_ShouldReturnOkObjectResult()
        {
            string VehicleNumber = "MH 12 HK 6375";

            //Act
            var response = parkingController.GetVehicalByNumber(VehicleNumber) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "Vehical Available";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenPassWrongVehicleNumberField_ShouldReturnNotFoundObjectResult()
        {
            string VehicleNumber = "MH 12 HK 6000";

            //Act
            var response = parkingController.GetVehicalByNumber(VehicleNumber) as NotFoundObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "Vehical Not Available";

            //Asserting Values.
            Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }


        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenPassIncurrectVehicleNumberField_ShouldReturnBadRequestObjectResult()
        {
            string VehicleNumber = "MH 12 H 600";

            //Act
            var response = parkingController.GetVehicalByNumber(VehicleNumber) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
          
        }

        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenPassVehicleColorField_ShouldReturnOkObjectResult()
        {
            string VehicleNumber = "Black";

            //Act
            var response = parkingController.GetVehicalDetailsByColor(VehicleNumber) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "Vehical Color Available";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenVehicleColorField_ShouldReturnNotFoundObjectResult()
        {
            string VehicleNumber = "Orange";

            //Act
            var response = parkingController.GetVehicalDetailsByColor(VehicleNumber) as NotFoundObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "Vehical Color Not Available";

            //Asserting Values.
            Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }


        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenVehicleColorInCurrectField_ShouldReturnBadRequestObjectResult()
        {
            string VehicleNumber = "black";

            //Act
            var response = parkingController.GetVehicalDetailsByColor(VehicleNumber) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);

        }

        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenPassVehicleBrandField_ShouldReturnOkObjectResult()
        {
            string VehicleNumber = "Bajaj";

            //Act
            var response = parkingController.GetVehicalDetailsByBrand(VehicleNumber) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "Vehical Brand Available";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenVehicleIncurrectBrandField_ShouldReturnNotFoundObjectResult()
        {
            string VehicleNumber = "Mercedies";

            //Act
            var response = parkingController.GetVehicalDetailsByBrand(VehicleNumber) as NotFoundObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "Vehical Brand Not Available";

            //Asserting Values.
            Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }


        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenVehicleBrandInCurrectField_ShouldReturnBadRequestObjectResult()
        {
            string VehicleNumber = "tata";

            //Act
            var response = parkingController.GetVehicalDetailsByBrand(VehicleNumber) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);

        }


        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenPassVehicleBrandAndColorField_ShouldReturnOkObjectResult()
        {
            string Brand = "Bajaj";
            string Color = "Black";

            //Act
            var response = parkingController.GetVehicalDetailsByBrandAndColor(Brand, Color) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "Vehical Brand And Color Available";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenVehicleIncurrectBrandAndColorField_ShouldReturnNotFoundObjectResult()
        {
            string Brand = "Mercedies";
            string Color = "Black";

            //Act
            var response = parkingController.GetVehicalDetailsByBrandAndColor(Brand, Color) as NotFoundObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "Vehical Brand And Color Not Available";

            //Asserting Values.
            Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }


        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenVehicleBrandAndColorInCurrectField_ShouldReturnBadRequestObjectResult()
        {
            string Brand = "tata";
            string Color = "black";

            //Act
            var response = parkingController.GetVehicalDetailsByBrandAndColor(Brand, Color) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);

        }

        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenPassVehicleParkingSlotField_ShouldReturnOkObjectResult()
        {
            string ParkingSlot = "A";

            //Act
            var response = parkingController.GetVehicalDetailsByParkingSlot(ParkingSlot) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = " Vahicals Found In a Slot ";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenVehicleIncurrectParkingSlotField_ShouldReturnNotFoundObjectResult()
        {
            string ParkingSlot = "D";

            //Act
            var response = parkingController.GetVehicalDetailsByParkingSlot(ParkingSlot) as NotFoundObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = " Vahicals Not Found In a Slot ";

            //Asserting Values.
            Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }


        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenVehicleParkingSlotInCurrectField_ShouldReturnBadRequestObjectResult()
        {
            string ParkingSlot = "m";

            //Act
            var response = parkingController.GetVehicalDetailsByParkingSlot(ParkingSlot) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);

        }

        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenAllParkingVehicleField_ShouldReturnBadRequestObjectResult()
        {
            
            //Act
            var response = parkingController.CheckAllVehical() as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "Vehical Found";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);

        }

        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenAllParkVehicleField_ShouldReturnBadRequestObjectResult()
        {

            //Act
            var response = parkingController.CheckParkVehical() as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "Park Vehical Is Available";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);

        }

        /// <summary>
        /// Test Case For Parking API Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenAllUnParkVehicleField_ShouldReturnBadRequestObjectResult()
        {

            //Act
            var response = parkingController.CheckUnParkVehical() as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "UnPark Vehical Is Available";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);

        }

    }
}
