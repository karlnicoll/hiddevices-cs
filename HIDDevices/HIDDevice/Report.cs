using System;
using System.Collections.Generic;
using System.Text;

namespace HIDDevices
{
    /// <summary>
    /// A Class representing a pretty HID device report
    /// </summary>
    public class HIDDeviceReport
    {
        //==================================================================================
        #region Data Structures



        #endregion

        //==================================================================================
        #region Enumerations



        #endregion

        //==================================================================================
        #region Constants


        #endregion

        //==================================================================================
        #region Events



        #endregion

        //==================================================================================
        #region Private Variables

        long reportID;      //Holds the ID of this report
        byte reportType;    //The type of the report
        byte[] report;      //Holds the contents of the report (minus the report type)


        #endregion

        //==================================================================================
        #region Constructors/Destructors

        /// <summary>
        /// Constructor
        /// </summary>
        public HIDDeviceReport()
        {
            SetDefaults(-1, 0x00, null);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="RepID">The unique ID of this report</param>
        public HIDDeviceReport(long RepID)
        {
            SetDefaults(RepID, 0x00, null);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="RepType">The type of the report</param>
        /// <param name="RepID">The unique ID of this report</param>
        /// <param name="Rep">The report itself</param>
        public HIDDeviceReport(long RepID, byte[] RawReport)
        {
            SetReport(RepID, RawReport);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="RepID">The unique ID of the report</param>
        /// <param name="ReportType">The type of the report</param>
        /// <param name="ReportContents">The contents of the report</param>
        public HIDDeviceReport(long RepID, byte ReportType, byte[] ReportContents)
        {
            SetDefaults(RepID, ReportType, ReportContents);
        }

        #endregion

        //==================================================================================
        #region Public Properties

        /// <summary>
        /// Gets or sets the contents of this report (does not include the report Type).
        /// NOTE: When setting the report, the value should NOT include the report type. If
        /// the report is "Raw" (i.e. contains the report type as the first byte), then you
        /// must use the SetReport(long, byte[]) method.
        /// </summary>
        public byte[] Content   
        {
            get { return report; }
            set { report = value; }
        }

        /// <summary>
        /// Gets or sets the ID of this report
        /// </summary>
        public long ID          
        {
            get { return reportID; }
            set { reportID = value; }
        }

        /// <summary>
        /// Gets or sets the type of report that this instance represents
        /// </summary>
        public byte Type        
        {
            get { return reportType; }
            set { reportType = value; }
        }

        /// <summary>
        /// Gets whether or not this is a valid report
        /// </summary>
        public bool IsValid     
        {
            get { return (reportID >= 0 && report != null); }
        }

        #endregion

        //==================================================================================
        #region Public Methods

        /// <summary>
        /// Sets the values of this report
        /// </summary>
        /// <param name="Rep">The content of the report</param>
        public void SetReport(byte[] RawReport)
        {
            SetReport(this.reportID, RawReport);
        }
        /// <summary>
        /// Sets the values of this report
        /// </summary>
        /// <param name="RepID">The ID of this report</param>
        /// <param name="Rep">The content of the report</param>
        public void SetReport(long RepID, byte[] RawReport)
        {
            //Split the report type from the raw report
            byte type;
            byte[] newReport = new byte[RawReport.Length];

            type = RawReport[0];
            for (int i = 1; i < RawReport.Length; i++)
            {
                newReport[i - 1] = RawReport[i];
            }

            SetDefaults(RepID, type, newReport);
        }

        /// <summary>
        /// Converts this report into it's raw byte array. This means that the report is
        /// returned as a single byte array with the first byte of the array containing
        /// the report type
        /// </summary>
        /// <returns>The raw data report</returns>
        public byte[] ToRaw()
        {
            byte[] returnedArray = new byte[report.Length + 1];

            returnedArray[0] = reportType;
            for (int i = 0; i < report.Length; i++)
            {
                returnedArray[i + 1] = report[i];
            }

            return returnedArray;
        }

        #endregion

        //==================================================================================
        #region Private/Protected Methods

        #region Report Initialization

        /// <summary>
        /// Sets the variables of this class
        /// </summary>
        /// <param name="reptype">The type of report</param>
        /// <param name="id">The report ID of this report instance</param>
        /// <param name="rep">An array reference to the report itself</param>
        private void SetDefaults(long id, byte reptype, byte[] rep)
        {
            reportID = id;
            reportType = reptype;
            report = rep;
        }

        #endregion

        #endregion
    }
}
