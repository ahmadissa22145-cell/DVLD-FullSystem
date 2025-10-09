using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsLicenseClassData
    {
        public static bool FindLicenseClassByID(int licenseClassID, ref string className, ref string classDescription,
                                                ref int minimumAllowedAge, ref int defaultValidityLength, ref float classFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM LicenseClasses 
                             WHERE LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    className             = Convert.ToString(reader["ClassName"]);
                    classDescription      = Convert.ToString(reader["ClassDescription"]);
                    minimumAllowedAge     = Convert.ToInt32(reader["MinimumAllowedAge"]);
                    defaultValidityLength = Convert.ToInt32(reader["DefaultValidityLength"]);
                    classFees             = Convert.ToSingle(reader["ClassFees"]);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool FindLicenseClassByName(string className, ref int licenseClassID, ref string classDescription,
                                                ref int minimumAllowedAge, ref int defaultValidityLength, ref float classFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM LicenseClasses 
                             WHERE ClassName = @ClassName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClassName", className);
            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    licenseClassID        = Convert.ToInt32(reader["licenseClassID"]);
                    classDescription      = Convert.ToString(reader["ClassDescription"]);
                    minimumAllowedAge     = Convert.ToInt32(reader["MinimumAllowedAge"]);
                    defaultValidityLength = Convert.ToInt32(reader["DefaultValidityLength"]);
                    classFees             = Convert.ToSingle(reader["ClassFees"]);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int GetMinimumAllowedAgeForLicenseClass(int licenseClassID)
        {
            int minimumAllowedAge = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT MinimumAllowedAge FROM LicenseClasses 
                             WHERE LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
            try
            {
                connection.Open();

                object result = command.ExecuteScalar();



                minimumAllowedAge = result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            finally
            {
                connection.Close();
            }

            return minimumAllowedAge;
        }

        public static DataTable GetAllLicenseClasses()
        {
            DataTable dtLicenseClasses = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM LicenseClasses";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtLicenseClasses.Load(reader);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dtLicenseClasses;
        }

        public static int AddNewLicenseClass(string className, string classDescription,
                                             int minimumAllowedAge, int defaultValidityLength, float classFees)
        {
            int licenseClassID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO LicenseClasses(ClassName, ClassDescription,
                                                      MinimumAllowedAge, DefaultValidityLength,
                                                      ClassFees)
    
                              VALUES(@ClassName, @ClassDescription,
                                     @MinimumAllowedAge, @DefaultValidityLength,
                                     @ClassFees) 

                                SELECT SCOPE_IDENTITY()";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClassName", className);
            command.Parameters.AddWithValue("@ClassDescription", classDescription);
            command.Parameters.AddWithValue("@MinimumAllowedAge", minimumAllowedAge);
            command.Parameters.AddWithValue("@DefaultValidityLength", defaultValidityLength);
            command.Parameters.AddWithValue("@ClassFees", classFees);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();



                licenseClassID = result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                licenseClassID = -1;
            }
            finally
            {
                connection.Close();
            }

            return licenseClassID;
        }

        public static bool UpdateLicenseClass(int licenseClassID, string className, string classDescription,
                                             int minimumAllowedAge, int defaultValidityLength, float classFees)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE LicenseClasses
                             SET ClassName = @ClassName,
                                 ClassDescription = @ClassDescription,
                                 MinimumAllowedAge = @MinimumAllowedAge,
                                 DefaultValidityLength = @DefaultValidityLength,
                                 ClassFees = @ClassFees
                             WHERE LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
            command.Parameters.AddWithValue("@ClassName", className);
            command.Parameters.AddWithValue("@ClassDescription", classDescription);
            command.Parameters.AddWithValue("@MinimumAllowedAge", minimumAllowedAge);
            command.Parameters.AddWithValue("@DefaultValidityLength", defaultValidityLength);
            command.Parameters.AddWithValue("@ClassFees", classFees);


            try
            {
                connection.Open();

                 rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                rowsAffected = 0;
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }

    }
}
