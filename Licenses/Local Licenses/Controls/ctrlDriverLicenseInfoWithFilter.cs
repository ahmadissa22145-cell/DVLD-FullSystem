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

namespace DVLD.Licenses.Local_Licenses.Controls
{
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl
    {

        public event Action<int> OnLicenseSelected;
        public int LicenseID { get { return ctrlDriverLicenseInfo1.LicenseID; } }

        public clsLicense SelectedLicenseInfo { get { return ctrlDriverLicenseInfo1.SelectedLicenseInfo; } }

        public bool FilterEnabled 
        {
            get { return gbFilters.Enabled; }

            set
            {
                gbFilters.Enabled = value;
            }
        }

        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
        }

        public void ResetDefaultValues()
        {
            txtLicenseID.Text = string.Empty;

            ctrlDriverLicenseInfo1.ResetDefaultValues();

            txtLicenseID.Focus();
        }

        public void LoadLicenseInfo(int licenseID)
        {
            ctrlDriverLicenseInfo1.LoadControlInfo(licenseID);

            if (SelectedLicenseInfo == null)
            {
                _ClearLicenseID();
                return;
            }

            txtLicenseID.Text = LicenseID.ToString();
            txtLicenseID.Focus();

            OnLicenseSelected?.Invoke(this.LicenseID);
        }

        public void txtLicenseIDFocus()
        {
            txtLicenseID.Focus();
        }

        private void _ClearLicenseID()
        {
            txtLicenseID.Text = string.Empty;

            txtLicenseID.Focus();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                                "Validation Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtLicenseID.Focus();
                return;
            }

            int licenseID = Convert.ToInt32(txtLicenseID.Text);

            LoadLicenseInfo(licenseID);

           
            
        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnFind.PerformClick();
                return;
            }

            e.Handled = (!char.IsDigit(e.KeyChar)) && (!char.IsControl(e.KeyChar));
        }

        private void txtLicenseID_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLicenseID.Text))
            {
                errorProvider1.SetError(txtLicenseID, "cannot be a blank");
                return;
            }

            errorProvider1.SetError(txtLicenseID, null);
        }
    }
}
