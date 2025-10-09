using DVLD.Properties;
using DVLD_Buisness;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests.Controls
{
    public partial class ctrlScheduledTest : UserControl
    {

        public int TestAppointmentID {  get; private set; }

        public clsTestAppointment TestAppointment { get; private set; }


        private clsTestType.enTestTypes _testType = clsTestType.enTestTypes.VisionTest;

        public clsTestType.enTestTypes TestType 
        {
            get {  return _testType; }
            set
            {
                _testType = value;

                switch (_testType)
                {
                    case clsTestType.enTestTypes.VisionTest:
                        pbTestTypeImage.Image = Resources.Vision_512;
                        gbTestType.Text = "Vision Test";
                        break;

                    case clsTestType.enTestTypes.WrittenTest:
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        gbTestType.Text = "Written Test";
                        break;

                    case clsTestType.enTestTypes.StreetTest:
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        gbTestType.Text = "Street Test";
                        break;
                }
            }
        }

        public int TestID {  get; private set; }
        public ctrlScheduledTest()
        {
            InitializeComponent();
        }


        public void LoadInfo(int testAppointmentID, clsTestType.enTestTypes testType)
        {
            TestAppointmentID = testAppointmentID;
            TestType = testType;

            if (!_LoadTestAppointment())
                return;

            TestID = TestAppointment.TestID;

            lblLocalDrivingLicenseAppID.Text = TestAppointment.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = TestAppointment.LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblFullName.Text = TestAppointment.LocalDrivingLicenseApplication.ApplicantPersonInfo.FullName;
            lblTrial.Text = TestAppointment.LocalDrivingLicenseApplication.TotalTrialsPerTest(TestType).ToString();
            lblDate.Text  = TestAppointment.AppointmentDate.ToString();
            lblFees.Text = TestAppointment.PaidFees.ToString();
            lblTestID.Text = TestAppointment.TestID > 0 ? TestID.ToString() : "Not Taken Yet";

        }

        public void SetTestID(int testID)
        {
            TestID = testID;
            lblTestID.Text = TestID.ToString();
        }

        private bool _LoadTestAppointment()
        {
            TestAppointment = clsTestAppointment.Find(TestAppointmentID);

            if (TestAppointment == null)
            {
                MessageBox.Show($"Error : No Test Appointment with id = {TestAppointmentID} was found",
                                "Error : Test Appointment Not Found",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                TestAppointmentID = -1;
                return false;
            }
            return true;
        }
    }
}
