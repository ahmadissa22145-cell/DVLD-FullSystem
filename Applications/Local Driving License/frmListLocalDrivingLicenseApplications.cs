using DVLD.Global_Classes;
using DVLD.Licenses;
using DVLD.Licenses.Detain_License;
using DVLD.Licenses.Local_Licenses;
using DVLD.Tests;
using DVLD.Tests.Schedule_Test;
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
using System.Xml.Linq;

namespace DVLD.Applications.Local_Driving_License
{
    public partial class frmListLocalDrivingLicenseApplications : Form
    {

        private DataTable _dtLocalDrivingLicenseApplications;
        public frmListLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }

        private void frmListLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            _dtLocalDrivingLicenseApplications = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplicationID();

            dgvLocalDrivingLicenseApplications.DataSource = _dtLocalDrivingLicenseApplications;

            int recordsCount = dgvLocalDrivingLicenseApplications.Rows.Count;
            lblRecordsCount.Text = recordsCount.ToString();

           
            if(recordsCount < 0)
            {
                MessageBox.Show("Not retrive any data please check your connection or database",
                                "Data Not Found",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dgvLocalDrivingLicenseApplications.Columns[0].HeaderText = "L.D.L.AppID";
            dgvLocalDrivingLicenseApplications.Columns[0].Width = 120;

            dgvLocalDrivingLicenseApplications.Columns[1].HeaderText = "Driving Class";
            dgvLocalDrivingLicenseApplications.Columns[1].Width = 300;

            dgvLocalDrivingLicenseApplications.Columns[2].HeaderText = "National No.";
            dgvLocalDrivingLicenseApplications.Columns[2].Width = 150;

            dgvLocalDrivingLicenseApplications.Columns[3].HeaderText = "Full Name";
            dgvLocalDrivingLicenseApplications.Columns[3].Width = 350;

            dgvLocalDrivingLicenseApplications.Columns[4].HeaderText = "Application Date";
            dgvLocalDrivingLicenseApplications.Columns[4].Width = 170;

            dgvLocalDrivingLicenseApplications.Columns[5].HeaderText = "Passed Tests";
            dgvLocalDrivingLicenseApplications.Columns[5].Width = 130;

            dgvLocalDrivingLicenseApplications.Columns[6].HeaderText = "Status";
            dgvLocalDrivingLicenseApplications.Columns[6].Width = 150;

            cbFilterBy.SelectedIndex = 0;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            switch (cbFilterBy.Text)
            {
                case "None":
                    txtFilterValue.Visible = false; 
                    break;

                default:
                    txtFilterValue.Visible = true; 
                    txtFilterValue.Clear();
                    break;
            }
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13) // Enter press
            {
                dgvLocalDrivingLicenseApplications.Focus();
            }

            switch (cbFilterBy.Text)
            {
                case "L.D.L.AppID":
                    e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
                    break;

                case "Full Name":
                case "Status":
                    e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar);
                    break;
            }
        }

        private string GetSelectedFilterColumnName()
        {
            switch (cbFilterBy.Text)
            {
                case "L.D.L.AppID":
                    return "LocalDrivingLicenseApplicationID";

                case "National No.":
                    return "NationalNo";

                case "Full Name":
                    return "FullName";

                default:
                   return cbFilterBy.Text;
            }
        }

        private string GenerateSearchQuery(string textFilterColumn, string textFilterValue)
        {
            string exactMatchFilter = string.Format("{0} = {1}", textFilterColumn, textFilterValue);

            string likeMatchFilter = string.Format("{0} Like '{1}%'", textFilterColumn, textFilterValue);

            return textFilterColumn == "LocalDrivingLicenseApplicationID" ? exactMatchFilter : likeMatchFilter;
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string textFilterValue = txtFilterValue.Text.Trim();
            string textFilterColumn = GetSelectedFilterColumnName();
            string queryFilter = string.Empty;
                                                                                 // Numeric columns use '=', text columns use LIKE
            queryFilter = string.IsNullOrEmpty(textFilterValue) ? string.Empty : GenerateSearchQuery(textFilterColumn, textFilterValue); 

            lblRecordsCount.Text = FilterHelper.ApplyFilter(queryFilter, _dtLocalDrivingLicenseApplications).ToString();
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication();
            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int l_D_L_AppID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            frmLocalDrivingLicenseApplicationInfo frm = new frmLocalDrivingLicenseApplicationInfo(l_D_L_AppID);
            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int l_D_L_AppID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication(l_D_L_AppID);
            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void DeleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure do you want delete this application",
                                                  "Confirm",
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                int l_D_L_AppID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

                clsLocalDrivingLicenseApplication localDrivingApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(l_D_L_AppID);

                if (!localDrivingApplication.Delete())
                {
                     MessageBox.Show("Could not delete application , other data depends on it",
                                     "Error : The Application Was Not Deleted",
                                     MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                MessageBox.Show("Application deleted successfully",
                                "Deleted",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                frmListLocalDrivingLicenseApplications_Load(null, null);
            }
        }

        private void CancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure do you want cancel this application",
                                                  "Confirm",
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                int l_D_L_AppID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

                clsLocalDrivingLicenseApplication localDrivingApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(l_D_L_AppID);

                if (!localDrivingApplication.Cancel())
                {
                    MessageBox.Show($"Could not cancel application",
                                   "Error : The Application Was Not Canceled",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Application canceled successfully",
                                "Canceled",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                frmListLocalDrivingLicenseApplications_Load(null, null);
            }

        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            int l_D_L_AppID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApplication localDrivingApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(l_D_L_AppID);
            clsLocalDrivingLicenseApplication.enStatus applicationStatus = localDrivingApplication.ApplicationStatus;

            if (applicationStatus == clsLocalDrivingLicenseApplication.enStatus.Completed) 
            {
                foreach(ToolStripItem tool in cmsApplications.Items)
                {
                    tool.Enabled = false;
                }

                showDetailsToolStripMenuItem.Enabled = true;
                showLicenseToolStripMenuItem.Enabled = true;
                showPersonLicenseHistoryToolStripMenuItem.Enabled = true;

                return;
            }

            if(applicationStatus == clsLocalDrivingLicenseApplication.enStatus.Cancelled)
            {
                foreach (ToolStripItem tool in cmsApplications.Items)
                {
                    tool.Enabled = false;
                }

                showDetailsToolStripMenuItem.Enabled = true;
                showPersonLicenseHistoryToolStripMenuItem.Enabled = true;

                return;
            }


            if(applicationStatus == clsLocalDrivingLicenseApplication.enStatus.New)
            {
                bool doesPassedVisionTest = localDrivingApplication.DoesPassTestType(clsTestType.enTestTypes.VisionTest);
                bool doesPassedWrittenTest = localDrivingApplication.DoesPassTestType(clsTestType.enTestTypes.WrittenTest);
                bool doesPassedStreetTest = localDrivingApplication.DoesPassTestType(clsTestType.enTestTypes.StreetTest);
                bool doesPassedAllTests = clsLocalDrivingLicenseApplication.PassedAllTests(l_D_L_AppID);

                showDetailsToolStripMenuItem.Enabled = true;
                editToolStripMenuItem.Enabled = !doesPassedVisionTest;
                DeleteApplicationToolStripMenuItem.Enabled = !doesPassedVisionTest;
                CancelApplicaitonToolStripMenuItem.Enabled = true;
                ScheduleTestsMenue.Enabled = (!doesPassedVisionTest) || (!doesPassedWrittenTest) || (!doesPassedStreetTest);

                bool hasLicense = localDrivingApplication.IsLicenseIssued();

                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (!hasLicense) && doesPassedAllTests;
                showLicenseToolStripMenuItem.Enabled = hasLicense;

 

                showPersonLicenseHistoryToolStripMenuItem.Enabled = true;

                if (ScheduleTestsMenue.Enabled)
                {
                    scheduleVisionTestToolStripMenuItem.Enabled = !doesPassedVisionTest;

                    scheduleWrittenTestToolStripMenuItem.Enabled = doesPassedVisionTest && !doesPassedWrittenTest;

                    scheduleStreetTestToolStripMenuItem.Enabled = doesPassedVisionTest && doesPassedWrittenTest && !doesPassedStreetTest;
                }
            }
           
          
        }

        private void scheduleTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;

            int localDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            int testType = Convert.ToInt32(toolStripMenuItem.Tag);

            frmListTestAppointments listTestAppointments = new frmListTestAppointments(localDrivingLicenseApplicationID, (clsTestType.enTestTypes)testType);
            listTestAppointments.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void dgvLocalDrivingLicenseApplications_DoubleClick(object sender, EventArgs e)
        {
            int l_D_L_AppID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            frmLocalDrivingLicenseApplicationInfo frm = new frmLocalDrivingLicenseApplicationInfo(l_D_L_AppID);
            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int l_D_L_AppID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            frmIssueLicenseForTheFirstTime frm = new frmIssueLicenseForTheFirstTime(l_D_L_AppID);
            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int l_D_L_AppID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            int licenseID = clsLocalDrivingLicenseApplication.
                            FindByLocalDrivingLicenseApplicationID(l_D_L_AppID).
                            GetActiveLicenseID();

            frmShowLicenseInfo frm = new frmShowLicenseInfo(licenseID);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string nationalNo = dgvLocalDrivingLicenseApplications.CurrentRow.Cells[2]?.Value.ToString();
            
            clsPerson person = clsPerson.Find(nationalNo);

            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(person.PersonID);
            frm.ShowDialog();
        }

        private void releaseLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int l_D_L_AppID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            int licenseID = clsLocalDrivingLicenseApplication.
                            FindByLocalDrivingLicenseApplicationID(l_D_L_AppID).
                            GetActiveLicenseID();



            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense(licenseID);
            frm.ShowDialog();
        }
    }
}
