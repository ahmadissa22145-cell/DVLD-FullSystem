using System;
using System.Configuration;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    static class clsDataAccessSettings
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

       
       
    }
}
