using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsApplicationData
    {

        public static bool GetApplicationInfoByID(int applicationID, ref int applicantPersonID, ref DateTime applicationDate,
                                                   ref int applicationTypeID, ref byte applicationStatus,
                                                   ref DateTime lastStatusDate, ref float paidFees, ref int createdByUserID)
        {

            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM Applications 
                            WHERE ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", applicationID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    applicantPersonID = Convert.ToInt32(reader["ApplicantPersonID"]);
                    applicationDate   = Convert.ToDateTime(reader["ApplicationDate"]);
                    applicationTypeID = Convert.ToInt32(reader["ApplicationTypeID"]);
                    applicationStatus = Convert.ToByte(reader["ApplicationStatus"]);
                    lastStatusDate    = Convert.ToDateTime(reader["LastStatusDate"]);
                    paidFees          = Convert.ToSingle(reader["PaidFees"]);
                    createdByUserID   = Convert.ToInt32(reader["CreatedByUserID"]);

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

        public static DataTable GetAllApplications()
        {
            DataTable dtApplications = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM Applications";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtApplications.Load(reader);
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

            return dtApplications;
        }

        public static int AddNewApplication(int applicantPersonID, DateTime applicationDate,
                                            int applicationTypeID, byte applicationStatus,
                                            DateTime lastStatusDate, float paidFees, int createdByUserID)
        {

            int applicationID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO Applications(ApplicantPersonID, ApplicationDate, ApplicationTypeID, 
                                                      ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID)
                             VALUES
                                    (@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID, 
                                    @ApplicationStatus, @LastStatusDate, @PaidFees, @CreatedByUserID)
                            
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", applicantPersonID);
            command.Parameters.AddWithValue("@ApplicationDate", applicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatus", applicationStatus);
            command.Parameters.AddWithValue("@LastStatusDate", lastStatusDate);
            command.Parameters.AddWithValue("@PaidFees", paidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                applicationID = result != null ? Convert.ToInt32(result) : -1;
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

            return applicationID;
        }

        public static bool UpdateApplication(int applicationID, int applicantPersonID, DateTime applicationDate,
                                           int applicationTypeID, byte applicationStatus,
                                           DateTime lastStatusDate, float paidFees, int createdByUserID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE Applications
                             SET ApplicantPersonID = @ApplicantPersonID,
                                 ApplicationDate   = @ApplicationDate,
                                 ApplicationTypeID = @ApplicationTypeID,
                                 ApplicationStatus = @ApplicationStatus,
                                 LastStatusDate    = @LastStatusDate,
                                 PaidFees          = @PaidFees,
                                 CreatedByUserID   = @CreatedByUserID
                            
                             WHERE ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", applicationID);
            command.Parameters.AddWithValue("@ApplicantPersonID", applicantPersonID);
            command.Parameters.AddWithValue("@ApplicationDate", applicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatus", applicationStatus);
            command.Parameters.AddWithValue("@LastStatusDate", lastStatusDate);
            command.Parameters.AddWithValue("@PaidFees", paidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }

        public static bool DeleteApplication(int applicationID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE FROM Applications 
                            WHERE ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", applicationID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }

        public static bool IsApplicationExists(int applicationID)
        {

            bool isExists = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT 1 FROM Applications 
                            WHERE ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", applicationID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                isExists = result != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                isExists = false;
            }
            finally
            {
                connection.Close();
            }

            return isExists;
        }

        public static bool DoesPersonHaveActiveApplication(int personID, int applicationTypeID)
        {
            return (GetActiveApplicationID(personID, applicationTypeID) != -1);
        }

        public static int GetActiveApplicationID(int personID, int applicationTypeID)
        {
            int activeApplicationID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT ActiveApplicationID = ApplicationID FROM Applications
                             WHERE ApplicantPersonID = @ApplicantPersonID and
                                   ApplicationTypeID = @ApplicationTypeID and
                                   ApplicationStatus = 1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", personID);
            command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                activeApplicationID = result != null ? (int)result : -1;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            finally
            {
                connection.Close();
            }

            return activeApplicationID;
        }

        public static int GetActiveApplicationIDForLicenseClass(int personID, int applicationTypeID, int licenseClassID)
        {
            int activeApplicationID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"
                            SELECT ActiveApplicationID=Applications.ApplicationID  
                            From
                            Applications INNER JOIN
                            LocalDrivingLicenseApplications ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                            WHERE ApplicantPersonID = @ApplicantPersonID 
                            AND ApplicationTypeID = @ApplicationTypeID 
							AND LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                            AND ApplicationStatus IN (1,3)";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", personID);
            command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);
            command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                activeApplicationID = result != null ? (int)result : -1;
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

            return activeApplicationID;
        }

        public static bool UpdateStatus(int applicationID, byte newStatus)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE Applications
                             SET ApplicationStatus = @NewStatus,
                                 LastStatusDate    = @LastStatusDate
                            
                             WHERE ApplicationID   = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", applicationID);
            command.Parameters.AddWithValue("@NewStatus", newStatus);
            command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }
    }
}
