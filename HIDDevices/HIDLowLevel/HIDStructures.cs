/*==================================================================================*
 * Project:             HIDDevices                                                  *
 * Project Description: A class library that allows Nintendo's Wii Remote to be     *
 *                      used in .NET projects as a peripheral                       *
 *                      manipulated using the Nintendo Wii Remote (A.K.A WiiMote)   *
 * Date:                15th December 2008                                          *
 * Author:              Karl Nicoll                                                 *
 * Licence:             Pending                                                     *
 * File:                HIDStructures.cs                                            *
 * File Description:    Structures used in the low level HID functions that have    *
 *                      been obtained through P/Invoke                              *
 *==================================================================================*/

using System;
using System.Runtime.InteropServices;


namespace HIDDevices
{
    /// <summary>
    /// Class containing data structures used for HID device interfacing
    /// </summary>
    public class HIDStructures
    {
        //==================================================================================
        #region Enumerations


        /// <summary>
        /// Flags controlling what is included in the device information set (the container that
        /// contains information about an HID device) built by SetupDiGetClassDevs.
        /// </summary>
        [Flags]
        public enum DIGCF : int
        {
            /// <summary>
            /// Return only information about the devices that use the system's default device interface.
            /// Is only valid if the <see cref="DEVICEINTERFACE">DIGCF_DEVICEINTERFACE</see> flag
            /// is also set
            /// </summary>
            DEFAULT = 0x00000001,
            /// <summary>
            /// Return information about only devices that are present
            /// </summary>
            PRESENT = 0x00000002,
            /// <summary>
            /// Return a list of installed devices for all device setup classes or all device interface classes
            /// </summary>
            ALLCLASSES = 0x00000004,
            /// <summary>
            /// Return devices that are part of the current hardware profile
            /// </summary>
            PROFILE = 0x00000008,
            /// <summary>
            /// Return device information about devices whos device interface matches the device interface that is being specified
            /// </summary>
            DEVICEINTERFACE = 0x00000010
        }


        /// <summary>
        /// Enumeration containing File Attribute Flags
        /// </summary>
        [Flags]
        public enum FileAttributes : uint
        {
            Readonly = 0x00000001,
            Hidden = 0x00000002,
            System = 0x00000004,
            Directory = 0x00000010,
            Archive = 0x00000020,
            Device = 0x00000040,
            Normal = 0x00000080,
            Temporary = 0x00000100,
            SparseFile = 0x00000200,
            ReparsePoint = 0x00000400,
            Compressed = 0x00000800,
            Offline = 0x00001000,
            NotContentIndexed = 0x00002000,
            Encrypted = 0x00004000,
            Write_Through = 0x80000000,
            Overlapped = 0x40000000,
            NoBuffering = 0x20000000,
            RandomAccess = 0x10000000,
            SequentialScan = 0x08000000,
            DeleteOnClose = 0x04000000,
            BackupSemantics = 0x02000000,
            PosixSemantics = 0x01000000,
            OpenReparsePoint = 0x00200000,
            OpenNoRecall = 0x00100000,
            FirstPipeInstance = 0x00080000
        }


        #endregion

        //==================================================================================
        #region Data Structures

        /// <summary>
        /// A Data structure that contains data about a device that is part of a device information set.
        /// </summary>
        /// <remarks>Based on <see cref="SP_DEVINFO_DATA"/></remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct DeviceInfo
        {
            public uint cbSize;
            public Guid ClassGuid;
            public uint DevInst;
            public IntPtr Reserved;
        }

        /// <summary>
        /// Details an HID Device Interface.
        /// </summary>
        /// <remarks>Based on <see cref="SP_DEVICE_INTERFACE_DATA"/></remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct DeviceInterfaceInformation
        {
            public int cbSize;
            public Guid InterfaceClassGuid;
            public int Flags;
            public IntPtr RESERVED;
        }


        /// <summary>
        /// Contains the path of a devices interface. This path can be used in functions such as
        /// <see cref="CreateFile">CreateFile()</see> to setup a "File-esque" data stream between
        /// the computer and the device.
        /// </summary>
        /// <remarks>Based on <see cref="SP_DEVICE_INTERFACE_DETAIL_DATA"/></remarks>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct DevicePath
        {
            public UInt32 cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string path;
        }

        /// <summary>
        /// Contains Vendor information about an HID Class device
        /// </summary>
        /// <remarks>Based on <see cref="HIDD_ATTRIBUTES"/></remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct HIDDeviceAttributes
        {
            public int Size;
            public short VendorID;
            public short ProductID;
            public short VersionNumber;
        }

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
        #region public Properties



        #endregion


        //==================================================================================
        #region public Methods
       

        #endregion


        //==================================================================================
        #region Private/Protected Methods



        #endregion

        
        
        


        
    }
}
