using DVLD.Properties;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.People.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        public enum enGender{Male, Female };


        private int _PersonID = -1;

        private clsPerson _Person;

        public int PersonID { get { return _PersonID; } }

        public clsPerson SelctedPersonInfo { get { return _Person; } }

        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        public void LoadPersonInfo(object sender, int personID)
        {
            _Person = clsPerson.Find(personID);

            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show($"No Person with person ID = {personID} was found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _PersonID = personID;
            _FillPersonInfo();
        }

        public void LoadPersonInfo(object sender, string nationalNumber)
        {
            _Person = clsPerson.Find(nationalNumber);

            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show($"No Person with national number = {nationalNumber} was found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _PersonID = _Person.PersonID;
            _FillPersonInfo();
        }

        private void _LoadPersonImage()
        {
            bool isMale = _Person.Gender == (short)enGender.Male;

            pbGender.Image = isMale ? Resources.Man_32 : Resources.Woman_32;

            pbPersonImage.Image = isMale ? Resources.Male_512 : Resources.Female_512;

            string imagePath = _Person.ImagePath;

            if (!string.IsNullOrEmpty(_Person.ImagePath))
            {
                if (File.Exists(imagePath))

                    pbPersonImage.ImageLocation = imagePath;

                else

                    MessageBox.Show($"Could not find this image : {imagePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void _FillPersonInfo()
        {
            lblPersonID.Text = _Person.PersonID.ToString();
            lblFullName.Text = _Person.FullName.ToString();
            lblNationalNo.Text = _Person.NationalNo.ToString();
            lblGender.Text = ((enGender)_Person.Gender).ToString();
            lblEmail.Text = _Person.Email.ToString();
            lblAddress.Text = _Person.Address.ToString();
            lblDateOfBirth.Text = _Person.DateOfBirth.ToString();
            lblPhone.Text = _Person.Phone.ToString();
            lblCountry.Text = _Person.CountryInfo.CountryName;

            _LoadPersonImage();

            llEditPersonInfo.Enabled = true;
        }

        public void ResetPersonInfo()
        {
            _PersonID = -1;
            lblPersonID.Text = lblFullName.Text = lblNationalNo.Text = lblGender.Text =
            lblEmail.Text = lblAddress.Text = lblDateOfBirth.Text = lblPhone.Text =
            lblCountry.Text = "[????]";

            pbPersonImage.Image = Resources.Male_512;
            pbGender.Image = Resources.Man_32;

            llEditPersonInfo.Enabled = false;
        }
        
        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson frmAddUpdatePerson = new frmAddUpdatePerson(_PersonID);

            frmAddUpdatePerson.DataBack += LoadPersonInfo; // Subscribe to Refresh after save

            frmAddUpdatePerson.ShowDialog();

        }
    }
}
