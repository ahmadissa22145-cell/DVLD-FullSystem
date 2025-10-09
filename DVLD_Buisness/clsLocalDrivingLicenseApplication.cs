using DVLD_Buisness;
using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsLocalDrivingLicenseApplication : clsApplication
    {
        public new enum enMode { AddNew = 0, Update = 1 };

        public new enMode Mode { get; private set; }

        public int LocalDrivingLicenseApplicationID { get; private set; }

        public clsLicenseClass.enLicenseClassID LicenseClassID { get; set; }

        public clsLicenseClass LicenseClassInfo {  get; private set; }

        public clsLocalDrivingLicenseApplication()
               : base()
        {
            LocalDrivingLicenseApplicationID = -1;
            LicenseClassID = clsLicenseClass.enLicenseClassID.SmallMotorcycle;

            LicenseClassInfo = null;

            this.Mode = enMode.AddNew;
        }

        private clsLocalDrivingLicenseApplication(int localDrivingLicenseApplicationID, int applicationID, int applicantPersonID, DateTime applicationDate,
                                          enApplicationType applicationTypeID, enStatus applicationStatus,
                                          DateTime lastStatusDate, float paidFees, int createdByUserID, clsLicenseClass.enLicenseClassID licenseClassID)

                                          : base(applicationID, applicantPersonID, applicationDate,
                                                applicationTypeID, applicationStatus, lastStatusDate,
                                                paidFees, createdByUserID)
        {
            LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            LicenseClassID = licenseClassID;

            LicenseClassInfo = clsLicenseClass.Find((clsLicenseClass.enLicenseClassID)LicenseClassID);

            this.Mode = enMode.Update;
        }

        public static clsLocalDrivingLicenseApplication FindByLocalDrivingLicenseApplicationID(int localDrivingLicenseApplicationID)
        {
            int applicationID = -1, licenseClassID = -1;

            bool isFound = clsLocalDrivingApplicationData.FindByLocalDrivingLicenseApplicationID(localDrivingLicenseApplicationID,
                                                                                            ref applicationID, ref licenseClassID);

            if (!isFound) return null;

            clsApplication application = clsApplication.FindBaseApplication(applicationID);

            return new clsLocalDrivingLicenseApplication(localDrivingLicenseApplicationID, applicationID,
                                                  application.ApplicantPersonID, application.ApplicationDate,
                                                  application.ApplicationTypeID, application.ApplicationStatus, application.LastStatusDate,
                                                  application.PaidFees, application.CreatedByUserID, (clsLicenseClass.enLicenseClassID)licenseClassID);
        }

        public static clsLocalDrivingLicenseApplication FindByApplicationID(int applicationID)
        {
            int localDrivingLicenseApplicationID = -1, licenseClassID = -1;

            bool isFound = clsLocalDrivingApplicationData.FindByApplicationID(applicationID, ref localDrivingLicenseApplicationID,
                                                                              ref licenseClassID);

            if (!isFound) return null;

            clsApplication application = clsApplication.FindBaseApplication(applicationID);

            return new clsLocalDrivingLicenseApplication(localDrivingLicenseApplicationID, applicationID,
                                                  application.ApplicantPersonID, application.ApplicationDate,
                                                  application.ApplicationTypeID, application.ApplicationStatus, application.LastStatusDate,
                                                  application.PaidFees, application.CreatedByUserID, (clsLicenseClass.enLicenseClassID)licenseClassID);
        }

        private bool _AddNewLocalDrivingApplication()
        {
            this.LocalDrivingLicenseApplicationID = clsLocalDrivingApplicationData.AddNewLocalDrivingLicenseApplication(this.ApplicationID, (int)this.LicenseClassID);

            return (this.LocalDrivingLicenseApplicationID != -1);
        }

        private bool _UpdateLocalDrivingApplication()
        {

            return clsLocalDrivingApplicationData.UpdateLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID, this.ApplicationID, (int)this.LicenseClassID);
        }

        public override bool Save()
        {

            if (!base.Save())
                return false;


            switch (this.Mode)
            {
                case enMode.AddNew:

                    if (_AddNewLocalDrivingApplication())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateLocalDrivingApplication();

                default:
                    return false;
            }
        }

        public bool Delete()
        {
            return clsLocalDrivingApplicationData.DeleteLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID);
        }

        public static bool Delete(int localDrivingLicenseApplicationID)
        {
            return clsLocalDrivingApplicationData.DeleteLocalDrivingLicenseApplication(localDrivingLicenseApplicationID);
        }

        public static DataTable GetAllLocalDrivingLicenseApplicationID()
        {
            return clsLocalDrivingApplicationData.GetAllLocalDrivingLicenseApplications();
        }

        public static bool DoesAttendTestType(int localDrivingLicenseApplicationID, clsTestType.enTestTypes testType)
        {
            return clsLocalDrivingApplicationData.DoesAttendTestType(localDrivingLicenseApplicationID, (int)testType);
        }

        public bool DoesAttendTestType(clsTestType.enTestTypes testType)
        {
            return clsLocalDrivingApplicationData.DoesAttendTestType(this.LocalDrivingLicenseApplicationID, (int)testType);
        }

        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestTypes TestTypeID)
        {
            return clsLocalDrivingApplicationData.TotalTrialsPerTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public byte TotalTrialsPerTest(clsTestType.enTestTypes TestTypeID)
        {
            return clsLocalDrivingApplicationData.TotalTrialsPerTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestTypes TestTypeID)
        {

            return clsLocalDrivingApplicationData.IsThereAnActiveScheduledTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public bool DoesPassTestType(clsTestType.enTestTypes TestTypeID)

        {
            return clsLocalDrivingApplicationData.DoesPassTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public static bool DoesPassTestType(int localDrivingLicenseApplicationID, clsTestType.enTestTypes TestTypeID)

        {
            return clsLocalDrivingApplicationData.DoesPassTestType(localDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public bool DoesPassPreviousTest(clsTestType.enTestTypes CurrentTestType)
        {

            switch (CurrentTestType)
            {
                case clsTestType.enTestTypes.VisionTest:
                    //in this case no required prvious test to pass.
                    return true;

                case clsTestType.enTestTypes.WrittenTest:
                    //Written Test, you cannot sechdule it before person passes the vision test.
                    //we check if pass visiontest 1.

                    return this.DoesPassTestType(clsTestType.enTestTypes.VisionTest);


                case clsTestType.enTestTypes.StreetTest:

                    //Street Test, you cannot sechdule it before person passes the written test.
                    //we check if pass Written 2.
                    return this.DoesPassTestType(clsTestType.enTestTypes.WrittenTest);

                default:
                    return false;
            }
        }

        public static byte GetPassedTestsCount(int localDrivingLicenseApplicationID)
        {
            return clsLocalDrivingApplicationData.GetPassedTestsCount(localDrivingLicenseApplicationID);
        }

        public static bool PassedAllTests(int localDrivingLicenseApplicationID)
        {
            return clsLocalDrivingApplicationData.GetPassedTestsCount(localDrivingLicenseApplicationID) == 3;
        }
        public bool PassedAllTests()
        {
            return clsLocalDrivingApplicationData.GetPassedTestsCount(this.LocalDrivingLicenseApplicationID) == 3;
        }

        public bool IsLicenseIssued()
        {
            return (GetActiveLicenseID() != -1); 
        }

        public int GetActiveLicenseID()
        {
          return clsLicense.GetActiveLicenseIDByPersonID(this.ApplicantPersonID, this.LicenseClassID);
        }

        public int IssueLicenseForTheFirstTime(string notes, int createdByUserID)
        {
            int licenseID = clsLicense.GetActiveLicenseIDByPersonID(this.ApplicantPersonID, this.LicenseClassID);

            if (licenseID != -1)
                return -2;

            clsLicense license = new clsLicense();

            int driverID = _EnsureApplicantHasDriverRecord(createdByUserID);

            if (driverID == -1) return -1;

            license.ApplicationID = this.ApplicationID;
            license.DriverID = driverID;
            license.LicenseClassID = this.LicenseClassID;
            license.IssueDate = DateTime.Now.Date;

            int defaultValidityLength = this.LicenseClassInfo.DefaultValidityLength;

            license.ExpirationDate = license.IssueDate.AddYears(defaultValidityLength);
            license.Notes = notes;
            license.PaidFees = this.LicenseClassInfo.ClassFees;
            license.IsActive = true;
            license.IssueReason = clsLicense.enIssueReason.FirstTime;
            license.CreatedByUserID = createdByUserID;

            if(!license.Save()) return -1;

            this.SetComplete();

            return license.LicenseID;
        }

        private int _EnsureApplicantHasDriverRecord(int createdByUserID)
        {
            int personID = this.ApplicantPersonID;

            clsDriver driver = clsDriver.GetByPersonID(personID);

            if (driver != null)
            {
                return driver.DriverID;
            }

            driver = new clsDriver();

            driver.PersonID = personID;
            driver.CreatedByUserID = createdByUserID;
            driver.CreatedDate = DateTime.Now;

            if (!driver.Save())
            {
                return -1;
            }

            return driver.DriverID;
        }
    }
}
