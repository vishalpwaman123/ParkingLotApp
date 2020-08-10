using System;
using System.Collections.Generic;
using BusinessLayer.Interface;
using CommanLayer.Enum;
using CommanLayer.Exceptions;
using CommanLayer.RequestModel;
using CommanLayer.ResponseModel;
using CommonLayer.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interface;
using RepositoryLayer.Services;

namespace ParkingLotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        //UserBl Reference.
        private IUserBL userBL;

        //IConfiguration Reference for JWT.
        private IConfiguration configuration;


        /// <summary>
        /// Constructor for UserBL Reference.
        /// </summary>
        /// <param name="userBL"></param>
        public UserController(IUserBL userBL, IConfiguration configuration)
        {
            this.userBL = userBL;
            this.configuration = configuration;
        }




        /// <summary>
        /// Function For Resgister User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Registration")]
        public IActionResult RegisterUser([FromBody] RegistrationUserModel user)
        {
            try
            {
                RUserModel responseUser = null;

                if (!user.Equals(null))
                {
                    responseUser = userBL.RegisterUser(user);
                }
                else
                {
                    throw new Exception(UserExceptions.ExceptionType.INVALID_ROLE_EXCEPTION.ToString());
                }

                if (responseUser != null)
                {
                    bool Success = true;
                    var Message = "Registration Successfull";
                    return Ok(new { Success , Message , Data = responseUser });
                }
                else
                {
                    bool Success = false;
                    var Message = "User Already Exists";
                    return Conflict(new { Success , Message });
                }
            }
            catch (Exception exception)
            {
                bool Success = false;
                return BadRequest(new { Success , Message = exception.Message });
            }
        }

        /// <summary>
        /// Function For User Login.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult UserLogin([FromBody] UserLoginModel user)
        {
            try
            {
                RUserModel responseUser = null;

                if (!user.Equals(null))
                {
                    responseUser = userBL.Userlogin(user);
                }
                else
                {
                    throw new Exception(UserExceptions.ExceptionType.INVALID_ROLE_EXCEPTION.ToString());
                }

                if (responseUser != null)
                {
                    bool Success = true;
                    var Message = "Login Successfull";
                    return Ok(new { Success, Message, Data = responseUser });
                }
                else
                {
                    bool Success = false;
                    var Message = "Login Failed";
                    return Conflict(new { Success, Message });
                }
            }
            catch (Exception exception)
            {
                bool Success = false;
                return BadRequest(new { Success, Message = exception.Message });
            }
        }


        /// <summary>
        /// declaration of get all employee method
        /// </summary>
        /// <returns>return action result</returns>
        [HttpGet]
        //[Authorize]
        [Route("User")]
        public IActionResult GetAllEmployee()
        {
            try
            {
                List<RUserModel> employees = null;        
                employees = this.userBL.GetAllUser();
                                    
                if (employees != null)
                {
                    bool Success = true;
                    var Message = " User Data Fetch Sucessfully ";
                    return this.Ok(new { Success, Message, data = employees });
                }
                else
                {
                    bool Success = false;
                    var Message = " Failed Fetch User Data ";
                    return this.BadRequest(new { Success, Message });
                }
            }
            catch (Exception e)
            {
                bool Success = false;
                return this.BadRequest(new { Success, message = e.Message });
            }
        }


        /// <summary>
        /// Function For Resgister User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut]
        [Route("UserId")]
        public IActionResult UpdateUserData([FromBody] UUserModel user)
        {
            try
            {
                UUserModel responseUser = null;

                if (!user.Equals(null))
                {
                    responseUser = userBL.UpdateUserData(user);
                }
                else
                {
                    throw new Exception(UserExceptions.ExceptionType.INVALID_ROLE_EXCEPTION.ToString());
                }

                if (responseUser != null)
                {
                    bool Success = true;
                    var Message = "User Updation is Successfull";
                    return Ok(new { Success, Message, Data = responseUser });
                }
                else
                {
                    bool Success = false;
                    var Message = "User Updation Failed";
                    return Conflict(new { Success, Message });
                }
            }
            catch (Exception exception)
            {
                bool Success = false;
                return BadRequest(new { Success, Message = exception.Message });
            }
        }

        [HttpGet("{UserId}")]
        public ActionResult GetUserDetail([FromRoute] int UserId)
        {
            try
            {
                RUserModel responseUser = null;

                if (!UserId.Equals(null))
                {
                    responseUser = userBL.GetUserDetail(UserId);
                }
                else
                {
                    throw new Exception(UserExceptions.ExceptionType.INVALID_ROLE_EXCEPTION.ToString());
                }

                if (responseUser != null)
                {
                    bool Success = true;
                    var Message = "User Data Found";
                    return Ok(new { Success, Message, Data = responseUser });
                }
                else
                {
                    bool Success = false;
                    var Message = "User Data Not Found";
                    return Conflict(new { Success, Message });
                }
            }
            catch (Exception exception)
            {
                bool Success = false;
                return BadRequest(new { Success, Message = exception.Message });
            }
        }


        /// <summary>
        /// Function For Resgister User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpDelete("{UserId}")]
        
        public IActionResult DeleteUserData([FromRoute] int UserId)
        {
            try
            {
                RUserModel responseUser = null;

                if (UserId > 0 )
                {
                    responseUser = userBL.DeleteUserData(UserId);
                }
                else
                {
                    throw new Exception(UserExceptions.ExceptionType.INVALID_ROLE_EXCEPTION.ToString());
                }

                if (responseUser != null)
                {
                    bool Success = true;
                    var Message = "User Deletion is Successfull";
                    return Ok(new { Success, Message, Data = responseUser });
                }
                else
                {
                    bool Success = false;
                    var Message = "User Deletion Failed";
                    return Conflict(new { Success, Message });
                }
            }
            catch (Exception exception)
            {
                bool Success = false;
                return BadRequest(new { Success, Message = exception.Message });
            }
        }


    }
}
    