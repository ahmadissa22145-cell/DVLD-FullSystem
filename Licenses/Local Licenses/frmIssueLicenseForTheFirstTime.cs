using DVLD.Global_Classes;
using DVLD_Buisness;
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

namespace DVLD.Licenses
{
    public partial class frmIssueLicenseForTheFirstTime : Form
    {

        int _LocalDrivingLicenseApplicationID { get; set; } = -1;
        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication { get { return ctrlDrivingLicenseApplicationInfo1.LocalDrivingLicenseApplicationInfo; } }
        clsLicense _License {  get; set; }

        public frmIssueLicenseForTheFirstTime(int localDrivingLicenseApplicationID)
        {
            InitializeComponent();

            _LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
        }

        private void frmIssueLicenseForTheFirstTime_Load(object sender, EventArgs e)
        {

            txtNotes.Focus();
            ctrlDrivingLicenseApplicationInfo1.LoadByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);

            // we show message box in control
            if(_LocalDrivingLicenseApplication == null)
            {
                this.Close();
                return;
            }

            if (!_LocalDrivingLicenseApplication.PassedAllTests())
            {
                MessageBox.Show("Person Should Pass All Tests First.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            int licenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();

            if (licenseID != -1)
            {
                MessageBox.Show("Person already has License before with License ID=" + licenseID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;

            }

            txtNotes.Text = string.Empty;
        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {

            int licenseID = _LocalDrivingLicenseApplication.IssueLicenseForTheFirstTime(txtNotes.Text.Trim(), clsGlobal.CurrentUserInfo.UserID);

            //If Falied Saved
            if (licenseID == -1)
            {
                MessageBox.Show("Failed to issue the license. Please try again or contact the administrator.",
                                "License Issuance Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);


                return;
            }

            //If Not First Time
            if (licenseID == -2)
            {
                MessageBox.Show("This applicant already has an active license for the same class.",
                                "Duplicate License",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                return;
            }

            //Saved Success
            MessageBox.Show($"License issued successfully.\n License ID: {licenseID}",
                             "License Issued",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Information);


        }
    }
}
