using CommonLayer.Exceptions;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryLayer.Service
{
    public class ParkingRL : IParkingRL
    {

        //Constants.
        public int TotalLotLimit { get; set; } = 100;
        private const int LotALimit = 25;
        private const int LotBLimit = 25;
        private const int LotCLimit = 25;
        private const int LotDLimit = 25;
        private const double RatePerHour = 40;


        private static readonly string ConnectionDeclaration = "Server=.; Database=ParkingLotDatabase; Trusted_Connection=true;MultipleActiveResultSets=True";

        SqlConnection sqlConnectionVariable = new SqlConnection(ConnectionDeclaration);

        public RParkingModel Park(ParkingModel parkingAttribute)
        {
            try
            {
                int HandicaptStatus;

                if (parkingAttribute.VehicalOwnerName == null || parkingAttribute.VehicalNumber == null || parkingAttribute.VehicalOwnerEmail == null ||
                     parkingAttribute.Brand == null || parkingAttribute.Color == null || parkingAttribute.DriverName == null || parkingAttribute.ParkingSlot == null)
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.NULL_EXCEPTION.ToString());
                }

                if (parkingAttribute.VehicalOwnerName == "" || parkingAttribute.VehicalNumber == "" || parkingAttribute.VehicalOwnerEmail == "" ||
                    parkingAttribute.Brand == "" || parkingAttribute.Color == "" || parkingAttribute.DriverName == "" || parkingAttribute.ParkingSlot == "")
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.EMPTY_EXCEPTION.ToString());
                }

                if (parkingAttribute.IsHandicap == "y" || parkingAttribute.IsHandicap == "Y" || parkingAttribute.IsHandicap == "yes" || parkingAttribute.IsHandicap == "Yes")
                {
                    HandicaptStatus = 1;
                }
                else
                {
                    HandicaptStatus = 0;
                }

                //Assiging ParkingSlot 
                parkingAttribute.ParkingSlot = AssignSlot(parkingAttribute);

                if (parkingAttribute.ParkingSlot == "A" || parkingAttribute.ParkingSlot == "B" ||
                        parkingAttribute.ParkingSlot == "C" || parkingAttribute.ParkingSlot == "D")
                {

                    int status = 0;
                    RParkingModel usermodel = new RParkingModel();

                    SqlCommand sqlCommand = new SqlCommand("spParkingRegistration", this.sqlConnectionVariable);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@VehicalOwnerName", parkingAttribute.VehicalOwnerName);
                    sqlCommand.Parameters.AddWithValue("@VehicalOwnerEmail", parkingAttribute.VehicalOwnerEmail);
                    sqlCommand.Parameters.AddWithValue("@VehicalNumber", parkingAttribute.VehicalNumber);
                    sqlCommand.Parameters.AddWithValue("@Brand", parkingAttribute.Brand);
                    sqlCommand.Parameters.AddWithValue("@Color", parkingAttribute.Color);
                    sqlCommand.Parameters.AddWithValue("@DriverName", parkingAttribute.DriverName);
                    sqlCommand.Parameters.AddWithValue("@IsHandicap", HandicaptStatus);
                    sqlCommand.Parameters.AddWithValue("@ParkingSlot", parkingAttribute.ParkingSlot);
                    sqlCommand.Parameters.AddWithValue("@Status", "Parked");
                    this.sqlConnectionVariable.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        status = sqlDataReader.GetInt32(0);

                        if (status > 0)
                        {
                            usermodel.ReceiptNumber = Convert.ToInt32(sqlDataReader["ReceiptNumber"]);
                            usermodel.Status = sqlDataReader["Status"].ToString();
                            usermodel.ParkingDate = sqlDataReader["ParkingDate"].ToString();
                            usermodel.UnparkDate = sqlDataReader["UnparkDate"].ToString();
                            usermodel.TotalTime = sqlDataReader["TotalTime"].ToString();
                            usermodel.TotalAmount = sqlDataReader["TotalAmount"].ToString();
                            return DataCopy(usermodel, parkingAttribute);
                        }
                        else
                        {
                            return null;
                        }
                    }

                }else
                {
                    throw new ParkingLotExceptions(ParkingLotExceptions.ExceptionType.SLOT_NOT_AVAILABLE,"Slot Not Available");
                }
                return null;
            }  
            catch (Exception exception)
            {
                throw exception;
            }
        }


        private RParkingModel DataCopy(RParkingModel usermodel, ParkingModel user)
        {
            usermodel.VehicalOwnerName = user.VehicalOwnerName;
            usermodel.VehicalOwnerEmail = user.VehicalOwnerEmail;
            usermodel.VehicalNumber = user.VehicalNumber;
            usermodel.Brand = user.Brand;
            usermodel.Color = user.Color;
            usermodel.DriverName = user.DriverName;
            usermodel.IsHandicap = user.IsHandicap;
            usermodel.ParkingSlot = user.ParkingSlot;
            return usermodel;
        }

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


        public string AssignSlot(ParkingModel parkingDetails)
        {
            try
            {
                int TotalLimitCondition, lotAUnavailable, lotBUnavailable, lotCUnavailable, lotDUnavailable;
               
                bool HandicaptStatus;
                SqlCommand sqlCommand = new SqlCommand("spCheckParkedCount", this.sqlConnectionVariable);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                this.sqlConnectionVariable.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                if (parkingDetails.IsHandicap == "y" || parkingDetails.IsHandicap == "Y" || parkingDetails.IsHandicap == "yes" || parkingDetails.IsHandicap == "Yes")
                {
                    HandicaptStatus = true;
                }
                else
                {
                    HandicaptStatus = false;
                }

                while (sqlDataReader.Read())
                {

                        TotalLimitCondition = Convert.ToInt32(sqlDataReader["TotalParked"]);
                        lotAUnavailable = Convert.ToInt32(sqlDataReader["TotalAParked"]);
                        lotBUnavailable = Convert.ToInt32(sqlDataReader["TotalBParked"]);
                        lotCUnavailable = Convert.ToInt32(sqlDataReader["TotalCParked"]);
                        lotDUnavailable = Convert.ToInt32(sqlDataReader["TotalDParked"]);

                        if (TotalLimitCondition < TotalLotLimit)
                        {
                            

                            if (HandicaptStatus)
                            {
                                
                                if (lotAUnavailable < LotALimit)
                                {
                                    return "A";
                                }
                                else if (lotBUnavailable < LotBLimit)
                                {
                                    return "B";
                                }
                                else if (lotCUnavailable < LotCLimit)
                                {
                                    return "C";
                                }
                                else if (lotDUnavailable < LotDLimit)
                                {
                                    return "D";
                                }
                            }
                            else
                            {
                                //Depending On Vaccancy, Slot will be Provided.
                                if (lotAUnavailable < LotALimit )
                                {
                                    return "A";
                                }
                                else if (lotBUnavailable < LotBLimit )
                                {
                                    return "B";
                                }
                                else if (lotCUnavailable < LotCLimit )
                                {
                                    return "C";
                                }
                                else if (lotDUnavailable < LotDLimit)
                                {
                                    return "D";
                                }
                            }
                        }
                        return "UnAvailable";
                }
                return "Unavailable";
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                this.sqlConnectionVariable.Close();
            }
        }


    }
}
