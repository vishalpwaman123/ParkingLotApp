using System;
using System.Collections.Generic;
using System.Text;
using CommanLayer.RequestModel;
using CommanLayer.ResponseModel;
using CommonLayer.RequestModel;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {

        /// <summary>
        /// Abstact Function For Register User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        RUserModel RegisterUser(RegistrationUserModel user);

        RUserModel Userlogin(UserLoginModel user);

        List<RUserModel> GetAllUser();

        UUserModel UpdateUserData(UUserModel user);

        RUserModel DeleteUserData(int user);

        RUserModel GetUserDetail(int user);
    }
}
