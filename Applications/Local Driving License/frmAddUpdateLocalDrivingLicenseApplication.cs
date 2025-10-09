using DVLD.Global_Classes;
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

namespace DVLD.Applications.Local_Driving_License
{
    public partial class frmAddUpdateLocalDrivingLicenseApplication : Form
    {
        public enum enMode { AddNew =  0, Update = 1 }

        public enMode Mode { get; private set; } = enMode.AddNew;

        public int LocalDrivingLicenseApplicationID { get; private set; } = -1;

        public clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication { get; private set; }

        int _selectedPersonID = -1;

        const clsApplication.enApplicationType _applicationType = clsApplication.enApplicationType.NewDrivingLicense;

        readonly float _applicationFees = clsApplicationType.Find((int)_applicationType).ApplicationFees;

        public frmAddUpdateLocalDrivingLicenseApplication()
        {
            InitializeComponent();

            this.Mode = enMode.AddNew;
        }

        public frmAddUpdateLocalDrivingLicenseApplication(int localDrivingLicenseApplicationID)
        {
            InitializeComponent();

            this.LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            this.Mode = enMode.Update;
        }

        private void _ResetDefaultValues()
        {

            _FillLicenseClassesInCompoBox();

            if (Mode == enMode.AddNew)
            {
                this.Text = "Add Local Driving License Application";
                lblTitle.Text = "Add Local Driving License Application";

                LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();

                ctrlPersonCardWithFilter1.ResetPersonCard();
                lblLocalDrivingLicebseApplicationID.Text = "[???]";
                lblApplicationDate.Text = DateTime.Now.Date.ToShortDateString();
                lblFees.Text = _applicationFees.ToString();
                lblCreatedByUser.Text = clsGlobal.CurrentUserInfo.UserName;
                lblCreatedByUser.Tag = clsGlobal.CurrentUserInfo.UserID;

                if (cbLicenseClass.Items.Count > 0)
                    cbLicenseClass.SelectedIndex = 2;

                btnSave.Enabled = false;
                tpApplicationInfo.Enabled = false;
                tcApplicationInfo.SelectedTab = tpPersonalInfo;
            }
            else
            {
                this.Text = "Update Local Driving License Application";
                lblTitle.Text = "Update Local Driving License Application";

                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                ctrlPersonCardWithFilter1.FilterEnabled = false;
                tcApplicationInfo.SelectedTab = tpPersonalInfo;
            }

                
        }

        private void _LoadData()
        {
            LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);

            if (LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show($"Not found any local driving license application with ID = {this.LocalDrivingLicenseApplicationID}",
                                 "Local Driving License Application Not Found",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.Close();
                return;
            }

            ctrlPersonCardWithFilter1.LoadPersonInfo(LocalDrivingLicenseApplication.ApplicantPersonID);

            lblLocalDrivingLicebseApplicationID.Text = LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = LocalDrivingLicenseApplication.ApplicationDate.Date.ToShortDateString();
            cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(clsLicenseClass.Find(LocalDrivingLicenseApplication.LicenseClassID).ClassName);
            lblFees.Text = clsApplicationType.Find((int)_applicationType).ApplicationFees.ToString();
            lblCreatedByUser.Text = LocalDrivingLicenseApplication.CreatedByUserInfo.UserName;
            lblCreatedByUser.Tag  = LocalDrivingLicenseApplication.CreatedByUserID;
        }

        private void _FillLicenseClassesInCompoBox()
        {
            DataTable dtLicenseClasses = clsLicenseClass.GetAllLicenseClasses();

            foreach(DataRow row in dtLicenseClasses.Rows)
            {
                cbLicenseClass.Items.Add(row["ClassName"]);
            }
        }

        private void frmAddUpdateLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.OnPersonSelected += ctrlPersonCardWithFilter1_OnPersonSelected;

            _ResetDefaultValues();

            if (this.Mode == enMode.Update)
                _LoadData();
        }

        private void btnApplicationInfoNext_Click(object sender, EventArgs e)
        {

            if (this.Mode == enMode.AddNew)
            {
                if (_selectedPersonID <= 0)
                {
                    MessageBox.Show("Please select a person before continue",
                                    "No Person Selected Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                    btnSave.Enabled = false;
                    tpApplicationInfo.Enabled = false;
                    return;
                }

                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;

            }

            tcApplicationInfo.SelectedTab = tpApplicationInfo;
            cbLicenseClass.Focus();
        }

        private void _FillLocalDrivingLicenseApplicationInfo()
        {
            //Local Application Info
            // {
            //Application Info
            LocalDrivingLicenseApplication.ApplicantPersonID = _selectedPersonID;
            LocalDrivingLicenseApplication.ApplicationDate = Convert.ToDateTime(lblApplicationDate.Text);
            LocalDrivingLicenseApplication.ApplicationTypeID = _applicationType;
            LocalDrivingLicenseApplication.ApplicationStatus = clsApplication.enStatus.New;
            LocalDrivingLicenseApplication.LastStatusDate = DateTime.Now;
            LocalDrivingLicenseApplication.PaidFees = Convert.ToInt32(lblFees.Text);
            LocalDrivingLicenseApplication.CreatedByUserID = Convert.ToInt32(lblCreatedByUser.Tag);

            
            LocalDrivingLicenseApplication.LicenseClassID = clsLicenseClass.Find(cbLicenseClass.Text).LicenseClassID;
            // }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are not valid , put the mouse over the red icon(s) to see the error",
                                "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillLocalDrivingLicenseApplicationInfo();

            int activeApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass
                                      (_selectedPersonID, _applicationType, LocalDrivingLicenseApplication.LicenseClassID);

            if(activeApplicationID > -1)
            {
                MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected class with id=" + activeApplicationID,
                                "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsLicenseClass.enLicenseClassID licenseClassID = LocalDrivingLicenseApplication.LicenseClassID;

            int minimumAllowedAge = clsLicenseClass.GetMinimumAllowedAgeForLicenseClass(licenseClassID);

            int personAge = clsPerson.Find(_selectedPersonID).Age;

            if (personAge < minimumAllowedAge)
            {
                MessageBox.Show("The selected person does not meet the minimum age requirement ("
                                + minimumAllowedAge + " years). Please choose another license class or select a different person.",
                                "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (!LocalDrivingLicenseApplication.Save())
            {
                MessageBox.Show("An error oocured , The save process was not completed.",
                                "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Data Saved Successfully",
                            "Data Saved",
                             MessageBoxButtons.OK, MessageBoxIcon.Information);


            if(this.Mode == enMode.AddNew)
            {
                this.Mode = enMode.Update;

                this.Text = "Update Local Driving License Application";
                lblTitle.Text = "Update Local Driving License Application";
                LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
                lblLocalDrivingLicebseApplicationID.Text = LocalDrivingLicenseApplicationID.ToString();
                ctrlPersonCardWithFilter1.FilterEnabled = false;
            }
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _selectedPersonID = obj;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddUpdateLocalDrivingLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }

       
    }
}
