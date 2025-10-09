using DVLD.People;
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

namespace DVLD.Applications.Controls
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {
        public int ApplicationBasicID { get; private set; } = -1;

        public clsApplication ApplicationBasicInfo { get; private set; }

        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }

        public void _ResetApplicationBasicInfo()
        {
            lblApplicationID.Text = "[???]";
            lblStatus.Text = "[???]";
            lblFees.Text = "[$$$]";
            lblType.Text = "[???]";
            lblApplicant.Text = "[???]";
            lblDate.Text = "[??/??/????]";
            lblStatusDate.Text = "[??/??/????]";
            lblCreatedByUser.Text = "[????]";
        }

        private void _FillApplicationBasicInfo()
        {
            lblApplicationID.Text = ApplicationBasicID.ToString();
            lblStatus.Text = ApplicationBasicInfo.ApplicationStatus.ToString();
            lblFees.Text = ApplicationBasicInfo.PaidFees.ToString();
            lblType.Text = ApplicationBasicInfo.ApplicationTypeID.ToString();
            lblApplicant.Text = ApplicationBasicInfo.ApplicantPersonInfo.FullName;
            lblDate.Text = ApplicationBasicInfo.ApplicationDate.ToString();
            lblStatusDate.Text = ApplicationBasicInfo.LastStatusDate.ToString();
            lblCreatedByUser.Text = ApplicationBasicInfo.CreatedByUserInfo.UserName;
        }

        public void LoadByApplicationBasicID(int applicationBasicID)
        {
            ApplicationBasicInfo = clsApplication.FindBaseApplication(applicationBasicID);

            if(ApplicationBasicInfo == null)
            {
                _ResetApplicationBasicInfo();
                MessageBox.Show($"Not found any application with ID = {applicationBasicID}",
                                 "Application Not Found",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ApplicationBasicID = applicationBasicID;
            _FillApplicationBasicInfo();

        }

        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo(ApplicationBasicInfo.ApplicantPersonID);
            frm.ShowDialog();
        }
    }
}
