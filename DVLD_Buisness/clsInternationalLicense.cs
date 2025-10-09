using DVLD_Business;
using DVLD_DataAccess;
using System;
using System.Data;

namespace DVLD_Buisness
{
    public class clsInternationalLicense : clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode { get; private set; }

        public int InternationalLicenseID { get; private set; }
        public int DriverID { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }
        public clsLicense LicenseInfo { get; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }


        public clsInternationalLicense() : base()
        {
            this.InternationalLicenseID = -1;
            this.DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.MinValue;
            this.ExpirationDate = DateTime.MinValue;
            this.IsActive = true;

            LicenseInfo = null;

            this.Mode = enMode.AddNew;
        }

        public clsInternationalLicense(int internationalLicenseID, int applicationID, int driverID,
                                       int issuedUsingLocalLicenseID, DateTime issueDate,
                                       DateTime expirationDate, bool isActive, int createdByUserID,

                                       int applicantPersonID, DateTime applicationDate,
                                       enApplicationType applicationTypeID, enStatus applicationStatus,
                                       DateTime lastStatusDate, float paidFees)


            : base(applicationID, applicantPersonID, applicationDate,
                  applicationTypeID, applicationStatus, lastStatusDate,
                  paidFees, createdByUserID)
        {

            this.InternationalLicenseID = internationalLicenseID;
            this.DriverID = driverID;
            this.IssuedUsingLocalLicenseID = issuedUsingLocalLicenseID;
            this.IssueDate = issueDate;
            this.ExpirationDate = expirationDate;
            this.IsActive = isActive;

            LicenseInfo = clsLicense.Find(this.IssuedUsingLocalLicenseID);

            this.Mode = enMode.Update;
        }

        public static clsInternationalLicense GetInternationalLicenseByID(int internationalLicenseID)
        {
            int applicationID = -1, driverID = -1, createdByUserID = -1, issuedUsingLocalLicenseID = -1;
            DateTime issueDate = DateTime.MinValue, expirationDate = DateTime.MinValue;
            bool isActive = false;

            bool isFound = clsInternationalLicenseData.GetInternationalLicenseByID(internationalLicenseID, ref applicationID, ref driverID, ref issuedUsingLocalLicenseID,
                                                                                   ref issueDate, ref expirationDate,
                                                                                   ref isActive, ref createdByUserID);

            if (!isFound) return null;

            clsApplication application = clsApplication.FindBaseApplication(applicationID);

            return new clsInternationalLicense(internationalLicenseID, applicationID, driverID,
                                               issuedUsingLocalLicenseID, issueDate, expirationDate,
                                               isActive, createdByUserID, application.ApplicantPersonID, application.ApplicationDate,
                                               application.ApplicationTypeID, application.ApplicationStatus,
                                               application.LastStatusDate, application.PaidFees);
        }

        public override bool Save()
        {
            
            base.Mode = (clsApplication.enMode)Mode;

            if (!base.Save()) return false;

            switch (this.Mode)
            {
                case enMode.AddNew:

                    _AddNewInternationalLicense();

                    if (this.InternationalLicenseID != -1)
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateInternationalLicense();

                default:
                    return false;
            }
        }

        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses();
        }

        public static DataTable GetAllInternationalLicensesByDriverID(int driverID)
        {
            return clsInternationalLicenseData.GetAllInternationalLicensesByDriverID(driverID);
        }

        public static int GetActiveInternationalLicenseByDriverID(int driverID)
        {
            return clsInternationalLicenseData.GetActiveInternationalLicenseByDriverID(driverID);
        }

        private bool _UpdateInternationalLicense()
        {
            return clsInternationalLicenseData.UpdateInternationalLicense(this.InternationalLicenseID, this.ApplicationID, this.DriverID,
                                                                          this.IssuedUsingLocalLicenseID, this.IssueDate,
                                                                          this.ExpirationDate, this.IsActive, this.CreatedByUserID);
        }

        private bool _AddNewInternationalLicense()
        {
            this.InternationalLicenseID = clsInternationalLicenseData.AddNewInternationalLicense(this.ApplicationID, this.DriverID,
                                                                                                 this.IssuedUsingLocalLicenseID, this.IssueDate,
                                                                                                 this.ExpirationDate, this.IsActive, this.CreatedByUserID);

            return this.InternationalLicenseID != -1;
        }
    }
}
