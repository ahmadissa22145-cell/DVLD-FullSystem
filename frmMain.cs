
using DVLD.Applications.Apllication_Types;
using DVLD.Applications.International_License;
using DVLD.Applications.Local_Driving_License;
using DVLD.Applications.New_International_License;
using DVLD.Applications.Renew_Driver_License_Application;
using DVLD.Applications.Replace_Lost_Or_Damaged_License;
using DVLD.Drivers;
using DVLD.Global_Classes;
using DVLD.Licenses.Detain_License;
using DVLD.LogIn;
using DVLD.People;
using DVLD.Tests.Test_Types;
using DVLD.User;
using DVLD.User.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace DVLD
{

    public partial class frmMain : Form
    {
        frmLogin _frmLogin;
        public frmMain(frmLogin Login)
        {
            InitializeComponent();

            this._frmLogin = Login;
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListPeople listPeople = new frmListPeople();

            listPeople.ShowDialog();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo userInfo = new frmUserInfo(clsGlobal.CurrentUserInfo.UserID);

            userInfo.ShowDialog();
        }

        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
             frmListUsers listUsers = new frmListUsers();
            listUsers.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword changePassword = new frmChangePassword(clsGlobal.CurrentUserInfo.UserID);
            changePassword.ShowDialog();
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListApplicationTypes listApplicationTypes = new frmListApplicationTypes();
            listApplicationTypes.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListTestTypes listTestTypes = new frmListTestTypes();
            listTestTypes.ShowDialog();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUserInfo = null;
            
            this.Close();

            _frmLogin.Show();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (clsGlobal.CurrentUserInfo != null)
            {
                Application.Exit();
            }
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplication localDrivingLicenseApplication = new frmAddUpdateLocalDrivingLicenseApplication();
            localDrivingLicenseApplication.ShowDialog();
        }

        private void manageLocalDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListLocalDrivingLicenseApplications frm = new frmListLocalDrivingLicenseApplications();
            frm.ShowDialog();
        }

        private void retakeTestToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmListLocalDrivingLicenseApplications frm = new frmListLocalDrivingLicenseApplications();
            frm.ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenewDriverLicense frm = new frmRenewDriverLicense();
            frm.ShowDialog();
        }

        private void ReplacementLostOrDamagedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReplaceLostOrDamagedLicense frm = new frmReplaceLostOrDamagedLicense();
            frm.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListDrivers frm = new frmListDrivers();
            frm.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.ShowDialog();
        }

        private void ManageDetainedLicensestoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmListDetainedLicense frm = new frmListDetainedLicense();
            frm.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNewInternationalLicense frm = new frmNewInternationalLicense();
            frm.ShowDialog();
        }

        private void ManageInternationaDrivingLicenseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmListInternationalLicenseApplications frm = new frmListInternationalLicenseApplications();
            frm.ShowDialog();
        }
    }
}
