/*==================================================================================*
 * Project:             HIDDevices                                                  *
 * Project Description: A class library that allows easy communication between .NET *
 *                      applications and HID devices                                *
 * Date:                15th December 2008                                          *
 * Author:              Karl Nicoll                                                 *
 * Licence:             Pending                                                     *
 * File:                HIDDevice.cs                                                *
 * File Description:    Holds the high level .NET class that represents an HID      *
 *                      device.                                                     *
 * Changes:             2009/01/17: Removed loop from Asynchronous read function,   *
 *                                  was not necessary and created a memory leak     *
 *                      2009/03/08: Added event for when the device is disconnected *
 *                                  which allows applications to know when the      *
 *                                  device is disconnected.                         *
 *==================================================================================*/

using System;
using System.IO;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace HIDDevices
{
    /// <summary>
    /// Generic Class for an HID device. This device does not NEED to be inherited, but it is highly recommended to inherit this class to implement your
    /// own HID device.
    /// </summary>
    public class HIDDevice : System.IDisposable
    {
        //==================================================================================
        #region Enumerations



        #endregion

        //==================================================================================
        #region Constants

        private const int DEFAULT_REPORT_LENGTH = 22;

        #endregion

        //==================================================================================
        #region Events

        public event EventHandler<HIDDeviceReportReceivedEventargs> ReportReceived;     //Fired when a report is received by the device

        /// <summary>
        /// Event that is fired when the device is disconnected
        /// </summary>
        public event EventHandler<EventArgs> DeviceDisconnected;

        #endregion

        //==================================================================================
        #region Private Variables

        //Device Details
        protected string devicePath;                //Holds the device Path
        protected short  vendorID;                  //Holds the vendor ID of the device
        protected short  productID;                 //Holds the product ID of the device
        protected short  productVersion;            //Holds the version attribute of the device
        protected string serialNo;                  //Holds the serial number of the device
        protected string productName;               //Holds the device's "product string" A.K.A. the Product Name
        protected string manufacturerName;          //Holds the name of the manufacturer of this device
        protected string physicalDescriptor;        //Holds the device's "Physical Descriptor"

        //Communication Details
        protected int           reportLength;       //Holds the length of incoming and outgoing reports
        protected FileStream    readStream;         //Holds the I/O stream that can be used to read data coming from the device easily
        protected bool          reading;            //Set to TRUE when we are reading from the device, otherwise FALSE
        protected ManualResetEvent waitForReport;   //Makes the asynchronous reading function pseudo-synchronous
        System.Threading.Thread reportReader;       //A thread that performs asynchronous report reading
        protected long nextReportID;                //Holds the unique timestamp for the next report that will be received. NOTE: There is a potential issue, but I'll be damned if I'm going to deal with an exception that may occur when the report queue holds 9,223,372,036,854,775,808 reports.
        
        #endregion

        //==================================================================================
        #region Constructors/Destructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public HIDDevice()
            :this("")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="DevPath">The unique path to the device</param>
        public HIDDevice(string DevPath)
        {
            SetDefaults(DevPath, 0, 0, 0, "", "", "", "",DEFAULT_REPORT_LENGTH, null);
        }

        /// <summary>
        /// Destructor for this class
        /// </summary>
        ~HIDDevice()
        {
            this.Dispose();
        }

        #endregion
        
        //==================================================================================
        #region Public Properties


        /// <summary>
        /// Gets the version number of this device, as specified by the HID devices attributes
        /// </summary>
        public short DeviceVersion                  
        {
            get { return productVersion; }
            //set { productVersion = value; }
        }

        /// <summary>
        /// Gets the Vendor ID of this device's vendor
        /// </summary>
        public short VendorID                       
        {
            get { return vendorID; }
            //set { vendorID = value; }
        }

        /// <summary>
        /// Gets the product ID of this HID device
        /// </summary>
        public short ProductID                      
        {
            get { return productID; }
            //set { productID = value; }
        }

        /// <summary>
        /// Gets the Device Path of this device
        /// </summary>
        public string DevicePath                    
        {
            get { return devicePath; }
            set { devicePath = value; }
        }

        /// <summary>
        /// Gets the serial number for this device
        /// </summary>
        public string SerialNumber                  
        {
            get
            {
                return serialNo;
            }
        }

        /// <summary>
        /// Gets the product name for this device
        /// </summary>
        public string Name                          
        {
            get { return productName; }
        }

        /// <summary>
        /// Gets the Physical Descriptor for this device
        /// </summary>
        public string PhysicalDescriptor            
        {
            get { return physicalDescriptor; }
        }

        /// <summary>
        /// Gets the manufacturer of this device
        /// </summary>
        public string Manufacturer                  
        {
            get { return manufacturerName; }
        }

        /// <summary>
        /// Gets whether or not this class is sending and/or receiving reports from the device
        /// </summary>
        public bool IsConnected                     
        {
            get
            {
                return (readStream != null && readStream.CanRead);
            }
        }

        /// <summary>
        /// Gets whether or not this instance is reading reports asynchronously from the device
        /// </summary>
        public bool IsAsyncReading                  
        {
            get { return reading; }
        }

        #endregion

        //==================================================================================
        #region Public Methods

        #region Device Property Setting

        #region Device Path

        /// <summary>
        /// Sets this devices unique path.
        /// </summary>
        /// <param name="Path">The device's unique path</param>
        public void SetDevicePath(HIDStructures.DevicePath Path)
        {
            //Set the Device Path
            devicePath = Path.path;
        }

        /// <summary>
        /// Sets this devices unique path.
        /// </summary>
        /// <param name="Path">The device's unique path</param>
        public void SetDevicePath(string Path)
        {
            //Set the Device Path
            devicePath = Path;
        }

        #endregion

        #region Device Attributes

        /// <summary>
        /// Sets the Devices attributes based on those that are returned from the device
        /// </summary>
        /// <exception cref="HIDDeviceInvalidPathException">Thrown if the path to the device is invalid</exception>
        public void SetDeviceAttributes()
        {
            Exception thrownException = null;
            if (this.devicePath != string.Empty)
            {

                //Device Attributes
                try
                {
                    HIDStructures.HIDDeviceAttributes attrs = HIDDeviceInterface.GetAttributesForDevice(this.devicePath);
                    this.vendorID = attrs.VendorID;
                    this.productID = attrs.ProductID;
                    this.productVersion = attrs.VersionNumber;
                }
                catch (Exception exc)
                {
                    if (thrownException == null) thrownException = exc;
                }


                //Device Serial Number
                try
                {
                    this.serialNo = HIDDeviceInterface.GetDeviceSerialNumber(this.devicePath);
                }
                catch (Exception exc)
                {
                    if (thrownException == null) thrownException = exc;
                }


                //Device Product String
                try
                {
                    this.productName = HIDDeviceInterface.GetDeviceProductString(this.devicePath);
                }
                catch (Exception exc)
                {
                    if (thrownException == null) thrownException = exc;
                }


                //Device Physical Descriptor
                try
                {
                    this.physicalDescriptor = HIDDeviceInterface.GetDevicePhysicalDescriptor(this.devicePath);
                }
                catch (Exception exc)
                {
                    if (thrownException == null) thrownException = exc;
                }

                //Device Manufacturer
                try
                {
                    this.manufacturerName = HIDDeviceInterface.GetDeviceManufacturerString(this.devicePath);
                }
                catch (Exception exc)
                {
                    if (thrownException == null) thrownException = exc;
                }



            }
            else
            {
                throw new HIDDeviceInvalidPathException("Device Path \"" + this.devicePath + "\" is Invalid");
            }
        }

        /// <summary>
        /// Sets the device's attributes
        /// </summary>
        /// <param name="VenID">The Vendor ID of this device</param>
        /// <param name="ProdID">The Product ID of this device</param>
        /// <param name="ProdVersion">The Product Version of this device</param>
        /// <param name="Serial">The serial number of this device</param>
        /// <remarks>It is highly recommended to use the non-parameterized version of this function, which gets the attributes from this device
        /// directly from the Operating System.</remarks>
        public void SetDeviceAttributes(short VenID, short ProdID, short ProdVersion, string Serial, string ProductString, string PhysDescriptor)
        {
            this.vendorID = VenID;
            this.productID = ProdID;
            this.productVersion = ProdVersion;
            this.productName = ProductString;
            this.physicalDescriptor = PhysDescriptor;
        }

        #endregion

        #endregion

        #region Device Communication

        /// <summary>
        /// Opens a .NET file stream so that we can communicate with the device
        /// </summary>
        /// <exception cref="HIDDeviceInvalidPathException">Thrown if the path to the device is invalid</exception>
        /// <exception cref="HIDDeviceConnectionException">Thrown if a handle to the device could not be created</exception>
        public void Connect()
        {
            //Check to see if it is impossible to connect
            if (this.devicePath == string.Empty)
            {
                throw new HIDDeviceInvalidPathException("There is no device path for this device, therefore a connection is impossible. Try again when the class is populated.");
            }

            //Create the file handle using Win32 methods
            SafeFileHandle deviceHandle = HIDLowLevelFunctions.CreateFile(this.devicePath, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, HIDStructures.FileAttributes.Overlapped, IntPtr.Zero);

            //If we still don't have a handle to the device, something is wrong
            if (deviceHandle == null)
            {
                throw new HIDDeviceConnectionException("Handle to device could not be created.");
            }

            //Initialize the communication stream
            readStream = new FileStream(deviceHandle, FileAccess.Read, reportLength, true);
        }

        /// <summary>
        /// Starts the class asynchronously reading data from the device
        /// </summary>
        public void StartReadingData(bool ConnectIfNotConnected)
        {
            //If we are already reading, we don't want to start reading again!
            if (!reading)
            {
                //Check to see if we are connected to the device
                if (!this.IsConnected)
                {
                    //Try to connect to the device if requested
                    if (ConnectIfNotConnected == true)
                    {
                        //Connect to the device
                        this.Connect();
                    }
                    else
                    {
                        throw new HIDDeviceConnectionException("Cannot read from device, device communication stream has not been connected to the device.");
                    }
                }

                //Now that we are all connected, start reading
                reading = true;
                reportReader = new System.Threading.Thread(new System.Threading.ThreadStart(AsyncReadReport));
                reportReader.Start();
            }
        }

        /// <summary>
        /// Stops this instance from reading data from the hardware
        /// </summary>
        public void StopReadingData()
        {
            reading = false;
        }

        #endregion

        #region Device Destruction

        #region IDisposable Members

        /// <summary>
        /// Disposes of this 
        /// </summary>
        public void Dispose()
        {
            Disconnect();
            SetDefaults("", 0, 0, 0,"","","","", 0, null);
        }


        #endregion

        /// <summary>
        /// Ends communications with a device
        /// </summary>
        /// <returns>TRUE if the device closed communications properly, false otherwise</returns>
        public bool Disconnect()
        {
            bool retVal;
            try
            {
                StopReadingData();
                readStream.Close();
                readStream.SafeFileHandle.Close();
                retVal = true;

                //Fire the event to let other stuff know that the device has been disconnected
                DeviceDisconnected(retVal, EventArgs.Empty);
            }
            catch
            {
                retVal = false;
            }

            return retVal;
        }

        #endregion

        #endregion

        //==================================================================================
        #region Private/Protected Methods

        #region Device Initialization


        /// <summary>
        /// Sets the values of all the variables of this class simultaneously
        /// </summary>
        /// <param name="dvPath">The Device Path of the device</param>
        /// <param name="venID">The Vendor ID of the device</param>
        /// <param name="prodID">The Product ID of the device</param>
        /// <param name="prodVer">The Version of the device</param>
        /// <param name="Serial">The serial number of the device</param>
        /// <param name="ProdName">The name of the product that is the device</param>
        /// <param name="ProdManufacturer">The name of the manufacturer of the device</param>
        /// <param name="PhysDescriptor">The physical descriptor for the device</param>
        /// <param name="ReportLength">The length of input and output reports</param>
        /// <param name="cStream">The communication stream for this device</param>
        private void SetDefaults(string dvPath, short venID, short prodID, short prodVer,
                                    string Serial, string ProdName, string ProdManufacturer,
                                    string PhysDescriptor, int ReportLength, FileStream cStream)
        {
            devicePath = dvPath;
            vendorID = venID;
            productID = prodID;
            productVersion = prodVer;
            serialNo = Serial;
            productName = ProdName;
            manufacturerName = ProdManufacturer;
            physicalDescriptor = PhysDescriptor;
            reportLength = ReportLength;
            readStream = cStream;
        }


        #endregion

        #region Device Communication

        /// <summary>
        /// Sends a report to the HID Device
        /// </summary>
        /// <param name="Report">The report to send</param>
        protected void SendReport(HIDDeviceReport Report)   
        {
            SendReport(Report.ToRaw());
        }
        /// <summary>
        /// Sends a report to the HID Device
        /// </summary>
        /// <param name="Report">The raw report to send</param>
        /// <param name="Length">The length of the report</param>
        protected void SendReport(byte[] Report)            
        {
            //If we cannot write to the stream, the stream may not be instantiated, therefore we must instantiate the stream
            if (!IsConnected)
            {
                throw new HIDDeviceConnectionException("The device is not connected");
            }

            //Put the report into an array that is of the size that the stream is expecting.
            //For example, if the report is only 10 bytes long, but the stream is expecting
            //a 22 byte array to send, then we must pad the array. Therefore an array is
            //created that is the expected size, and the report copied into it.
            byte[] readyReport = new byte[reportLength];
            Array.Copy(Report, 0, readyReport, 0, Report.Length);

            //Send the report
            /*HACK ALERT:   Note that we use the file handle to the read stream here.
             *              This is allowed because the handle itself is a Read/Write
             *              handle, even though the stream is Read-only. This saves
             *              us having to store the handle separately.
             */
            HIDDeviceInterface.SendReport(readStream.SafeFileHandle, readyReport);

            

        }

        /// <summary>
        /// Starts reading reports from the device synchronously.
        /// </summary>
        /// <remarks>It is best to run this function as a thread, which means that reports will be read asynchronously, which is why the "ReportReceived" event exists</remarks>
        protected void AsyncReadReport()                    
        {
            byte[] report = null;       //The report received

            //Start reading reports until requested to stop
            if (reading)
            {
                //Setup the report and read one in
                report = new byte[reportLength];

                //Grab the next report
                readStream.BeginRead(report, 0, reportLength, new AsyncCallback(ProcessReport), report);

            }
            
        }

        /// <summary>
        /// When a report is received, this function resets the ManualResetEvent, which 
        /// allows the report reader to continue processing.
        /// </summary>
        protected void ProcessReport(IAsyncResult OperationResult)
        {
            //Create the report
            HIDDeviceReport report = new HIDDeviceReport(GetNextUniqueReportID(), (byte[])OperationResult.AsyncState);

            //Stop the asynchronous read method from reading attempting any more readage
            /* NOTE:    Notice that the raising of the "ReportReceived" event is inside this try block.
             *          This is because the report event should NOT fire if the stream reading fails.
             */
            try
            {
                readStream.EndRead(OperationResult);

                //Raise the event that we have read a report
                if (ReportReceived != null) ReportReceived(readStream, new HIDDeviceReportReceivedEventargs(report));
            }
            //Catch "OperationCanceledException"s because this usually means that the wiimote was disconnected.
            catch (OperationCanceledException)
            {
                StopReadingData();
            }
            catch (IOException)
            {
                Disconnect();
            }

            //Start another asynchronous read
            AsyncReadReport();
        }

        /// <summary>
        /// Fetches the next unique report ID for this device to use in a report, and increments the device report ID
        /// </summary>
        /// <returns>A unique device report ID</returns>
        protected long GetNextUniqueReportID()
        {
            nextReportID++;
            return nextReportID - 1;
        }

        #endregion

        #endregion

    }
}
