using System;
using System.Collections.Generic;
using System.Text;

namespace HIDDevices
{
    /// <summary>
    /// Exception that is thrown when an HID Device Fails for some reason
    /// </summary>
    public class HIDDeviceException : ApplicationException
    {
        /// <summary>
		/// Default constructor
		/// </summary>
		public HIDDeviceException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">Error message</param>
		public HIDDeviceException(string message) : base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">Error message</param>
		/// <param name="innerException">Inner exception</param>
		public HIDDeviceException(string message, Exception innerException) : base(message, innerException)
		{
		}
    }

    /// <summary>
    /// Exception that is thrown when an HID Device Fails to connect with the program
    /// </summary>
    public class HIDDeviceConnectionException : ApplicationException
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public HIDDeviceConnectionException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        public HIDDeviceConnectionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Inner exception</param>
        public HIDDeviceConnectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Exception that is thrown when an HID Device Fails to connect with the program
    /// </summary>
    public class HIDDeviceInvalidPathException : ApplicationException
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public HIDDeviceInvalidPathException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        public HIDDeviceInvalidPathException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Inner exception</param>
        public HIDDeviceInvalidPathException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// This exception is thrown when an HID device failed to send a report for some reason
    /// </summary>
    public class HIDDeviceReportSendingException : ApplicationException
    {
        private int errNo;      //The Win32 error code

        /// <summary>
        /// Gets the error number that was retrieved when this exception occured
        /// </summary>
        public int ErrorNumber
        {
            get { return errNo; }
        }


        /// <summary>
        /// Default constructor
        /// </summary>
        public HIDDeviceReportSendingException()
            : this("Error while sending report to HID Device")
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ErrorCode">The win32 error code that was raised</param>
        public HIDDeviceReportSendingException(int ErrorCode)
            : this("Error while sending report to HID Device")
        {
            errNo = ErrorCode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        public HIDDeviceReportSendingException(string message)
            : this(message, 0, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="ErrorCode">The win32 error code that was raised</param>
        public HIDDeviceReportSendingException(string message, int ErrorCode)
            : this(message, 0, null)
        {
            errNo = ErrorCode;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Inner exception</param>
        public HIDDeviceReportSendingException(string message, Exception innerException)
            : this(message, 0, innerException)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="ErrorCode">The error code</param>
        /// <param name="innerException">any pre-requisite exceptions that led to this exception occuring</param>
        public HIDDeviceReportSendingException(string message, int ErrorCode, Exception innerException)
        : base(message, innerException)
        {
            errNo = ErrorCode;
        }
    }
}
