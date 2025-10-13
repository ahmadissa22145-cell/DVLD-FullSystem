using DVLD_Shared;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    public class clsDetainLicenseData
    {

        public static bool GetDetainLicenseInfoByID(int detainID, ref int licenseID, ref DateTime detainDate,
                                                    ref float fineFees, ref int createdByUserID, ref bool isReleased,
                                                    ref DateTime releaseDate, ref int releasedByUserID, ref int releaseApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM DetainedLicenses
                             WHERE DetainID = @DetainID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainID", detainID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    licenseID = Convert.ToInt32(reader["LicenseID"]);
                    detainDate = Convert.ToDateTime(reader["DetainDate"]);
                    fineFees = Convert.ToSingle(reader["FineFees"]);
                    createdByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    isReleased = Convert.ToBoolean(reader["IsReleased"]);

                    if (reader["ReleaseDate"] == DBNull.Value)
                    {
                        releaseDate = DateTime.MaxValue;
                    }
                    else
                    {
                        releaseDate = Convert.ToDateTime(reader["ReleaseDate"]);
                    }

                    if (reader["ReleasedByUserID"] == DBNull.Value)
                    {
                        releasedByUserID = -1;
                    }
                    else
                    {
                        releasedByUserID = Convert.ToInt32(reader["ReleasedByUserID"]);
                    }


                    if (reader["ReleaseApplicationID"] == DBNull.Value)
                    {
                        releaseApplicationID = -1;
                    }
                    else
                    {
                        releaseApplicationID = Convert.ToInt32(reader["ReleaseApplicationID"]);
                    }

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

        public static bool GetDetainLicenseInfoByLicenseID(int licenseID, ref int detainID, ref DateTime detainDate,
                                            ref float fineFees, ref int createdByUserID, ref bool isReleased,
                                            ref DateTime releaseDate, ref int releasedByUserID, ref int releaseApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM DetainedLicenses
                             WHERE LicenseID = @LicenseID
                             AND   IsReleased = 0";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", licenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    detainID = Convert.ToInt32(reader["DetainID"]);
                    detainDate = Convert.ToDateTime(reader["DetainDate"]);
                    fineFees = Convert.ToSingle(reader["FineFees"]);
                    createdByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    isReleased = Convert.ToBoolean(reader["IsReleased"]);

                    if (reader["ReleaseDate"] == DBNull.Value)
                    {
                        releaseDate = DateTime.MaxValue;
                    }
                    else
                    {
                        releaseDate = Convert.ToDateTime(reader["ReleaseDate"]);
                    }

                    if (reader["ReleasedByUserID"] == DBNull.Value)
                    {
                        releasedByUserID = -1;
                    }
                    else
                    {
                        releasedByUserID = Convert.ToInt32(reader["ReleasedByUserID"]);
                    }


                    if (reader["ReleaseApplicationID"] == DBNull.Value)
                    {
                        releaseApplicationID = -1;
                    }
                    else
                    {
                        releaseApplicationID = Convert.ToInt32(reader["ReleaseApplicationID"]);
                    }
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


        public static int AddNewDetainLicense(int licenseID, DateTime detainDate,
                                              float fineFees, int createdByUserID)
        {
            int detainID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO DetainedLicenses(LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased)
                              VALUES (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, @IsReleased)
    
                             SELECT SCOPE_IDENTITY()";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", licenseID);
            command.Parameters.AddWithValue("@DetainDate", detainDate);
            command.Parameters.AddWithValue("@FineFees", fineFees);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
            command.Parameters.AddWithValue("@IsReleased", 0);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    detainID = insertedID;
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogIntoEventViewer(clsGlobal.source, ex.Message, EventLogEntryType.Error);
                detainID = -1;
            }
            finally
            {
                connection.Close();
            }

            return detainID;
        }

        public static bool UpdateDetainLicense(int detainID, int licenseID, DateTime detainDate,
                                              float fineFees, int createdByUserID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE DetainedLicenses
                             SET LicenseID = @LicenseID,
                             	DetainDate = @DetainDate,
                             	FineFees = @FineFees,
                             	CreatedByUserID = @CreatedByUserID
                             WHERE DetainID = @DetainID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", licenseID);
            command.Parameters.AddWithValue("@DetainDate", detainDate);
            command.Parameters.AddWithValue("@FineFees", fineFees);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
            command.Parameters.AddWithValue("@DetainID", detainID);

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

        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dtDetainedLicenses = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Select * from DetainedLicenses_View";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtDetainedLicenses.Load(reader);
                }
            }
            catch (Exception ex)
            {
                clsLogger.LogIntoEventViewer(clsGlobal.source, ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                connection.Close();
            }

            return dtDetainedLicenses;
        }

        public static bool ReleaseDetainLicense(int detainID, int releasedByUserID,
                                                int releaseApplicationID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE DetainedLicenses
                             SET IsReleased = @IsReleased,
                                 ReleaseDate = @ReleaseDate,
                             	 ReleasedByUserID = @ReleasedByUserID,
                             	 ReleaseApplicationID = @ReleaseApplicationID
                             WHERE DetainID = @DetainID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@IsReleased", 1);
            command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
            command.Parameters.AddWithValue("@ReleasedByUserID", releasedByUserID);
            command.Parameters.AddWithValue("@ReleaseApplicationID", releaseApplicationID);
            command.Parameters.AddWithValue("@DetainID", detainID);

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

        public static bool IsLicenseDetained(int licenseID)
        {
            bool isDetained = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT 1 from DetainedLicenses
                             WHERE LicenseID = @LicenseID
                             AND   IsReleased = 0";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", licenseID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                isDetained = (result != null);
            }
            catch (Exception ex)
            {
                clsLogger.LogIntoEventViewer(clsGlobal.source, ex.Message, EventLogEntryType.Error);
                isDetained = false;
            }
            finally
            {
                connection.Close();
            }

            return isDetained;
        }

    }
}
