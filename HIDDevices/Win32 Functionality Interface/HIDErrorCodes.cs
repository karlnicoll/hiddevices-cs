using System;
using System.Collections.Generic;
using System.Text;

namespace HIDDevices
{
    internal class HIDErrorCodes
    {
        //==================================================================================
        #region Enumerations



        #endregion

        //==================================================================================
        #region Data Structures



        #endregion

        //==================================================================================
        #region Constants

        #region Win32 Error Codes

        /// <summary>
        /// A Win32 Error that states that the member index of a function that uses member indexes was greater than the amount of items accessible.
        /// </summary>
        internal const int ERROR_NO_MORE_ITEMS = 259;
        /// <summary>
        /// Presented when the size of a buffer passed to a function is not big enough
        /// </summary>
        internal const int ERROR_INSUFFICIENT_BUFFER = 122;
        /// <summary>
        /// Generally emitted by these classes when a device pointed at by a device path cannot be located. (usually a result of the device not being connected at that time)
        /// </summary>
        internal const int ERROR_FILE_NOT_FOUND = 2;

        #endregion

        #endregion

        //==================================================================================
        #region Private Variables



        #endregion

        //==================================================================================
        #region Constructors



        #endregion

        //==================================================================================
        #region Public Properties



        #endregion


        //==================================================================================
        #region Public Methods


        #endregion


        //==================================================================================
        #region Private/Protected Methods



        #endregion
    }

}
