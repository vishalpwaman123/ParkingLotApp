using System;
using System.Collections.Generic;
using BusinessLayer.Interface;
using CommanLayer.Enum;
using CommanLayer.Exceptions;
using CommanLayer.RequestModel;
using CommanLayer.ResponseModel;
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


        /// <summary>
        /// Function For Resgister User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Registration")]
        public IActionResult RegisterUser([FromBody] UserModel user)
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



    }
}
    