using CommonLayer.Exceptions;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

namespace RepositoryLayer.Service
{
    public class ParkingRL : IParkingRL
    {

        //Constants.
        public int TotalLotLimit { get; set; } = 16;
        private const int LotALimit = 4;
        private const int LotBLimit = 4;
        private const int LotCLimit = 4;
        private const int LotDLimit = 4;
        private const int RatePerMin = 10;


        private static readonly string ConnectionDeclaration = "Server=.; Database=ParkingLotDatabase; Trusted_Connection=true;MultipleActiveResultSets=True";

        SqlConnection sqlConnectionVariable = new SqlConnection(ConnectionDeclaration);

        public RParkingModel Park(ParkingModel parkingAttribute)
        {
            try
            {
                int HandicaptStatus;

                if (parkingAttribute.VehicalOwnerName == null || parkingAttribute.VehicalNumber == null || parkingAttribute.VehicalOwnerEmail == null ||
                     parkingAttribute.Brand == null || parkingAttribute.Color == null || parkingAttribute.DriverName == null )
                {
                    throw new Exception(ParkingLotExceptions.ExceptionType.NULL_EXCEPTION.ToString());
                }

                if (parkingAttribute.VehicalOwnerName == "" || parkingAttribute.VehicalNumber == "" || parkingAttribute.VehicalOwnerEmail == "" ||
                    parkingAttribute.Brand == "" || parkingAttribute.Color == "" || parkingAttribute.DriverName == "")
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
                string ParkingSlot = AssignSlot(parkingAttribute);

                if (ParkingSlot == "A" || ParkingSlot == "B" ||
                       ParkingSlot == "C" || ParkingSlot == "D")
                {

                    int status = 0;
                    RParkingModel usermodel = new RParkingModel();

                    SqlCommand sqlCommand = new SqlCommand("spParkVehical", this.sqlConnectionVariable);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@VehicalOwnerName", parkingAttribute.VehicalOwnerName);
                    sqlCommand.Parameters.AddWithValue("@VehicalOwnerEmail", parkingAttribute.VehicalOwnerEmail);
                    sqlCommand.Parameters.AddWithValue("@VehicalNumber", parkingAttribute.VehicalNumber);
                    sqlCommand.Parameters.AddWithValue("@Brand", parkingAttribute.Brand);
                    sqlCommand.Parameters.AddWithValue("@Color", parkingAttribute.Color);
                    sqlCommand.Parameters.AddWithValue("@DriverName", parkingAttribute.DriverName);
                    sqlCommand.Parameters.AddWithValue("@IsHandicap", HandicaptStatus);
                    sqlCommand.Parameters.AddWithValue("@ParkingSlot", ParkingSlot);
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
                            usermodel.ParkingSlot = ParkingSlot;
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


        /// <summary>
        /// Function For Unpark Api.
        /// </summary>
        /// <param name="VehicalNumber"></param>
        /// <returns></returns>
        public ParkDetailedModel UnPark(string VehicalNumber)
        {
            try
            {
                int status;
                ParkDetailedModel Parkmodel = new ParkDetailedModel();
                if (VehicalNumber != null)
                {
                    
                    SqlCommand sqlCommand = new SqlCommand("spGetUnParkDetail", this.sqlConnectionVariable);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@VehicalNumber", VehicalNumber);
                    this.sqlConnectionVariable.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        status = sqlDataReader.GetInt32(0);

                        if (status > 0)
                        {
                            Parkmodel.ReceiptNumber = Convert.ToInt32(sqlDataReader["ReceiptNumber"]);
                            Parkmodel.VehicalOwnerName = sqlDataReader["VehicalOwnerName"].ToString();
                            Parkmodel.VehicalOwnerEmail = sqlDataReader["VehicalOwnerEmail"].ToString();
                            Parkmodel.VehicalNumber = sqlDataReader["VehicalNumber"].ToString();
                            Parkmodel.Brand = sqlDataReader["Brand"].ToString();
                            Parkmodel.Color = sqlDataReader["Color"].ToString();
                            Parkmodel.DriverName = sqlDataReader["DriverName"].ToString();
                            Parkmodel.IsHandicap = sqlDataReader["IsHandicap"].ToString();
                            Parkmodel.ParkingSlot = sqlDataReader["ParkingSlot"].ToString();
                            Parkmodel.Status = sqlDataReader["Status"].ToString();
                            Parkmodel.ParkingDate = sqlDataReader["ParkingDate"].ToString();
                            Parkmodel.UnparkDate = sqlDataReader["UnparkDate"].ToString();
                            Parkmodel.TotalTime = sqlDataReader["Minutes"].ToString()+" Minute";
                            Parkmodel.TotalAmount = AmountCanculation(Convert.ToInt32(sqlDataReader["Minutes"])).ToString()+" Rupees";
                            break;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                return Parkmodel;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private int AmountCanculation(int Minute)
        {
            int Amount = Minute * RatePerMin;
            int TotalAmount = Amount > RatePerMin ? Amount : RatePerMin;
            return TotalAmount;
        }


        /// <summary>
        /// Function For Checking Parking Lot Status.
        /// </summary>
        /// <returns></returns>
        public bool CheckLotStatus()
        {
            try
            {
                int parkedVehicalCount=0;
                SqlCommand sqlCommand = new SqlCommand("spCheckParkedCount", this.sqlConnectionVariable);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                this.sqlConnectionVariable.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    parkedVehicalCount = Convert.ToInt32(sqlDataReader["TotalParked"]);
                }

                bool status = parkedVehicalCount < TotalLotLimit ? true : false;
                return status;
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function To Find Vehical By Number.
        /// </summary>
        /// <param name="vehicalNumber"></param>
        /// <returns></returns>
        public RParkingModel GetVehicalByNumber(string VehicalNumber)
        {
            try
            {

                

                int status = 0;
                RParkingModel pmode = new RParkingModel();
                SqlCommand sqlCommand = new SqlCommand("spGetVehicalByNumber", this.sqlConnectionVariable);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@VehicalNumber", VehicalNumber);
                this.sqlConnectionVariable.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    status = sqlDataReader.GetInt32(0);
                    if ( status > 0 )
                    {   
                        pmode.ReceiptNumber = Convert.ToInt32(sqlDataReader["ReceiptNumber"]);
                        pmode.VehicalOwnerEmail = sqlDataReader["VehicalOwnerEmail"].ToString();
                        pmode.VehicalOwnerName = sqlDataReader["VehicalOwnerName"].ToString();
                        pmode.Brand = sqlDataReader["Brand"].ToString();
                        pmode.Color = sqlDataReader["Color"].ToString();
                        pmode.DriverName = sqlDataReader["DriverName"].ToString();
                        pmode.ParkingDate = sqlDataReader["ParkingDate"].ToString();
                        pmode.ParkingSlot = sqlDataReader["ParkingSlot"].ToString();
                        pmode.Status = sqlDataReader["Status"].ToString();
                        pmode.VehicalNumber = sqlDataReader["VehicalNumber"].ToString();
                        if (Convert.ToInt32(sqlDataReader["IsHandicap"]) == 1)
                        {
                            pmode.IsHandicap = "Yes";
                        }
                        else
                        {
                            pmode.IsHandicap = "No";
                        }
                        return pmode;
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }


        /// <summary>
        /// Function To Find Vehical By Number.
        /// </summary>
        /// <param name="vehicalNumber"></param>
        /// <returns></returns>
        public List<RParkingModel> GetVehicalDetailsByColor(string Color)
        {
            try
            {


                int status = 0;
                List<RParkingModel> ParkingModelsList = new List<RParkingModel>();
                
                SqlCommand sqlCommand = new SqlCommand("spGetVehicalByColor", this.sqlConnectionVariable);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Color", Color);
                this.sqlConnectionVariable.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    status = sqlDataReader.GetInt32(0);
                    if (status > 0)
                    {
                        RParkingModel pmode = new RParkingModel();
                        pmode.ReceiptNumber = Convert.ToInt32(sqlDataReader["ReceiptNumber"]);
                        pmode.VehicalOwnerEmail = sqlDataReader["VehicalOwnerEmail"].ToString();
                        pmode.VehicalOwnerName = sqlDataReader["VehicalOwnerName"].ToString();
                        pmode.Brand = sqlDataReader["Brand"].ToString();
                        pmode.Color = sqlDataReader["Color"].ToString();
                        pmode.DriverName = sqlDataReader["DriverName"].ToString();
                        pmode.ParkingDate = sqlDataReader["ParkingDate"].ToString();
                        pmode.ParkingSlot = sqlDataReader["ParkingSlot"].ToString();
                        pmode.Status = sqlDataReader["Status"].ToString();
                        pmode.VehicalNumber = sqlDataReader["VehicalNumber"].ToString();
                        if (Convert.ToInt32(sqlDataReader["IsHandicap"]) == 1)
                        {
                            pmode.IsHandicap = "Yes";
                        }
                        else
                        {
                            pmode.IsHandicap = "No";
                        }
                        ParkingModelsList.Add(pmode);
                    }
                    else
                    {
                        return null;
                    }
                }
                this.sqlConnectionVariable.Close();
                return ParkingModelsList;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function To Find Vehical By Number.
        /// </summary>
        /// <param name="vehicalNumber"></param>
        /// <returns></returns>
        public List<RParkingModel> GetVehicalDetailsByBrand(string Brand)
        {
            try
            {

                int status = 0;
                List<RParkingModel> ParkingModelsList = new List<RParkingModel>();
                
                SqlCommand sqlCommand = new SqlCommand("spGetVehicalByBrand", this.sqlConnectionVariable);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Brand", Brand);
                this.sqlConnectionVariable.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    status = sqlDataReader.GetInt32(0);
                    if (status > 0)
                    {
                        RParkingModel pmode = new RParkingModel();
                        pmode.ReceiptNumber = Convert.ToInt32(sqlDataReader["ReceiptNumber"]);
                        pmode.VehicalOwnerEmail = sqlDataReader["VehicalOwnerEmail"].ToString();
                        pmode.VehicalOwnerName = sqlDataReader["VehicalOwnerName"].ToString();
                        pmode.Brand = sqlDataReader["Brand"].ToString();
                        pmode.Color = sqlDataReader["Color"].ToString();
                        pmode.DriverName = sqlDataReader["DriverName"].ToString();
                        pmode.ParkingDate = sqlDataReader["ParkingDate"].ToString();
                        pmode.ParkingSlot = sqlDataReader["ParkingSlot"].ToString();
                        pmode.Status = sqlDataReader["Status"].ToString();
                        pmode.VehicalNumber = sqlDataReader["VehicalNumber"].ToString();
                        if (Convert.ToInt32(sqlDataReader["IsHandicap"]) == 1)
                        {
                            pmode.IsHandicap = "Yes";
                        }
                        else
                        {
                            pmode.IsHandicap = "No";
                        }
                        ParkingModelsList.Add(pmode);
                    }
                    else
                    {
                        return null;
                    }
                }
                return ParkingModelsList;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function To Find Vehical By Number.
        /// </summary>
        /// <param name="vehicalNumber"></param>
        /// <returns></returns>
        public List<RParkingModel> GetVehicalDetailsByBrandAndColor(string Brand, string Color)
        {
            try
            {

                int status = 0;
                List<RParkingModel> ParkingModelsList = new List<RParkingModel>();
                SqlCommand sqlCommand = new SqlCommand("spGetVehicalByColorAndBrand", this.sqlConnectionVariable);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Color", Color);
                sqlCommand.Parameters.AddWithValue("@Brand", Brand);
                this.sqlConnectionVariable.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    status = sqlDataReader.GetInt32(0);
                    if (status > 0)
                    {
                        RParkingModel pmode = new RParkingModel();
                        pmode.ReceiptNumber = Convert.ToInt32(sqlDataReader["ReceiptNumber"]);
                        pmode.VehicalOwnerEmail = sqlDataReader["VehicalOwnerEmail"].ToString();
                        pmode.VehicalOwnerName = sqlDataReader["VehicalOwnerName"].ToString();
                        pmode.Brand = sqlDataReader["Brand"].ToString();
                        pmode.Color = sqlDataReader["Color"].ToString();
                        pmode.DriverName = sqlDataReader["DriverName"].ToString();
                        pmode.ParkingDate = sqlDataReader["ParkingDate"].ToString();
                        pmode.ParkingSlot = sqlDataReader["ParkingSlot"].ToString();
                        pmode.Status = sqlDataReader["Status"].ToString();
                        pmode.VehicalNumber = sqlDataReader["VehicalNumber"].ToString();
                        if (Convert.ToInt32(sqlDataReader["IsHandicap"]) == 1)
                        {
                            pmode.IsHandicap = "Yes";
                        }
                        else
                        {
                            pmode.IsHandicap = "No";
                        }
                        ParkingModelsList.Add(pmode);
                    }
                    else
                    {
                        return null;
                    }
                }
                return ParkingModelsList;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function To Find Vehical By Number.
        /// </summary>
        /// <param name="vehicalNumber"></param>
        /// <returns></returns>
        public List<RParkingModel> GetVehicalDetailsByParkingSlot(string ParkingSlot)
        {
            try
            {

                int status = 0;
                List<RParkingModel> ParkingModelsList = new List<RParkingModel>();
                
                SqlCommand sqlCommand = new SqlCommand("GetVehicalDetailsByParkingSlot", this.sqlConnectionVariable);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@ParkingSlot", ParkingSlot);
                
                this.sqlConnectionVariable.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    status = sqlDataReader.GetInt32(0);
                    if (status > 0)
                    {
                        RParkingModel pmode = new RParkingModel();
                        pmode.ReceiptNumber = Convert.ToInt32(sqlDataReader["ReceiptNumber"]);
                        pmode.VehicalOwnerEmail = sqlDataReader["VehicalOwnerEmail"].ToString();
                        pmode.VehicalOwnerName = sqlDataReader["VehicalOwnerName"].ToString();
                        pmode.Brand = sqlDataReader["Brand"].ToString();
                        pmode.Color = sqlDataReader["Color"].ToString();
                        pmode.DriverName = sqlDataReader["DriverName"].ToString();
                        pmode.ParkingDate = sqlDataReader["ParkingDate"].ToString();
                        pmode.ParkingSlot = sqlDataReader["ParkingSlot"].ToString();
                        pmode.Status = sqlDataReader["Status"].ToString();
                        pmode.VehicalNumber = sqlDataReader["VehicalNumber"].ToString();
                        if (Convert.ToInt32(sqlDataReader["IsHandicap"]) == 1)
                        {
                            pmode.IsHandicap = "Yes";
                        }
                        else
                        {
                            pmode.IsHandicap = "No";
                        }
                        ParkingModelsList.Add(pmode);
                    }
                    else
                    {
                        return null;
                    }
                }
                return ParkingModelsList;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function To Find Vehical By Number.
        /// </summary>
        /// <param name="vehicalNumber"></param>
        /// <returns></returns>
        public List<RParkingModel> CheckAllVehical()
        {
            try
            {

                int status = 0;
                List<RParkingModel> ParkingModelsList = new List<RParkingModel>();

                SqlCommand sqlCommand = new SqlCommand("spParkDetails", this.sqlConnectionVariable);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Status", "ALL");
                this.sqlConnectionVariable.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    status = sqlDataReader.GetInt32(0);
                    if (status > 0)
                    {
                        RParkingModel pmode = new RParkingModel();
                        pmode.ReceiptNumber = Convert.ToInt32(sqlDataReader["ReceiptNumber"]);
                        pmode.VehicalOwnerEmail = sqlDataReader["VehicalOwnerEmail"].ToString();
                        pmode.VehicalOwnerName = sqlDataReader["VehicalOwnerName"].ToString();
                        pmode.Brand = sqlDataReader["Brand"].ToString();
                        pmode.Color = sqlDataReader["Color"].ToString();
                        pmode.DriverName = sqlDataReader["DriverName"].ToString();
                        pmode.ParkingDate = sqlDataReader["ParkingDate"].ToString();
                        pmode.ParkingSlot = sqlDataReader["ParkingSlot"].ToString();
                        pmode.Status = sqlDataReader["Status"].ToString();
                        pmode.VehicalNumber = sqlDataReader["VehicalNumber"].ToString();
                        pmode.VehicalNumber = sqlDataReader["UnparkDate"].ToString();
                        pmode.VehicalNumber = sqlDataReader["TotalTime"].ToString();
                        pmode.VehicalNumber = sqlDataReader["TotalAmount"].ToString();
                        if (Convert.ToInt32(sqlDataReader["IsHandicap"]) == 1)
                        {
                            pmode.IsHandicap = "Yes";
                        }
                        else
                        {
                            pmode.IsHandicap = "No";
                        }
                        ParkingModelsList.Add(pmode);
                    }
                    else
                    {
                        return null;
                    }
                }
                return ParkingModelsList;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function To Find Vehical By Number.
        /// </summary>
        /// <param name="vehicalNumber"></param>
        /// <returns></returns>
        public List<RParkingModel> CheckParkVehical()
        {
            try
            {

                int status = 0;
                List<RParkingModel> ParkingModelsList = new List<RParkingModel>();

                SqlCommand sqlCommand = new SqlCommand("spParkDetails", this.sqlConnectionVariable);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Status", "Parked");
                this.sqlConnectionVariable.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    status = sqlDataReader.GetInt32(0);
                    if (status > 0)
                    {
                        RParkingModel pmode = new RParkingModel();
                        pmode.ReceiptNumber = Convert.ToInt32(sqlDataReader["ReceiptNumber"]);
                        pmode.VehicalOwnerEmail = sqlDataReader["VehicalOwnerEmail"].ToString();
                        pmode.VehicalOwnerName = sqlDataReader["VehicalOwnerName"].ToString();
                        pmode.Brand = sqlDataReader["Brand"].ToString();
                        pmode.Color = sqlDataReader["Color"].ToString();
                        pmode.DriverName = sqlDataReader["DriverName"].ToString();
                        pmode.ParkingDate = sqlDataReader["ParkingDate"].ToString();
                        pmode.ParkingSlot = sqlDataReader["ParkingSlot"].ToString();
                        pmode.Status = sqlDataReader["Status"].ToString();
                        pmode.VehicalNumber = sqlDataReader["VehicalNumber"].ToString();
                        pmode.VehicalNumber = sqlDataReader["UnparkDate"].ToString();
                        pmode.VehicalNumber = sqlDataReader["TotalTime"].ToString();
                        pmode.VehicalNumber = sqlDataReader["TotalAmount"].ToString();
                        if (Convert.ToInt32(sqlDataReader["IsHandicap"]) == 1)
                        {
                            pmode.IsHandicap = "Yes";
                        }
                        else
                        {
                            pmode.IsHandicap = "No";
                        }
                        ParkingModelsList.Add(pmode);
                    }
                    else
                    {
                        return null;
                    }
                }
                return ParkingModelsList;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function To Find Vehical By Number.
        /// </summary>
        /// <param name="vehicalNumber"></param>
        /// <returns></returns>
        public List<RParkingModel> CheckUnParkVehical()
        {
            try
            {

                int status = 0;
                List<RParkingModel> ParkingModelsList = new List<RParkingModel>();

                SqlCommand sqlCommand = new SqlCommand("spParkDetails", this.sqlConnectionVariable);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Status", "UnParked");
                this.sqlConnectionVariable.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    status = sqlDataReader.GetInt32(0);
                    if (status > 0)
                    {
                        RParkingModel pmode = new RParkingModel();
                        pmode.ReceiptNumber = Convert.ToInt32(sqlDataReader["ReceiptNumber"]);
                        pmode.VehicalOwnerEmail = sqlDataReader["VehicalOwnerEmail"].ToString();
                        pmode.VehicalOwnerName = sqlDataReader["VehicalOwnerName"].ToString();
                        pmode.Brand = sqlDataReader["Brand"].ToString();
                        pmode.Color = sqlDataReader["Color"].ToString();
                        pmode.DriverName = sqlDataReader["DriverName"].ToString();
                        pmode.ParkingDate = sqlDataReader["ParkingDate"].ToString();
                        pmode.ParkingSlot = sqlDataReader["ParkingSlot"].ToString();
                        pmode.Status = sqlDataReader["Status"].ToString();
                        pmode.VehicalNumber = sqlDataReader["VehicalNumber"].ToString();
                        pmode.VehicalNumber = sqlDataReader["UnparkDate"].ToString();
                        pmode.VehicalNumber = sqlDataReader["TotalTime"].ToString();
                        pmode.VehicalNumber = sqlDataReader["TotalAmount"].ToString();
                        if (Convert.ToInt32(sqlDataReader["IsHandicap"]) == 1)
                        {
                            pmode.IsHandicap = "Yes";
                        }
                        else
                        {
                            pmode.IsHandicap = "No";
                        }
                        ParkingModelsList.Add(pmode);
                    }
                    else
                    {
                        return null;
                    }
                }
                return ParkingModelsList;
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
            
            return usermodel;
        }

        public bool EmailChecking(string emailId)
        {

            SqlCommand sqlCommand = new SqlCommand("spcheckemailId", this.sqlConnectionVariable);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@EmailId", emailId);
            sqlCommand.Parameters.AddWithValue("@Table", "Parking");
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
