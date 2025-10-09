using DVLD_Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Global_Classes
{
    /*
    *1 Global static clsUser Proparties to use this in system
    *2 declear method to get stored data 
    *3 declear method to save credintial user data in file 
    */
    internal static class clsGlobal
    {
        internal static clsUser CurrentUserInfo { get; set; }

        private readonly static  string _directoryProjectPath = System.IO.Directory.GetCurrentDirectory();
        private readonly static string _fileSaveCredentialsPath = Path.Combine(_directoryProjectPath, "data.txt");


        // Save credentials as plain text with separator and return boolean
        internal static bool RememberUsernameAndPassword(string username, string password)
        {
            string separator = "#//#";

            try
            {

                if(username == string.Empty)
                {
                    if (File.Exists(_fileSaveCredentialsPath))
                    {
                        File.Delete(_fileSaveCredentialsPath);  
                    }

                    return true;
                }

                string dataToSave = username + separator + password;

                using (StreamWriter writer = new StreamWriter(_fileSaveCredentialsPath))
                {
                    writer.WriteLine(dataToSave);
                    
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occuerd : " + ex.Message);
                return false;
            }
        }


        // to read stored data in file and return username and password
        internal static bool GetStoredCredentialsData(ref string username, ref string password)
        {

            try
            {
                if (!File.Exists(_fileSaveCredentialsPath))
                {
                    return false;
                }

                string line = "";

                using (StreamReader reader = new StreamReader(_fileSaveCredentialsPath))
                {
                    line = reader.ReadLine();
                    string[] result = line.Split(new string[] { "#//#" }, StringSplitOptions.None);

                    username = result[0];
                    password = result[1];

                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred : " + ex.Message);
                return false;
            }
        }
    }
}
