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


namespace DVLD.Tests.Test_Types
{
    public partial class frmEditTestType : Form
    {
        private clsTestType.enTestTypes _testTypeID ;

        private clsTestType _testType;
        public frmEditTestType(clsTestType.enTestTypes testTypeID)
        {
            InitializeComponent();

            _testTypeID = testTypeID;
        }


        private void _fillTestTypeInfo()
        {
            lblTestTypeID.Text = ((int)_testTypeID).ToString();
            txtTitle.Text = _testType.TestTypeTitle.ToString();
            txtDescription.Text = _testType.TestTypeDescription.ToString();
            txtFees.Text = _testType.TestTypeFees.ToString();

        }
        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            _testType = clsTestType.FindTestTypeByID(_testTypeID);

            if(_testType == null)
            {
                MessageBox.Show($"Not found any test type with ID = {_testType}",
                              "Test Type Not Found",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);

                this.Close();
                return;
            }

            _fillTestTypeInfo();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valid!, put the mouse over the red icon(s) to see the error",
                                "Validation Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            _testType.TestTypeTitle = txtTitle.Text.Trim().ToString();
            _testType.TestTypeDescription = txtDescription.Text.Trim().ToString();
            _testType.TestTypeFees = Convert.ToSingle(txtFees.Text.Trim());

            if (!_testType.UpdateTestTypes())
            {
                MessageBox.Show($"An error occurred : Test Type info was not updated",
                                 "Test Type was not updated",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);

                return;
            }

            MessageBox.Show($"Test Type info was updated successfully",
                             "Updated successfully",
                             MessageBoxButtons.OK,
                             MessageBoxIcon.Information);
        }

        private void TextBox_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            string text = textBox.Text.Trim();

            if(text == string.Empty)
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox, "This field is requried");
                return;
            }

            errorProvider1.SetError(textBox, null);
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            string fees = txtFees.Text.Trim();

            if (txtFees.Text == string.Empty)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "This field is requried");
                return;
            }

            if (!clsValidation.IsNumber(fees))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "this format is invalid");
                return;
            }

            errorProvider1.SetError(txtFees, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.AutoValidate = AutoValidate.Disable;
            this.Close();
        }

        
    }
}
