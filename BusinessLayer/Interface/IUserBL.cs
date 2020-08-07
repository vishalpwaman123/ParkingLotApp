using System;
using System.Collections.Generic;
using System.Text;
using CommanLayer.RequestModel;
using CommanLayer.ResponseModel;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {

        /// <summary>
        /// Abstact Function For Register User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        RUserModel RegisterUser(UserModel user);

    }
}
