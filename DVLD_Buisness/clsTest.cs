using DVLD_Business;
using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsTest
    {
        public enum enMode {AddNew = 0, Update = 1 }

        public enMode Mode { get; private set; }

        public int TestID { get; private set; }

        public int TestAppointmentID { get; set; }

        public clsTestAppointment TestAppointmentInfo { get; private set; }

        public bool TestResult { get; set; }

        public string Notes { get; set; }
        
        public int CreatedByUserID { get; set; }

        public clsUser CreatedByUserInfo { get; private set; }

        public clsTest()
        {
            TestID = -1;
            TestAppointmentID = -1;
            TestResult = false;
            Notes = string.Empty;
            CreatedByUserID = -1;

            TestAppointmentInfo = null;
            CreatedByUserInfo = null;

            Mode = enMode.AddNew;
        }

        private clsTest(int testID, int testAppointmentID, bool testResult, string notes, int createdByUserID)
        {
            TestID = testID;
            TestAppointmentID = testAppointmentID;
            TestResult = testResult;
            Notes = notes;
            CreatedByUserID = createdByUserID;

            TestAppointmentInfo = clsTestAppointment.Find(TestAppointmentID);
            CreatedByUserInfo = clsUser.FindByUserID(CreatedByUserID);

            Mode = enMode.Update;
        }


        public static clsTest Find(int testID)
        {
            int testAppointmentID = -1, createdByUserID = -1;
            bool testResult = false;
            string notes = string.Empty;

            bool isFound = clsTestData.GetTestInfoByID(testID, ref testAppointmentID, ref testResult, ref notes, ref createdByUserID);

            if (!isFound)
                return null;

            return new clsTest(testID, testAppointmentID, testResult, notes, createdByUserID);
        }

        public static clsTest FindLastTestByPersonAndTestTypeAndLicenseClass(int personID, int testTypeID, int licenseClass)
        {

            int testID = -1, testAppointmentID = -1, createdByUserID = -1;
            bool testResult = false;
            string notes = string.Empty;

            bool isFound = clsTestData.GetLastTestByPersonAndTestTypeAndLicenseClass(personID, testTypeID, licenseClass, ref testID,
                                                                                     ref testAppointmentID, ref testResult,
                                                                                     ref notes, ref createdByUserID);
            if (!isFound)
                return null;

            return new clsTest(testID, testAppointmentID, testResult, notes, createdByUserID);
        }

        public static DataTable GetAllTests()
        {
            return clsTestData.GetAllTests();
        }

        public bool Save()
        {
            switch (this.Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTest())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateTest();

                default:
                    return false;
            }
        }

        private bool _AddNewTest()
        {
            this.TestID = clsTestData.AddNewTest(this.TestAppointmentID, this.TestResult,
                                                 this.Notes, this.CreatedByUserID);

            return this.TestID > -1;
        }

        private bool _UpdateTest()
        {
            return clsTestData.UpdateTest(this.TestID, this.TestAppointmentID, this.TestResult,
                                          this.Notes, this.CreatedByUserID);
        }

        
    }
}
