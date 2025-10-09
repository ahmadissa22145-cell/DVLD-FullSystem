using DVLD.Properties;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses.Local_Licenses.Controls
{

    public partial class ctrlDriverLicenseInfo : UserControl
    {
        public enum enGender {Male = 0, Female = 1 };

        public int LicenseID { get; private set; } = -1;
        public clsLicense SelectedLicenseInfo { get; private set; }
        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
           
        }

        public void ResetDefaultValues()
        {
            lblClass.Text = "[???]";
            lblFullName.Text = "[????]";
            lblLicenseID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblGendor.Text = "[????]";
            lblIssueDate.Text = "[????]";
            lblIssueReason.Text = "[????]";
            lblNotes.Text = "[????]";
            lblIsActive.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblDriverID.Text = "[????]";
            lblExpirationDate.Text = "[????]";
            lblIsDetained.Text = "[????]";

            pbPersonImage.Image = Resources.Male_512;

            LicenseID = -1;
            SelectedLicenseInfo = null;
        }

        public bool LoadControlInfo(int licenseID)
        {
            LicenseID = licenseID;
            SelectedLicenseInfo = clsLicense.Find(LicenseID);

            if (SelectedLicenseInfo == null)
            {
                ResetDefaultValues();
                MessageBox.Show($"Could not find any license with ID = {licenseID}, Please try with another license ID",
                                "Error : License Not Found",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            _FillLicenseInfo();

            return true;
        }

        private void _FillLicenseInfo()
        {
            clsPerson person = SelectedLicenseInfo.DriverInfo.Person;

            lblClass.Text = SelectedLicenseInfo.LicenseClassInfo.ClassName;
            lblFullName.Text = person.FullName;
            lblLicenseID.Text = SelectedLicenseInfo.LicenseID.ToString();
            lblNationalNo.Text = person.NationalNo;
            lblGendor.Text = person.Gender == (int)enGender.Male ? "Male" : "Female";
            lblIssueDate.Text = SelectedLicenseInfo.IssueDate.ToShortDateString();
            lblIssueReason.Text = SelectedLicenseInfo.IssueReason.ToString();

            string notes = SelectedLicenseInfo.Notes;

            lblNotes.Text = string.IsNullOrEmpty(notes) ? "No Notes" : notes;
            lblIsActive.Text = SelectedLicenseInfo.IsActive ? "Yes" : "No";
            lblDateOfBirth.Text = person.DateOfBirth.ToShortDateString();
            lblDriverID.Text = SelectedLicenseInfo.DriverID.ToString();
            lblExpirationDate.Text = SelectedLicenseInfo.ExpirationDate.ToShortDateString();
            lblIsDetained.Text = SelectedLicenseInfo.IsDetained ? "Yes" : "No";

            pbPersonImage.Image = person.Gender == (int)enGender.Male ? Resources.Man_32
                                                                      : Resources.Woman_32;

            _LoadPersonImage(person);
        }

        private void _LoadPersonImage(clsPerson person)
        {
           
            string imagePath = person.ImagePath;

            if (!string.IsNullOrEmpty(imagePath))
            {
                if (File.Exists(imagePath))
                {
                    pbPersonImage.ImageLocation = imagePath;
                    return;
                }

                MessageBox.Show($"Could not find Image :{imagePath}",
                                 "Image Not Found",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

            pbPersonImage.Image = person.Gender == (int)enGender.Male ? Resources.Male_512 
                                                                      : Resources.Female_512;
        }
    }
}
