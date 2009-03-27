/*==================================================================================*
 * Project:             HIDDevices                                                  *
 * Project Description: A class library that allows easy communication between .NET *
 *                      applications and HID devices                                *
 * Date:                15th December 2008                                          *
 * Author:              Karl Nicoll                                                 *
 * Licence:             Pending                                                     *
 * File:                HIDImportFunctions.cs                                       *
 * File Description:    Holds the P/Invoke syntax for functions that have been      *
 *                      imported from files such as hid.dll and setupapi.dll. These *
 *                      functions are used to Get device paths for the HID devices  *
 *==================================================================================*/

using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace HIDDevices
{

    /// <summary>
    /// A Static Class containing all the HID functions that are necessary for WiiMote communication.
    /// These functions do not exist in the .NET framework, therefore they have to be accessed using
    /// the "Platform Invoke" or P/Invoke service provided in the .NET framework
    /// </summary>
    internal class HIDLowLevelFunctions
    {

        #region HID Device Functions

        /// <summary>
        /// Gets the Device Interface Class GUID of the HID class
        /// </summary>
        /// <param name="gHid">An output parameter that contains the GUID of the HID Device Interface Class</param>
        [DllImport(@"hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern void HidD_GetHidGuid(out Guid gHid);

        /// <summary>
        /// Gets vendor attributes about an HID Device object
        /// </summary>
        /// <param name="HidDeviceObject">An open Handle to the HID Device</param>
        /// <param name="Attributes">A Reference to an HIDD_ATTRIBUTES data structure that (when the function ends) will contain Vendor data about the device</param>
        /// <returns>TRUE if the function completed without error, FALSE otherwise</returns>
        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool HidD_GetAttributes(IntPtr HidDeviceObject, ref HIDStructures.HIDDeviceAttributes Attributes);

        /// <summary>
        /// Sends a Report to the HID Device
        /// </summary>
        /// <param name="HidDeviceObject">An open handle to the device that should receive the report</param>
        /// <param name="lpReportBuffer">The contents of the report</param>
        /// <param name="ReportBufferLength">The Length of the report (in bytes)</param>
        /// <returns>TRUE if the operation was successful, FALSE otherwise</returns>
        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal extern static bool HidD_SetOutputReport(IntPtr HidDeviceObject, byte[] lpReportBuffer, uint ReportBufferLength);

        /// <summary>
        /// Fetches the serial for a specified HID Device
        /// </summary>
        /// <param name="HIDDeviceObject">A Pointer to the an open handle to the Device</param>
        /// <param name="buffer">An unmanaged memory pointer to a block of memory that will hold the serial number</param>
        /// <param name="SerialNoBufferLength">The length of "buffer"</param>
        /// <returns>TRUE if the operation succeeded, FALSE otherwise</returns>
        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal extern static bool HidD_GetSerialNumberString(IntPtr HIDDeviceObject, IntPtr Buffer, uint SerialNoBufferLength);

        /// <summary>
        /// Fetches the product string for a specified HID Device
        /// </summary>
        /// <param name="HIDDeviceObject">A Pointer to the an open handle to the Device</param>
        /// <param name="buffer">An unmanaged memory pointer to a block of memory that will hold the product string</param>
        /// <param name="SerialNoBufferLength">The length of "buffer"</param>
        /// <returns>TRUE if the operation succeeded, FALSE otherwise</returns>
        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal extern static bool HidD_GetProductString(IntPtr HIDDeviceObject, IntPtr Buffer, uint ProductStrBufferLength);

        /// <summary>
        /// Fetches the product string for a specified HID Device
        /// </summary>
        /// <param name="HIDDeviceObject">A Pointer to the an open handle to the Device</param>
        /// <param name="buffer">An unmanaged memory pointer to a block of memory that will hold the product string</param>
        /// <param name="SerialNoBufferLength">The length of "buffer"</param>
        /// <returns>TRUE if the operation succeeded, FALSE otherwise</returns>
        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal extern static bool HidD_GetManufacturerString(IntPtr HIDDeviceObject, IntPtr Buffer, uint ManufacturerStrBufferLength);

        /// <summary>
        /// Fetches the physical descriptor for a specified HID Device
        /// </summary>
        /// <param name="HIDDeviceObject">A Pointer to the an open handle to the Device</param>
        /// <param name="buffer">An unmanaged memory pointer to a block of memory that will hold the physical descriptor</param>
        /// <param name="SerialNoBufferLength">The length of "buffer"</param>
        /// <returns>TRUE if the operation succeeded, FALSE otherwise</returns>
        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal extern static bool HidD_GetPhysicalDescriptor(IntPtr HIDDeviceObject, IntPtr Buffer, uint DescriptorBufferLength);

        #endregion

        #region Information Set Functions

        /// <summary>
        /// Gets the "Information Set" for all the devices that are in a Device Interface Class
        /// </summary>
        /// <param name="ClassGuid">The Global Unique Identifier (GUID) of the Device Interface Class</param>
        /// <param name="Enumerator">Optional: A String that contains PnP Device instance Identifier</param>
        /// <param name="hwndParent">Optional: A handle of a window for installing a device instance in the device information set.</param>
        /// <param name="Flags">A 32-bit integer that represents a set of flags that tell the function which devices in the class to return.</param>
        /// <returns>A Handle to the "Information Set" of the devices found matching the criteria</returns>
        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr SetupDiGetClassDevs(
            ref Guid ClassGuid,
            [MarshalAs(UnmanagedType.LPTStr)] string Enumerator,
            IntPtr hwndParent,
            UInt32 Flags
        );

        /// <summary>
        /// Disposes of a specified Device Information Set
        /// </summary>
        /// <param name="hDevInfoSet">A Pointer to the Device Information Set</param>
        /// <returns>TRUE if the function executed successfully, FALSE otherwise</returns>
        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetupDiDestroyDeviceInfoList(IntPtr hDevInfoSet);

        #endregion

        #region Device Information Functions

        /// <summary>
        /// Gets a Device Information Element for a specified device
        /// </summary>
        /// <param name="hDevInfoSet">The Device Information Set from which to extract the device information element</param>
        /// <param name="memberIndex">The zero-based index of the device information element that you want</param>
        /// <param name="deviceInformationData">A Pointer to the device information data that is returned to this function</param>
        /// <returns>TRUE if the operation retrieved the data item successfully, FALSE if the member index is too great or too small, or if the operation failed</returns>
        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetupDiEnumDeviceInfo(
            IntPtr hDevInfoSet,
            int memberIndex,
            ref HIDStructures.DeviceInfo deviceInformationData
        );

        #endregion

        #region Device Interface Functions

        /// <summary>
        /// Gets the information about a particular device interface from the specified Information Set
        /// </summary>
        /// <param name="hDevInfoSet">A Handle to the Information Set that we want to get the device interfaces from</param>
        /// <param name="hDevInfoData">Optional: A Handle to a structure of type <see cref="SP_DEVINFO_DATA"/> that specifies the device information element to get interface information about. This is usually obtained by using the SetupDiEnumDeviceInfo function. If you do not wish to use this parameter, it should be set to IntPtr.Zero</param>
        /// <param name="interfaceClassGuid">The Device Interface Class (e.g. HID) GUID that the device interface must belong to</param>
        /// <param name="memberIndex">Which interface matching these criteria should be output. To iterate through all device interfaces matching the requirements, you can set this to zero, and increment this parameter by one in a loop to access each interface until this function returns the value FALSE</param>
        /// <param name="deviceInterfaceData">A <see cref="SP_DEVICE_INTERFACE_DATA"/> structure that will return with details about the found device interface</param>
        /// <returns>TRUE, if the operation has completed, or FALSE if there was an error and/or if no more device interfaces could be found</returns>
        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern Boolean SetupDiEnumDeviceInterfaces(
            IntPtr hDevInfoSet,
            IntPtr hDevInfoData,
            ref Guid interfaceClassGuid,
            Int32 memberIndex,
            ref HIDStructures.DeviceInterfaceInformation deviceInterfaceData
        );

        /// <summary>
        /// Get's details about a device interface
        /// </summary>
        /// <param name="hDevInfoSet">The Device information set that the device interface was found in when the <see cref="SetupDiGetClassDevs"/> function was performed</param>
        /// <param name="deviceInterfaceData">The Interface data that was enumerated from the Device Information set</param>
        /// <param name="deviceInterfaceDetailData">A pointer to an instantiated <see cref="SP_DEVICE_INTERFACE_DETAIL_DATA"/> structure that will hold the details about the device interface. NOTE: The cbSize member of this structure (if not NULL) should always be set to the size of the structure, minus the variable part of the string that holds the device path. In .NET this must be set to 5 if running in an x86 OS, or 8 if running an x64 OS. This is because structure packing in x64 processors is 8 bytes (i.e. the byte size of the structure is rounded up to the nearest multiple of 8) which means that given that even though the structure in x64 is 6 bytes long (1 x DWORD + 1 x unicode character), it will get rounded up to 8, whereas the packing size in x86 is only 1 byte (i.e. no padding), which means that the struct will only occupy the 5 bytes it needs to.</param>
        /// <param name="deviceInterfaceDetailDataSize">The Size of the <see cref="SP_DEVICE_INTERFACE_DETAIL_DATA"/> data structure. This size must be big enough to hold the device path. If <see cref="deviceInterfaceDetailData"/> is NULL, then this size should be zero.</param>
        /// <param name="requiredSize">The size that the <see cref="SP_DEVICE_INTERFACE_DETAIL_DATA"/> structure will be when filled with the device path. This parameter is optional, and is only used when determining the size of the structure before its values are set; however, because it is an output variable, the parameter should be set to the same value as <see cref="deviceInterfaceDetailDataSize"/> when it is not going to emit anything.</param>
        /// <param name="deviceInfoData">A <see cref="SP_DEVINFO_DATA"/> structure that will receive information about the device that uses this interface</param>
        /// <returns>TRUE if the procedure completed successfully, FALSE otherwise</returns>
        [DllImport(@"setupapi.dll", SetLastError = true)]
        internal static extern Boolean SetupDiGetDeviceInterfaceDetail(
            IntPtr hDevInfoSet,
            ref HIDStructures.DeviceInterfaceInformation deviceInterfaceData,
            IntPtr deviceInterfaceDetailData,
            UInt32 deviceInterfaceDetailDataSize,
            out UInt32 requiredSize,
            IntPtr deviceInfoData
        );

        /// <summary>
        /// Get's details about a device interface
        /// </summary>
        /// <param name="hDevInfoSet">The Device information set that the device interface was found in when the <see cref="SetupDiGetClassDevs"/> function was performed</param>
        /// <param name="deviceInterfaceData">The Interface data that was enumerated from the Device Information set</param>
        /// <param name="deviceInterfaceDetailData">An instantiated <see cref="SP_DEVICE_INTERFACE_DETAIL_DATA"/> structure that will hold the details about the device interface. NOTE: The cbSize member of this structure (if not NULL) should always be set to the size of the structure, minus the variable part of the string that holds the device path. In .NET this must be set to 5 if running in an x86 OS, or 8 if running an x64 OS. This is because structure packing in x64 processors is 8 bytes (i.e. the byte size of the structure is rounded up to the nearest multiple of 8) which means that given that even though the structure in x64 is 6 bytes long (1 x DWORD + 1 x unicode character), it will get rounded up to 8, whereas the packing size in x86 is only 1 byte (i.e. no padding), which means that the struct will only occupy the 5 bytes it needs to.</param>
        /// <param name="deviceInterfaceDetailDataSize">The Size of the <see cref="SP_DEVICE_INTERFACE_DETAIL_DATA"/> data structure. This size must be big enough to hold the device path. If <see cref="deviceInterfaceDetailData"/> is NULL, then this size should be zero.</param>
        /// <param name="requiredSize">The size that the <see cref="SP_DEVICE_INTERFACE_DETAIL_DATA"/> structure will be when filled with the device path. This parameter is optional, and is only used when determining the size of the structure before its values are set; however, because it is an output variable, the parameter should be set to the same value as <see cref="deviceInterfaceDetailDataSize"/> when it is not going to emit anything.</param>
        /// <param name="deviceInfoData">A <see cref="SP_DEVINFO_DATA"/> structure that will receive information about the device that uses this interface</param>
        /// <returns>TRUE if the procedure completed successfully, FALSE otherwise</returns>
        [DllImport(@"setupapi.dll", SetLastError = true)]
        internal static extern Boolean SetupDiGetDeviceInterfaceDetail(
            IntPtr hDevInfoSet,
            ref HIDStructures.DeviceInterfaceInformation deviceInterfaceData,
            ref HIDStructures.DevicePath deviceInterfaceDetailData,
            UInt32 deviceInterfaceDetailDataSize,
            out UInt32 requiredSize,
            IntPtr deviceInfoData
        );

        #endregion

        #region Device Handle Setup Functions

        /// <summary>
        /// Creates a communication between the computer and a resource
        /// </summary>
        /// <param name="fileName">The name of the file or device to be accessed/opened</param>
        /// <param name="fileAccess">The access permissions that the program has to the device/file</param>
        /// <param name="fileShare">Sharing permissions for this file/device</param>
        /// <param name="securityAttributes">Optional: A Pointer to a SECURITY_ATTRIBUTES structure that contains the security descriptor and whether or not these security attributes can be inherited by child processes</param>
        /// <param name="creationDisposition">What the function should do if the file does not already exist</param>
        /// <param name="flags">The file or device attributes and flags</param>
        /// <param name="template">Optional: A Handle to a template file with the GENERIC_READ access right.</param>
        /// <returns>A Handle to the file that is created</returns>
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
            [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] HIDStructures.FileAttributes flags,
            IntPtr template);

        /// <summary>
        /// Closes a given handle to an object
        /// </summary>
        /// <param name="hObject">The handle to close</param>
        /// <returns>TRUE if the operation was successful, false otherwise</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(IntPtr hObject);

        #endregion
    }
}
