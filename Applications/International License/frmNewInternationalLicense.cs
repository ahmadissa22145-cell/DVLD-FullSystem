using DVLD.Global_Classes;
using DVLD.Licenses;
using DVLD.Licenses.International_License;
using DVLD_Buisness;
using DVLD_Business;
using System;
using System.Windows.Forms;

namespace DVLD.Applications.New_International_License
{
    public partial class frmNewInternationalLicense : Form
    {
        int _selectedLicenseID = -1;

        int _internationalLicenseID = -1;
        clsLicense SelectedLicenseInfo { get { return ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo; } }
        public frmNewInternationalLicense()
        {
            InitializeComponent();
        }

        private void _ResetDefaultValues()
        {
            lblApplicationID.Text = "[???]";
            lblInternationalLicenseID.Text = "[???]";
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();
            lblExpirationDate.Text = DateTime.Now.ToShortDateString();
            lblLocalLicenseID.Text = "[???]";
            lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).ApplicationFees.ToString();
            lblCreatedByUser.Text = clsGlobal.CurrentUserInfo.UserName;

            btnIssueLicense.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;

            _selectedLicenseID = -1;
            _internationalLicenseID = -1;
        }

        private void frmNewInternationalLicense_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.ResetDefaultValues();
            _ResetDefaultValues();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _selectedLicenseID = obj;

            lblLocalLicenseID.Text = _selectedLicenseID.ToString();

            llShowLicenseHistory.Enabled = _selectedLicenseID != -1;

            if (_selectedLicenseID == -1)
            {
                btnIssueLicense.Enabled = false;
                return;
            }

            if (!SelectedLicenseInfo.IsActive)
            {
                btnIssueLicense.Enabled = false;
                MessageBox.Show("The selected license is not active, so an International License cannot be issued.",
                                "Invalid License",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (SelectedLicenseInfo.IsDetained)
            {
                btnIssueLicense.Enabled = false;
                MessageBox.Show("The selected license is currently detained and cannot be used to issue an International License.",
                                "Invalid License",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (SelectedLicenseInfo.LicenseClassInfo.LicenseClassID != clsLicenseClass.enLicenseClassID.OrdinaryDrivingLicense)
            {
                btnIssueLicense.Enabled = false;
                MessageBox.Show("Only an Ordinary Driving License can be used to issue an International License.",
                                "Invalid License Class",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int activeInternationalLicense = clsInternationalLicense.GetActiveInternationalLicenseByDriverID(SelectedLicenseInfo.DriverID);

            if (activeInternationalLicense != -1)
            {
                _internationalLicenseID = activeInternationalLicense;
                llShowLicenseInfo.Enabled = true;
                btnIssueLicense.Enabled = false;
                MessageBox.Show($"This driver already has an active International License with ID = {activeInternationalLicense}. A new one cannot be issued until the current license expires or is deactivated.",
                                "Active International License",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            btnIssueLicense.Enabled = true;
        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to issue the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            clsInternationalLicense internationalLicense = new clsInternationalLicense();

            internationalLicense.ApplicantPersonID = SelectedLicenseInfo.DriverInfo.PersonID;
            internationalLicense.ApplicationDate = DateTime.Now.Date;
            internationalLicense.ApplicationTypeID = clsApplication.enApplicationType.NewInternationalLicense;
            internationalLicense.ApplicationStatus = clsApplication.enStatus.Completed;
            internationalLicense.LastStatusDate = DateTime.Now.Date;
            internationalLicense.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).ApplicationFees;
            internationalLicense.CreatedByUserID = clsGlobal.CurrentUserInfo.UserID;


            internationalLicense.DriverID = SelectedLicenseInfo.DriverID;
            internationalLicense.IssuedUsingLocalLicenseID = SelectedLicenseInfo.LicenseID;
            internationalLicense.IssueDate = DateTime.Now.Date;
            int defaultValidityLength = clsLicenseClass.Find(clsLicenseClass.enLicenseClassID.OrdinaryDrivingLicense).DefaultValidityLength;
            internationalLicense.ExpirationDate = DateTime.Now.AddYears(defaultValidityLength).Date;
            internationalLicense.IsActive = true;

            if (!internationalLicense.Save())
            {
                MessageBox.Show("Failed to create the international license application. Please try again or contact the administrator.",
                                "Application Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            lblApplicationID.Text = internationalLicense.ApplicationID.ToString();
            lblInternationalLicenseID.Text = internationalLicense.InternationalLicenseID.ToString();
            llShowLicenseInfo.Enabled = true;
            btnIssueLicense.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;

            _internationalLicenseID = internationalLicense.InternationalLicenseID;

            MessageBox.Show($"International License issued successfully.\nLicense ID: {internationalLicense.InternationalLicenseID}",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(_internationalLicenseID);
            frm.ShowDialog();
        }
    }
}
