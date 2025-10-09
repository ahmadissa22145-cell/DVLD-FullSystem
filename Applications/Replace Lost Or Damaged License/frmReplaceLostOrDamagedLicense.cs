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
using static DVLD.Applications.Replace_Lost_Or_Damaged_License.frmReplaceLostOrDamagedLicense;
using static DVLD_Business.clsLicense;

namespace DVLD.Applications.Replace_Lost_Or_Damaged_License
{
    public partial class frmReplaceLostOrDamagedLicense : Form
    {

        public enum enReplaceFor{Dameged, Lost};

        private int _newLicenseID = -1;
        public frmReplaceLostOrDamagedLicense()
        {
            InitializeComponent();
        }

        private int GetApplicationTypeID()
        {

            if (rbDamagedLicense.Checked)

                return (int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense;

            else

                return (int)clsApplication.enApplicationType.ReplaceLostDrivingLicense;

        }

        private enIssueReason GetIssueReason()
        {
          return  rbDamagedLicense.Checked ? enIssueReason.DamagedReplacement :
                                             enIssueReason.LostReplacement;
        }

        private void _ResetDefaultValues()
        {
            ctrlDriverLicenseInfoWithFilter1.ResetDefaultValues();

            lblApplicationID.Text = "[???]";
            lblRreplacedLicenseID.Text = "[???]";
            lblOldLicenseID.Text = "[???]";
            lblCreatedByUser.Text = clsGlobal.CurrentUserInfo.UserName;
            lblApplicationFees.Text = "[$$$]";
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
        }

        private void frmReplaceLostOrDamagedLicense_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int selectedLicenseID = obj;

            lblOldLicenseID.Text = selectedLicenseID.ToString();

            llShowLicenseHistory.Enabled = (selectedLicenseID != -1);

            if (selectedLicenseID == -1)
            {
                return;
            }

            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is not Not Active, choose an active license."
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnIssueReplacement.Enabled = false;
                return;
            }

            btnIssueReplacement.Enabled = true;
        }

        private void CheckedChanged(object sender, EventArgs e)
        {

            lblTitle.Text = rbDamagedLicense.Checked 
                            ?"Replacement for Damaged License"
                            :"Replacement for Lost License";

            this.Text = lblTitle.Text;
            lblApplicationFees.Text = clsApplicationType.Find(GetApplicationTypeID()).
                                                         ApplicationFees.ToString();
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to Replace the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                return;
            }

            clsLicense newLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Replace(GetIssueReason(), clsGlobal.CurrentUserInfo.UserID);

            if(newLicense == null)
            {
                MessageBox.Show("An error occurred. The license was not replaced. Please contact the administrator or try again.",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            _newLicenseID = newLicense.LicenseID;
            lblRreplacedLicenseID.Text = _newLicenseID.ToString();
            lblApplicationID.Text = newLicense.ApplicationID.ToString();

            MessageBox.Show("Licensed Replaced Successfully with ID=" + _newLicenseID.ToString(),
                            "License Issued",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnIssueReplacement.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            gbReplacementFor.Enabled = false;
            llShowLicenseInfo.Enabled = true;
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frmShowLicenseInfo = new frmShowLicenseInfo(_newLicenseID);

            frmShowLicenseInfo.ShowDialog();
        }


        private void frmReplaceLostOrDamagedLicense_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
