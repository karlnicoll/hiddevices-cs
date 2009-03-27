namespace HIDDeviceList
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstDevicePaths = new System.Windows.Forms.ListBox();
            this.gbDeviceDetails = new System.Windows.Forms.GroupBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.lblVendor = new System.Windows.Forms.Label();
            this.lblManufacturer = new System.Windows.Forms.Label();
            this.lblPhysicalDescriptor = new System.Windows.Forms.Label();
            this.lblProductStr = new System.Windows.Forms.Label();
            this.lblSerial = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblProduct = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.btnCopyDevicePath = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.gbDeviceDetails.SuspendLayout();
            this.gbOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstDevicePaths
            // 
            this.lstDevicePaths.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstDevicePaths.FormattingEnabled = true;
            this.lstDevicePaths.Location = new System.Drawing.Point(13, 13);
            this.lstDevicePaths.Name = "lstDevicePaths";
            this.lstDevicePaths.Size = new System.Drawing.Size(687, 316);
            this.lstDevicePaths.TabIndex = 0;
            this.lstDevicePaths.SelectedIndexChanged += new System.EventHandler(this.lstDevicePaths_SelectedIndexChanged);
            // 
            // gbDeviceDetails
            // 
            this.gbDeviceDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDeviceDetails.Controls.Add(this.lblPath);
            this.gbDeviceDetails.Controls.Add(this.lblVendor);
            this.gbDeviceDetails.Controls.Add(this.lblManufacturer);
            this.gbDeviceDetails.Controls.Add(this.lblPhysicalDescriptor);
            this.gbDeviceDetails.Controls.Add(this.lblProductStr);
            this.gbDeviceDetails.Controls.Add(this.lblSerial);
            this.gbDeviceDetails.Controls.Add(this.lblVersion);
            this.gbDeviceDetails.Controls.Add(this.label8);
            this.gbDeviceDetails.Controls.Add(this.label7);
            this.gbDeviceDetails.Controls.Add(this.label6);
            this.gbDeviceDetails.Controls.Add(this.lblProduct);
            this.gbDeviceDetails.Controls.Add(this.label5);
            this.gbDeviceDetails.Controls.Add(this.label4);
            this.gbDeviceDetails.Controls.Add(this.label3);
            this.gbDeviceDetails.Controls.Add(this.label2);
            this.gbDeviceDetails.Controls.Add(this.label1);
            this.gbDeviceDetails.Location = new System.Drawing.Point(13, 335);
            this.gbDeviceDetails.Name = "gbDeviceDetails";
            this.gbDeviceDetails.Size = new System.Drawing.Size(687, 180);
            this.gbDeviceDetails.TabIndex = 1;
            this.gbDeviceDetails.TabStop = false;
            this.gbDeviceDetails.Text = "Device Details";
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(127, 20);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(58, 13);
            this.lblPath.TabIndex = 1;
            this.lblPath.Text = "<No Path>";
            // 
            // lblVendor
            // 
            this.lblVendor.AutoSize = true;
            this.lblVendor.Location = new System.Drawing.Point(127, 40);
            this.lblVendor.Name = "lblVendor";
            this.lblVendor.Size = new System.Drawing.Size(84, 13);
            this.lblVendor.TabIndex = 1;
            this.lblVendor.Text = "<No Vendor ID>";
            // 
            // lblManufacturer
            // 
            this.lblManufacturer.AutoSize = true;
            this.lblManufacturer.Location = new System.Drawing.Point(126, 160);
            this.lblManufacturer.Name = "lblManufacturer";
            this.lblManufacturer.Size = new System.Drawing.Size(99, 13);
            this.lblManufacturer.TabIndex = 1;
            this.lblManufacturer.Text = "<No Manufacturer>";
            // 
            // lblPhysicalDescriptor
            // 
            this.lblPhysicalDescriptor.AutoSize = true;
            this.lblPhysicalDescriptor.Location = new System.Drawing.Point(126, 140);
            this.lblPhysicalDescriptor.Name = "lblPhysicalDescriptor";
            this.lblPhysicalDescriptor.Size = new System.Drawing.Size(84, 13);
            this.lblPhysicalDescriptor.TabIndex = 1;
            this.lblPhysicalDescriptor.Text = "<No Descriptor>";
            // 
            // lblProductStr
            // 
            this.lblProductStr.AutoSize = true;
            this.lblProductStr.Location = new System.Drawing.Point(126, 120);
            this.lblProductStr.Name = "lblProductStr";
            this.lblProductStr.Size = new System.Drawing.Size(103, 13);
            this.lblProductStr.TabIndex = 1;
            this.lblProductStr.Text = "<No Product String>";
            // 
            // lblSerial
            // 
            this.lblSerial.AutoSize = true;
            this.lblSerial.Location = new System.Drawing.Point(126, 100);
            this.lblSerial.Name = "lblSerial";
            this.lblSerial.Size = new System.Drawing.Size(62, 13);
            this.lblSerial.TabIndex = 1;
            this.lblSerial.Text = "<No Serial>";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(126, 80);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(71, 13);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "<No Version>";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 160);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Product Manufacturer:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 140);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Physical Descriptor:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Product Name:";
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Location = new System.Drawing.Point(126, 60);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(87, 13);
            this.lblProduct.TabIndex = 1;
            this.lblProduct.Text = "<No Product ID>";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Device Serial:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Device Version:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Device Product ID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Device Vendor ID:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Device Path:";
            // 
            // gbOptions
            // 
            this.gbOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbOptions.Controls.Add(this.btnCopyDevicePath);
            this.gbOptions.Controls.Add(this.btnConnect);
            this.gbOptions.Controls.Add(this.button1);
            this.gbOptions.Location = new System.Drawing.Point(13, 521);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Size = new System.Drawing.Size(687, 49);
            this.gbOptions.TabIndex = 2;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Options";
            // 
            // btnCopyDevicePath
            // 
            this.btnCopyDevicePath.Location = new System.Drawing.Point(115, 19);
            this.btnCopyDevicePath.Name = "btnCopyDevicePath";
            this.btnCopyDevicePath.Size = new System.Drawing.Size(154, 23);
            this.btnCopyDevicePath.TabIndex = 1;
            this.btnCopyDevicePath.Text = "Copy Selected Device Path";
            this.btnCopyDevicePath.UseVisualStyleBackColor = true;
            this.btnCopyDevicePath.Click += new System.EventHandler(this.btnCopyDevicePath_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(10, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Refresh Devices";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(275, 20);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(95, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 582);
            this.Controls.Add(this.gbOptions);
            this.Controls.Add(this.gbDeviceDetails);
            this.Controls.Add(this.lstDevicePaths);
            this.Name = "frmMain";
            this.Text = "HID Device List";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.gbDeviceDetails.ResumeLayout(false);
            this.gbDeviceDetails.PerformLayout();
            this.gbOptions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstDevicePaths;
        private System.Windows.Forms.GroupBox gbDeviceDetails;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.Label lblVendor;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnCopyDevicePath;
        private System.Windows.Forms.Label lblSerial;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblProductStr;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblPhysicalDescriptor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblManufacturer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnConnect;
    }
}

