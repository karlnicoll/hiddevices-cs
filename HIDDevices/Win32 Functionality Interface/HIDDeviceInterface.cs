using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace HIDDevices
{

    /// <summary>
    /// A Sealed class containing a variety of functions for getting details about HID Devices
    /// </summary>
    public sealed class HIDDeviceInterface
    {
        //==================================================================================
        #region Enumerations


        #endregion


        //==================================================================================
        #region Constants


        #endregion


        //==================================================================================
        #region Private Variables

        
        #endregion


        //==================================================================================
        #region Constructors
        

        #endregion
        
        //==================================================================================
        #region Public Properties

        /// <summary>
        /// Gets the HID Device Interface Class GUID
        /// </summary>
        public static Guid HIDDeviceClassGUID 
        {
            get
            {
                Guid returnedValue = Guid.Empty;
                HIDLowLevelFunctions.HidD_GetHidGuid(out returnedValue);
                return returnedValue;
            }
        }

        #endregion


        //==================================================================================
        #region Public Methods

        /// <summary>
        /// Gets an HIDDevice Class that represents a device that has been identified by its Device Instance Handle
        /// </summary>
        /// <param name="DevInstHandle">A handle to the Devnode for the device</param>
        /// <returns>The HIDDevice class that represents the device that was identified.</returns>
        public static HIDDevice GetDeviceFromDevInstHandle(uint DevInstHandle)  
        {
            //Set up the class that will be returned from this function
            HIDDevice returnedValue = new HIDDevice();

            //Structures that hold the data that will be contained within the class
            HIDStructures.DeviceInfo devInfo = new HIDStructures.DeviceInfo();
            HIDStructures.DeviceInterfaceInformation[] devInterfaces = null;
            string devicePath = string.Empty;
            HIDStructures.HIDDeviceAttributes devAttr = new HIDStructures.HIDDeviceAttributes();
            Exception thrownException = null;


            //Set up the Device Information Set (DIS)
            //=======================================
            
            //Set up the flags
            uint flags = (uint)HIDStructures.DIGCF.DEVICEINTERFACE;

            //Get the pointer to the DIS
            IntPtr deviceInformationSetPointer = GetDeviceInformationSet(HIDDeviceClassGUID, flags);

            try
            {
                //Get the device information
                devInfo = GetDeviceInfoFromDevnodeHandle(deviceInformationSetPointer, DevInstHandle);

                //Get the interfaces exposed by this device
                devInterfaces = GetDeviceInterfaces(deviceInformationSetPointer, devInfo);

                //Get the device path for this device by looking at the details of each interface exposed by the device
                int index = 0;
                while (devicePath == string.Empty && index < devInterfaces.Length)
                {
                    try
                    {
                        devicePath = GetDevicePathFromInterface(deviceInformationSetPointer, devInterfaces[index]);
                    }
                    catch { }

                    index++;
                }

                //If the device path is an empty string, then something has bodged, and we need to tell the user/developer
                if (devicePath == string.Empty)
                {
                    throw new Exception("Device Path could not be fetched for this device");
                }

                //Get the device's HID attributes
                devAttr = GetAttributesForDevice(devicePath);

                //Finally, put all the acquired information into the returned HIDDevice Class
                returnedValue.SetDevicePath(devicePath);
            }
            catch (Exception exc)
            {
                thrownException = exc;
            }
            finally
            {
                //Delete the device information set so as to prevent memory leaks!
                DeleteDeviceInformationSet(deviceInformationSetPointer);
            }

            //If an error exists, throw the exception (which we saved until now so that we can clean up the DIS)
            if (thrownException != null)
            {
                throw thrownException;
            }

            return returnedValue;
        }

        /// <summary>
        /// Gets all the available HID Devices
        /// </summary>
        /// <param name="PresentOnly">Return only devices actually present in the system at this time</param>
        /// <param name="CurrentProfileOnly">Return devices associated with this hardware profile only</param>
        /// <returns>An Collection of HID Devices matching the criteria specified</returns>
        public static HIDDeviceCollection GetAllHIDDevices(bool PresentOnly, bool CurrentProfileOnly)
        {
            //Declare the device collection that we'll be returning
            HIDDeviceCollection devices = new HIDDeviceCollection();

            //Declare the variables that each devices raw data will be held in
            HIDStructures.DeviceInterfaceInformation[] deviceInterfaces;
            HIDStructures.DeviceInfo[] devicesInfo = null;
            string devicePath;

            //Holds the exception that was raised by this function so that it can be thrown after cleanup
            Exception thrownException = null;

            //Declare loop iterators.
            int deviceInterfaceIndex;        //Holds which device path we are looking at

            //Setup the flags to specify which devices to get, and then get the
            //Device Information Set
            uint flags = 0;
            if(PresentOnly) flags = flags | (uint)HIDStructures.DIGCF.PRESENT;
            if(CurrentProfileOnly) flags = flags | (uint)HIDStructures.DIGCF.PROFILE;
            IntPtr infoSet = GetDeviceInformationSet(HIDDeviceClassGUID, flags);


            //Now that we have all the device information we need, loop through
            //each device, and use the interfaces for each device to obtain the
            //details about that device
            try
            {

                //Get all the devices available to us
                devicesInfo = GetDevicesFromInformationSet(infoSet);

                //Only get interfaces etc if any devices were found
                if (devicesInfo != null)
                {
                    //Loop through the devices, and get the interfaces for each one,
                    //then, get the device path and attributes for the device using
                    //the interfaces discovered
                    foreach (HIDStructures.DeviceInfo curDeviceInfo in devicesInfo)
                    {
                        //Reset all device data-holding structures
                        devicePath = string.Empty;
                        deviceInterfaces = null;
                        deviceInterfaceIndex = 0;

                        //Get the interfaces for this device
                        deviceInterfaces = GetDeviceInterfaces(infoSet, curDeviceInfo);

                        //Attempt to get the available device paths for this device from its interfaces
                        if (deviceInterfaces != null)
                        {

                            //Get the device path
                            while (deviceInterfaceIndex < deviceInterfaces.Length && devicePath == string.Empty)
                            {
                                devicePath = GetDevicePathFromInterface(infoSet, deviceInterfaces[deviceInterfaceIndex]);
                                deviceInterfaceIndex++;
                            }
                        }
                        else
                        {
                            //If the device returns no available interfaces, then something has gone wrong, and we must throw an exception.
                            //NOTE: Devices must expose at least 1 interface
                            throw new NullReferenceException("No Device Interface Elements were returned for this device");
                        }


                        //Setup the device details
                        HIDDevice newDevice = new HIDDevice();
                        newDevice.SetDevicePath(devicePath);

                        //Add the assembled device to the list of devices!
                        devices.Add(newDevice);
                    }
                }
            }
            catch (Exception exc)
            {
                thrownException = exc;
            }
            finally
            {
                //Delete the device information set (to prevent memory leaks)
                DeleteDeviceInformationSet(infoSet);
            }

            //If an exception was encountered, throw the exception, otherwise return the list of devices
            if (thrownException != null)
            {
                throw thrownException;
            }
            else
            {
                return devices;
            }

        }

        #region HID Device Manipulation Functions

        /// <summary>
        /// Gets the Product ID, Vendor ID and Version number for the device at the end of the specified device Path
        /// </summary>
        /// <param name="devicePath">The Path to the device</param>
        /// <returns>An HIDDeviceAttributes structure that contains the requested details</returns>
        public static HIDStructures.HIDDeviceAttributes GetAttributesForDevice(string DevicePath)
        {
            //Create a file handle (for communication with the device)
            SafeFileHandle deviceCommHandle = HIDLowLevelFunctions.CreateFile(DevicePath, FileAccess.Read, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, HIDStructures.FileAttributes.Overlapped, IntPtr.Zero);

            //Create the attributes structure that will be returned and set it's size
            HIDStructures.HIDDeviceAttributes attributes = new HIDStructures.HIDDeviceAttributes();
            attributes.Size = Marshal.SizeOf(attributes);

            //Attempt to populate the attributes structure
            bool operationSuccess;

            //Get the attributes
            operationSuccess = HIDLowLevelFunctions.HidD_GetAttributes(deviceCommHandle.DangerousGetHandle(), ref attributes);

            deviceCommHandle.Dispose();

            //If the operation failed, then throw an exception, unless the device could not be found, in which case the device attributes should just return their default values
            if (!operationSuccess && Marshal.GetLastWin32Error() != HIDErrorCodes.ERROR_NO_MORE_ITEMS)
            {
                throw new Exception("Attributes could not be obtained for the specified device. Error Code: " + Marshal.GetLastWin32Error() + ".");
            }

            //If the operation was successful, then return the attributes structure
            return attributes;
        }

        /// <summary>
        /// Gets the serial number for a device
        /// </summary>
        /// <param name="DevicePath">The Device path for the device to get the serial number of</param>
        /// <returns>A string containing the serial number of the device</returns>
        public static string GetDeviceSerialNumber(string DevicePath)
        {
            string returnedResult = string.Empty;

            //A pointer to some unmanaged memory that will hold the serial number for the device we are getting data for
            IntPtr serialNoBuffer = Marshal.AllocHGlobal(256);

            //Create a file handle (for communication with the device)
            SafeFileHandle deviceCommHandle = HIDLowLevelFunctions.CreateFile(DevicePath, FileAccess.Read, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, HIDStructures.FileAttributes.Overlapped, IntPtr.Zero);

            //bool result = HIDLowLevelFunctions.HidD_GetSerialNumberString(deviceCommHandle.DangerousGetHandle(), serialNoBuffer, 127);
            if (HIDLowLevelFunctions.HidD_GetSerialNumberString(deviceCommHandle.DangerousGetHandle(), serialNoBuffer, 127))
            {
                returnedResult = Marshal.PtrToStringAuto(serialNoBuffer);
            }
            else
            {
                throw new Exception("Unable to obtain serial number. Error code: " + Marshal.GetLastWin32Error().ToString());
            }

            return returnedResult;

        }

        /// <summary>
        /// Gets the product string for the specified HID device
        /// </summary>
        /// <param name="DevicePath">The path to the Device</param>
        /// <returns>The Product string for the device</returns>
        public static string GetDeviceProductString(string DevicePath)
        {
            string returnedResult = string.Empty;

            //A pointer to some unmanaged memory that will hold the product string for the device we are getting data for
            IntPtr ProductStringBuffer = Marshal.AllocHGlobal(256);

            //Create a file handle (for communication with the device)
            SafeFileHandle deviceCommHandle = HIDLowLevelFunctions.CreateFile(DevicePath, FileAccess.Read, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, HIDStructures.FileAttributes.Overlapped, IntPtr.Zero);

            //bool result = HIDLowLevelFunctions.HidD_GetSerialNumberString(deviceCommHandle.DangerousGetHandle(), serialNoBuffer, 127);
            if (HIDLowLevelFunctions.HidD_GetProductString(deviceCommHandle.DangerousGetHandle(), ProductStringBuffer, 127))
            {
                returnedResult = Marshal.PtrToStringAuto(ProductStringBuffer);
            }
            else
            {
                throw new Exception("Unable to obtain Product String. Error code: " + Marshal.GetLastWin32Error().ToString());
            }

            return returnedResult;
        }

        /// <summary>
        /// Gets the physical descriptor for the specified device
        /// </summary>
        /// <param name="DevicePath">A Path to the device to retrieve the descriptor for</param>
        /// <returns>The physical descriptor for this device</returns>
        public static string GetDevicePhysicalDescriptor(string DevicePath)
        {
            string returnedResult = string.Empty;

            //A pointer to some unmanaged memory that will hold the descriptor for the device we are getting data for
            IntPtr descriptorBuffer = Marshal.AllocHGlobal(2048);

            //Create a file handle (for communication with the device)
            SafeFileHandle deviceCommHandle = HIDLowLevelFunctions.CreateFile(DevicePath, FileAccess.Read, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, HIDStructures.FileAttributes.Overlapped, IntPtr.Zero);

            //bool result = HIDLowLevelFunctions.HidD_GetSerialNumberString(deviceCommHandle.DangerousGetHandle(), serialNoBuffer, 127);
            if (HIDLowLevelFunctions.HidD_GetPhysicalDescriptor(deviceCommHandle.DangerousGetHandle(), descriptorBuffer, 1024))
            {
                returnedResult = Marshal.PtrToStringAuto(descriptorBuffer);
            }
            else
            {
                throw new Exception("Unable to obtain Physical Descriptor for this device. Error code: " + Marshal.GetLastWin32Error().ToString());
            }

            return returnedResult;
        }

        /// <summary>
        /// Fetches the manufacturer string for this device
        /// </summary>
        /// <param name="DevicePath">The path the the device to retrieve the manufacturer for</param>
        /// <returns>The manufacturer of this device</returns>
        public static string GetDeviceManufacturerString(string DevicePath)
        {
            string returnedResult = string.Empty;

            //A pointer to some unmanaged memory that will hold the descriptor for the device we are getting data for
            IntPtr descriptorBuffer = Marshal.AllocHGlobal(256);

            //Create a file handle (for communication with the device)
            SafeFileHandle deviceCommHandle = HIDLowLevelFunctions.CreateFile(DevicePath, FileAccess.Read, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, HIDStructures.FileAttributes.Overlapped, IntPtr.Zero);

            //bool result = HIDLowLevelFunctions.HidD_GetSerialNumberString(deviceCommHandle.DangerousGetHandle(), serialNoBuffer, 127);
            if (HIDLowLevelFunctions.HidD_GetManufacturerString(deviceCommHandle.DangerousGetHandle(), descriptorBuffer, 127))
            {
                returnedResult = Marshal.PtrToStringAuto(descriptorBuffer);
            }
            else
            {
                throw new Exception("Unable to obtain Manufacturer String for this device. Error code: " + Marshal.GetLastWin32Error().ToString());
            }

            return returnedResult;
        }

        #endregion

        #region HID device reporting methods

        /// <summary>
        /// Sends a report to the the device given by it's device handle
        /// </summary>
        /// <param name="DeviceHandle">The handle to the device that should receive the report</param>
        /// <param name="Report">The report to send</param>
        public static void SendReport(SafeFileHandle DeviceHandle, byte[] Report)  
        {
            //Try to Send the report using the Win32 SetOutputReport method
            if (!HIDLowLevelFunctions.HidD_SetOutputReport(DeviceHandle.DangerousGetHandle(), Report, (uint)Report.Length))
            {
                throw new HIDDeviceReportSendingException("Unable to send the report. Error Code: " + Marshal.GetLastWin32Error().ToString(), Marshal.GetLastWin32Error());
            }
        }

        #endregion


        #endregion


        //==================================================================================
        #region Private/Protected Methods

        #region Device Information Set Manipulation

        /// <summary>
        /// Gets a pointer to the device information set for a specified Class of devices
        /// </summary>
        /// <param name="DeviceInterfaceClass">The GUID of the device interface class that you want to get devices for</param>
        /// <returns>A Pointer to the produces Device Information Set</returns>
        private static IntPtr GetDeviceInformationSet(Guid DeviceInterfaceClass, uint Flags)        
        {
            Flags = Flags | (uint)HIDStructures.DIGCF.DEVICEINTERFACE;
            return HIDLowLevelFunctions.SetupDiGetClassDevs(ref DeviceInterfaceClass, null, IntPtr.Zero, Flags);
        }

        /// <summary>
        /// Deletes the Device Information Set from unmanaged memory
        /// </summary>
        /// <param name="DeviceInformationSet">A Pointer to the device information set to delete</param>
        private static void DeleteDeviceInformationSet(IntPtr DeviceInformationSet)                 
        {
            bool operationSuccess = HIDLowLevelFunctions.SetupDiDestroyDeviceInfoList(DeviceInformationSet);

            if (!operationSuccess)
            {
                throw new Exception("Unable to Delete the Device Information Set. Error Code: " + Marshal.GetLastWin32Error());
            }
        }

        #endregion

        #region Device Information Functions

        /// <summary>
        /// Fetches all the devices that are listed in a given Device Information Set
        /// </summary>
        /// <param name="infoSet">A pointer to a generated Device Information Set</param>
        /// <returns>A List of all the available devices in the information set, or null if no devices are available</returns>
        private static HIDStructures.DeviceInfo[] GetDevicesFromInformationSet(IntPtr infoSet)                                              
        {
            //Linked list to hold all the found devices
            LinkedList<HIDStructures.DeviceInfo> devicesInfo = new LinkedList<HIDStructures.DeviceInfo>();

            //Iterator to control which device we are fetching from the device information set
            int deviceIndex = 0;

            //Array in which the devices will be returned
            HIDStructures.DeviceInfo[] returnedDevices;

            //Get all the devices available to us
            //NOTE: This loop is "infinite" but in a Try...catch loop. This is because
            //      there is no way of telling how many devices there are in the DIS.
            //      Therefore, when we go beyond the last device in the DIS, an exception
            //      will be raised. This means that although the loop _appears_ infinite,
            //      it is in-fact not.
            try
            {
                while (true)
                {
                    //Get the device information for all the devices
                    devicesInfo.AddLast(GetDeviceInfo(infoSet, deviceIndex));
                    deviceIndex++;
                }
            }
            catch
            { }

            //If we found devices, return an array of the devices, otherwise return a NULL array
            if (devicesInfo.Count > 0)
            {
                //Set the array size to be the same size as the amount of devices in the list
                returnedDevices = new HIDStructures.DeviceInfo[devicesInfo.Count];

                //Put the devices into the array
                deviceIndex = 0;
                foreach (HIDStructures.DeviceInfo curDevice in devicesInfo)
                {
                    returnedDevices[deviceIndex] = curDevice;
                    deviceIndex++;
                }
            }
            else
            {
                returnedDevices = null;
            }

            //Return the array (or null)
            return returnedDevices;

        }

        /// <summary>
        /// Retrieves the device information for a device located at the specified index in the device information set
        /// </summary>
        /// <param name="InformationSet">The information set to retrieve device information from</param>
        /// <param name="MemberIndex">The zero-based index of the device information element to retrieve</param>
        /// <returns>A structure containing the retrieved information</returns>
        private static HIDStructures.DeviceInfo GetDeviceInfo(IntPtr InformationSet, int MemberIndex)                                       
        {
            //Initialize the structure we will return
            HIDStructures.DeviceInfo returnedValue = new HIDStructures.DeviceInfo();
            returnedValue.cbSize = (uint)Marshal.SizeOf(returnedValue.GetType());

            bool operationSuccessful = HIDLowLevelFunctions.SetupDiEnumDeviceInfo(InformationSet, MemberIndex, ref returnedValue);
            
            //If the operation failed, we want to find out why!
            if (!operationSuccessful)
            {
                //If the Member index was invalid, we want to throw an IndexOutOfRangeException exception class
                int errorCode = Marshal.GetLastWin32Error();
                if (errorCode == HIDErrorCodes.ERROR_NO_MORE_ITEMS)
                {
                    throw new IndexOutOfRangeException("The MemberIndex is out of the range of the set of DIEs (Device Information Elements)");
                }
                else
                {
                    throw new Exception("HIDStructures.DeviceInfo SetupDiEnumDeviceInfo(" + InformationSet.ToString() + ", " + MemberIndex.ToString() + ") Failed with error code: " + errorCode.ToString());
                }
            }

            
            return returnedValue;
        }

        /// <summary>
        /// Fetches the DeviceInfo Structure for a specified device that is identified by it's Devnode handle
        /// </summary>
        /// <param name="InformationSet">The information set to retrieve the Device information from</param>
        /// <param name="DevNode">The Handle to the devnode to seek out</param>
        /// <returns>The matching DeviceInfo structure for the device specified by its Devnode</returns>
        private static HIDStructures.DeviceInfo GetDeviceInfoFromDevnodeHandle(IntPtr InformationSet, uint DevNode)                         
        {
            //Required Variables
            bool itemFound = false;
            bool lastFetchFailed = false;
            int index = 0;
            Exception err = null;
            HIDStructures.DeviceInfo curDeviceInfo = new HIDStructures.DeviceInfo();

            //Loop through the Device Information Set in an attempt to get the device details
            //===============================================================================
            while (!itemFound && !lastFetchFailed)
            {
                try
                {
                    //Get the device Information Element at the specified index
                    curDeviceInfo = GetDeviceInfo(InformationSet, index);

                    //If we found the item, set itemFound to be true, and break out of the loop,
                    //otherwise increment index and continue looking. That is until we run out 
                    //of items to look at (which will be true (normally) when an exception is
                    //thrown by the GetDeviceInfo() function
                    if (curDeviceInfo.DevInst == DevNode)
                    {
                        itemFound = true;
                    }
                    else
                    {
                        index++;
                    }
                }
                catch(Exception exc)
                {
                    err = exc;
                    lastFetchFailed = true;
                }
            }


            //If the item was not located, then we need to throw an exception here to let the
            //user know that we have failed
            if (!itemFound || lastFetchFailed)
            {
                if (err == null)
                {
                    err = new Exception("Could not locate device specified by parameter \"DevNode\". Has the device been uninstalled from this system?");
                }

                throw err;
            }

            //return the last device information we found. This point in the function is only
            //reached if the devnode we are looking for was found. If it was not, then the
            //exception will be thrown on the lines above, and this line below will never
            //be executed
            return curDeviceInfo;
        }

        #endregion

        #region Device Interface Functions

        /// <summary>
        /// Gets all the device interfaces exposed by a particular device. Note that this function will return NULL if no interfaces can be found
        /// </summary>
        /// <param name="InformationSet">A Pointer to the Device Information Set from which we will obtain the interface data</param>
        /// <param name="DeviceInfoClassGUID">The Global Unique Identifier of the Device interface class for which the device interfaces will be obtained</param>
        /// <param name="Device">The DeviceInfo class containing the known information about the class. Set to NULL to return all interfaces to all devices in the specified Device Information Class</param>
        /// <returns>An array containing all the discovered interfaces, or NULL if no interfaces were found</returns>
        private static HIDStructures.DeviceInterfaceInformation[] GetDeviceInterfaces(IntPtr InformationSet, System.Nullable<HIDStructures.DeviceInfo> Device)
        {
            HIDStructures.DeviceInterfaceInformation[] returnedInterfaces = null;
            HIDStructures.DeviceInterfaceInformation currentInterface = new HIDStructures.DeviceInterfaceInformation();
            LinkedList<HIDStructures.DeviceInterfaceInformation> interfaceList = new LinkedList<HIDStructures.DeviceInterfaceInformation>();
            Exception thrownException = null;       //Used to hold any exceptions that are generated in this function. Any exceptions are suppressed until the end of the function so that memory cleanup can occur if necessary.
            IntPtr deviceInfoPointer = IntPtr.Zero; //A Pointer to some unmanaged memory so that we can send the device information to the low level (i.e. P/Invoke'd) functions

            //A Guid Class to hold the GUID of the HID Device Interface Class
            Guid hidClass = HIDDeviceClassGUID;

            
            //This try...catch...finally block is important to prevent memory leaks with the unmanaged memory. Any
            //exceptions that get raised in this try block are repeated later on in the code once all cleanup has occured
            try
            {
                //Change the device info to an IntPtr class so that it can be passed to the low level function.
                //NOTE: We check here to see if the device is null, if so, then we can pass IntPtr.Zero to the
                //function to retrieve all device interfaces found in the specific Device Information Class.
                if (Device != null)
                {
                    //Allocate some unmanaged memory to allow the P/Invoked function(s) to access the Device
                    //information
                    deviceInfoPointer = Marshal.AllocHGlobal(Marshal.SizeOf(Device));
                    Marshal.StructureToPtr(Device, deviceInfoPointer, true);
                }

                //Get a list of all the available interfaces for this device
                //==========================================================

                //Set the size of the interface information structure
                currentInterface.cbSize = Marshal.SizeOf(currentInterface.GetType());

                //Loop through the Device information set getting all the interfaces for this device
                int index = 0;
                bool lastAttemptFailed = false;
                lastAttemptFailed = !HIDLowLevelFunctions.SetupDiEnumDeviceInterfaces(InformationSet, deviceInfoPointer, ref hidClass, index, ref currentInterface);

                while (!lastAttemptFailed)
                {
                    //Add the interface to the list of interfaces found
                    interfaceList.AddLast(currentInterface);

                    //Increment the member index and get the next available interface
                    index++;
                    lastAttemptFailed = !HIDLowLevelFunctions.SetupDiEnumDeviceInterfaces(InformationSet, deviceInfoPointer, ref hidClass, index, ref currentInterface);
                }
            }
            catch (Exception exc)
            {
                //Store the exception that occured so that it can be thrown later on
                thrownException = exc;
            }
            finally
            {
                //Clean up the unmanaged memory
                Marshal.FreeHGlobal(deviceInfoPointer);
            }

            //If the exception was something other than going beyond the bounds of the array, create an exception to let the dev/user know
            //Note, this exception is only created if the thrownException variable is null. This is because any exceptions raised before
            //we got to the end of the list 
            if (Marshal.GetLastWin32Error() != HIDErrorCodes.ERROR_NO_MORE_ITEMS && thrownException == null)
            {
                thrownException = new Exception("HIDStructures.DeviceInfo SetupDiEnumDeviceInterfaces(" + InformationSet.ToString() + ", " + Device.ToString() +") Failed with error code: " + Marshal.GetLastWin32Error().ToString());
            }

            //Throw any exception that has been raised!
            if (thrownException != null)
            {
                throw thrownException;
            }

            //Finally, convert the linked list to an array to return to the caller.
            if (interfaceList.Count > 0)
            {
                returnedInterfaces = new HIDStructures.DeviceInterfaceInformation[interfaceList.Count];
                LinkedListNode<HIDStructures.DeviceInterfaceInformation> curNode = interfaceList.First;
                int index = 0;
                while (curNode != null)
                {
                    returnedInterfaces[index] = curNode.Value;
                    curNode = curNode.Next;
                    index++;
                }
            }

            return returnedInterfaces;

        }

        /// <summary>
        /// Retrieves the device path via a given interface
        /// </summary>
        /// <param name="InformationSet">The device information set from which device and interface details are located</param>
        /// <param name="InterfaceInfo">The Interface Information that specified which interface the device path should be retrieved from</param>
        /// <returns>An HIDStructures.DevicePath class containing the path to the device</returns>
        private static string GetDevicePathFromInterface(IntPtr InformationSet, HIDStructures.DeviceInterfaceInformation InterfaceInfo)     
        {
            HIDStructures.DevicePath pathStruct = GetDevicePathStructureFromInterface(InformationSet, InterfaceInfo);
            return pathStruct.path;
        }

        /// <summary>
        /// Retrieves the device path via a given interface
        /// </summary>
        /// <param name="InformationSet">The device information set from which device and interface details are located</param>
        /// <param name="InterfaceInfo">The Interface Information that specified which interface the device path should be retrieved from</param>
        /// <returns>An HIDStructures.DevicePath class containing the path to the device</returns>
        private static HIDStructures.DevicePath GetDevicePathStructureFromInterface(IntPtr InformationSet, HIDStructures.DeviceInterfaceInformation InterfaceInfo)
        {
            HIDStructures.DevicePath devPath = new HIDStructures.DevicePath();
            bool operationSuccessful;

            /* Set the cbSize property to be the marshalled size of the structure.
             * NOTE:    .NET doesn't deal very well with structure packing, so we must take this into
             *          account when we're setting this cbSize property. In x64 Windows, all structures
             *          are packed with a pack size of 8 (i.e. the size of the structure will be rounded
             *          up to the nearest 8 byte boundary. For example, if a structure occupies 35
             *          bytes in memory, it would actually occupy 40 bytes in memory, because 35 does
             *          not divide by 8, but 40 does. In the case of the DevicePath structure, this
             *          structure (minus the variable string length, which is what the P/Invoke'd function
             *          wants) occupies 6 bytes of memory (4-byte integer and a 2-byte character for the
             *          null terminator) in Windows x64, therefore the size of the structure is 8. However
             *          in x86 Windows (i.e. Windows 32-bit), the pack size is 1 (as opposed to 8 for
             *          Windows 64-bit), and obviously 5 (Be aware that the character size by default in
             *          32-bit Windows is 1-byte ASCII, therefore the size is one less) divides by 1
             *          without remainder. Therefore, in Windows 32-bit, the size is 5, and in Windows
             *          64-bit, this size is 8). Because .NET does not take this into account for
             *          marshalled structures, we must set the size manually. Far from ideal, but it is 
             *          necessary.
             * NOTE:    This is probably the longest comment I've ever written :-P
             */
            devPath.cbSize = (uint)(IntPtr.Size == 8 ? 8 : 5);

            /* Get the required size of the device path string. This is done by calling the
             * SetupDiGetDeviceInterfaceDetail function with the returned value structure set to null
             * and the "size" variable set to the size variable in this code. This allows us to obtain
             * the length of the device path, so that the function can accurately occupy the string with
             * the device path. This is odd at first sight because it looks like we're calling the same
             * function twice, and we are, however because the parameter values are different, the
             * function performs a different task each time. For a better explanation of what is going on
             * here, refer to: http://msdn.microsoft.com/en-us/library/ms792901.aspx
             */
            uint requiredStructSize = 0;
            operationSuccessful = HIDLowLevelFunctions.SetupDiGetDeviceInterfaceDetail(InformationSet, ref InterfaceInfo, IntPtr.Zero, 0, out requiredStructSize, IntPtr.Zero);

            //Throw an exception if we encounter an error
            if (!operationSuccessful && Marshal.GetLastWin32Error() != HIDErrorCodes.ERROR_INSUFFICIENT_BUFFER)
            {
                throw new Exception("Could not get the size of the device path buffer. See inner exception for Win32 Error Code", new Exception("SetupDiGetDeviceInterfaceDetail(" + InformationSet.ToString() + ", ref " + InterfaceInfo.ToString() + ", IntPtr.Zero, 0, out " + requiredStructSize.ToString() + ", IntPtr.Zero) Failed with Error Code: " + Marshal.GetLastWin32Error().ToString()));
            }


            //Now that we have the buffer size, then we call the function again to get the device path
            operationSuccessful = HIDLowLevelFunctions.SetupDiGetDeviceInterfaceDetail(InformationSet, ref InterfaceInfo, ref devPath, requiredStructSize, out requiredStructSize, IntPtr.Zero);

            //Again, throw an exception if we encounter an error
            if (!operationSuccessful)
            {
                throw new Exception("Could not get the device path. See inner exception for Win32 Error Code", new Exception("SetupDiGetDeviceInterfaceDetail(" + InformationSet.ToString() + ", ref " + InterfaceInfo.ToString() + ", IntPtr.Zero, 0, out " + requiredStructSize.ToString() + ", IntPtr.Zero) Failed with Error Code: " + Marshal.GetLastWin32Error().ToString()));
            }

            //If we reach here, nothing went wrong and we can return the device path
            return devPath;


        }

        #endregion



        #endregion

    }
}
