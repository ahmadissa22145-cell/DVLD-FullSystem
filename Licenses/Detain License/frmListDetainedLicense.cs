using DVLD.Licenses.Local_Licenses;
using DVLD.People;
using DVLD_Buisness;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses.Detain_License
{
    public partial class frmListDetainedLicense : Form
    {
        private DataTable _dtDetainedLicenses;
        public frmListDetainedLicense()
        {
            InitializeComponent();
        }

        private void frmListDetainedLicense_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            _dtDetainedLicenses = clsDetainLicense.GetAllDetainedLicenses();

            dgvDetainedLicenses.DataSource = _dtDetainedLicenses;

            lblTotalRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();

            if(dgvDetainedLicenses.Rows.Count > 0)
            {
                if (dgvDetainedLicenses.Rows.Count > 0)
                {
                    dgvDetainedLicenses.Columns[0].HeaderText = "D.ID";
                    dgvDetainedLicenses.Columns[0].Width = 90;

                    dgvDetainedLicenses.Columns[1].HeaderText = "L.ID";
                    dgvDetainedLicenses.Columns[1].Width = 90;

                    dgvDetainedLicenses.Columns[2].HeaderText = "D.Date";
                    dgvDetainedLicenses.Columns[2].Width = 160;

                    dgvDetainedLicenses.Columns[3].HeaderText = "Is Released";
                    dgvDetainedLicenses.Columns[3].Width = 110;

                    dgvDetainedLicenses.Columns[4].HeaderText = "Fine Fees";
                    dgvDetainedLicenses.Columns[4].Width = 110;

                    dgvDetainedLicenses.Columns[5].HeaderText = "Release Date";
                    dgvDetainedLicenses.Columns[5].Width = 160;

                    dgvDetainedLicenses.Columns[6].HeaderText = "N.No.";
                    dgvDetainedLicenses.Columns[6].Width = 90;

                    dgvDetainedLicenses.Columns[7].HeaderText = "Full Name";
                    dgvDetainedLicenses.Columns[7].Width = 330;

                    dgvDetainedLicenses.Columns[8].HeaderText = "Rlease App.ID";
                    dgvDetainedLicenses.Columns[8].Width = 150;

                }
            }

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbFilterBy.Text)
            {
                case"None":
                    txtFilterValue.Visible = false; 
                    cbIsReleased.Visible = false; 
                    break;

                case "Is Released":
                    cbIsReleased.SelectedIndex = 0;
                    txtFilterValue.Visible = false; 
                    cbIsReleased.Visible = true; 
                    break;

                default:
                    txtFilterValue.Text = string.Empty;
                    txtFilterValue.Visible = true;
                    cbIsReleased.Visible = false;
                    break;
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterValue = txtFilterValue.Text.Trim();
            string filterBy = _GetOrginalFilterName();

            _dtDetainedLicenses.DefaultView.RowFilter = _GetRowFilterStatment(filterBy, filterValue);
            lblTotalRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text.Equals("Detain ID") || cbFilterBy.Text.Equals("Release Application ID"))
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
            else
            {
                e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar);
            }
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filterValue = cbIsReleased.Text;
            string filterBy = _GetOrginalFilterName();
            string rowFilter = string.Empty;

            if (filterValue.Equals("All"))
            {
                rowFilter = string.Empty;

            }
            else if (filterValue.Equals("Yes"))
            {
                rowFilter = string.Format("[{0}] = true", filterBy);
            }
            else
            {
                rowFilter = string.Format("[{0}] = false", filterBy);
            }

            _dtDetainedLicenses.DefaultView.RowFilter = rowFilter;
            lblTotalRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();
        }

        private string _GetOrginalFilterName()
        {
            switch (cbFilterBy.Text)
            {
                case "Detain ID":
                    return "DetainID";

                case "Is Released":
                    return "IsReleased";

                case "National No.":
                    return "NationalNo";

                case "Full Name":
                    return "FullName";

                case "Release Application ID":
                    return "ReleaseApplicationID";

                default:
                    return "None";
            }
        }

        private string _GetRowFilterStatment(string filterBy, string filterValue)
        {
            string rowFilter = string.Empty;

            if (filterBy.Equals("None") || string.IsNullOrEmpty(filterValue))
            {
                rowFilter = string.Empty;

            }
            else if (filterBy.Equals("DetainID") || filterBy.Equals("ReleaseApplicationID"))
            {
                rowFilter = string.Format("[{0}] = {1}", filterBy, filterValue);
            }
            else
            {
                rowFilter = string.Format("[{0}] LIKE '{1}%'", filterBy, filterValue);
            }

            return rowFilter;
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();
        }

        private void btnReleaseDetainedLicense_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.ShowDialog();
        }

        private void PesonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string NationalNo = dgvDetainedLicenses.CurrentRow.Cells[6].Value.ToString();

            frmShowPersonInfo frm = new frmShowPersonInfo(NationalNo);
            frm.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int licneseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;

            frmShowLicenseInfo frm = new frmShowLicenseInfo(licneseID);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string NationalNo = dgvDetainedLicenses.CurrentRow.Cells[6].Value.ToString();

            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(clsPerson.Find(NationalNo).PersonID);
            frm.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int licneseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;

            if (!clsLicense.Find(licneseID).IsActive)
            {
                MessageBox.Show("The license is inactive and cannot be released",
                               "Information",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense(licneseID);
            frm.ShowDialog();
        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            bool isReleased = (bool)dgvDetainedLicenses.CurrentRow.Cells[3].Value;

            releaseDetainedLicenseToolStripMenuItem.Enabled = !isReleased;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
