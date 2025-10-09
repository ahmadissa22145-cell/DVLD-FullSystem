using DVLD.Tests.Controls;
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

namespace DVLD.Tests.Schedule_Test
{
    public partial class frmScheduleTest : Form
    {
        int _LocalDrivingLicenseApplicationID { get; set; } = -1;

        int _TestAppointmentID = -1;

        clsTestType.enTestTypes _TestTypeID = clsTestType.enTestTypes.VisionTest;

        public frmScheduleTest(int localDrivingLicenseApplicationID, clsTestType.enTestTypes testTypeID, int testAppointmentID = -1)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            _TestAppointmentID = testAppointmentID;
            _TestTypeID = testTypeID;
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            ctrlScheduleTest1.TestTypeID = _TestTypeID;
            ctrlScheduleTest1.LoadInfo(_LocalDrivingLicenseApplicationID, _TestAppointmentID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
