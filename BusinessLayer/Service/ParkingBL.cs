using BusinessLayer.Interface;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class ParkingBL : IParkingBL
    {

        /// <summary>
        /// ParkingLotRL Reference.
        /// </summary>
        private IParkingRL parkingLotRL;

        /// <summary>
        /// Constructor For Setting ParkingLotRL Instance.
        /// </summary>
        /// <param name="parkingLotRL"></param>
        public ParkingBL(IParkingRL parkingLotRL)
        {
            this.parkingLotRL = parkingLotRL;
        }

        /// <summary>
        /// Function For Parking Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        public RParkingModel Park(ParkingModel parkingDetails)
        {
            try
            {
                return this.parkingLotRL.Park(parkingDetails);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function For Parking Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        public ParkDetailedModel UnPark(String VehicalNumber)
        {
            try
            {
                return this.parkingLotRL.UnPark(VehicalNumber);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function For Parking Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        public bool CheckLotStatus()
        {
            try
            {
                return this.parkingLotRL.CheckLotStatus();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function For Parking Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        public RParkingModel GetVehicalByNumber(string VehicalNumber)
        {
            try
            {
                return this.parkingLotRL.GetVehicalByNumber(VehicalNumber);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function For Parking Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        public List<RParkingModel> GetVehicalDetailsByColor(string Color)
        {
            try
            {
                return this.parkingLotRL.GetVehicalDetailsByColor(Color);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        

        /// <summary>
        /// Function For Parking Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        public List<RParkingModel> GetVehicalDetailsByBrand(string Brand)
        {
            try
            {
                return this.parkingLotRL.GetVehicalDetailsByBrand(Brand);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function For Parking Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        public List<RParkingModel> GetVehicalDetailsByBrandAndColor(string Brand, string Color)
        {
            try
            {
                return this.parkingLotRL.GetVehicalDetailsByBrandAndColor(Brand,Color);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Function For Parking Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        public List<RParkingModel> GetVehicalDetailsByParkingSlot(string ParkingSlot)
        {
            try
            {
                return this.parkingLotRL.GetVehicalDetailsByParkingSlot(ParkingSlot);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

    }
}
