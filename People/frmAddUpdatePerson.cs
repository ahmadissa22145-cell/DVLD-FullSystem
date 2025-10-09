using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Properties;
using DVLD_Business;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Runtime.ConstrainedExecution;
using DVLD.Global_Classes;

namespace DVLD.People
{
    public partial class frmAddUpdatePerson : Form
    {

        public delegate void DataBackEventHandler(object sender, int personID);

        public event DataBackEventHandler DataBack;

        public enum enGender {Male = 0, Female = 1 }
        public enum enMode {AddNew = 0, Update = 1 }

        private enMode _Mode ;

        private int _PersonID = -1;

        private clsPerson _Person;

        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdatePerson(int personID)
        {
            InitializeComponent();

            _Mode = enMode.Update;
            _PersonID = personID;
        }

        private void _FillCountriesInComboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();

            foreach(DataRow row in dtCountries.Rows)
            {
                cbCountry.Items.Add(row["CountryName"]);
            }
        }

        private void _ResetDefualtValues()
        {
            _FillCountriesInComboBox();

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Person";
                _Person = new clsPerson();
                cbCountry.SelectedIndex = cbCountry.FindString("Jordan");
            }
            else
            {
                lblTitle.Text = "Update Person";
            }

            lblPersonID.Text = "N/A";

            txtFirstName.Text = txtSecondName.Text = txtThirdName.Text = txtLastName.Text =
            txtNationalNo.Text = txtEmail.Text = txtAddress.Text = txtPhone.Text = string.Empty;

            pbPersonImage.ImageLocation = null;

            pbPersonImage.Image = rbMale.Checked ? Resources.Male_512 
                                                            :
                                                   Resources.Female_512;


            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);

            llRemoveImage.Visible = false;  

        }

        private void _FillPersonInfo()
        {
            lblPersonID.Text = _PersonID.ToString();

            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtNationalNo.Text = _Person.NationalNo;
            txtEmail.Text = _Person.Email;
            txtAddress.Text = _Person.Address;
            txtPhone.Text = _Person.Phone;

            cbCountry.SelectedIndex = cbCountry.FindString(_Person.CountryInfo.CountryName);

            if (_Person.Gender == (byte)enGender.Male)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

            if (string.IsNullOrEmpty(_Person.ImagePath))

                pbPersonImage.Image = rbMale.Checked ? Resources.Male_512 : Resources.Female_512;

            else

                pbPersonImage.ImageLocation = _Person.ImagePath;

            llRemoveImage.Visible = (!string.IsNullOrEmpty(_Person.ImagePath));
        }

        private void _LoadData()
        {
            _Person = clsPerson.Find(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show($"Error occured : Not found any person with ID {_PersonID}","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
                return;
            }

            _FillPersonInfo();
        }

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if(_Mode == enMode.Update)
                _LoadData();    
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Title = "Select your image";
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.wep";
            openFileDialog1.Multiselect = false;
            openFileDialog1.FilterIndex = 1;

            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog1.FileName;
                pbPersonImage.Load(selectedFilePath);
                llRemoveImage.Visible = true;

            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;
            pbPersonImage.Image = rbMale.Checked ? Resources.Male_512 : Resources.Female_512;
            llRemoveImage.Visible = false;
        }

        private bool _HandlePersonImage()
        {
            // check if the image has been changed by comparing the current Image path with the picture box location
            if(_Person.ImagePath != pbPersonImage.ImageLocation)
            {

                if (!string.IsNullOrEmpty(_Person.ImagePath))
                {
                    try
                    {
                        if (File.Exists(_Person.ImagePath))
                                File.Delete(_Person.ImagePath);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(pbPersonImage.ImageLocation))
                {
                    string sourceImagePath = pbPersonImage.ImageLocation.ToString();

                    if (clsUtil.CopyImageToProjectImagesFolder(ref sourceImagePath))
                    {
                        pbPersonImage.ImageLocation = sourceImagePath;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_HandlePersonImage())
            {
                MessageBox.Show("An error occurred while handling the image.", "Handling Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int nationalityCountryID = clsCountry.Find(cbCountry.Text).ID;

            _Person.FirstName            = txtFirstName.Text.Trim();
            _Person.SecondName           = txtSecondName.Text.Trim();
            _Person.ThirdName            = txtThirdName.Text.Trim();
            _Person.LastName             = txtLastName.Text.Trim();
            _Person.NationalNo           = txtNationalNo.Text.ToUpper().Trim();
            _Person.Email                = txtEmail.Text.Trim();
            _Person.Phone                = txtEmail.Text.Trim();
            _Person.Address              = txtAddress.Text.Trim();
            _Person.DateOfBirth          = dtpDateOfBirth.Value;
            _Person.NationalityCountryID = nationalityCountryID;

            _Person.Gender = rbMale.Checked ? (byte) enGender.Male : (byte) enGender.Female;

            if (pbPersonImage.ImageLocation != null)
                _Person.ImagePath = pbPersonImage.ImageLocation;
            else
                _Person.ImagePath = string.Empty;

            if (!_Person.Save())
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);


            if (_Mode == enMode.AddNew)
            {
                _Mode            = enMode.Update;
                _PersonID        = _Person.PersonID;
                lblTitle.Text    = "Update Person";
                lblPersonID.Text = _Person.PersonID.ToString();
            }

            DataBack?.Invoke(this, _Person.PersonID);

        }

        private void TextBox_Validating(object sender, CancelEventArgs e)
        {

            TextBox textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                errorProvider1.SetError(textBox, "This field is required!");
                e.Cancel = true;
            }
            else
                errorProvider1.SetError(textBox, null);
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            string nationalNo = txtNationalNo.Text.ToUpper().Trim();

            if (!nationalNo.StartsWith("N"))
            {
                errorProvider1.SetError(txtNationalNo, "National Number must begin with 'N'");
                e.Cancel = true;
                return;
            }

            if (nationalNo.Length <= 1)
            {
                errorProvider1.SetError(txtNationalNo, "National Number must be longer than 1 Charcter");
                e.Cancel = true;
                return;
            }

            if (!txtNationalNo.Text.Equals(_Person.NationalNo) && clsPerson.isPersonExist(nationalNo))
            {
                errorProvider1.SetError(txtNationalNo, "National Number is used for another person!");
                e.Cancel = true;
                return;
            }

            errorProvider1.SetError(txtNationalNo, null);
            e.Cancel = false;
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            string emailAddress = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(emailAddress))
                return;

            if (!clsValidation.ValidateEmail(emailAddress))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format");
                return;
            }

            errorProvider1.SetError(txtEmail, null);
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(pbPersonImage.ImageLocation))
                pbPersonImage.Image = Resources.Male_512;
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(pbPersonImage.ImageLocation))
                pbPersonImage.Image = Resources.Female_512;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
