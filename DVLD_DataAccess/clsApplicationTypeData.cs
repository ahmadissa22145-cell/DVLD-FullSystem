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
    public class clsApplicationTypeData
    {

        public static bool FindApplicationTypeByID(int applicationTypeID, ref string applicationTypeTitle, ref float applicationFees)
        {
            bool isFound = false;


            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM ApplicationTypes
                            WHERE ApplicationTypeID = @ApplicationTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    applicationTypeTitle = reader["ApplicationTypeTitle"].ToString();
                    applicationFees = Convert.ToSingle(reader["ApplicationFees"]);
                }

                reader.Close();
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

            return isFound;
        }

        public static bool UpdateApplicationType(int applicationTypeID, string applicationTypeTitle, float applicationFees)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE ApplicationTypes
                             SET ApplicationTypeTitle = @ApplicationTypeTitle,
                                 ApplicationFees      = @ApplicationFees
                           WHERE ApplicationTypeID    = @ApplicationTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationTypeTitle", applicationTypeTitle);
            command.Parameters.AddWithValue("@ApplicationFees", applicationFees);
            command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);

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

        public static DataTable GetAllApplicationTypes()
        {
            DataTable dtApplicationTypes = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM ApplicationTypes";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtApplicationTypes.Load(reader);
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

            return dtApplicationTypes;
        }
    }
}
