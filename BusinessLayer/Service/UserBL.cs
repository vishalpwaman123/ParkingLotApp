using BusinessLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Interface;
using CommanLayer.ResponseModel;
using CommanLayer.RequestModel;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;

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
        public RUserModel RegisterUser(RegistrationUserModel user)
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

        public RTUserModel Userlogin(UserLoginModel user)
        {
            try
            {
                return this.userRL.Userlogin(user);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<RUserModel> GetAllUser()
        {
            try
            {
                return this.userRL.GetAllUser();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public UUserModel UpdateUserData(UUserModel user)
        {
            try
            {
                return this.userRL.UpdateUserData(user);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public RUserModel DeleteUserData(int user)
        {
            try
            {
                return this.userRL.DeleteUserData(user);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public RUserModel GetUserDetail(int user)
        {
            try
            {
                return this.userRL.GetUserDetail(user);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


    }
}
