using DVLD_Shared;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    public static class clsTestData
    {

        public static bool GetTestInfoByID(int testID, ref int testAppointmentID, ref bool testResult,
                                           ref string notes, ref int createdByUserID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM TESTS
                             WHERE TestID = @TestID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestID", testID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    testAppointmentID = Convert.ToInt32(reader["TestAppointmentID"]);
                    testResult = Convert.ToBoolean(reader["TestResult"]);

                    notes = reader["Notes"] == DBNull.Value ? string.Empty : reader["Notes"].ToString();

                    createdByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                clsLogger.LogIntoEventViewer(clsGlobal.source, ex.Message, EventLogEntryType.Error);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass(int personID, int testTypeID, int licenseClass, ref int testID, ref int testAppointmentID, ref bool testResult,
                                                                         ref string notes, ref int createdByUserID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT TOP 1 TESTS.* FROM TESTS
                             INNER JOIN TestAppointments
                             	 ON TESTS.TestAppointmentID = TestAppointments.TestAppointmentID
                             INNER JOIN LocalDrivingLicenseApplications
                             	 ON TestAppointments.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID
                             INNER JOIN Applications
                             	 ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                             WHERE Applications.ApplicantPersonID = 1
                             	 AND   TestAppointments.TestTypeID = 1
                             	 AND   LocalDrivingLicenseApplications.LicenseClassID = 2
                             ORDER BY TestAppointments.TestAppointmentID DESC";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", personID);
            command.Parameters.AddWithValue("@TestTypeID", testTypeID);
            command.Parameters.AddWithValue("@LicenseClassID", licenseClass);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    testID = Convert.ToInt32(reader["TestID"]);
                    testAppointmentID = Convert.ToInt32(reader["TestAppointmentID"]);
                    testResult = Convert.ToBoolean(reader["TestResult"]);

                    notes = reader["Notes"] == DBNull.Value ? null : reader["Notes"].ToString();

                    createdByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                clsLogger.LogIntoEventViewer(clsGlobal.source, ex.Message, EventLogEntryType.Error);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static DataTable GetAllTests()
        {
            DataTable dtTests = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM TESTS";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtTests.Load(reader);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                clsLogger.LogIntoEventViewer(clsGlobal.source, ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                connection.Close();
            }

            return dtTests;
        }

        public static int AddNewTest(int testAppointmentID, bool testResult,
                                      string notes, int createdByUserID)
        {
            int testID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO Tests(TestAppointmentID, TestResult, Notes, CreatedByUserID)
                             VALUES
                              (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID);
            
                             UPDATE TestAppointments
                             SET IsLocked = 1
                             WHERE TestAppointmentID = @TestAppointmentID;
                             
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentID);
            command.Parameters.AddWithValue("@TestResult", testResult);

            if (string.IsNullOrEmpty(notes))
                command.Parameters.AddWithValue("@Notes", DBNull.Value);
            else
                command.Parameters.AddWithValue("@Notes", notes);

            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    testID = id;
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogIntoEventViewer(clsGlobal.source, ex.Message, EventLogEntryType.Error);
                testID = -1;
            }
            finally
            {
                connection.Close();
            }

            return testID;
        }

        public static bool UpdateTest(int testID, int testAppointmentID, bool testResult,
                                      string notes, int createdByUserID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE Tests
                             SET TestAppointmentID = @TestAppointmentID,
                                 TestResult = @TestResult,
                                 Notes = @Notes,
                                 CreatedByUserID = @CreatedByUserID
                            
                             WHERE TestID = @TestID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestID", testID);
            command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentID);
            command.Parameters.AddWithValue("@TestResult", testResult);

            if (string.IsNullOrEmpty(notes))
                command.Parameters.AddWithValue("@Notes", DBNull.Value);
            else
                command.Parameters.AddWithValue("@Notes", notes);

            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                clsLogger.LogIntoEventViewer(clsGlobal.source, ex.Message, EventLogEntryType.Error);
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
