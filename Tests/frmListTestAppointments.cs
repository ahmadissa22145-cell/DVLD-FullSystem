using DVLD.Properties;
using DVLD.Tests.Schedule_Test;
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

namespace DVLD.Tests
{
    public partial class frmListTestAppointments : Form
    {
        private clsTestType.enTestTypes _testType = clsTestType.enTestTypes.VisionTest;

        public clsTestType.enTestTypes TestType
        {
            get { return _testType; }

            set 
            { 
                _testType = value;

                switch (_testType)
                {
                    case clsTestType.enTestTypes.VisionTest:
                        this.Text = "Vision Test Appointments";
                        lblTitle.Text = "WVisionritten Test Appointments";
                        pbTestTypeImage.Image = Resources.Vision_512;
                        break;

                    case clsTestType.enTestTypes.WrittenTest:
                        this.Text = "Written Test Appointments";
                        lblTitle.Text = "Written Test Appointments";
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        break;

                    case clsTestType.enTestTypes.StreetTest:
                        this.Text = "Street Test Appointments";
                        lblTitle.Text = "Street Test Appointments";
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        break;

                }
            }
        }


        int _LocalDrivingLicenseApplicationID = -1;

        private DataTable _dtTestAppointments;


        public frmListTestAppointments(int localDrivingLicenseApplicationID, clsTestType.enTestTypes testType)
        {
            InitializeComponent();

            TestType = testType;
            _LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
        }

        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            ctrlDrivingLicenseApplicationInfo1.LoadByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);
            RefreshData();
        }

        private void RefreshData()
        {
            _dtTestAppointments = clsTestAppointment.GetAllTestAppointmentsPerTestType(_LocalDrivingLicenseApplicationID, TestType);

            dgvLicenseTestAppointments.DataSource = _dtTestAppointments;

            lblRecordsCount.Text = dgvLicenseTestAppointments.Rows.Count.ToString();

            if(dgvLicenseTestAppointments.Rows.Count > 0)
            {
                dgvLicenseTestAppointments.Columns[0].HeaderText = "Test Appointment ID";
                dgvLicenseTestAppointments.Columns[0].Width = 180;

                dgvLicenseTestAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvLicenseTestAppointments.Columns[1].Width = 180;

                dgvLicenseTestAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvLicenseTestAppointments.Columns[2].Width = 120;

                dgvLicenseTestAppointments.Columns[3].HeaderText = "Created By UserID";
                dgvLicenseTestAppointments.Columns[3].Width = 170;

                dgvLicenseTestAppointments.Columns[4].HeaderText = "Is Locked";
                dgvLicenseTestAppointments.Columns[4].Width = 130;
            }
        }

        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {

            if (clsLocalDrivingLicenseApplication.DoesPassTestType(_LocalDrivingLicenseApplicationID, TestType))
            {
                MessageBox.Show("This person has already passed this test. Scheduling a new appointment is not allowed.",
                                "Not Allowed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }


            if (clsLocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_LocalDrivingLicenseApplicationID, TestType))
            {
                MessageBox.Show("Person Already have an active appointment for this test, You cannot add new appointment",
                    "Not allowed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            frmScheduleTest scheduleTest = new frmScheduleTest(_LocalDrivingLicenseApplicationID, TestType);
            scheduleTest.ShowDialog();

            RefreshData();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int testAppointmentID = (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;

            frmScheduleTest scheduleTest = new frmScheduleTest(_LocalDrivingLicenseApplicationID, TestType, testAppointmentID);
            scheduleTest.ShowDialog();

            RefreshData();
        }

        private void dgvLicenseTestAppointments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int testAppointmentID = (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;

            frmScheduleTest scheduleTest = new frmScheduleTest(_LocalDrivingLicenseApplicationID, TestType, testAppointmentID);
            scheduleTest.ShowDialog();

            RefreshData();
        }


        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int testAppointmentID = (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;

            frmTakeTest frm = new frmTakeTest(testAppointmentID, TestType);
            frm.ShowDialog();

            RefreshData();
        }
    }
}
