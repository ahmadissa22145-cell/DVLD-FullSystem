using DVLD.Global_Classes;
using DVLD.Properties;
using DVLD_Buisness;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests.Controls
{
    public partial class ctrlScheduleTest : UserControl
    {
        public enum enMode { AddNew = 0, Update = 1};

        private enMode _Mode = enMode.AddNew;

        public enum enCreationMode { FirstTimeSchedule = 0, ReTakeTestSchedule = 1 }

        private enCreationMode _CreationMode = enCreationMode.FirstTimeSchedule;

        public int LocalDrivingLicenseApplicationID { get; private set; }

        public clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication { get; private set; }

        public int TestAppointmentID { get; set; }

        public clsTestAppointment TestAppointmentInfo { get; private set; }

        private clsTestType.enTestTypes _testTypeID = clsTestType.enTestTypes.VisionTest;

        public clsTestType.enTestTypes TestTypeID 
        {
            get { return _testTypeID; }
            set
            {
                _testTypeID = value;

                switch (_testTypeID)
                {
                    case clsTestType.enTestTypes.VisionTest:
                        gbTestType.Text = "Vision Test";
                        pbTestTypeImage.Image = Resources.Vision_512;
                        break;

                    case clsTestType.enTestTypes.WrittenTest:
                        gbTestType.Text = "Written Test";
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        break;

                    case clsTestType.enTestTypes.StreetTest:
                        gbTestType.Text = "Street Test";
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        break;
                }
            }
        }

        public ctrlScheduleTest()
        {
            InitializeComponent();

        }

       
        public void LoadInfo(int localDrivingLicenseApplicationID, int testAppointmentID = -1)
        {
            this._Mode = testAppointmentID == -1 ? enMode.AddNew : enMode.Update;


            LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            TestAppointmentID = testAppointmentID;

            if (!_InitializeFormData())
                return;

            if(this._Mode == enMode.Update)
            {
                if (!_LoadTestAppointmentDataIntoForm())
                    return;
            }

            lblTotalFees.Text = ((Convert.ToSingle(lblFees.Text)) + (Convert.ToSingle(lblRetakeAppFees.Text))).ToString();

            if (!_HandleAppointmentLockedConstraint())
                return;
        }



        //Get Data
        
        private bool _LoadTestAppointmentDataIntoForm()
        {

            TestAppointmentInfo = clsTestAppointment.Find(TestAppointmentID);

            if (TestAppointmentInfo == null)
            {
                MessageBox.Show($"Error : No Test Appointment with id = {TestAppointmentID} was found",
                                 "Error : Test Appointment Not Found",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }

            if((DateTime.Compare(DateTime.Now, TestAppointmentInfo.AppointmentDate)) < 0)
                dtpTestDate.MinDate = DateTime.Now;
            else
                dtpTestDate.MinDate = TestAppointmentInfo.AppointmentDate;

            dtpTestDate.Value = TestAppointmentInfo.AppointmentDate;

            lblFees.Text = TestAppointmentInfo.PaidFees.ToString();


            if (TestAppointmentInfo.RetakeTestApplicationID != -1)
            {
                lblRetakeTestAppID.Text = TestAppointmentInfo.RetakeTestApplicationID.ToString();
                lblRetakeAppFees.Text = TestAppointmentInfo.RetakeTestApplicationInfo.PaidFees.ToString();
            }
            else
            {
                lblRetakeTestAppID.Text = "N/A";
                lblRetakeAppFees.Text   = "0";
            }

                return true;
        }

        private bool _LoadLocalApplication()
        {
            LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);

            if (LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show($"Error : No local driving licence application with id = {LocalDrivingLicenseApplicationID} was found",
                                "Error : Application Not Found",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }

            return true;
        }

        private void _FillLocalApplicationDataIntoForm()
        {
            lblLocalDrivingLicenseAppID.Text = LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = clsLicenseClass.Find(LocalDrivingLicenseApplication.LicenseClassID).ClassName.ToString();
            lblFullName.Text = LocalDrivingLicenseApplication.ApplicantPersonInfo.FullName;
            lblTrial.Text = LocalDrivingLicenseApplication.TotalTrialsPerTest(TestTypeID).ToString();
        }

        private void _SetupRetakeOrFirstTimeMode()
        {
            _CreationMode = LocalDrivingLicenseApplication.DoesAttendTestType(TestTypeID) ?
                            enCreationMode.ReTakeTestSchedule :
                            enCreationMode.FirstTimeSchedule;

            if (_CreationMode == enCreationMode.ReTakeTestSchedule)
            {
                lblRetakeAppFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).ApplicationFees.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                lblRetakeTestAppID.Text = "0";
            }
            else
            {
                lblRetakeAppFees.Text = "0";
                gbRetakeTestInfo.Enabled = false;
                lblTitle.Text = "Schedule Test";
                lblRetakeTestAppID.Text = "N/A";
            }
        }

        private bool _InitializeNewAppointment()
        {
            if (_CreationMode == enCreationMode.ReTakeTestSchedule)
                dtpTestDate.MinDate = clsTestAppointment.GetLastTestAppointment(LocalDrivingLicenseApplicationID, TestTypeID).AppointmentDate.Date.AddDays(1);
            else
                dtpTestDate.MinDate = DateTime.Now;

            lblFees.Text = clsTestType.FindTestTypeByID(_testTypeID).TestTypeFees.ToString();

            TestAppointmentInfo = new clsTestAppointment();

            if (!_HandleTestHasPassedConstraint())
                return false;

            if (!_HandleActiveTestAppointmentConstraint())
                return false;

            if (!_HandlePrviousTestConstraint())
                return false;

            return true;
        }

        private bool _InitializeFormData()
        {

            if(!_LoadLocalApplication())
                return false;

            // Local Application Values
            _FillLocalApplicationDataIntoForm();
            //--

            //Retake Test Values
            _SetupRetakeOrFirstTimeMode();
            //--

            //Test Appointment Values 
            if (this._Mode == enMode.AddNew)
            {
                if(!_InitializeNewAppointment())
                    return false;
            }
            //--

            return true;
        }

        // Handel Constraints
        private bool _HandleActiveTestAppointmentConstraint()
        {
            if (clsLocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(LocalDrivingLicenseApplicationID, TestTypeID))
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Person Already have an active appointment for this test";
                btnSave.Enabled = false;
                dtpTestDate.Enabled = false;
                return false;
            }
            else
                lblUserMessage.Visible = false;

            return true;
        }

        private bool _HandleAppointmentLockedConstraint()
        {
            //if appointment is locked that means the person already sat for this test
            //we cannot update locked appointment
            if (TestAppointmentInfo.IsLocked)
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "The person has already sat for the test, and the appointment is locked.";
                dtpTestDate.Enabled = false;
                btnSave.Enabled = false;
                return false;

            }
            else
                lblUserMessage.Visible = false;

            return true;
        }

        private bool _HandleTestHasPassedConstraint()
        {
            if(LocalDrivingLicenseApplication.DoesPassTestType(TestTypeID))
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "This applicant has already passed this test. Scheduling a new appointment is not allowed.";
                btnSave.Enabled = false;
                dtpTestDate.Enabled = false;
                return false;
            }

            return true;
        } 

        private bool _HandlePrviousTestConstraint()
        {
            //we need to make sure that this person passed the prvious required test before apply to the new test.
            //person cannno apply for written test unless s/he passes the vision test.
            //person cannot apply for street test unless s/he passes the written test.

            switch (TestTypeID)
            {
                case clsTestType.enTestTypes.VisionTest:
                    //in this case no required prvious test to pass.
                    lblUserMessage.Visible = false;

                    return true;

                case clsTestType.enTestTypes.WrittenTest:
                    //Written Test, you cannot sechdule it before person passes the vision test.
                    //we check if pass visiontest 1.
                    if (!LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestTypes.VisionTest))
                    {
                        lblUserMessage.Text = "Cannot Sechule, Vision Test should be passed first";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }

                    return true;

                case clsTestType.enTestTypes.StreetTest:

                    //Street Test, you cannot sechdule it before person passes the written test.
                    //we check if pass Written 2.
                    if (!LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestTypes.WrittenTest))
                    {
                        lblUserMessage.Text = "Cannot Sechule, Written Test should be passed first";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }

                    return true;

            }
            return true;

        }

        private bool _HandleRetakeApplication()
        {

            if (_Mode == enMode.AddNew && _CreationMode == enCreationMode.ReTakeTestSchedule)
            {
                clsApplication application =  new clsApplication();

                application.ApplicantPersonID = LocalDrivingLicenseApplication.ApplicantPersonID;
                application.ApplicationDate = DateTime.Now;
                application.ApplicationTypeID = clsApplication.enApplicationType.RetakeTest;
                application.ApplicationStatus = clsApplication.enStatus.Completed;
                application.LastStatusDate = DateTime.Now;
                application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).ApplicationFees;
                application.CreatedByUserID = clsGlobal.CurrentUserInfo.UserID;


                if (!application.Save())
                {
                    TestAppointmentInfo.RetakeTestApplicationID = -1;
                    MessageBox.Show("Faild to Create application", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                TestAppointmentInfo.RetakeTestApplicationID = application.ApplicationID;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!_HandleRetakeApplication())
                return;

            TestAppointmentInfo.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            TestAppointmentInfo.TestTypeID = TestTypeID;
            TestAppointmentInfo.AppointmentDate = dtpTestDate.Value;
            TestAppointmentInfo.PaidFees = Convert.ToSingle(lblFees.Text);
            TestAppointmentInfo.CreatedByUserID = clsGlobal.CurrentUserInfo.UserID;

            if (!TestAppointmentInfo.Save())
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _Mode = enMode.Update;
            MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            lblRetakeTestAppID.Text = TestAppointmentInfo.RetakeTestApplicationID.ToString();
        }
    }
}
