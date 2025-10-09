using DVLD.Licenses;
using DVLD.People;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Drivers
{
    public partial class frmListDrivers : Form
    {
        private DataTable _dtDrivers = new DataTable();
        public frmListDrivers()
        {
            InitializeComponent();
        }

        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            _dtDrivers = clsDriver.GetAllDrivers();

            dgvDrivers.DataSource = _dtDrivers;

            lblRecordsCount.Text = dgvDrivers.Rows.Count.ToString();

            if(dgvDrivers.Rows.Count > 0)
            {
                dgvDrivers.Columns[0].HeaderText = "Driver ID";
                dgvDrivers.Columns[0].Width = 120;

                dgvDrivers.Columns[1].HeaderText = "Person ID";
                dgvDrivers.Columns[1].Width = 120;

                dgvDrivers.Columns[2].HeaderText = "National No";
                dgvDrivers.Columns[2].Width = 120;

                dgvDrivers.Columns[3].HeaderText = "Full Name";
                dgvDrivers.Columns[3].Width = 300;

                dgvDrivers.Columns[4].HeaderText = "Date";
                dgvDrivers.Columns[4].Width = 250;

                dgvDrivers.Columns[5].HeaderText = "Active Licenses";
                dgvDrivers.Columns[5].Width = 150;
            }
        }

        private string _GetOrginalFilterName()
        {
            switch (cbFilterBy.Text)
            {
                case "Driver ID":
                    return "DriverID";

                case "Person ID":
                    return "PersonID";

                case "National No.":
                    return "NationalNo";

                case "Full Name":
                    return "FullName";

                case "Date":
                    return "CreatedDate";

                case "Active Licenses":
                    return "NumberOfActiveLicenses";

                default:
                    return string.Empty;
            }
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                dgvDrivers.Focus();
                return;
            }

            if(cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "Driver ID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
            else if (cbFilterBy.Text == "Full Name")
            {
                e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar);
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterBy = _GetOrginalFilterName();
            string filterValue = txtFilterValue.Text.Trim();

            if (string.IsNullOrEmpty(filterValue))
            {
                _dtDrivers.DefaultView.RowFilter = string.Empty;
                lblRecordsCount.Text = dgvDrivers.Rows.Count.ToString();
                return;
            }

            if(cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "Driver ID")
            {
                _dtDrivers.DefaultView.RowFilter = string.Format("[{0}] = {1}", filterBy, filterValue);
            }
            else
            {
                _dtDrivers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", filterBy, filterValue);
            }

            lblRecordsCount.Text = dgvDrivers.Rows.Count.ToString();
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
                    txtFilterValue.Focus();
                    break;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personID = (int)dgvDrivers.CurrentRow.Cells[1].Value;
            frmShowPersonInfo showPersonInfo = new frmShowPersonInfo(personID);
            showPersonInfo.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personID = (int) dgvDrivers.CurrentRow.Cells[1].Value;
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(personID);
            frm.ShowDialog();
        }
    }
}
