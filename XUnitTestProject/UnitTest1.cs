using BusinessLayer.Interface;
using ParkingLotApi.Controllers;
using RepositoryLayer.Interface;
using System;
using Xunit;

namespace XUnitTestProject
{
    public class UnitTest1
    {

        //Controller Reference.
        UserController userController;
        ParkingController parkingController;

        //Reference BL and RL.
        private readonly IUserBL userBL;
        private readonly IUserRL userRL;
        private readonly IParkingLotBL parkingLotBL;
        private readonly IParkingLotRL parkingLotRL;


        [Fact]
        public void Test1()
        {

        }
    }
}
