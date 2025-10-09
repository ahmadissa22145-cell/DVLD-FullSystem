using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsLicenseClass
    {
        public enum enLicenseClassID 
        { 
            SmallMotorcycle = 1,
            HeavyMotorcycleLicense = 2,
            OrdinaryDrivingLicense = 3,
            Commercial = 4,
            Agricultural = 5,
            SmallAndMediumBus = 6,
            TruckAndHeavyVehicle = 7
        };

        public enum enMode { AddNew = 0, Update = 1 };

        public enMode Mode { get; private set; }

        public enLicenseClassID LicenseClassID { get; private set; }

        public string ClassName { get; set; }

        public string ClassDescription { get; set; }

        public int MinimumAllowedAge { get; set; }

        public int DefaultValidityLength { get; set; }

        public float ClassFees { get; set; }

        public clsLicenseClass()
        {
            this.LicenseClassID = enLicenseClassID.SmallMotorcycle;
            this.ClassName = string.Empty;
            this.ClassDescription = string.Empty;
            this.MinimumAllowedAge = -1;
            this.DefaultValidityLength = -1;
            this.ClassFees = 0.0f;

            this.Mode = enMode.AddNew;
        }

        private clsLicenseClass(enLicenseClassID licenseClassID, string className, string classDescription,
                                int minimumAllowedAge, int defaultValidityLength, float classFees)
        {

            this.LicenseClassID = licenseClassID;
            this.ClassName = className;
            this.ClassDescription = classDescription;
            this.MinimumAllowedAge = minimumAllowedAge;
            this.DefaultValidityLength = defaultValidityLength;
            this.ClassFees = classFees;

            this.Mode = enMode.Update;

        }


        public static clsLicenseClass Find(enLicenseClassID licenseClassID)
        {
            string className = string.Empty, classDescription = string.Empty;
            int minimumAllowedAge = 0, defaultValidityLength = 0;
            float classFees = 0.0f;

            bool isFound = clsLicenseClassData.FindLicenseClassByID((int)licenseClassID, ref className, ref classDescription,
                                                                    ref minimumAllowedAge, ref defaultValidityLength, ref classFees);

            if (!isFound) return null;

            return new clsLicenseClass(licenseClassID, className, classDescription,
                                       minimumAllowedAge, defaultValidityLength, classFees);
        }

        public static clsLicenseClass Find(string className)
        {
            string classDescription = string.Empty;
            int licenseClassID = 0, minimumAllowedAge = 0, defaultValidityLength = 0;
            float classFees = 0.0f;

            bool isFound = clsLicenseClassData.FindLicenseClassByName(className, ref licenseClassID, ref classDescription,
                                                                    ref minimumAllowedAge, ref defaultValidityLength, ref classFees);

            if (!isFound) return null;

            return new clsLicenseClass((enLicenseClassID)licenseClassID, className, classDescription,
                                       minimumAllowedAge, defaultValidityLength, classFees);
        }

        public static DataTable GetAllLicenseClasses()
        {
            return clsLicenseClassData.GetAllLicenseClasses();
        }

        public static int GetMinimumAllowedAgeForLicenseClass(enLicenseClassID licenseClassID)
        {
            return clsLicenseClassData.GetMinimumAllowedAgeForLicenseClass((int)licenseClassID);
        }

        public bool Save()
        {
            switch (this.Mode)
            {
                case enMode.AddNew:

                    if (_AddNewLicenseClass())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateLicenseClass();

                default: 
                    return false;
            }
        }

        private bool _AddNewLicenseClass()
        {
            this.LicenseClassID = (enLicenseClassID)clsLicenseClassData.AddNewLicenseClass(this.ClassName, this.ClassDescription,
                                                                         this.MinimumAllowedAge, this.DefaultValidityLength,
                                                                         this.ClassFees);
            return this.LicenseClassID > 0;
        }

        private bool _UpdateLicenseClass()
        {
            return clsLicenseClassData.UpdateLicenseClass((int)this.LicenseClassID, this.ClassName, this.ClassDescription,
                                                          this.MinimumAllowedAge, this.DefaultValidityLength,
                                                          this.ClassFees);

        }
    }
}
