using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.Local_Driving_License.Controls
{
    public partial class ctrlDrivingLicenseApplicationInfo : UserControl
    {

        public int LocalDrivingLicenseApplicationID { get; private set; } = -1;

        public clsLocalDrivingLicenseApplication LocalDrivingLicenseApplicationInfo { get; private set; }

        public clsLicenseClass.enLicenseClassID LicenseID { get; private set; } = clsLicenseClass.enLicenseClassID.SmallMotorcycle;

        public clsApplication ApplicationBasicInfo { get { return ctrlApplicationBasicInfo1.ApplicationBasicInfo; } }

        public ctrlDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        private void _ResetLocalDrivingLicenseApplicationInfo()
        {
            lblLocalDrivingLicenseApplicationID.Text = "[????]";
            lblAppliedFor.Text = "[????]";
            lblPassedTests.Text = "?/?";

            ctrlApplicationBasicInfo1._ResetApplicationBasicInfo();
        }

        private void _FillLocalDrivingLicenseApplicationInfo()
        {
            LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationInfo.LocalDrivingLicenseApplicationID;

            // we will return to it
            LicenseID = LocalDrivingLicenseApplicationInfo.LicenseClassID;

            llShowLicenceInfo.Enabled = (LicenseID != clsLicenseClass.enLicenseClassID.SmallMotorcycle);

            lblLocalDrivingLicenseApplicationID.Text = LocalDrivingLicenseApplicationID.ToString();
            lblAppliedFor.Text = clsLicenseClass.Find(LicenseID).ClassName.ToString();
            lblPassedTests.Text = clsLocalDrivingLicenseApplication.GetPassedTestsCount(LocalDrivingLicenseApplicationID).ToString() + "/3";

            ctrlApplicationBasicInfo1.LoadByApplicationBasicID(LocalDrivingLicenseApplicationInfo.ApplicationID);
        }

        private void _ValidateAndLoadLocalApplicationInfo()
        {
            if (LocalDrivingLicenseApplicationInfo == null)
            {
                _ResetLocalDrivingLicenseApplicationInfo();
                MessageBox.Show($"Not found any Local Driving License Application with ID = {LocalDrivingLicenseApplicationID}",
                                 "Local Driving License Application Not Found",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _FillLocalDrivingLicenseApplicationInfo();

        }

        public void LoadByLocalDrivingAppID(int localDrivingLicenseApplicationID)
        {

            LocalDrivingLicenseApplicationInfo = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(localDrivingLicenseApplicationID);

            _ValidateAndLoadLocalApplicationInfo();
        }

        public void LoadByApplicationID(int applicationBasicID)
        {
            LocalDrivingLicenseApplicationInfo = clsLocalDrivingLicenseApplication.FindByApplicationID(applicationBasicID);

            _ValidateAndLoadLocalApplicationInfo();
        }

        private void llShowLicenceInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("This form is being developed", "Coming Soon", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
