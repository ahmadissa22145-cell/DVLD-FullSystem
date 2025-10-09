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
    public class clsTestAppointment
    {
        public enum enMode { AddNew = 1, Update = 2};

        public enMode Mode { get; private set; }

        public int TestAppointmentID {  get; private set; }

        public clsTestType.enTestTypes TestTypeID {  get; set; }

        public int LocalDrivingLicenseApplicationID { get; set; }

        public clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication { get; private set; }

        public DateTime AppointmentDate { get; set; }

        public float PaidFees { get; set; }

        public int CreatedByUserID { get; set; }

        public clsUser CreatedUserInfo { get; private set; }   

        public bool IsLocked { get; set; }

        public int RetakeTestApplicationID { get; set; } = -1;

        public clsApplication RetakeTestApplicationInfo { get; private set; }

        public int TestID
        {
            get {  return GetTestID(TestAppointmentID); }
        }

        public clsTestAppointment()
        {
            TestAppointmentID = -1;
            TestTypeID = clsTestType.enTestTypes.VisionTest;
            LocalDrivingLicenseApplicationID = -1;
            AppointmentDate = DateTime.Now;
            PaidFees = 0.0f;
            CreatedByUserID = -1;
            IsLocked = false;
            RetakeTestApplicationID = -1;

            LocalDrivingLicenseApplication = null;
            CreatedUserInfo = null;
            RetakeTestApplicationInfo = null;

            Mode = enMode.AddNew;
        }

        private clsTestAppointment(int testAppointmentID, clsTestType.enTestTypes testTypeID, int localDrivingLicenseApplicationID,
                                   DateTime appointmentDate, float paidFees, int createdByUserID,
                                   bool isLocked, int retakeTestApplicationID)
        {
            TestAppointmentID = testAppointmentID;
            TestTypeID = testTypeID;
            LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            AppointmentDate = appointmentDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserID;
            IsLocked = isLocked;
            RetakeTestApplicationID = retakeTestApplicationID;


            LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
            CreatedUserInfo = clsUser.FindByUserID(createdByUserID);

            if (retakeTestApplicationID > 0)
                RetakeTestApplicationInfo = clsApplication.FindBaseApplication(retakeTestApplicationID);
            else
                RetakeTestApplicationInfo = null;

            Mode = enMode.Update;  
        }

        public static clsTestAppointment Find(int testAppointmentID)
        {
            int localDrivingLicenseApplicationID = -1, createdByUserID = -1, retakeTestApplicationID = -1;
            int testTypeID = -1;
            DateTime appointmentDate = DateTime.Now;
            float paidFees = 0.0f;
            bool isLocked = false;

            bool isFound = clsTestAppointmentData.GetTestAppointmentByID(testAppointmentID, ref testTypeID, ref localDrivingLicenseApplicationID,
                                                                         ref appointmentDate, ref paidFees, ref createdByUserID,
                                                                         ref isLocked, ref retakeTestApplicationID);

            if (!isFound)
                return null;
            
            return new clsTestAppointment(testAppointmentID, (clsTestType.enTestTypes)testTypeID, localDrivingLicenseApplicationID,
                                          appointmentDate, paidFees, createdByUserID,
                                          isLocked, retakeTestApplicationID);
        }

        public static clsTestAppointment GetLastTestAppointment(int localDrivingLicenseApplicationID, clsTestType.enTestTypes testType)
        {
            int testAppointmentID = -1, createdByUserID = -1, retakeTestApplicationID = -1;
            DateTime appointmentDate = DateTime.Now;
            float paidFees = 0.0f;
            bool isLocked = false;

            bool isFound = clsTestAppointmentData.GetLastTestAppointment(localDrivingLicenseApplicationID, (int)testType, ref testAppointmentID,
                                                                         ref appointmentDate, ref paidFees, ref createdByUserID,
                                                                         ref isLocked, ref retakeTestApplicationID);

            if (!isFound)
                return null;

            return new clsTestAppointment(testAppointmentID, (clsTestType.enTestTypes)testType, localDrivingLicenseApplicationID,
                                          appointmentDate, paidFees, createdByUserID,
                                          isLocked, retakeTestApplicationID);
        }

        private bool _AddNewTestAppointment()
        {
            this.TestAppointmentID = clsTestAppointmentData.AddNewTestAppointment(this.LocalDrivingLicenseApplicationID, (int)this.TestTypeID,
                                                                                  this.AppointmentDate, this.PaidFees, this.CreatedByUserID,
                                                                                  this.IsLocked, this.RetakeTestApplicationID);

            return this.TestAppointmentID != -1;
        }

        private bool _UpdateTestAppointment()
        {
            return clsTestAppointmentData.UpdateTestAppointment(this.TestAppointmentID, this.LocalDrivingLicenseApplicationID, (int)this.TestTypeID,
                                                         this.AppointmentDate, this.PaidFees, this.CreatedByUserID,
                                                         this.IsLocked, this.RetakeTestApplicationID);

        }

        public bool Save()
        {

            switch (this.Mode)
            {
                case enMode.AddNew:

                    if (_AddNewTestAppointment())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateTestAppointment();

                default:
                    return false;
            }

        }

        public static bool DeleteTestAppointment(int testAppointmentID)
        {
            return clsTestAppointmentData.DeleteTestAppointment(testAppointmentID);
        }

        public bool DeleteTestAppointment()
        {
            return clsTestAppointmentData.DeleteTestAppointment(this.TestAppointmentID);
        }

        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentData.GetAllTestAppointments();
        }

        public static DataTable GetAllTestAppointmentsPerTestType(int localDrivingLicenseApplicationID, clsTestType.enTestTypes testTypeID)
        {
            return clsTestAppointmentData.GetAllTestAppointmentsPerTestType(localDrivingLicenseApplicationID, (int)testTypeID);
        }

        public static clsTestType.enTestTypes GetTestTypeID(int testAppointmentID)
        {
            return (clsTestType.enTestTypes) clsTestAppointmentData.GetTestTypeID(testAppointmentID);
        }

        public static int GetTestID(int TestAppointmentID)
        {
            return clsTestAppointmentData.GetTestID(TestAppointmentID);
        }
    }
}
