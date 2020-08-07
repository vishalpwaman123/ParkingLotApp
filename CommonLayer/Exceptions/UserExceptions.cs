using System;
using System.Collections.Generic;
using System.Text;

namespace CommanLayer.Exceptions
{
    public class UserExceptions : Exception
    {
        /// <summary>
        /// Enum For Exception types.
        /// </summary>
        public enum ExceptionType
        {
            INVALID_ROLE_EXCEPTION,
            NULL_EXCEPTION,
            EMPTY_EXCEPTION
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
        public UserExceptions(UserExceptions.ExceptionType type, string message) : base(message)
        {
            this.type = type;
        }

    }
}
