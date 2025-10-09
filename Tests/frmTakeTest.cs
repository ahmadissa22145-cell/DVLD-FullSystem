using DVLD.Global_Classes;
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

namespace DVLD.Tests
{
    public partial class frmTakeTest : Form
    {
        public enum enMode {AddNew, Update };
        private enMode _mode;

        int _testAppointmentID = -1;

        clsTestAppointment _TestAppointment { get { return ctrlScheduledTest1.TestAppointment; } }


        clsTestType.enTestTypes _testType = clsTestType.enTestTypes.VisionTest;
        clsTest _test;
        int _TestID { get { return ctrlScheduledTest1.TestID; } }

        public frmTakeTest(int testAppointmentID, clsTestType.enTestTypes testType)
        {
            InitializeComponent();
            _testAppointmentID = testAppointmentID;
            _testType = testType;
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlScheduledTest1.LoadInfo(_testAppointmentID, _testType);

            if (ctrlScheduledTest1.TestAppointmentID == -1) 
            {
                btnSave.Enabled = false;
                return;
            }


            if (_TestID != -1)
            {
                _mode = enMode.Update;
                _test = clsTest.Find(_TestID);

                if (_test.TestResult)
                {
                    rbPass.Checked = true;
                }
                else
                {
                    rbFail.Checked = true;
                }

                txtNotes.Text = _test.Notes;

                lblUserMessage.Visible = false;
                rbPass.Enabled = false;
                rbFail.Enabled = false;
            }
            else
            {
                _mode = enMode.AddNew;
                _test = new clsTest();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (_mode == enMode.AddNew)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to save? After that you cannot change the Pass/Fail results after you save?.",
                                                 "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }

                _test.TestAppointmentID = _testAppointmentID;
                _test.TestResult = rbPass.Checked;
                _test.CreatedByUserID = clsGlobal.CurrentUserInfo.UserID;
            }
         
            _test.Notes = txtNotes.Text.Trim();

            if (!_test.Save())
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ctrlScheduledTest1.SetTestID(_test.TestID);
            _TestAppointment.IsLocked = true;

            lblUserMessage.Visible = false;
            rbPass.Enabled = false;
            rbFail.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
