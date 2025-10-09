using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsUserData
    {


        public static bool GetUserInfoByUserID(int userID, ref int personID, ref string username,
                                    ref string password, ref bool isActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM Users 
                            WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", userID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    personID = (int)reader["PersonID"];
                    username = (string)reader["Username"];
                    password = (string)reader["Password"];
                    isActive = (bool)reader["IsActive"];
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static bool GetUserInfoByPersonID(int personID, ref int userID, ref string username,
                                    ref string password, ref bool isActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM Users 
                            WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", personID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound  = true;
                    userID   = (int)reader["UserID"];
                    username = (string)reader["Username"];
                    password = (string)reader["Password"];
                    isActive = (bool)reader["IsActive"];
                    
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static bool GetUserInfoByUsernameAndPassword(string username,string password, ref int userID,
                                                         ref int personID, ref bool isActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM Users 
                            WHERE Username = @Username and Password = @Password";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound  = true;
                    userID   = (int)reader["UserID"];
                    personID = (int)reader["personID"];
                    isActive = (bool)reader["IsActive"];
                    
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }


        public static int AddNewUser(int personID, string username, string password, bool isActive)
        {
            int userID = -1; 
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"
                              INSERT INTO Users (PersonID, Username, Password, IsActive)

                              VALUES
                              (@PersonID, @Username, @Password, @IsActive)  
                    
                              SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", personID);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);
            command.Parameters.AddWithValue("@IsActive", isActive);

            try
            {
                connection.Open();

                //becuses one filed return 
                object result = command.ExecuteScalar();

                if(result != null ) 
                    userID = Convert.ToInt32(result); 

            }
            catch (Exception ex)
            {
                //  Console.WriteLine(ex.Message);
                return -1;
            }
            finally
            {
                connection.Close();
            }

            return userID;
        }

        public static bool UpdateUser(int userID, int personID, string username,
                                      string password, bool isActive)
        {
            SqlConnection connection = new SqlConnection (clsDataAccessSettings.ConnectionString);

            string query = @"
                             UPDATE Users
                             SET PersonID = @PersonID,
                                 UserName = @Username,
                                 Password = @Password,
                                 IsActive = @IsActive

                           WHERE UserID   = @UserID";

            SqlCommand command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@UserID"  ,   userID);
            command.Parameters.AddWithValue("@PersonID", personID);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);
            command.Parameters.AddWithValue("@IsActive", isActive);

            try
            {
                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                return (rowsAffected > 0);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }

        }

        public static bool DeleteUser(int userID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE FROM Users
                             WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand (query,connection);

            command.Parameters.AddWithValue("@UserID", userID);

            try
            {
                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                return (rowsAffected > 0);
            }
            catch (Exception ex)
            {
              //  Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public static DataTable GetAllUsers()
        {
            DataTable dtUsers = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"
                             SELECT Users.UserID, Users.PersonID,
                             (People.FirstName + ' '+ People.SecondName +' '+ ISNULL(People.ThirdName,'') + People.LastName) AS FullName,
                             Users.UserName, Users.IsActive
                             FROM  Users INNER JOIN
                                    People on Users.PersonID = People.PersonID  ";

            SqlCommand command = new SqlCommand (query,connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtUsers.Load(reader);
                }

                reader.Close();
            }
            catch(Exception ex)
            {
                // Console.WriteLine(ex.Message);
                return dtUsers;
            }
            finally
            {
                connection.Close();
            }
            return dtUsers;
        }

        public static bool IsUserExist(int userID)
        {
            bool isExist = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT 1 FROM Users
                             WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", userID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                isExist = (result != null);
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
            return isExist;
        }

        public static bool IsUserExist(string username)
        {
            bool isExist = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT 1 FROM Users
                             WHERE UserName = @UserName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", username);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                isExist = (result != null);
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
            return isExist;
        }

        public static bool DoesPersonHaveUser(int personID)
        {
            bool isExist = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT 1 FROM Users
                             WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", personID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                isExist = (result != null);
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
            return isExist;
        }

        public static bool ChangePassword(int userID, string newPassword)
        {

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"
                             UPDATE Users
                             SET Password = @Password
                                
                             WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", userID);
            command.Parameters.AddWithValue("@Password", newPassword);

            try
            {
                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {

                // Console.WriteLine(ex.Message);
                return false;

            }
            finally
            {
                connection.Close();
            }
        }


    }
}
