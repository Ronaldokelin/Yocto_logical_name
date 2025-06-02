using System;
using System.Windows.Forms;

namespace Yocto_logical_name
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            labelValor1.Text = "N/A";
            labelName1.Text = "No sensor detected";
            timer1.Interval = 100;
            timer1.Tick += timer1_Tick;
            timer1.Enabled = true;
        }

        private Timer timer1 = new Timer();
        YSensor s1 = null;
        YSensor s2 = null;
        YSensor s3 = null;
        YSensor s4 = null;
        int hardwaredetect = 0;

        // Updates the value of the found sensors
        private void timer1_Tick(object sender, EventArgs e)
        {
            string errmsg = "";
            if (hardwaredetect == 0) YAPI.UpdateDeviceList(ref errmsg);
            hardwaredetect = (hardwaredetect + 1) % 20;
            deviceArrival();

            //Sensor 1#
            YAPI.HandleEvents(ref errmsg);
            if (s1 == null) s1 = YSensor.FirstSensor();
            if (s1 != null & cboYoctoList.Items.Count > 0)
            {

                if (s1.isOnline())
                {
                    labelValor1.Text = s1.get_currentValue() + s1.get_unit();
                    labelName1.Text = s1.get_friendlyName();
                    s2 = s1.nextSensor();
                }
                else
                {
                    labelValor1.Text = "OFFLINE";
                    labelName1.Text = "Sensor is offline";
                    s1 = null;
                }
            }

            //Sensor 2#
            YAPI.HandleEvents(ref errmsg);
            if (s2 == null) s2 = YSensor.FirstSensor();
            if (s2 != null & cboYoctoList.Items.Count > 1)
            {

                if (s2.isOnline())
                {
                    labelValor2.Text = s2.get_currentValue() + s2.get_unit();
                    labelName2.Text = s2.get_friendlyName();
                    s3 = s2.nextSensor();
                }
                else
                {
                    labelValor2.Text = "OFFLINE";
                    labelName2.Text = "Sensor is offline";
                    s2 = null;
                }
            }

            //Sensor 3#
            YAPI.HandleEvents(ref errmsg);
            if (s3 == null) s3 = YSensor.FirstSensor();
            if (s3 != null & cboYoctoList.Items.Count > 2)
            {

                if (s3.isOnline())
                {
                    labelValor3.Text = s3.get_currentValue() + s3.get_unit();
                    labelName3.Text = s3.get_friendlyName();
                    s4 = s3.nextSensor();
                }
                else
                {
                    labelValor3.Text = "OFFLINE";
                    labelName3.Text = "Sensor is offline";
                    s3 = null;
                }
            }

            //Sensor 4#
            YAPI.HandleEvents(ref errmsg);
            if (s4 == null) s4 = YSensor.FirstSensor();
            if (s1 != null & cboYoctoList.Items.Count > 3)
            {

                if (s4.isOnline())
                {
                    labelValor4.Text = s4.get_currentValue() + s4.get_unit();
                    labelName4.Text = s4.get_friendlyName();
                }
                else
                {
                    labelValor4.Text = "OFFLINE";
                    labelName4.Text = "Sensor is offline";
                    s4 = null;
                }
            }
        }
        // Counts the number of sensors
        public void setSensorCount()
        {
            if (cboYoctoList.Items.Count <= 0) Status.Text = "No sensor found, check USB cable";
            else if (cboYoctoList.Items.Count == 1) Status.Text = "One sensor found";
            else Status.Text = cboYoctoList.Items.Count + " sensors found";
            Application.DoEvents();
        }

        // Update the cbo with the sensors
        public void deviceArrival()
        {
            YSensor s = YSensor.FirstSensor();
            while (s != null)
            {
                if (!cboYoctoList.Items.Contains(s))
                {
                    int index = cboYoctoList.Items.Add(s);
                }
                s = s.nextSensor();
            }
            cboYoctoList.Enabled = cboYoctoList.Items.Count > 0;
            if ((cboYoctoList.SelectedIndex < 0) && (cboYoctoList.Items.Count > 0))
                cboYoctoList.SelectedIndex = 0;

            setSensorCount();
        }

        // select the sensor from the list cbo
        private YSensor getSelectedSensor()
        {
            int index = cboYoctoList.SelectedIndex;
            if (index < 0) return null;
            return (YSensor)cboYoctoList.Items[index];
        }

        // updates text box with selected sensor
        private void cboYoctoList_SelectedIndexChanged(object sender, EventArgs e)
        {
            YSensor s = getSelectedSensor();
            if (s != null)
            {
                YModule m = s.get_module();
                txtSerialNumber.Text = m.get_serialNumber();
                txtLogicalName.Text = s.get_logicalName();
            }
        }

        // change the name of the selected sensor
        private void btnSetYoctoName_Click(object sender, EventArgs e)
        {
            string newname = txtLogicalName.Text;
            YSensor s = getSelectedSensor();
            if (s != null)
            {
                YModule m = s.get_module();
                if (m.isOnline())
                {
                    s.set_logicalName(newname);
                    m.saveToFlash();
                    string errmsg = "";
                    YAPI.UpdateDeviceList(ref errmsg);
                }
                else
                {
                    MessageBox.Show("not connected (check identification and USB cable");
                }
            }
        }
    }
}
