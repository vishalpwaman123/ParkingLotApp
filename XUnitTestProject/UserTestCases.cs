using BusinessLayer.Interface;
using BusinessLayer.Service;
using CommanLayer.RequestModel;
using CommonLayer.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ParkingLotApi.Controllers;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using RepositoryLayer.Services;
using System;
using System.Data.SqlClient;
using System.Security.Claims;
using Xunit;

namespace XUnitTestProject
{
    public class UserTestCases
    {

        //Controller Reference.
        UserController userController;
        

        //Reference BL and RL.
        private readonly IUserBL userBL;
        private readonly IUserRL userRL;
        


        private readonly IConfiguration configuration;

        private static readonly string ConnectionDeclaration = "Server=.; Database=ParkingLotDatabase; Trusted_Connection=true;MultipleActiveResultSets=True";
        SqlConnection sqlConnectionVariable = new SqlConnection(ConnectionDeclaration);

        public UserTestCases()
        {
            userRL = new UserRL();
            userBL = new UserBL(userRL);
            userController = new UserController(userBL, configuration);
        }

        //Constants.
        private const bool SuccessFalse = false;
        private const bool SuccessTrue = true;
        private const string Message_NullException = "NULL_EXCEPTION";
        private const string Message_EmptyException = "EMPTY_EXCEPTION";

        RegistrationUserModel user = new RegistrationUserModel();
        UserLoginModel User = new UserLoginModel();

        [Fact]
        public void GivenTestCase_WhenAllNullFields_ShouldReturnBadRequestObjectResult()
        {

            user.FirstName = null;
            user.LastName = null;
            user.Role = null;
            user.Password = null;
            user.EmailId = null;
            user.Gender = null;
            user.LocalAddress = null;
            user.MobileNumber = null;
            
            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);


        }

