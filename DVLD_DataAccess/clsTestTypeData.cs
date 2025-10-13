using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_Shared;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    public class clsTestTypeData
    {

        public static bool GetTestTypeByID(int testTypeID, ref string testTypeTitle,
                                           ref string testTypeDescription, ref float testTypeFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM TestTypes
                             WHERE TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeID", testTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    testTypeTitle       = reader["TestTypeTitle"].ToString();
                    testTypeDescription = reader["TestTypeDescription"].ToString();
                    testTypeFees        = Convert.ToSingle(reader["TestTypeFees"]);
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

        public static DataTable GetAllTestTypes()
        {
            DataTable _dtTestTypes = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM TestTypes";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    _dtTestTypes.Load(reader);
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

            return _dtTestTypes;
        }

        public static bool UpdateTestType(int testTypeID, string testTypeTitle,
                                           string testTypeDescription, float testTypeFees)
        {
            
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query =
                            @"UPDATE TestTypes
                              SET TestTypeTitle       = @TestTypeTitle,
                                  TestTypeDescription = @TestTypeDescription,
                                  TestTypeFees        = @TestTypeFees
                
                              WHERE TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeTitle", testTypeTitle);
            command.Parameters.AddWithValue("@TestTypeDescription", testTypeDescription);
            command.Parameters.AddWithValue("@TestTypeFees", testTypeFees);
            command.Parameters.AddWithValue("@TestTypeID", testTypeID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                clsLogger.LogIntoEventViewer(clsGlobal.source, ex.Message, EventLogEntryType.Error);
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
