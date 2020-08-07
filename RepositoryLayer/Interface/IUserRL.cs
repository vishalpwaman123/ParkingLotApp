using System;
using System.Collections.Generic;
using System.Text;
using CommanLayer.RequestModel;
using CommanLayer.ResponseModel;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {

        /// <summary>
        /// Abstact Function For Register User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        RUserModel RegisterUser(UserModel user);


    }
}
