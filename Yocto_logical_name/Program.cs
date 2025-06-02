using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yocto_logical_name
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string errsmg = "";
            if (YAPI.RegisterHub("usb", ref errsmg) == YAPI.SUCCESS)
            {
                Application.Run(new Form1());
            }
            else
            {
                MessageBox.Show("Init error:" + errsmg + "\n \n Check if the Virtual Hub is running");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }
    }
}
