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

    }
}
