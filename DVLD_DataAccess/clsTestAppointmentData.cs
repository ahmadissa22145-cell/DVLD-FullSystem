using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsTestAppointmentData
    {

        public static bool GetTestAppointmentByID(int testAppointmentID, ref int testTypeID, ref int localDrivingLicenseApplicationID,
                                                   ref DateTime appointmentDate, ref float paidFees, ref int createdByUserID,
                                                   ref bool isLocked, ref int retakeTestApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM TestAppointments
                             WHERE TestAppointmentID = @TestAppointmentID";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentID);
  
            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    testTypeID = (int)reader["TestTypeID"];
                    localDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                    appointmentDate = (DateTime)reader["AppointmentDate"];
                    paidFees = Convert.ToSingle(reader["PaidFees"]);
                    createdByUserID = (int)reader["CreatedByUserID"];
                    isLocked = (bool)reader["IsLocked"];

                    if (reader["RetakeTestApplicationID"] == DBNull.Value)

                        retakeTestApplicationID = -1;
                    else
                        retakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetLastTestAppointment(int localDrivingLicenseApplicationID, int testTypeID, ref int testAppointmentID,
                                                  ref DateTime appointmentDate, ref float paidFees, ref int createdByUserID,
                                                  ref bool isLocked, ref int retakeTestApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT TOP 1 * FROM TestAppointments
                             WHERE (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)
                             AND (TestTypeID = @TestTypeID)
                             ORDER BY TestAppointmentID DESC";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", testTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    testAppointmentID = (int)reader["TestAppointmentID"];
                    appointmentDate = (DateTime)reader["AppointmentDate"];
                    paidFees = Convert.ToSingle(reader["PaidFees"]);
                    createdByUserID = (int)reader["CreatedByUserID"];
                    isLocked = (bool)reader["IsLocked"];

                    if (reader["RetakeTestApplicationID"] == DBNull.Value)

                        retakeTestApplicationID = -1;
                    else
                        retakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int AddNewTestAppointment(int localDrivingLicenseApplicationID, int testTypeID,
                                                DateTime appointmentDate, float paidFees, int createdByUserID,
                                                bool isLocked, int retakeTestApplicationID)
        {

            int testAppointmentID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO TestAppointments(TestTypeID , LocalDrivingLicenseApplicationID,
                                                          AppointmentDate, PaidFees, CreatedByUserID,
                                                          IsLocked, RetakeTestApplicationID)
    
                              VALUES 
                                    (@TestTypeID, @LocalDrivingLicenseApplicationID,
                                     @AppointmentDate, @PaidFees, @CreatedByUserID,
                                     @IsLocked, @RetakeTestApplicationID)

                             SELECT SCOPE_IDENTITY()";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeID", testTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@AppointmentDate", appointmentDate);
            command.Parameters.AddWithValue("@PaidFees", paidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
            command.Parameters.AddWithValue("@IsLocked", isLocked);

            if (retakeTestApplicationID != -1)

                command.Parameters.AddWithValue("@RetakeTestApplicationID", retakeTestApplicationID);

            else
                command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                testAppointmentID = result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
            finally
            {
                connection.Close();
            }

            return testAppointmentID;
        }

        public static bool UpdateTestAppointment(int testAppointmentID, int localDrivingLicenseApplicationID, int testTypeID,
                                                DateTime appointmentDate, float paidFees, int createdByUserID,
                                                bool isLocked, int retakeTestApplicationID)
        {
            int rowsAffected = 0;
            
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"
                            UPDATE TestAppointments
                            SET 
                                TestTypeID = @TestTypeID,
                                LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID,
                                AppointmentDate = @AppointmentDate,
                                PaidFees = @PaidFees,
                                CreatedByUserID = @CreatedByUserID,
                                IsLocked = @IsLocked,
                                RetakeTestApplicationID = @RetakeTestApplicationID
                            WHERE TestAppointmentID = @TestAppointmentID";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentID);
            command.Parameters.AddWithValue("@TestTypeID", testTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@AppointmentDate", appointmentDate);
            command.Parameters.AddWithValue("@PaidFees", paidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
            command.Parameters.AddWithValue("@IsLocked", isLocked);

            if (retakeTestApplicationID != -1)

                command.Parameters.AddWithValue("@RetakeTestApplicationID", retakeTestApplicationID);

            else
                command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }

        public static bool DeleteTestAppointment(int testAppointmentID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE FROM TestAppointments
                             WHERE TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
               // Console.WriteLine(ex.ToString());

                return false;
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }

        public static DataTable GetAllTestAppointments()
        {
            DataTable dtTestAppointments = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM TestAppointments_View    
                             ORDER BY TestAppointmentID DESC";

            SqlCommand command = new SqlCommand(query, connection);


            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtTestAppointments.Load(reader);
                }

                reader.Close(); 
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }

            return dtTestAppointments;
        }

        public static DataTable GetAllTestAppointmentsPerTestType(int localDrivingLicenseApplicationID, int testTypeID)
        {
            DataTable dtTestAppointments = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT TestAppointmentID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked FROM TestAppointments
                             WHERE (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)
                             AND (TestTypeID = @TestTypeID)
                             ORDER BY TestAppointmentID DESC";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", testTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtTestAppointments.Load(reader);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }

            return dtTestAppointments;
        }

        public static int GetTestTypeID(int testAppointmentID)
        {
            int testTypeID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT TestTypeID FROM TestAppointments
                             WHERE TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                testTypeID = result != null ? (int)result : -1;
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.ToString());

                return -1;
            }
            finally
            {
                connection.Close();
            }

            return testTypeID;
        }


        public static int GetTestID(int TestAppointmentID)
        {
            int TestID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"select TestID from Tests where TestAppointmentID=@TestAppointmentID;";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestID = insertedID;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }


            return TestID;

        }

    }
}
