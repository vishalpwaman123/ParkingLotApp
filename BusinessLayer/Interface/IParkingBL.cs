using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IParkingBL
    {

        /// <summary>
        /// Abstract Function For Parking Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        RParkingModel Park(ParkingModel parkingDetails);

        /// <summary>
        /// Abstract Function For UnParking Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        ParkDetailedModel UnPark(String VehicalNumber);

        /// <summary>
        /// Abstract Function For UnParking Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        bool CheckLotStatus();

        /// <summary>
        /// Abstract Function For GetVehicalByNumber Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        RParkingModel GetVehicalByNumber(string VehicalNumber);

        /// <summary>
        /// Abstract Function For GetVehicalDetailsByColor Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        List<RParkingModel> GetVehicalDetailsByColor(string Color);

        /// <summary>
        /// Abstract Function For GetVehicalDetailsByColor Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        List<RParkingModel> GetVehicalDetailsByBrand(string Brand);

        /// <summary>
        /// Abstract Function For GetVehicalDetailsByColor Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        List<RParkingModel> GetVehicalDetailsByBrandAndColor(string Brand,string Color);



        /// <summary>
        /// Abstract Function For GetVehicalDetailsByColor Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        List<RParkingModel> GetVehicalDetailsByParkingSlot(string ParkingSlot);
    }
}
