using DVLD.Global_Classes;
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

namespace DVLD.Applications.Application_Types
{
    public partial class frmEditApplicationType : Form
    {

        int _applicationTypeID = -1;

        clsApplicationType _applicationType;

        public frmEditApplicationType(int applicationTypeID)
        {
            InitializeComponent();

            _applicationTypeID = applicationTypeID;
        }


        private void _FillApplicationTypeInfo()
        {
            lblApplicationTypeID.Text = _applicationType.ApplicationTypeID.ToString();
            txtTitle.Text = _applicationType.ApplicationTypeTitle.ToString();
            txtFees.Text = _applicationType.ApplicationFees.ToString();
        }

        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            
            _applicationType = clsApplicationType.Find(_applicationTypeID);

          
            if (_applicationType == null)
            {
                MessageBox.Show($"Not found any application type with ID = {_applicationTypeID}",
                                "Application Type Not Found",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                this.Close();
                return;
            }

            _FillApplicationTypeInfo();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _applicationType.ApplicationTypeTitle = txtTitle.Text.Trim();
            _applicationType.ApplicationFees = Convert.ToSingle(txtFees.Text.Trim());


            if (!_applicationType.UpdateApplicationType())
            {
                MessageBox.Show($"An error occurred : Application Type info was not updated",
                                 "Application Type was not updated",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);

                return;
            }

            MessageBox.Show($"Application Type info was updated successfully",
                             "Updated successfully",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Information);

        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            string fees = txtFees.Text.Trim();

            if (string.IsNullOrEmpty(fees))
            {
                errorProvider1.SetError(txtFees, "this field is required");
                return;
            }

            if (!clsValidation.IsNumber(fees))
            {
                errorProvider1.SetError(txtFees, "this format is invalid");
                return;
            }

            errorProvider1.SetError(txtFees, null);
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            string text = txtTitle.Text.Trim();

            if (string.IsNullOrEmpty(text))
            {
                errorProvider1.SetError(txtFees, "this field is required");
                return;
            }

            errorProvider1.SetError(txtFees, null);
        }
    }
}
