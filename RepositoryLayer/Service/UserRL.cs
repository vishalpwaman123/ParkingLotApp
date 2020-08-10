 using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CommanLayer;
using CommanLayer.Enum;
using CommanLayer.Exceptions;
using CommanLayer.RequestModel;
using CommanLayer.ResponseModel;
using CommonLayer.RequestModel;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Services
{

    public class UserRL : IUserRL
    {
        /// <summary>
        /// Database Connection Variable
        /// </summary>
        /*private SqlConnection sqlConnectionVariable;*/

        /*public UserRL()
        {
            var configuration = this.GetConfiguration();
            this.sqlConnectionVariable = new SqlConnection(configuration.GetSection("Data").GetSection("ConnectionVariable").Value);
        }

        /// <summary>
        /// Define information configuration method
        /// </summary>
        /// <returns>return builder</returns>
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }*/

        private static readonly string ConnectionDeclaration = "Server=.; Database=ParkingLotDatabase; Trusted_Connection=true;MultipleActiveResultSets=True";

        SqlConnection sqlConnectionVariable = new SqlConnection(ConnectionDeclaration);


        /// <summary>
        /// Function For Register User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public RUserModel RegisterUser(RegistrationUserModel user)
        {
            try
            {

                //Throws Custom Exception When Fields are Null.
                if (user.FirstName == null || user.Role == null || user.Password == null || user.EmailId == null || user.LastName == null)
                {
                    throw new Exception(UserExceptions.ExceptionType.NULL_EXCEPTION.ToString());
                }

                //Throws Custom Exception When Fields are Empty Strings.
                if (user.FirstName == "" || user.Role == "" || user.Password == "" || user.EmailId == "" || user.LastName == "")
                {
                    throw new Exception(UserExceptions.ExceptionType.EMPTY_EXCEPTION.ToString());
                }

                if (EmailChecking(user.EmailId) && (user.Role.Equals(Roles.Admin.ToString()) || user.Role.Equals(Roles.Driver.ToString()) ||
                   user.Role.Equals(Roles.Police.ToString()) || user.Role.Equals(Roles.Security.ToString()) ||
                   user.Role.Equals(Roles.Owner.ToString()) || user.Role.Equals(Roles.Attendant.ToString())))
                {

                    int status = 0;
                    RUserModel usermodel = new RUserModel();
                    user.Password = Encrypt(user.Password).ToString();
                    SqlCommand sqlCommand = new SqlCommand("spUserRegistration", this.sqlConnectionVariable);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("@Firstname", user.FirstName);
                    sqlCommand.Parameters.AddWithValue("@Lastname", user.LastName);
                    sqlCommand.Parameters.AddWithValue("@EmailID", user.EmailId);
                    sqlCommand.Parameters.AddWithValue("@Role", user.Role);
                    sqlCommand.Parameters.AddWithValue("@UserPassword", user.Password);
                    sqlCommand.Parameters.AddWithValue("@CurrentAddress", user.LocalAddress);
                    sqlCommand.Parameters.AddWithValue("@MobileNumber", user.MobileNumber);
                    sqlCommand.Parameters.AddWithValue("@Gender", user.Gender);
                    
                    this.sqlConnectionVariable.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                         status = sqlDataReader.GetInt32(0);
                        usermodel.UserId = Convert.ToInt32(sqlDataReader["UserId"]);
                        usermodel.DateAndTime = sqlDataReader["ModificationDate"].ToString();
                        if (status > 0)
                        {
                            return DataCopy(usermodel, user);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                return null;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public RUserModel Userlogin(UserLoginModel user)
        {
            try
            {
                int status = 0;
                int FlagsAttribute = 1;
                if(user.EmailId == null || user.Password == null )
                {
                    throw new Exception(UserExceptions.ExceptionType.NULL_EXCEPTION.ToString());
                }

                if(user.EmailId == "" || user.Password == "" )
                {
                    throw new Exception(UserExceptions.ExceptionType.EMPTY_EXCEPTION.ToString());
                }
                SqlCommand sqlCommand = new SqlCommand("spGetUserLogin", this.sqlConnectionVariable);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@EmailID", user.EmailId);
                user.Password = Encrypt(user.Password).ToString();
                sqlCommand.Parameters.AddWithValue("@UserPassword", user.Password);
                this.sqlConnectionVariable.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                RUserModel usermodel = new RUserModel();
                while (sqlDataReader.Read())
                {
                    status = sqlDataReader.GetInt32(0);
                    if ( status > 0 )
                    {
                        
                            usermodel.UserId = Convert.ToInt32(sqlDataReader["UserId"]);
                            usermodel.FirstName = sqlDataReader["FirstName"].ToString();
                            usermodel.LastName = sqlDataReader["LastName"].ToString();
                            usermodel.EmailId = sqlDataReader["EmailId"].ToString();
                            usermodel.LocalAddress = sqlDataReader["LocalAddress"].ToString();
                            usermodel.MobileNumber = sqlDataReader["MobileNumber"].ToString();
                            usermodel.Gender = sqlDataReader["Gender"].ToString();
                            usermodel.Role = sqlDataReader["Role"].ToString();
                            usermodel.DateAndTime = sqlDataReader["ModificationDate"].ToString();
                            FlagsAttribute = 0;
                            break;
                    }else
                    {
                            break;
                    }
                }

                this.sqlConnectionVariable.Close();
                if (FlagsAttribute == 0)
                {
                    return usermodel;
                }

                usermodel = null;
                return usermodel;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


         private RUserModel DataCopy(RUserModel usermodel, RegistrationUserModel user)
        {
            usermodel.FirstName = user.FirstName;
            usermodel.LastName = user.LastName;
            usermodel.EmailId = user.EmailId;
            usermodel.Gender = user.Gender;
            usermodel.LocalAddress = user.LocalAddress;
            usermodel.MobileNumber = user.MobileNumber;
            usermodel.Role = user.Role;
            return usermodel;
        }

        /// <summary>
        /// Declare email checking method
        /// </summary>
        /// <param name="emailId">Passing email id string</param>
        /// <returns>return boolean value</returns>
        public bool EmailChecking(string emailId)
        {

            SqlCommand sqlCommand = new SqlCommand("spcheckemailId", this.sqlConnectionVariable);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmailId", emailId);
            this.sqlConnectionVariable.Open();
            int status = 1;
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                status = sqlDataReader.GetInt32(0);
                if (status == 1)
                {
                    this.sqlConnectionVariable.Close();
                    return false;
                }
                else
                {
                    this.sqlConnectionVariable.Close();
                    return true;
                }
            }
            this.sqlConnectionVariable.Close();
            return true;
        }


        /// <summary>
        /// Declaration of encrypt method
        /// </summary>
        /// <param name="originalString">Passing password string</param>
        /// <returns>return string</returns>
        public static string Encrypt(string originalString)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes("ZeroCool");
            if (String.IsNullOrEmpty(originalString))
            {
                throw new ArgumentNullException("The string which needs to be encrypted can not be null.");
            }

            var cryptoProvider = new DESCryptoServiceProvider();
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes),
                CryptoStreamMode.Write);
            var writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        /// <summary>
        /// Declaration of decrypt method
        /// </summary>
        /// <param name="encryptedString">passing string</param>
        /// <returns>return string</returns>
        public static string Decrypt(string encryptedString)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes("ZeroCool");
            if (String.IsNullOrEmpty(encryptedString))
            {
                throw new ArgumentNullException("The string which needs to be decrypted can not be null.");
            }

            var cryptoProvider = new DESCryptoServiceProvider();
            var memoryStream = new MemoryStream(Convert.FromBase64String(encryptedString));
            var cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes),
                CryptoStreamMode.Read);
            var reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Declaration of get all employee method
        /// </summary>
        /// <returns>return list</returns>
        public List<RUserModel> GetAllUser()
        {
            try
            {
                List<RUserModel> employeeModelsList = new List<RUserModel>();
                SqlCommand sqlCommand = new SqlCommand("spGetAllUser", this.sqlConnectionVariable);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                this.sqlConnectionVariable.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    RUserModel userModel = new RUserModel();
                    if (Convert.ToInt32(sqlDataReader["PresentState"]) == 1)
                    {
                        userModel.UserId = Convert.ToInt32(sqlDataReader["UserId"]);
                        userModel.FirstName = sqlDataReader["FirstName"].ToString();
                        userModel.LastName = sqlDataReader["LastName"].ToString();
                        userModel.EmailId = sqlDataReader["EmailId"].ToString();
                        userModel.MobileNumber = sqlDataReader["MobileNumber"].ToString();
                        userModel.LocalAddress = sqlDataReader["LocalAddress"].ToString();
                        userModel.Gender = sqlDataReader["Gender"].ToString();
                        userModel.Role = sqlDataReader["Role"].ToString();
                        userModel.DateAndTime = sqlDataReader["ModificationDate"].ToString();
                        employeeModelsList.Add(userModel);
                    }
                }
                this.sqlConnectionVariable.Close();
                return employeeModelsList;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }


        /// <summary>
        /// Declaration update employee method 
        /// </summary>
        /// <param name="userModel">employee model object passing</param>
        /// <returns>Return boolean value</returns>
        public UUserModel UpdateUserData(UUserModel userModel)
        {
            try
            {
                if (!EmailChecking(userModel.EmailId))
                {
                    SqlCommand sqlCommand = new SqlCommand("spUpdateUserData", this.sqlConnectionVariable);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserId", userModel.UserId);
                    sqlCommand.Parameters.AddWithValue("@FirstName", userModel.FirstName);
                    sqlCommand.Parameters.AddWithValue("@LastName", userModel.LastName);
                    sqlCommand.Parameters.AddWithValue("@EmailId", userModel.EmailId);
                    sqlCommand.Parameters.AddWithValue("@MobileNumber", userModel.MobileNumber);
                    sqlCommand.Parameters.AddWithValue("@CurrentAddress", userModel.LocalAddress);
                    sqlCommand.Parameters.AddWithValue("@Gender", userModel.Gender);
                    sqlCommand.Parameters.AddWithValue("@Role", userModel.Role);
                    this.sqlConnectionVariable.Open();
                    //var response = sqlCommand.ExecuteNonQuery();
                    int status = 0;
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        status = sqlDataReader.GetInt32(0);
                        if (status == 1)
                        {
                            this.sqlConnectionVariable.Close();
                            return userModel;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                this.sqlConnectionVariable.Close();
                return null;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }


        public RUserModel DeleteUserData(int UserId)
        {
            try
            {
                int status = 0;
                int FlagsAttribute = 1;
                if (UserId == 0 )
                {
                    throw new Exception(UserExceptions.ExceptionType.NULL_EXCEPTION.ToString());
                }
                
                SqlCommand sqlCommand = new SqlCommand("spDeleteUser", this.sqlConnectionVariable);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@UserID", UserId);
                this.sqlConnectionVariable.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                RUserModel usermodel = new RUserModel();
                while (sqlDataReader.Read())
                {
                    status = sqlDataReader.GetInt32(0);
                    if (status == UserId)
                    {
                        usermodel.UserId = Convert.ToInt32(sqlDataReader["UserId"]);
                        usermodel.FirstName = sqlDataReader["FirstName"].ToString();
                        usermodel.LastName = sqlDataReader["LastName"].ToString();
                        usermodel.EmailId = sqlDataReader["EmailId"].ToString();
                        usermodel.LocalAddress = sqlDataReader["LocalAddress"].ToString();
                        usermodel.MobileNumber = sqlDataReader["MobileNumber"].ToString();
                        usermodel.Gender = sqlDataReader["Gender"].ToString();
                        usermodel.Role = sqlDataReader["Role"].ToString();
                        usermodel.DateAndTime = sqlDataReader["ModificationDate"].ToString();
                        FlagsAttribute = 0;
                        break;
                    }
                        break;  
                }

                this.sqlConnectionVariable.Close();
                if (FlagsAttribute == 0)
                {
                    return usermodel;
                }

                usermodel = null;
                return usermodel;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Declaration of employee details method
        /// </summary>
        /// <param name="Id">passing id</param>
        /// <returns>return employee model object</returns>
        public RUserModel GetUserDetail(int Id)
        {
            try
            {
                RUserModel userModel = new RUserModel();

                SqlCommand sqlCommand = new SqlCommand("spGetUserDetailById", this.sqlConnectionVariable);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@UserId", Id);
                this.sqlConnectionVariable.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                int status = 0;
                while (sqlDataReader.Read())
                {
                    status = sqlDataReader.GetInt32(0);
                    //EmployeeModel userModel = new EmployeeModel();
                    if (Id == status)
                    {
                        userModel.UserId = Convert.ToInt32(sqlDataReader["UserId"]);
                        userModel.FirstName = sqlDataReader["FirstName"].ToString();
                        userModel.LastName = sqlDataReader["LastName"].ToString();
                        userModel.EmailId = sqlDataReader["EmailId"].ToString();
                        userModel.MobileNumber = sqlDataReader["MobileNumber"].ToString();
                        userModel.LocalAddress = sqlDataReader["LocalAddress"].ToString();
                        userModel.Gender = sqlDataReader["Gender"].ToString();
                        userModel.Role = sqlDataReader["Role"].ToString();
                        userModel.DateAndTime = sqlDataReader["ModificationDate"].ToString();
                        break;
                    }
                    break;
                }

                if(status == 0)
                {
                    return null;
                }
                return userModel;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                this.sqlConnectionVariable.Close();
            }
        }
    }
}
