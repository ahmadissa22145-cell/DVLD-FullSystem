using DVLD_Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.CompilerServices;

namespace DVLD.Global_Classes
{

    internal static class clsGlobal
    {
        internal static clsUser CurrentUserInfo { get; set; }

        private readonly static  string _directoryProjectPath = System.IO.Directory.GetCurrentDirectory();
        private readonly static string _fileSaveCredentialsPath = Path.Combine(_directoryProjectPath, "data.txt");
        private const string KeyPath = @"HKEY_CURRENT_USER\Software\DVLD";
        private const string UsernameValueName = "Username";
        private const string PasswordValueName = "Password";
        private const string Separator = "#//#";

        // Save credentials as plain text with Separator and return boolean
        internal static bool RememberUsernameAndPasswordInsideRegistry(string username, string password)
        {
            bool isRemembered = false;

            if (username == null)
            {

            }

            try
            {

                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(KeyPath))
                {
                    if(key == null) return false;

                    key.SetValue(UsernameValueName, username);
                    key.SetValue(PasswordValueName, password);
                }

                isRemembered = true;
            }
            catch (Exception ex)
            {
                //  Console.WriteLine(ex.Message);
                isRemembered = false;
            }

            return isRemembered;
        }

        [Obsolete("This Is Old Way")]
        internal static bool RememberUsernameAndPassword(string username, string password)
        {
            string Separator = "#//#";

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

                string dataToSave = username + Separator + password;

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

        internal static bool GetStoredCredentialsDataFromRegistry(ref string username, ref string password)
        {
            bool isObtained = false;

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(KeyPath))
                {
                    if (key == null) return false;

                    username = key.GetValue(UsernameValueName, null) as string;
                    password = key.GetValue(PasswordValueName, null) as string;
                }


                isObtained = !string.IsNullOrEmpty(username) & !string.IsNullOrEmpty(password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                isObtained = false;
            }

            return isObtained;
        }

        [Obsolete("This Is Old Way")]
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
