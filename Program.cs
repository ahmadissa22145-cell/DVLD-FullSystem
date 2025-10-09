
using DVLD.Applications.Apllication_Types;
using DVLD.Applications.Application_Types;
using DVLD.Applications.International_License;
using DVLD.Applications.Local_Driving_License;
using DVLD.Applications.Renew_Driver_License_Application;
using DVLD.Drivers;
using DVLD.Licenses;
using DVLD.LogIn;
using DVLD.People;
using DVLD.Tests;
using DVLD.Tests.Test_Types;
using DVLD.User;
using DVLD.User.Controls;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new frmMain());
          // Application.Run(new frmTest2());
          Application.Run(new frmLogin());
         


        }
    }
}
