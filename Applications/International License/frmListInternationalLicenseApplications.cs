using DVLD.Applications.New_International_License;
using DVLD.Licenses;
using DVLD.Licenses.International_License;
using DVLD.People;
using DVLD_Buisness;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.International_License
{
    public partial class frmListInternationalLicenseApplications : Form
    {
        DataTable _dtInternationalLicenseApplications;
        public frmListInternationalLicenseApplications()
        {
            InitializeComponent();
        }

        private void frmListInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
           
            _dtInternationalLicenseApplications = clsInternationalLicense.GetAllInternationalLicenses();

            dgvInternationalLicenses.DataSource = _dtInternationalLicenseApplications;
            lblInternationalLicensesRecords.Text = dgvInternationalLicenses.RowCount.ToString();
            cbFilterBy.SelectedIndex = 0;

            if (dgvInternationalLicenses.Rows.Count > 0)
            {
                dgvInternationalLicenses.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicenses.Columns[0].Width = 160;

                dgvInternationalLicenses.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicenses.Columns[1].Width = 150;

                dgvInternationalLicenses.Columns[2].HeaderText = "Driver ID";
                dgvInternationalLicenses.Columns[2].Width = 130;

                dgvInternationalLicenses.Columns[3].HeaderText = "L.License ID";
                dgvInternationalLicenses.Columns[3].Width = 130;

                dgvInternationalLicenses.Columns[4].HeaderText = "Issue Date";
                dgvInternationalLicenses.Columns[4].Width = 180;

                dgvInternationalLicenses.Columns[5].HeaderText = "Expiration Date";
                dgvInternationalLicenses.Columns[5].Width = 180;

                dgvInternationalLicenses.Columns[6].HeaderText = "Is Active";
                dgvInternationalLicenses.Columns[6].Width = 120;

            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbFilterBy.Text)
            {
                case "None":
                    txtFilterValue.Visible = false;
                    cbIsReleased.Visible = false;
                    break;

                case "Is Active":
                    txtFilterValue.Visible = false;
                    cbIsReleased.Visible = true;
                    cbIsReleased.SelectedIndex = 0;
                    break;

                default:
                    txtFilterValue.Visible = true;
                    txtFilterValue.Text = string.Empty;
                    cbIsReleased.Visible = false;
                    break;
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterby = _GetOrginalFilterName();
            string filterValue = txtFilterValue.Text.Trim();

            if(filterby.Equals("None") || string.IsNullOrEmpty(filterValue))
            {
                _dtInternationalLicenseApplications.DefaultView.RowFilter = string.Empty;
                lblInternationalLicensesRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();
                return;
            }

            _dtInternationalLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", filterby, filterValue);
            lblInternationalLicensesRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();
        }


        private string _GetOrginalFilterName()
        {
            switch (cbFilterBy.Text)
            {
                case "International License ID":
                    return "InternationalLicenseID";

                case "Application ID":
                    return "ApplicationID";

                case "Driver ID":
                    return "DriverID";

                case "Local License ID":
                    return "IssuedUsingLocalLicenseID";

                case "Is Active":
                    return "IsActive";

                default:
                    return "None";
            }
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filterby = _GetOrginalFilterName();
            string filterValue = cbIsReleased.Text;

            if (filterValue.Equals("All"))
            {
                _dtInternationalLicenseApplications.DefaultView.RowFilter = string.Empty;
              
            } 
            else if (filterValue.Equals("Yes"))
            {
                _dtInternationalLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = true", filterby);
            }
            else
            {
                _dtInternationalLicenseApplications.DefaultView.RowFilter = "IsActive = false";
            }

            lblInternationalLicensesRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();
        }

        private void btnNewApplication_Click(object sender, EventArgs e)
        { 
            frmNewInternationalLicense frm = new frmNewInternationalLicense();
            frm.ShowDialog();
        }

        private void PesonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int driverID = (int)dgvInternationalLicenses.CurrentRow.Cells[2].Value;

            clsDriver driver = clsDriver.Find(driverID);
            frmShowPersonInfo frm = new frmShowPersonInfo(driver.PersonID);
            frm.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int internationalLicenseID = (int)dgvInternationalLicenses.CurrentRow.Cells[0].Value;
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(internationalLicenseID);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int driverID = (int)dgvInternationalLicenses.CurrentRow.Cells[2].Value;

            clsDriver driver = clsDriver.Find(driverID);
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(driver.PersonID);
            frm.ShowDialog();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
