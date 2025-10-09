using DVLD_Business;
using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsDetainLicense
    {
        public enum enMode {AddNew = 0, Update = 1 };
        private enMode _mode;
     
        public int DeteainID {  get; set; }

        public int LicenseID {  get; set; }

        public DateTime DeteainDate { get; set; }

        public float FineFees { get; set; }

        public int CreatedByUserID { get; set; }

        public clsUser CreatedByUserInfo { get; private set; }

        public bool IsReleased { get; set; }

        public DateTime ReleaseDate { get; set; }

        public int ReleasedByUserID { get; set; }

        public clsUser ReleasedByUserInfo {  get; private set; }

        public int ReleaseApplicationID { get; set; }

        public clsApplication ReleaseApplication { get; private set; }

        public clsDetainLicense()
        {
            this.DeteainID = -1;
            this.LicenseID = -1;
            this.DeteainDate = DateTime.MaxValue;
            this.FineFees = 0.0f;
            this.CreatedByUserID = -1;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.MaxValue;
            this.ReleasedByUserID = -1;
            this.ReleaseApplicationID = -1;

            ReleaseApplication = null;
            CreatedByUserInfo = null;
            ReleasedByUserInfo = null;

            _mode = enMode.AddNew;
        }

        public clsDetainLicense(int deteainID, int licenseID, DateTime deteainDate, float fineFees,
                                int createdByUserID, bool isReleased, DateTime releaseDate,
                                int releasedByUserID, int releaseApplicationID )
        {
            this.DeteainID = deteainID;
            this.LicenseID = licenseID;
            this.DeteainDate = deteainDate;
            this.FineFees = fineFees;
            this.CreatedByUserID = createdByUserID;
            this.IsReleased = isReleased;
            this.ReleaseDate = releaseDate;
            this.ReleasedByUserID = releasedByUserID;
            this.ReleaseApplicationID = releaseApplicationID;

            CreatedByUserInfo = clsUser.FindByUserID(this.CreatedByUserID);


            if (this.ReleaseApplicationID != -1)
            {
                ReleaseApplication = clsApplication.FindBaseApplication(this.ReleaseApplicationID);
                ReleasedByUserInfo = clsUser.FindByUserID(this.ReleasedByUserID);
            }

            _mode = enMode.Update;
        }

        public static clsDetainLicense Find(int deteainID)
        {
            int licenseID = -1, createdByUserID = -1, releasedByUserID = -1, releaseApplicationID = -1;
            DateTime deteainDate = DateTime.MaxValue, releaseDate = DateTime.MaxValue;
            float fineFees = 0.0f;
            bool isReleased = false;

            bool isFound = clsDetainLicenseData.GetDetainLicenseInfoByID(deteainID,ref  licenseID,ref  deteainDate,
                                                                         ref fineFees,ref  createdByUserID,ref  isReleased,
                                                                         ref releaseDate,ref  releasedByUserID,ref  releaseApplicationID);

            if(!isFound) return null;

            return new clsDetainLicense(deteainID, licenseID, deteainDate, fineFees,
                                        createdByUserID, isReleased, releaseDate,
                                        releasedByUserID, releaseApplicationID);
        }

        public static clsDetainLicense FindByLicenseID(int licenseID)
        {
            int deteainID = -1, createdByUserID = -1, releasedByUserID = -1, releaseApplicationID = -1;
            DateTime deteainDate = DateTime.MaxValue, releaseDate = DateTime.MaxValue;
            float fineFees = 0.0f;
            bool isReleased = false;

            bool isFound = clsDetainLicenseData.GetDetainLicenseInfoByLicenseID(licenseID, ref deteainID, ref deteainDate,
                                                                                ref fineFees, ref createdByUserID, ref isReleased,
                                                                                ref releaseDate, ref releasedByUserID, ref releaseApplicationID);

            if (!isFound) return null;

            return new clsDetainLicense(deteainID, licenseID, deteainDate, fineFees,
                                        createdByUserID, isReleased, releaseDate,
                                        releasedByUserID, releaseApplicationID);
        }

        public bool Save()
        {
            switch (this._mode)
            {
                case enMode.AddNew:

                    if (_AddNewDetainLicense())
                    {
                        this._mode = enMode.Update;
                        return true;
                    }

                    return false;

                case enMode.Update:
                    return _UpdateDetainLicense();

                default:
                    return false;
            }
        }

        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainLicenseData.GetAllDetainedLicenses();
        }

        public bool ReleaseDetainLicense(int releasedByUserID, int releaseApplicationID)
        {
            return clsDetainLicenseData.ReleaseDetainLicense(this.DeteainID, releasedByUserID, releaseApplicationID);
        }

        public static bool IsLicenseDetained(int licenseID)
        {
            return clsDetainLicenseData.IsLicenseDetained(licenseID);
        }

        private bool _AddNewDetainLicense()
        {
            this.DeteainID = clsDetainLicenseData.AddNewDetainLicense(this.LicenseID, this.DeteainDate,
                                                                      this.FineFees, this.CreatedByUserID);

            return (this.DeteainID > 0);
        }

        private bool _UpdateDetainLicense()
        {
            return clsDetainLicenseData.UpdateDetainLicense(this.DeteainID, this.LicenseID, this.DeteainDate,
                                                            this.FineFees, this.CreatedByUserID);
        }

     
    }
}
