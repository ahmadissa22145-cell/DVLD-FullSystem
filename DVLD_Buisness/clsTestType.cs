using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsTestType
    {
        public enum enTestTypes { VisionTest = 1, WrittenTest = 2, StreetTest = 3};

        public enTestTypes TestTypeID { get; private set; }
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription { get; set; }
        public float TestTypeFees { get; set; }

        private clsTestType(enTestTypes testTypeID, string testTypeTitle, string testTypeDescription, float testTypeFees)
        {
            TestTypeID = testTypeID;
            TestTypeTitle = testTypeTitle;
            TestTypeDescription = testTypeDescription;
            TestTypeFees = testTypeFees;
        }

        public static clsTestType FindTestTypeByID(enTestTypes testTypeID)
        {
            string testTypeTitle = string.Empty, testTypeDescription = string.Empty;

            float testTypeFees = 0.0f;


            if (clsTestTypeData.GetTestTypeByID((int)testTypeID, ref testTypeTitle, ref testTypeDescription, ref testTypeFees))
            {
                return new clsTestType(testTypeID, testTypeTitle, testTypeDescription, testTypeFees);
            }

            return null;
        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypeData.GetAllTestTypes();
        }

        public bool UpdateTestTypes()
        {
            return clsTestTypeData.UpdateTestType((int)this.TestTypeID, this.TestTypeTitle,
                                                  this.TestTypeDescription, this.TestTypeFees);
        }
    }
}