        [Fact]
        public void GivenTestCase_WhenFirstNameNullFields_ShouldReturnBadRequestObjectResult()
        {

            user.FirstName = null;
            user.LastName = "Waman";
            user.Role = "Admin";
            user.Password = "vishal";
            user.EmailId = "vishal@gmail.com";
            user.Gender = "Male";
            user.LocalAddress = "Kondhwa";
            user.MobileNumber = "7758039722";

            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);
        }

        [Fact]
        public void GivenTestCase_WhenLastNameNullFields_ShouldReturnBadRequestObjectResult()
        {

            user.FirstName = "Vishal";
            user.LastName = null;
            user.Role = "Admin";
            user.Password = "vishal";
            user.EmailId = "vishal@gmail.com";
            user.Gender = "Male";
            user.LocalAddress = "Kondhwa";
            user.MobileNumber = "7758039722";

            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);

        }

        [Fact]
        public void GivenTestCase_WhenRoleNullFields_ShouldReturnBadRequestObjectResult()
        {

            user.FirstName = "Vishal";
            user.LastName = "Waman";
            user.Role = null;
            user.Password = "vishal";
            user.EmailId = "vishal@gmail.com";
            user.Gender = "Male";
            user.LocalAddress = "Kondhwa";
            user.MobileNumber = "7758039722";

            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);

        }

        [Fact]
        public void GivenTestCase_WhenPasswordNullFields_ShouldReturnBadRequestObjectResult()
        {

            user.FirstName = "Vishal";
            user.LastName = "Waman";
            user.Role = "Admin";
            user.Password = null;
            user.EmailId = "vishal@gmail.com";
            user.Gender = "Male";
            user.LocalAddress = "Kondhwa";
            user.MobileNumber = "7758039722";

            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);

        }

        [Fact]
        public void GivenTestCase_WhenEmailIdNullFields_ShouldReturnBadRequestObjectResult()
        {

            user.FirstName = "Vishal";
            user.LastName = "Waman";
            user.Role = "Admin";
            user.Password = "vishal";
            user.EmailId = null;
            user.Gender = "Male";
            user.LocalAddress = "Kondhwa";
            user.MobileNumber = "7758039722";

            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);

        }

        [Fact]
        public void GivenTestCase_WhenGenderNullFields_ShouldReturnBadRequestObjectResult()
        {

            user.FirstName = "Vishal";
            user.LastName = "Waman";
            user.Role = "Admin";
            user.Password = "vishal";
            user.EmailId = "vishal@gmail.com";
            user.Gender = null;
            user.LocalAddress = "Kondhwa";
            user.MobileNumber = "7758039722";

            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);

        }

        [Fact]
        public void GivenTestCase_WhenLocalAddressNullFields_ShouldReturnBadRequestObjectResult()
        {

            user.FirstName = "Vishal";
            user.LastName = "Waman";
            user.Role = "Admin";
            user.Password = "vishal";
            user.EmailId = "vishal@gmail.com";
            user.Gender = "Male";
            user.LocalAddress = null;
            user.MobileNumber = "7758039722";

            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);

        }

        [Fact]
        public void GivenTestCase_WhenMobileNullFields_ShouldReturnBadRequestObjectResult()
        {

            user.FirstName = "Vishal";
            user.LastName = "Waman";
            user.Role = "Admin";
            user.Password = "vishal";
            user.EmailId = "vishal@gmail.com";
            user.Gender = "Male";
            user.LocalAddress = "Kondhwa";
            user.MobileNumber = null;

            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);

        }


        [Fact]
        public void GivenTestCase_WhenAllEmptyFields_ShouldReturnBadRequestObjectResult()
        {

            user.FirstName = "";
            user.LastName = "";
            user.Role = "";
            user.Password = "";
            user.EmailId = "";
            user.Gender = "";
            user.LocalAddress = "";
            user.MobileNumber = "";

            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_EmptyException, responseMessage);


        }

        [Fact]
        public void GivenTestCase_WhenFirstNameEmptyFields_ShouldReturnBadRequestObjectResult()
        {

            user.FirstName = "";
            user.LastName = "Waman";
            user.Role = "Admin";
            user.Password = "vishal";
            user.EmailId = "vishal@gmail.com";
            user.Gender = "Male";
            user.LocalAddress = "Kondhwa";
            user.MobileNumber = "7758039722";

            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_EmptyException, responseMessage);
        }

        [Fact]
        public void GivenTestCase_WhenLastNameEmptyFields_ShouldReturnBadRequestObjectResult()
        {

            user.FirstName = "Vishal";
            user.LastName = "";
            user.Role = "Admin";
            user.Password = "vishal";
            user.EmailId = "vishal@gmail.com";
            user.Gender = "Male";
            user.LocalAddress = "Kondhwa";
            user.MobileNumber = "7758039722";

            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_EmptyException, responseMessage);

        }

        [Fact]
        public void GivenTestCase_WhenRoleEmptyFields_ShouldReturnBadRequestObjectResult()
        {

            user.FirstName = "Vishal";
            user.LastName = "Waman";
            user.Role = "";
            user.Password = "vishal";
            user.EmailId = "vishal@gmail.com";
            user.Gender = "Male";
            user.LocalAddress = "Kondhwa";
            user.MobileNumber = "7758039722";

            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_EmptyException, responseMessage);

        }

        [Fact]
        public void GivenTestCase_WhenPasswordEmptyFields_ShouldReturnBadRequestObjectResult()
        {

            user.FirstName = "Vishal";
            user.LastName = "Waman";
            user.Role = "Admin";
            user.Password = "";
            user.EmailId = "vishal@gmail.com";
            user.Gender = "Male";
            user.LocalAddress = "Kondhwa";
            user.MobileNumber = "7758039722";

            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_EmptyException, responseMessage);

        }

        [Fact]
        public void GivenTestCase_WhenEmailIdEmptyFields_ShouldReturnBadRequestObjectResult()
        {

            user.FirstName = "Vishal";
            user.LastName = "Waman";
            user.Role = "Admin";
            user.Password = "vishal";
            user.EmailId = "";
            user.Gender = "Male";
            user.LocalAddress = "Kondhwa";
            user.MobileNumber = "7758039722";

            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_EmptyException, responseMessage);

        }

        [Fact]
        public void GivenTestCase_WhenGenderEmptyFields_ShouldReturnBadRequestObjectResult()
        {

            user.FirstName = "Vishal";
            user.LastName = "Waman";
            user.Role = "Admin";
            user.Password = "vishal";
            user.EmailId = "vishal@gmail.com";
            user.Gender = "";
            user.LocalAddress = "Kondhwa";
            user.MobileNumber = "7758039722";

            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_EmptyException, responseMessage);

        }

        [Fact]
        public void GivenTestCase_WhenLocalAddressEmptyFields_ShouldReturnBadRequestObjectResult()
        {

            user.FirstName = "Vishal";
            user.LastName = "Waman";
            user.Role = "Admin";
            user.Password = "vishal";
            user.EmailId = "vishal@gmail.com";
            user.Gender = "Male";
            user.LocalAddress = "";
            user.MobileNumber = "7758039722";

            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_EmptyException, responseMessage);

        }

        [Fact]
        public void GivenTestCase_WhenMobileEmptyFields_ShouldReturnBadRequestObjectResult()
        {

            user.FirstName = "Vishal";
            user.LastName = "Waman";
            user.Role = "Admin";
            user.Password = "vishal";
            user.EmailId = "vishal@gmail.com";
            user.Gender = "Male";
            user.LocalAddress = "Kondhwa";
            user.MobileNumber = "";

            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_EmptyException, responseMessage);

        }

        /// <summary>
        /// Test Case For Register User Invalid Role Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenUserRoleInvalidFields_ShouldReturnBadRequest()
        {
            user.FirstName = "Vishal";
            user.LastName = "Waman";
            user.Password = "vishal";
            user.EmailId = "vishal@gmail.com";
            user.Gender = "Male";
            user.LocalAddress = "Kondhwa";
            user.MobileNumber = "9881563158";
            user.Role = "Tester";
            var response = userController.RegisterUser(user) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "INVALID_ROLE_EXCEPTION";

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Register User If User Exists Should Return Conflict.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenUserExistsFields_ShouldReturnConflict()
        {
            //Setting Values.
            user.FirstName = "Vishal";
            user.LastName = "Waman";
            user.Password = "vishal";
            user.EmailId = "vishal@gmail.com";
            user.Gender = "Male";
            user.LocalAddress = "Kondhwa";
            user.MobileNumber = "9881563158";
            user.Role = "Owner";
            var response = userController.RegisterUser(user) as ConflictObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "User Already Exists";

            //Asserting Values.
            Assert.IsType<ConflictObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Register User Valid Data Should Return Ok.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenUserValidFields_ShouldReturnOk()
        {
            //Values.
            user.FirstName = "Rahul";
            user.LastName = "Waman";
            user.Password = "rahul";
            user.EmailId = "rahul@gmail.com";
            user.Gender = "Male";
            user.LocalAddress = "Kondhwa";
            user.MobileNumber = "9881563158";
            user.Role = "Police";
            var response = userController.RegisterUser(user) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();
           

            //Expected Values.
            string Message = "Registration Successfull";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }


        /// <summary>
        /// Test Case For Login User Null Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenUserLoginNullFieldsFields_ShouldReturnBadRequest()
        {
            //Setting Values.
            User.EmailId = null;
            User.Password = null;
            var response = userController.UserLogin(User) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_NullException, responseMessage);
        }

        /// <summary>
        /// Test Case For Login User Empty Fields Should Return BadRequest.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenUserLoginEmptyFields_ShouldReturnBadRequest()
        {
            //Setting Values.
            User.EmailId = "";
            User.Password = "";
            var response = userController.UserLogin(User) as BadRequestObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Asserting Values.
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message_EmptyException, responseMessage);
        }

        /// <summary>
        /// Test Case For Login User Invalid Credentials Should Return NotFound.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenUserLoginInvalidUserFields_ShouldReturnNotFound()
        {
            //Setting Values.
            User.EmailId = "vishal@gmail.com";
            User.Password = "Vishal@124";
            var response = userController.UserLogin(User) as NotFoundObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "Login Failed";

            //Asserting Values.
            Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(SuccessFalse, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Login User Invalid Credentials Should Return NotFound.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenGetAllFields_ShouldReturnOk()
        {
            //Setting Values.
            //User.EmailId = "vishal@gmail.com";
            //User.Password = "Vishal@124";

            /*Claim Name = new Claim(ClaimTypes.Email, "vishal@gmail.com");
            ClaimsIdentity identity = new ClaimsIdentity();
            identity.AddClaim(Name);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(identity);
            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(x => x.User).Returns(claimsPrincipal);
            parkingController.ControllerContext.HttpContext = contextMock.Object;
*/

            var response = userController.GetAllEmployee() as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "User Data Fetch Sucessfully";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

        /// <summary>
        /// Test Case For Login User Invalid Credentials Should Return NotFound.
        /// </summary>
        [Fact]
        public void GivenTestCase_WhenGetSpecificFields_ShouldReturnOk()
        {
            //Setting Values.
            //User.EmailId = "vishal@gmail.com";
            //User.Password = "Vishal@124";

            /*Claim Name = new Claim(ClaimTypes.Email, "vishal@gmail.com");
            ClaimsIdentity identity = new ClaimsIdentity();
            identity.AddClaim(Name);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(identity);
            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(x => x.User).Returns(claimsPrincipal);
            parkingController.ControllerContext.HttpContext = contextMock.Object;
*/          int UserId = 1003;

            var response = userController.GetUserDetail(UserId) as OkObjectResult;
            var dataResponse = JToken.Parse(JsonConvert.SerializeObject(response.Value));
            var responseSuccess = dataResponse["Success"].ToObject<bool>();
            var responseMessage = dataResponse["Message"].ToString();

            //Expected Values.
            string Message = "User Data Found";

            //Asserting Values.
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(SuccessTrue, responseSuccess);
            Assert.Equal(Message, responseMessage);
        }

    }
}
