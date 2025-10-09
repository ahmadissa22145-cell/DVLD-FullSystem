using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsApplicationType
    {
        public int ApplicationTypeID { get; private set; }
        public string ApplicationTypeTitle { get; set; }
        public float ApplicationFees { get; set; }

        private clsApplicationType(int applicationTypeID, string applicationTypeTitle, float applicationFees)
        {
            this.ApplicationTypeID = applicationTypeID;
            this.ApplicationTypeTitle = applicationTypeTitle;
            this.ApplicationFees = applicationFees;
        }

        public static clsApplicationType Find(int applicationTypeID)
        {
            string applicationTypeTitle = string.Empty;
            float applicationFees = 0.0f;

            if(clsApplicationTypeData.FindApplicationTypeByID(applicationTypeID, ref applicationTypeTitle, ref applicationFees))
            {
                return new clsApplicationType(applicationTypeID, applicationTypeTitle, applicationFees);
            }

            return null;
        }

        public bool UpdateApplicationType()
        {
            return clsApplicationTypeData.UpdateApplicationType(this.ApplicationTypeID, 
                                                                this.ApplicationTypeTitle,
                                                                this.ApplicationFees);
        }

        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypeData.GetAllApplicationTypes();
        }


    }
}
