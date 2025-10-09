using DVLD_Buisness;
using DVLD_DataAccess;
using System;
using System.Data;

namespace DVLD_Business
{
    public class clsLicense
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode { get; private set; }

        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 };

        public int LicenseID { get; private set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public clsDriver DriverInfo { get; set; }
        public clsLicenseClass.enLicenseClassID LicenseClassID { get; set; }
        public clsLicenseClass LicenseClassInfo {  get; private set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public float PaidFees { get; set; }
        public bool IsActive { get; set; }
        public enIssueReason IssueReason { get; set; }
        public int CreatedByUserID { get; set; }
        public clsDetainLicense DetainLicenseInfo {  get; private set; }
        public bool IsDetained 
        { 
            get { return clsDetainLicense.IsLicenseDetained(this.LicenseID); }
        }

        // Default constructor (AddNew)
        public clsLicense()
        {
            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClassID = clsLicenseClass.enLicenseClassID.SmallMotorcycle;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = string.Empty;
            this.PaidFees = 0;
            this.IsActive = true;
            this.IssueReason = enIssueReason.Renew;
            this.CreatedByUserID = -1;

            LicenseClassInfo = null;
            DriverInfo = null;

            this.Mode = enMode.AddNew;
        }

        // Private constructor (Update mode)
        private clsLicense(int licenseID, int applicationID, int driverID, clsLicenseClass.enLicenseClassID licenseClass,
                          DateTime issueDate, DateTime expirationDate, string notes,
                          float paidFees, bool isActive, enIssueReason issueReason, int createdByUserID)
        {
            this.LicenseID = licenseID;
            this.ApplicationID = applicationID;
            this.DriverID = driverID;
            this.LicenseClassID = licenseClass;
            this.IssueDate = issueDate;
            this.ExpirationDate = expirationDate;
            this.Notes = notes;
            this.PaidFees = paidFees;
            this.IsActive = isActive;
            this.IssueReason = issueReason;
            this.CreatedByUserID = createdByUserID;

            LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);
            DriverInfo = clsDriver.Find(DriverID);
            DetainLicenseInfo = clsDetainLicense.FindByLicenseID(licenseID);

            this.Mode = enMode.Update;
        }
        
        public static clsLicense Find(int licenseID)
        {
            int applicationID = -1, driverID = -1, licenseClass = -1, issueReason = -1, createdByUserID = -1;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            string notes = "";
            float paidFees = 0;
            bool isActive = true;

            bool isFound = clsLicenseData.FindLicenseByID(licenseID, ref applicationID, ref driverID,
                                                          ref licenseClass, ref issueDate, ref expirationDate,
                                                          ref notes, ref paidFees, ref isActive,
                                                          ref issueReason, ref createdByUserID);

            if (!isFound) return null;

            return new clsLicense(licenseID, applicationID, driverID, (clsLicenseClass.enLicenseClassID)licenseClass,
                                  issueDate, expirationDate, notes, paidFees, isActive,
                                  (enIssueReason)issueReason, createdByUserID);
        }

        public static bool DoesLicenseExist(int licenseID)
        {
            return clsLicenseData.DoesLicenseExist(licenseID);
        }

        public static DataTable GetDriverLicenses(int driverID)
        {
            return clsLicenseData.GetDriverLicenses(driverID);
        }

        public static int GetActiveLicenseIDByPersonID(int personID, clsLicenseClass.enLicenseClassID licenseClassID)
        {
            return clsLicenseData.GetActiveLicenseIDByPersonID(personID, (int)licenseClassID);
        }

        public static bool IsLicenseExistByPersonID(int personID, clsLicenseClass.enLicenseClassID licenseClassID)
        {
            return (GetActiveLicenseIDByPersonID(personID, licenseClassID) != -1);
        }

        public static bool DeactivateLicense(int licenseID)
        {
            return clsLicenseData.DeactivateLicense(licenseID);
        }

        public bool DeactivateCurrentLicense()
        {
            return clsLicenseData.DeactivateLicense(this.LicenseID);
        }

        public bool IsLicenseExpired()
        {
            return (this.ExpirationDate < DateTime.Now);
        }

        public static DataTable GetAllLicenses()
        {
            return clsLicenseData.GetAllLicenses();
        }

        public clsLicense RenewLicense(string notes, int createdByUserID)
        {
            clsApplication application = _CreateAndSaveApplication(createdByUserID,
                                           clsApplication.enApplicationType.RenewDrivingLicense);

            if (application == null) return null;

            clsLicense newLicense = new clsLicense();

            newLicense.ApplicationID = application.ApplicationID;
            newLicense.DriverID = this.DriverID;
            newLicense.LicenseClassID = this.LicenseClassID;
            newLicense.IssueDate = DateTime.Now;

            int defaultValidityLength = clsLicenseClass.Find(this.LicenseClassID).DefaultValidityLength;

            newLicense.ExpirationDate = DateTime.Now.AddYears(defaultValidityLength);
            newLicense.Notes = notes;
            newLicense.PaidFees = this.LicenseClassInfo.ClassFees;
            newLicense.IsActive = true;
            newLicense.IssueReason = enIssueReason.Renew;
            newLicense.CreatedByUserID = createdByUserID;

            if (!newLicense.Save())
            {
                return null;
            }

            this.DeactivateCurrentLicense();

            return newLicense;
        }

        public clsLicense Replace(enIssueReason issueReason, int createdByUserID)
        {
            clsApplication.enApplicationType applicationType = issueReason == enIssueReason.DamagedReplacement ?
                                                                              clsApplication.enApplicationType.ReplaceDamagedDrivingLicense :
                                                                              clsApplication.enApplicationType.ReplaceLostDrivingLicense;

            clsApplication application = _CreateAndSaveApplication(createdByUserID, applicationType);

            if (application == null) return null;

            clsLicense newLicense = new clsLicense();

            newLicense.ApplicationID = application.ApplicationID;
            newLicense.DriverID = this.DriverID;
            newLicense.LicenseClassID = this.LicenseClassID;
            newLicense.IssueDate = DateTime.Now;
            newLicense.ExpirationDate = this.ExpirationDate;
            newLicense.Notes = this.Notes;
            newLicense.PaidFees = 0; //No fees for issue the license because it's a replacement 
            newLicense.IsActive = true;
            newLicense.IssueReason = issueReason;

            newLicense.CreatedByUserID = createdByUserID;

            if (!newLicense.Save())
            {
                return null;
            }

            this.DeactivateCurrentLicense();

            return newLicense;
        }

        public int Detain(float fineFees, int createdByUserID)
        {
            clsDetainLicense detainLicense = new clsDetainLicense();
            detainLicense.LicenseID = this.LicenseID;
            detainLicense.DeteainDate = DateTime.Now;
            detainLicense.FineFees = fineFees;
            detainLicense.CreatedByUserID = createdByUserID;
            detainLicense.IsReleased = false;

            if (!detainLicense.Save()) return -1; 

            return detainLicense.DeteainID;
        }

        public bool Release(int releaseByUserID, ref int releaseApplicationID)
        {
            releaseApplicationID = _CreateRealesApplication(releaseByUserID);


            return this.DetainLicenseInfo.ReleaseDetainLicense(releaseByUserID, releaseApplicationID);
        }

        private int _CreateRealesApplication(int createdByUserID)
        {
            clsApplication.enApplicationType applicationType = clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense;

            clsApplication newReleaseApplication = new clsApplication();

            newReleaseApplication.ApplicantPersonID = this.DriverInfo.PersonID;
            newReleaseApplication.ApplicationDate = DateTime.Now;
            newReleaseApplication.ApplicationTypeID = applicationType;
            newReleaseApplication.ApplicationStatus = clsApplication.enStatus.Completed;
            newReleaseApplication.LastStatusDate = DateTime.Now;
            newReleaseApplication.PaidFees = clsApplicationType.Find((int)applicationType).ApplicationFees;
            newReleaseApplication.CreatedByUserID = createdByUserID;

            if (!newReleaseApplication.Save())
            {
                return -1;
            }

            return newReleaseApplication.ApplicationID;
        }


        // ------------------ Save ------------------
        public bool Save()
        {
            switch (this.Mode)
            {
                case enMode.AddNew:

                    if (_AddNewLicense())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }

                    return false;

                case enMode.Update:
                    return _UpdateLicense();

                default:
                    return false;
            }
        }

        private bool _AddNewLicense()
        {
            this.LicenseID = clsLicenseData.AddNewLicense(this.ApplicationID, this.DriverID, (int)this.LicenseClassID,
                                                          this.IssueDate, this.ExpirationDate, this.Notes,
                                                          this.PaidFees, this.IsActive,(int)this.IssueReason, this.CreatedByUserID);

            return this.LicenseID != -1;
        }

        private bool _UpdateLicense()
        {
            return clsLicenseData.UpdateLicense(this.LicenseID, this.ApplicationID, this.DriverID, (int)this.LicenseClassID,
                                                this.IssueDate, this.ExpirationDate, this.Notes,
                                                this.PaidFees, this.IsActive, (int)this.IssueReason, this.CreatedByUserID);
        }

        private clsApplication _CreateAndSaveApplication(int createdByUserID, clsApplication.enApplicationType applicationType)
        {
            clsApplication application = new clsApplication();

            application.ApplicantPersonID = this.DriverInfo.PersonID;
            application.ApplicationDate = DateTime.Now;
            application.ApplicationTypeID = applicationType;
            application.ApplicationStatus = clsApplication.enStatus.Completed;
            application.LastStatusDate = DateTime.Now;
            application.PaidFees = clsApplicationType.Find((int)applicationType).ApplicationFees;
            application.CreatedByUserID = createdByUserID;

            if (!application.Save())
            {
                return null;
            }

            return application;
        }

    }
}
