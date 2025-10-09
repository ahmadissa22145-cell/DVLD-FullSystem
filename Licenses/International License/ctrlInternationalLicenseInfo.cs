using DVLD.Properties;
using DVLD_Buisness;
using DVLD_Business;
using System;
using System.IO;
using System.Windows.Forms;

namespace DVLD.Licenses.International_License
{
    public partial class ctrlInternationalLicenseInfo : UserControl
    {
        public int InternationalLicenseID { get; private set; } = -1;
        public clsInternationalLicense InternationalLicense { get; private set; }

        public ctrlInternationalLicenseInfo()
        {
            InitializeComponent();
        }

        public void ResetDefaultValues()
        {
            InternationalLicenseID = -1;

            lblFullName.Text = "[????]";
            lblInternationalLicenseID.Text = "[????]";
            lblApplicationID.Text = "[????]";
            lblLocalLicenseID.Text = "[????]";
            lblIsActive.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblGendor.Text = "[????]";
            lblDriverID.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblIssueDate.Text = "[????]";
            lblExpirationDate.Text = "[????]";

            pbPersonImage.Image = Resources.Male_512;
        }

        public void LoadInfo(int internationalLicenseID)
        {
            clsInternationalLicense internationalLicense = clsInternationalLicense.GetInternationalLicenseByID(internationalLicenseID);

            if (internationalLicense == null)
            {
                ResetDefaultValues();
                MessageBox.Show($"No international license was found with ID = {internationalLicenseID}",
                                 "Not Found",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);

                return;
            }

            InternationalLicenseID = internationalLicenseID;
            InternationalLicense = internationalLicense;
            clsPerson person = internationalLicense.LicenseInfo.DriverInfo.Person;


            lblFullName.Text = person.FullName;
            lblInternationalLicenseID.Text = internationalLicenseID.ToString();
            lblApplicationID.Text = internationalLicense.ApplicationID.ToString();
            lblLocalLicenseID.Text = internationalLicense.IssuedUsingLocalLicenseID.ToString();
            lblIsActive.Text = internationalLicense.IsActive ? "Yes" : "No";
            lblNationalNo.Text = person.NationalNo;
            lblDateOfBirth.Text = person.DateOfBirth.ToShortDateString();
            lblGendor.Text = person.Gender == 0 ? "Male" : "Female";
            lblDriverID.Text = internationalLicense.LicenseInfo.DriverID.ToString();
            lblIssueDate.Text = internationalLicense.IssueDate.ToShortDateString();
            lblExpirationDate.Text = internationalLicense.ExpirationDate.ToShortDateString();

            _HandlePersonImages(person.ImagePath, person.Gender);
        }

        private void _HandlePersonImages(string imagePath,short gender)
        {
            pbGendor.Image = gender == 0 ? Resources.Man_32 : Resources.Woman_32;

            if (!string.IsNullOrEmpty(imagePath))
            {
                if (File.Exists(imagePath))
                {
                    pbPersonImage.ImageLocation = imagePath;
                    return;
                }

                MessageBox.Show($"The specified image file was not found:\n{imagePath}",
                                 "Image Not Found",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Warning);
            }

            pbPersonImage.Image = gender == 0 ? Resources.Male_512 : Resources.Female_512;
        }
    }
}
