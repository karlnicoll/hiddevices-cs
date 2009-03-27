using System;
using System.Collections.Generic;
using System.Text;

namespace HIDDevices
{
    /// <summary>
    /// Holds the event arguments for when an HID Device receives an input report
    /// </summary>
    public class HIDDeviceReportReceivedEventargs: EventArgs
    {
        //==================================================================================
        #region Private Variables

        private HIDDeviceReport report;  //The report that was received

        #endregion
        
        //==================================================================================
        #region Public Properties

        /// <summary>
        /// Gets the report that was received
        /// </summary>
        public HIDDeviceReport Report    
        {
            get { return report; }
        }

        #endregion

        //==================================================================================
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ReceivedReport">The report that raised the event this set instance will be attached to</param>
        public HIDDeviceReportReceivedEventargs(HIDDeviceReport ReceivedReport)
        {
            this.report = ReceivedReport;
        }

        #endregion
    }
}
