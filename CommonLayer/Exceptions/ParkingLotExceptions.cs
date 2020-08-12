using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Exceptions
{
    public class ParkingLotExceptions : Exception
    {

        public enum ExceptionType
        {
            NULL_EXCEPTION,
            EMPTY_EXCEPTION,
            INVALID_VEHICAL_NUMBER,
            INVALID_SLOT,
            SLOT_NOT_AVAILABLE,
            INVALID_COLOR,
            INVALID_PARKING_SLOT
        }

        /// <summary>
        /// Exception type Reference.
        /// </summary>
        ExceptionType type;

        /// <summary>
        /// Constrcutor For Setting Exception Type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public ParkingLotExceptions(ParkingLotExceptions.ExceptionType type, string message) : base(message)
        {
            this.type = type;
        }
    }


}

