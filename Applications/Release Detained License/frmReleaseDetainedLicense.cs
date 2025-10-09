using DVLD.Global_Classes;
using DVLD.Licenses.Local_Licenses;
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

namespace DVLD.Licenses.Detain_License
{
    public partial class frmReleaseDetainedLicense : Form
    {
        int _selectedLicenseID = -1;
        private clsLicense LicenseInfo { get { return ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo; } }

        public frmReleaseDetainedLicense()
        {
            InitializeComponent();
        }

        public frmReleaseDetainedLicense(int licenseID)
        {
            InitializeComponent();

            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(licenseID);

            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo == null)
            {
                this.Close();
                return;
            }
            _selectedLicenseID = licenseID;
        }

        private void frmReleaseDetainedLicense_Load(object sender, EventArgs e)
        {
            if (_selectedLicenseID == -1)
            {
                ctrlDriverLicenseInfoWithFilter1.ResetDefaultValues();
                _ResetDefaultValues();
            }

        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int selectedLicenseID = obj;

            llShowLicenseHistory.Enabled = (selectedLicenseID != -1);

            if (selectedLicenseID == -1)
            {
                btnRelease.Enabled = false;
                return;
            }

            _selectedLicenseID = selectedLicenseID;

            lblLicenseID.Text = _selectedLicenseID.ToString();

            if (!LicenseInfo.IsDetained)
            {
                MessageBox.Show("This license is not currently detained, so it cannot be released",
                                "Information",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnRelease.Enabled = false;
                return;
            }

            if (!LicenseInfo.IsActive)
            {
                MessageBox.Show("The license is inactive and cannot be released",
                                "Information",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnRelease.Enabled = false;
                return;
            }

            clsApplication.enApplicationType applicationType = clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense;

            lblDetainID.Text = LicenseInfo.DetainLicenseInfo.DeteainID.ToString();
            lblDetainDate.Text = LicenseInfo.DetainLicenseInfo.DeteainDate.ToShortDateString();
            lblApplicationFees.Text = clsApplicationType.Find((int)applicationType).ApplicationFees.ToString();
            lblFineFees.Text = LicenseInfo.DetainLicenseInfo.FineFees.ToString();
            lblCreatedByUser.Text = LicenseInfo.DetainLicenseInfo.CreatedByUserInfo.UserName;

            lblTotalFees.Text = ((Convert.ToSingle(lblFineFees.Text)) + (Convert.ToSingle(lblApplicationFees.Text))).ToString();

            btnRelease.Enabled = true;
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to release this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int releseApplicationID = -1;

            bool isReleased = LicenseInfo.Release(clsGlobal.CurrentUserInfo.UserID,ref releseApplicationID);

            if(!isReleased)
            {
                MessageBox.Show("An error occurred and license detain was not released",
                                "License Detain Was Not Released",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnRelease.Enabled = false;
                return;
            }

            lblApplicationID.Text = releseApplicationID.ToString();
            llShowLicenseInfo.Enabled = true;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            btnRelease.Enabled = false;

            MessageBox.Show("The license released successfully ",
                            "Released Successfully",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void _ResetDefaultValues()
        {
            lblDetainID.Text = "[???]";
            lblDetainDate.Text = "[??/??/????]";
            lblLicenseID.Text = "[???]";
            lblCreatedByUser.Text = clsGlobal.CurrentUserInfo.UserName;
            lblApplicationFees.Text = "[$$$]";
            lblFineFees.Text = "[$$$]";
            lblTotalFees.Text = "[$$$]";
            lblApplicationID.Text = "[???]";

            btnRelease.Enabled = false;
            llShowLicenseInfo.Enabled = false;
            llShowLicenseHistory.Enabled = false;

            _selectedLicenseID = -1;
        }

        private void frmReleaseDetainedLicense_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(LicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_selectedLicenseID); 
            frm.ShowDialog();
        }
    }
}
