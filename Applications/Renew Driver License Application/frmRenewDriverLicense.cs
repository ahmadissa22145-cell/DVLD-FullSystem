using DVLD.Global_Classes;
using DVLD.Licenses.Local_Licenses;
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

namespace DVLD.Applications.Renew_Driver_License_Application
{
    public partial class frmRenewDriverLicense : Form
    {
        private int _NewLicenseID { get; set; } = -1;

        private clsLicense _SelectedLicenseInfo { get {  return ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo; } }

        private clsLocalDrivingLicenseApplication _localDrivingLicenseApplication;

        private readonly clsApplication.enApplicationType _formApplicationType = clsApplication.enApplicationType.RenewDrivingLicense;

        public frmRenewDriverLicense()
        {
            InitializeComponent();
        }

        private void _ResetDefualtValues()
        {
            ctrlDriverLicenseInfoWithFilter1.ResetDefaultValues();

            lblApplicationID.Text    = "[???]";
            lblRenewedLicenseID.Text = "[???]";
            lblApplicationDate.Text  = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();
            lblApplicationFees.Text = clsApplicationType.Find((int)_formApplicationType).ApplicationFees.ToString();
            lblLicenseFees.Text = "[$$$]";
            lblTotalFees.Text = "[$$$]";
            lblOldLicenseID.Text = "[???]";
            lblCreatedByUser.Text = clsGlobal.CurrentUserInfo.UserName;
            lblExpirationDate.Text = "[??/??/????]";
            txtNotes.Text = string.Empty; 
        }

  
        private void frmRenewDriverLicense_Load(object sender, EventArgs e)
        {
            this._ResetDefualtValues();

        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int _selectedLicenseID = obj;

            lblOldLicenseID.Text = _selectedLicenseID.ToString();

            llShowLicenseHistory.Enabled = (_selectedLicenseID != -1);

            if (_selectedLicenseID == -1)
            {
                return;
            }

            DateTime IssueDate = Convert.ToDateTime(lblIssueDate.Text).Date;
            lblLicenseFees.Text = _SelectedLicenseInfo.LicenseClassInfo.ClassFees.ToString();
            lblOldLicenseID.Text = _SelectedLicenseInfo.LicenseID.ToString();
            lblExpirationDate.Text = IssueDate.AddYears
                                    (_SelectedLicenseInfo.LicenseClassInfo.DefaultValidityLength).
                                    ToShortDateString();
            lblTotalFees.Text = (Convert.ToSingle(lblApplicationFees.Text) + (Convert.ToSingle(lblLicenseFees.Text))).ToString();
            txtNotes.Text = _SelectedLicenseInfo.Notes;

            if (_SelectedLicenseInfo.IsLicenseExpired())
            {
                 MessageBox.Show($"Selected License is not yet expiared, it will expire on: {_SelectedLicenseInfo.ExpirationDate.ToShortDateString()}"
                                , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 btnRenewLicense.Enabled = false;
                 return;
            }

            if (!_SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is not Not Active, choose an active license."
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnRenewLicense.Enabled = false;
                return;
            }

            btnRenewLicense.Enabled = true;
        }

        private void btnRenewLicense_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to Renew the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            string notes = txtNotes.Text.Trim();
            int createdByUserID = clsGlobal.CurrentUserInfo.UserID;

            clsLicense newLicense = _SelectedLicenseInfo.RenewLicense(notes, createdByUserID);

            if (newLicense == null)
            {
                MessageBox.Show("An error occuerd : new License was not saved ,\n please try again or contact with admin",
                                "Local Application Was Not Saved",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            _NewLicenseID = newLicense.LicenseID;
            lblApplicationID.Text = newLicense.ApplicationID.ToString();
            lblRenewedLicenseID.Text = newLicense.LicenseID.ToString();

            MessageBox.Show($"The license has been successfully renewed and its ID is now : {newLicense.LicenseID}",
                             "License Renewed",
                             MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnRenewLicense.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;

            llShowLicenseInfo.Enabled = true;
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_SelectedLicenseInfo.LicenseID);

            frm.ShowDialog();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void frmRenewDriverLicense_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();
        }
    }
}
