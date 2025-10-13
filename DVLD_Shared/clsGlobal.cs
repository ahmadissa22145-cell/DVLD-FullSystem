using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DVLD_Shared
{

    public static class clsGlobal
    {

        public static string source = "DVLD";

        private static int _eventLogID = (clsLogger.LoadValueFromCurrentUserRegistry("DVLD", "EventID") as int?) ?? 0;
        public static int EventLogID
        {
            get => _eventLogID;

            set => _eventLogID = value;
        }

 
    }
}
