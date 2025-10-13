using DVLD_Shared;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    public class clsInternationalLicenseData
    {

        public static bool GetInternationalLicenseByID(int internationalLicenseID, ref int applicationID, ref int driverID,
                                                       ref int issuedUsingLocalLicenseID, ref DateTime issueDate,
                                                       ref DateTime expirationDate, ref bool isActive, ref int createdByUserID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM InternationalLicenses
                             WHERE InternationalLicenseID = @InternationalLicenseID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@InternationalLicenseID", internationalLicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    applicationID = Convert.ToInt32(reader["ApplicationID"]);
                    driverID = Convert.ToInt32(reader["DriverID"]);
                    issuedUsingLocalLicenseID = Convert.ToInt32(reader["IssuedUsingLocalLicenseID"]);
                    issueDate = Convert.ToDateTime(reader["IssueDate"]);
                    expirationDate = Convert.ToDateTime(reader["ExpirationDate"]);
                    isActive = Convert.ToBoolean(reader["IsActive"]);
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

        public static DataTable GetAllInternationalLicenses()
        {
            DataTable dtInternationalLicenses = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM InternationalLicenses";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtInternationalLicenses.Load(reader);
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

            return dtInternationalLicenses;
        }

        public static DataTable GetAllInternationalLicensesByDriverID(int driverID)
        {
            DataTable dtdriverInternationalLicenses = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT
                             I.InternationalLicenseID,
                             I.ApplicationID,
                             I.IssuedUsingLocalLicenseID,
                             I.IssueDate,
                             I.ExpirationDate,
                             I.IsActive
                            FROM InternationalLicenses I
                            WHERE DriverID = @DriverID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DriverID", driverID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtdriverInternationalLicenses.Load(reader);
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

            return dtdriverInternationalLicenses;
        }

        public static int AddNewInternationalLicense(int applicationID, int driverID,
                                                      int issuedUsingLocalLicenseID, DateTime issueDate,
                                                      DateTime expirationDate, bool isActive, int createdByUserID)
        {
            int internationalLicenseID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE InternationalLicenses
                             SET IsActive = 0
                             WHERE DriverID = @DriverID;

                            INSERT INTO InternationalLicenses (ApplicationID, DriverID, IssuedUsingLocalLicenseID,
                                                                IssueDate, ExpirationDate, IsActive, CreatedByUserID)

                            VALUES 
                                   (@ApplicationID, @DriverID, @IssuedUsingLocalLicenseID,
                                   @IssueDate, @ExpirationDate, @IsActive, @CreatedByUserID);

                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", applicationID);
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", issuedUsingLocalLicenseID);
            command.Parameters.AddWithValue("@DriverID", driverID);
            command.Parameters.AddWithValue("@IssueDate", issueDate);
            command.Parameters.AddWithValue("@ExpirationDate", expirationDate);
            command.Parameters.AddWithValue("@IsActive", isActive);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                {
                    internationalLicenseID = ID;
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogIntoEventViewer(clsGlobal.source, ex.Message, EventLogEntryType.Error);
                internationalLicenseID = -1;
            }
            finally
            {
                connection.Close();
            }

            return internationalLicenseID;
        }

        public static bool UpdateInternationalLicense(int internationalLicenseID, int applicationID, int driverID,
                                                      int issuedUsingLocalLicenseID, DateTime issueDate,
                                                      DateTime expirationDate, bool isActive, int createdByUserID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE InternationalLicenses
                             SET ApplicationID = @ApplicationID,
                                 DriverID = @DriverID,
                                 IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID,
                                 IssueDate = @IssueDate,
                                 ExpirationDate = @ExpirationDate,
                                 IsActive = @IsActive,
                                 CreatedByUserID = @CreatedByUserID
                             WHERE InternationalLicenseID = @InternationalLicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@InternationalLicenseID", internationalLicenseID);
            command.Parameters.AddWithValue("@ApplicationID", applicationID);
            command.Parameters.AddWithValue("@DriverID", driverID);
            command.Parameters.AddWithValue("@IssueDate", issueDate);
            command.Parameters.AddWithValue("@ExpirationDate", expirationDate);
            command.Parameters.AddWithValue("@IsActive", isActive);
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

            return (rowsAffected > 0);
        }

        public static int GetActiveInternationalLicenseByDriverID(int driverID)
        {
            int internationalLicenseID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT InternationalLicenseID FROM InternationalLicenses
                             WHERE DriverID = @DriverID
                             AND   IsActive = 1";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", driverID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                {
                    internationalLicenseID = ID;
                }

            }
            catch (Exception ex)
            {
                clsLogger.LogIntoEventViewer(clsGlobal.source, ex.Message, EventLogEntryType.Error);
                internationalLicenseID = -1;
            }
            finally
            {
                connection.Close();
            }

            return internationalLicenseID;
        }

    }
}
