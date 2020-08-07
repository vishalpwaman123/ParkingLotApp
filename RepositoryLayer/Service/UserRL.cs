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
        public RUserModel RegisterUser(UserModel user)
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
                        if (status == 1)
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

        private RUserModel DataCopy(RUserModel usermodel, UserModel user)
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

    }
}
