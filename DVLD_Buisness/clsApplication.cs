using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsApplication
    {
        public enum enApplicationType
        {
            NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 7
        };

        public enum enStatus {New = 1, Cancelled = 2, Completed = 3};

        public enum enMode {AddNew = 0,Update = 1};
        public enMode Mode { get; protected set; }

        public int ApplicationID { get; set; } = -1;

        public int ApplicantPersonID {  get; set; }

        public clsPerson ApplicantPersonInfo {  get; private set; }

        public DateTime ApplicationDate {  get; set; }

        public enApplicationType ApplicationTypeID {  get; set; }

        public clsApplicationType ApplicationTypeInfo {  get; private set; }

        public enStatus ApplicationStatus {  get; set; }

        public DateTime LastStatusDate {  get; set; }

        public float PaidFees {  get; set; }

        public int CreatedByUserID {  get; set; }

        public clsUser CreatedByUserInfo { get; private set; }

        public clsApplication(int applicationID, int applicantPersonID, DateTime applicationDate,
                              enApplicationType applicationTypeID, enStatus applicationStatus,
                              DateTime lastStatusDate, float paidFees, int createdByUserID)
        {
            
            this.ApplicationID = applicationID;
            this.ApplicantPersonID = applicantPersonID;
            this.ApplicationDate = applicationDate;
            this.ApplicationTypeID = applicationTypeID;
            this.ApplicationStatus = applicationStatus;
            this.LastStatusDate = lastStatusDate;
            this.PaidFees = paidFees;
            this.CreatedByUserID = createdByUserID;

            ApplicantPersonInfo = clsPerson.Find(applicantPersonID);
            ApplicationTypeInfo = clsApplicationType.Find((int)applicationTypeID);
            CreatedByUserInfo   = clsUser.FindByUserID(createdByUserID);

            this.Mode = enMode.Update;
        }

        public clsApplication()
        {

            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = enApplicationType.NewDrivingLicense;
            this.ApplicationStatus = enStatus.New;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = 0.0f;
            this.CreatedByUserID = -1;

            ApplicantPersonInfo = null;
            ApplicationTypeInfo = null;
            CreatedByUserInfo   = null;

            this.Mode = enMode.AddNew;
        }

        private bool _AddNewApplication()
        {
            this.ApplicationID = clsApplicationData.AddNewApplication(this.ApplicantPersonID, this.ApplicationDate,
                                                                      (int)this.ApplicationTypeID, (byte)this.ApplicationStatus,
                                                                      this.LastStatusDate, this.PaidFees, this.CreatedByUserID);

            return this.ApplicationID > 0;
        }

        private bool _UpdateApplication()
        {
            return clsApplicationData.UpdateApplication(this.ApplicationID, this.ApplicantPersonID, this.ApplicationDate,
                                                        (int)this.ApplicationTypeID, (byte)this.ApplicationStatus,
                                                        this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
        }

        public static clsApplication FindBaseApplication(int applicationID)
        {
            int applicantPersonID = -1, applicationTypeID = -1, createdByUserID = -1;
            DateTime applicationDate = DateTime.Now, lastStatusDate = DateTime.Now;
            byte applicationStatus = 1;
            float paidFees = 0.0f;

           bool isFound = clsApplicationData.GetApplicationInfoByID(applicationID, ref applicantPersonID, ref applicationDate,
                                                     ref applicationTypeID, ref applicationStatus, ref lastStatusDate,
                                                     ref paidFees, ref createdByUserID);

            if (!isFound) return null;

            return new clsApplication(applicationID, applicantPersonID, applicationDate,
                                      (enApplicationType)applicationTypeID, (enStatus)applicationStatus,
                                      lastStatusDate, paidFees, createdByUserID); 
        }

        public bool Cancel()
        {
            return clsApplicationData.UpdateStatus(this.ApplicationID, (byte)enStatus.Cancelled);
        }

        public bool SetComplete()
        {
            return clsApplicationData.UpdateStatus(this.ApplicationID, (byte)enStatus.Completed);
        }

        public virtual bool Save()
        {

            switch (this.Mode)
            {
                case enMode.AddNew:

                    if (_AddNewApplication())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateApplication();

                default:
                    return false;
            }
        }

        public static bool DeleteApplication(int applicationID)
        {
            return clsApplicationData.DeleteApplication(applicationID);
        }

        public static DataTable GetAllApplications()
        {
            return clsApplicationData.GetAllApplications();
        }

        public static bool IsApplicationExists(int applicationID)
        {
            return clsApplicationData.IsApplicationExists(applicationID);
        }

        public static bool DoesPersonHaveActiveApplication(int personID, enApplicationType applicationTypeID)
        {
            return clsApplicationData.DoesPersonHaveActiveApplication(personID, (int)applicationTypeID);
        }

        public static int GetActiveApplicationID(int personID, enApplicationType applicationTypeID)
        {
            return clsApplicationData.GetActiveApplicationID(personID, (int)applicationTypeID);
        }

        public static int GetActiveApplicationIDForLicenseClass(int personID, enApplicationType applicationTypeID, clsLicenseClass.enLicenseClassID licenseClassID)
        {
            return clsApplicationData.GetActiveApplicationIDForLicenseClass(personID, (int)applicationTypeID, (int)licenseClassID);
        }

      
    }
}
