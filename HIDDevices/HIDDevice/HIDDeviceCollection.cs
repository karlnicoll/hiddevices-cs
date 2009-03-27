using System;
using System.Collections.Generic;
using System.Text;

namespace HIDDevices
{
    /// <summary>
    /// A Class that holds a collection of HID Devices that can be accessed individually or manipulated as a group
    /// </summary>
    public class HIDDeviceCollection: System.Collections.Generic.IList<HIDDevice>
    {
        //==================================================================================
        #region Enumerations



        #endregion

        //==================================================================================
        #region Data Structures



        #endregion

        //==================================================================================
        #region Constants


        #endregion

        //==================================================================================
        #region Private Variables

        private List<HIDDevice> devices;

        #endregion

        //==================================================================================
        #region Constructors/Destructors

        /// <summary>
        /// Constructor
        /// </summary>
        public HIDDeviceCollection()
        {
            devices = new List<HIDDevice>(1);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Devices">A collection of devices to contain within this class initially</param>
        public HIDDeviceCollection(IEnumerable<HIDDevice> Devices)
        {
            devices = new List<HIDDevice>(Devices);
            devices.Capacity = GetIdealListCapacity(devices.Count);
        }

        #endregion

        //==================================================================================
        #region Public Properties

        #region IList<HIDDevice> Members

        /// <summary>
        /// Gets or sets the specified item
        /// </summary>
        /// <param name="index">The index of the item to retrieve</param>
        /// <returns>The HID Device at the specified location</returns>
        public HIDDevice this[int index]
        {
            get
            {
                return devices[index];
            }
            set
            {
                devices[index] = value;
            }
        }

        #endregion

        #region ICollection<HIDDevice> Members

        /// <summary>
        /// Gets the amount of devices in this collection
        /// </summary>
        public int Count
        {
            get { return devices.Count; }
        }

        /// <summary>
        /// Determines if this collection is read-only
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #endregion

        //==================================================================================
        #region Public Methods

        #region IList<HIDDevice> Members

        /// <summary>
        /// Returns the index of a specific item in the list of items
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(HIDDevice item)
        {
            return devices.IndexOf(item);
        }

        /// <summary>
        /// Inserts a new item into the list of items at the specified location
        /// </summary>
        /// <param name="index">The location in the list to insert the item</param>
        /// <param name="item">The item to insert</param>
        public void Insert(int index, HIDDevice item)
        {
            devices.Insert(index, item);
        }

        /// <summary>
        /// Removes the item at the specified location
        /// </summary>
        /// <param name="index">The location of the item to remove</param>
        public void RemoveAt(int index)
        {
            devices.RemoveAt(index);
        }

        #endregion

        #region ICollection<HIDDevice> Members

        /// <summary>
        /// Adds a device to the end of the list of devices
        /// </summary>
        /// <param name="item">The device to add</param>
        public void Add(HIDDevice item)
        {
            //Always double to capacity of the list if we have reached capacity.
            //This makes the array less likely to resize as it gets larger, and means
            //that the larger it gets, the more efficient it becomes.
            if (devices.Count == devices.Capacity)
            {
                devices.Capacity = GetIdealListCapacity(devices.Count);
            }

            devices.Add(item);
        }

        /// <summary>
        /// Clears the list of devices
        /// </summary>
        public void Clear()
        {
            foreach (HIDDevice curDev in devices)
            {
                curDev.Dispose();
            }

            devices.Clear();
        }

        /// <summary>
        /// Determines if a specified device exists in the list of devices
        /// </summary>
        /// <param name="item">The device to look for</param>
        /// <returns>TRUE if the device was found in the list of devices, FALSE otherwise</returns>
        public bool Contains(HIDDevice item)
        {
            return devices.Contains(item);
        }

        /// <summary>
        /// Copies the devices to an array
        /// </summary>
        /// <param name="array">The array in which the devices will be moved to</param>
        /// <param name="arrayIndex">The index of the device to start copying from</param>
        public void CopyTo(HIDDevice[] array, int arrayIndex)
        {
            devices.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the specified device from this collection
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <returns>TRUE if the device was removed, FALSE otherwise</returns>
        public bool Remove(HIDDevice item)
        {
            return devices.Remove(item);
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            return devices.GetEnumerator();
        }

        #endregion

        #region Devices List Operations

        public void AddRange(IEnumerable<HIDDevice> DevicesToAdd)           
        {
            //Get the amount of devices we are adding
            int addedDeviceCount = 0;
            foreach (HIDDevice curDevice in DevicesToAdd)
            {
                addedDeviceCount++;
            }

            //Resize the array BEFORE we add the devices. This means
            //that the array will not resize twice.
            devices.Capacity = GetIdealListCapacity(devices.Count + addedDeviceCount);

            //Add the new devices to the list of devices
            devices.AddRange(DevicesToAdd);
        }

        /// <summary>
        /// Removes a range of devices from the list of devices
        /// </summary>
        /// <param name="LocationToStart">The zero-based index of the first item to remove</param>
        /// <param name="AmountToRemove">The amount of devices to remove</param>
        public void RemoveRange(int LocationToStart, int AmountToRemove)    
        {
            devices.RemoveRange(LocationToStart, AmountToRemove);
        }


        #endregion

        #region Device Group Operations

        /// <summary>
        /// Stops communications between all devices and this application
        /// </summary>
        public void DisconnectAllDevices()
        {
            foreach (HIDDevice curDevice in devices)
            {
                curDevice.Disconnect();
            }
        }

        #endregion

        #endregion

        //==================================================================================
        #region Private/Protected Methods

        #region IEnumerable<HIDDevice> Members

        IEnumerator<HIDDevice> IEnumerable<HIDDevice>.GetEnumerator()
        {
            return devices.GetEnumerator();
        }

        #endregion

        #region List Manipulation Operations

        /// <summary>
        /// Gets the ideal list capacity. The list capacity doubles every time it needs resizing.
        /// This makes it more efficient when adding many devices
        /// </summary>
        /// <param name="CurrentListSize">The current size of the list. This does not need to be a parameter because it could be taken straight from the list of devices. However including it as a parameter allows us to perform hypothetical resizings on the array without actually having to add items to the array to get the hypothetical size.</param>
        /// <returns>The ideal list capacity for this list</returns>
        private int GetIdealListCapacity(int CurrentListSize)
        {
            //Determine what the capacity of the list should be
            int listCapacity = 1;
            while (listCapacity <= CurrentListSize)
            {
                listCapacity *= 2;
            }
            return listCapacity;
        }

        #endregion

        #endregion




    }


}
