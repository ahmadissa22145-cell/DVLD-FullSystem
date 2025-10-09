using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsDriverData
    {

        public static bool GetDriverInfoByDriverID(int driverID, ref int personID,
                                                   ref int createdByUserID, ref DateTime createdDate)
        {

            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"SELECT * FROM Drivers
                             WHERE DriverID = @DriverID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@DriverID", driverID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;

                                personID = Convert.ToInt32(reader["PersonID"]);
                                createdByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                                createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //   Console.WriteLine(ex.Message);
                isFound = false;
            }

            return isFound;
        }


        public static bool GetDriverInfoByPersonID(int personID, ref int driverID,
                                                  ref int createdByUserID, ref DateTime createdDate)
        {

            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM Drivers
                             WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", personID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    driverID = Convert.ToInt32(reader["driverID"]);
                    createdByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                //   Console.WriteLine(ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }


        public static DataTable GetAllDrivers()
        {
            DataTable dtDrivers = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Drivers_View ORDER BY FullName";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtDrivers.Load(reader);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
             //   Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dtDrivers;
        }


        public static int AddNewDriver(int personID, int createdByUserID)
        {

            int driverID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO Drivers (PersonID, CreatedByUserID, CreatedDate)
                             VALUES (@PersonID, @CreatedByUserID, @CreatedDate)

                             SELECT SCOPE_IDENTITY()";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", personID);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now.Date);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if(result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    driverID = insertedID;
                }
            }
            catch (Exception ex)
            {
                //   Console.WriteLine(ex.Message);
                driverID = -1;
            }
            finally
            {
                connection.Close();
            }

            return driverID;
        }


        public static bool UpdateDriver(int driverID, int personID, int createdByUserID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE Drivers
                             SET PersonID = @PersonID,
                                 CreatedByUserID = @CreatedByUserID
                             WHERE DriverID = @DriverID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DriverID", driverID);
            command.Parameters.AddWithValue("@PersonID", personID);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //   Console.WriteLine(ex.Message);
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
