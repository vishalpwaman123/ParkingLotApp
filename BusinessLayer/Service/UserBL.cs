using BusinessLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Interface;
using CommanLayer.ResponseModel;
using CommanLayer.RequestModel;

namespace BusinessLayer.Service
{
     public class UserBL : IUserBL
    {
        /// <summary>
        /// RL Reference.
        /// </summary>
        private IUserRL userRL;

        /// <summary>
        /// Constructor For Setting UserRL Instance.
        /// </summary>
        /// <param name="userRL"></param>
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }

        /// <summary>
        /// Function For Register User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public RUserModel RegisterUser(UserModel user)
        {
            try
            {
                return this.userRL.RegisterUser(user);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

    }
}
