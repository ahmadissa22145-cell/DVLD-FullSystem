using DVLD.Licenses.Local_Licenses;
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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Drivers
{
    public partial class ctrlDriverLicenses : UserControl
    {
        private int _driverID;

        private DataTable _dtLocalLicenes = new DataTable();

        private DataTable _dtInternationalLicenes  = new DataTable();

        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }


        private void _LoadDgvLocalLicenses()
        {
            _dtLocalLicenes = clsLicense.GetDriverLicenses(_driverID);

            dgvLocalLicensesHistory.DataSource = _dtLocalLicenes;

            lblLocalLicensesRecords.Text = dgvLocalLicensesHistory.Rows.Count.ToString();

            if (dgvLocalLicensesHistory.Rows.Count > 0)
            {
                dgvLocalLicensesHistory.Columns[0].HeaderText = "Lic.ID";
                dgvLocalLicensesHistory.Columns[0].Width = 110;

                dgvLocalLicensesHistory.Columns[1].HeaderText = "App.ID";
                dgvLocalLicensesHistory.Columns[1].Width = 110;

                dgvLocalLicensesHistory.Columns[2].HeaderText = "Class Name";
                dgvLocalLicensesHistory.Columns[2].Width = 270;

                dgvLocalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicensesHistory.Columns[3].Width = 170;

                dgvLocalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicensesHistory.Columns[4].Width = 170;

                dgvLocalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvLocalLicensesHistory.Columns[5].Width = 90;
            }
        }

        private void _LoadDgvInternationalLicenses()
        {
            _dtInternationalLicenes = clsInternationalLicense.GetAllInternationalLicensesByDriverID(_driverID);

            dgvInternationalLicensesHistory.DataSource = _dtInternationalLicenes;

            lblInternationalLicensesRecords.Text = dgvInternationalLicensesHistory.Rows.Count.ToString();

            if (dgvInternationalLicensesHistory.Rows.Count > 0)
            {
                dgvInternationalLicensesHistory.Columns[0].HeaderText = "Int.LicenseID";
                dgvInternationalLicensesHistory.Columns[0].Width = 160;

                dgvInternationalLicensesHistory.Columns[1].HeaderText = "App.ID";
                dgvInternationalLicensesHistory.Columns[1].Width = 130;

                dgvInternationalLicensesHistory.Columns[2].HeaderText = "L.License ID";
                dgvInternationalLicensesHistory.Columns[2].Width = 130;

                dgvInternationalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvInternationalLicensesHistory.Columns[3].Width = 180;

                dgvInternationalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvInternationalLicensesHistory.Columns[4].Width = 180;

                dgvInternationalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvInternationalLicensesHistory.Columns[5].Width = 120;
            }
        }

        public void LoadData(int driverID)
        {

            _driverID = driverID;

            if(clsDriver.Find(_driverID) == null)
            {
                MessageBox.Show($"Could not found driver with ID = {driverID} , please try with another ID", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LoadDgvLocalLicenses();
            _LoadDgvInternationalLicenses();
        }

        public void LoadDataByPersonID(int personID)
        {

            clsDriver driver = clsDriver.GetByPersonID(personID);

            if (driver == null)
            {
                MessageBox.Show($"Could not found driver linked with person ID = {personID} , please try with another ID", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _driverID = driver.DriverID;

            _LoadDgvLocalLicenses();
            _LoadDgvInternationalLicenses();

        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int licenseID = (int)dgvLocalLicensesHistory.CurrentRow.Cells[0]?.Value;

            frmShowLicenseInfo frmShowLicense = new frmShowLicenseInfo(licenseID);
            frmShowLicense.ShowDialog();
        }

        public void Clear()
        {
            _driverID = -1;
            _dtLocalLicenes.Clear();
            _dtInternationalLicenes.Clear();
        }
    }
}
