using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HIDDevices;

namespace HIDDeviceList
{
    public partial class frmMain : Form
    {
        HIDDeviceCollection devices;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            

            
        }

        private void lstDevicePaths_SelectedIndexChanged(object sender, EventArgs e)
        {
            LookupDeviceByPath((string)lstDevicePaths.SelectedItem);
        }

        private void LookupDeviceByPath(string p)
        {
            foreach (HIDDevice curDevice in devices)
            {
                if (curDevice.DevicePath == p)
                {
                    //Set the labels for this device
                    lblProduct.Text = String.Format("{0:X}", curDevice.ProductID);
                    lblVendor.Text = String.Format("{0:X}", curDevice.VendorID);
                    lblVersion.Text = curDevice.DeviceVersion.ToString();
                    lblPath.Text = curDevice.DevicePath;
                    lblSerial.Text = curDevice.SerialNumber;
                    lblProductStr.Text = curDevice.Name;
                    lblPhysicalDescriptor.Text = curDevice.PhysicalDescriptor;
                    lblManufacturer.Text = curDevice.Manufacturer;
                    break;
                }
            }

        }

        private void GetDevices()
        {
            devices = HIDDeviceInterface.GetAllHIDDevices(true, false);

            //Add the devices to the listbox
            lstDevicePaths.Items.Clear();
            foreach (HIDDevice curDevice in devices)
            {
                lstDevicePaths.Items.Add(curDevice.DevicePath);
                try { curDevice.SetDeviceAttributes(); }
                catch { }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetDevices();
        }

        private void btnCopyDevicePath_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lblPath.Text, TextDataFormat.UnicodeText);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string p = (string)lstDevicePaths.SelectedItem;
            foreach (HIDDevice curDevice in devices)
            {
                if (curDevice.DevicePath == p)
                {
                    curDevice.Connect();
                    curDevice.StartReadingData(false);
                    curDevice.ReportReceived += new EventHandler<HIDDeviceReportReceivedEventargs>(curDevice_ReportReceived);
                    break;
                }
            }
        }

        void curDevice_ReportReceived(object sender, HIDDeviceReportReceivedEventargs e)
        {
            MessageBox.Show("Report of type '" + e.Report.Type.ToString() + "' received!");
        }
    }
}
