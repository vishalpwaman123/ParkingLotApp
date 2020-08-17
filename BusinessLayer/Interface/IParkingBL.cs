//-------------------------------------------------------------------------
// <copyright file="IParkingBL.cs" company="BridgeLab">
//      Copyright (c) Company. All rights reserved.
// </copyright>
// <author>Vishal Waman</author>
//-------------------------------------------------------------------------

namespace BusinessLayer.Interface
{
    using CommonLayer.RequestModel;
    using CommonLayer.ResponseModel;
    using System;
    using System.Collections.Generic;
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
        ParkDetailedModel DeleteVehicalById(int ParkingId);

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
        /// Abstract Function For GetVehicalDetailsByBrand Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        List<RParkingModel> GetVehicalDetailsByBrand(string Brand);

        /// <summary>
        /// Abstract Function For GetVehicalDetailsByBrandAndColor Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        List<RParkingModel> GetVehicalDetailsByBrandAndColor(string Brand,string Color);

        /// <summary>
        /// Abstract Function For GetVehicalDetailsByParkingSlot Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        List<RParkingModel> GetVehicalDetailsByParkingSlot(string ParkingSlot);

        /// <summary>
        /// Abstract Function For GetVehicalDetailsByParkingSlot Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        List<RParkingModel> CheckAllVehical();

        /// <summary>
        /// Abstract Function For GetVehicalDetailsByParkingSlot Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        List<RParkingModel> CheckParkVehical();

        /// <summary>
        /// Abstract Function For GetVehicalDetailsByParkingSlot Vehical.
        /// </summary>
        /// <param name="parkingDetails"></param>
        /// <returns></returns>
        List<RParkingModel> CheckUnParkVehical();
    }
}
