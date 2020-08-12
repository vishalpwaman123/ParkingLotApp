using System;
using System.Collections.Generic;
using System.Text;
using CommanLayer.RequestModel;
using CommanLayer.ResponseModel;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {

        /// <summary>
        /// Abstact Function For Register User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        RUserModel RegisterUser(RegistrationUserModel user);

        RTUserModel Userlogin(UserLoginModel user);

        List<RUserModel> GetAllUser();

        UUserModel UpdateUserData(UUserModel user);

        RUserModel DeleteUserData(int user);

        RUserModel GetUserDetail(int user);
    }
}
