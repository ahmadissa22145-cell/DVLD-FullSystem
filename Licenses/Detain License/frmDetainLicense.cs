using DVLD.Global_Classes;
using DVLD.Licenses;
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

namespace DVLD
{
    public partial class frmDetainLicense : Form
    {
        private int _licenseID;

        private clsLicense LicenseInfo { get { return ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo; } }

        private int _detainID;

        public frmDetainLicense()
        {
            InitializeComponent();
        }

        private void frmDetainLicense_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.ResetDefaultValues();
            _ResetDefaultValues();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to detain this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            float fineFees = Convert.ToSingle(txtFineFees.Text.Trim());

            _detainID = LicenseInfo.Detain(fineFees, clsGlobal.CurrentUserInfo.UserID);

            if (_detainID == -1)
            {
                MessageBox.Show("An error occurred : the license was not detained",
                                "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }


            btnDetain.Enabled = false;
            lblDetainID.Text = _detainID.ToString();
            llShowLicenseInfo.Enabled = true;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            txtFineFees.Enabled = false;

            MessageBox.Show($"license detained successfully with ID = {_detainID}",
                            "License Detained Successfully",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int selectedLicenseID = obj;

            lblLicenseID.Text = selectedLicenseID.ToString();

            llShowLicenseHistory.Enabled = (selectedLicenseID != -1);

            if (selectedLicenseID == -1)
            {
                btnDetain.Enabled = false;
                return;
            }

            _licenseID = selectedLicenseID;

            if (!LicenseInfo.IsActive)
            {
                MessageBox.Show("the license has inactive, please contact with administrator",
                                "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnDetain.Enabled = false;

                return;
            }

            if (LicenseInfo.IsDetained)
            {
                MessageBox.Show("the license has already detained, please select another one",
                                "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnDetain.Enabled = false;

                return;
            }


            txtFineFees.Focus();
            btnDetain.Enabled = true;

        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int personID = LicenseInfo.DriverInfo.PersonID;
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(personID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_licenseID);
            frm.ShowDialog();
        }

        private void txtFineFees_Validated(object sender, EventArgs e)
        {
            string fineFees = txtFineFees.Text.Trim();

            if (string.IsNullOrEmpty(fineFees))
            {
                errorProvider1.SetError(txtFineFees, "this field cannot be a blank");
                return;
            }

            if (!clsValidation.IsNumber(fineFees))
            {
                errorProvider1.SetError(txtFineFees, "please reEnter a valid number");
                return;
            }

            errorProvider1.SetError(txtFineFees, null);
        }

        private void txtFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnDetain.Focus();
                return;
            }

            e.Handled = !e.KeyChar.Equals('.') && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void _ResetDefaultValues()
        {
            lblDetainID.Text = "[???]";
            lblLicenseID.Text = "[???]";
            lblDetainDate.Text = DateTime.Now.ToShortDateString();
            lblCreatedByUser.Text = clsGlobal.CurrentUserInfo.UserName;
            txtFineFees.Text = string.Empty;

            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;
            btnDetain.Enabled = false;
        }

        private void frmDetainLicense_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();
        }
    }
}
