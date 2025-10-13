using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace DVLD_Shared
{
    public class clsLogger
    {
        public static bool LogIntoEventViewer(string source, string message, EventLogEntryType type, string logName = "Application")
        {
            var method = new StackTrace().GetFrame(1).GetMethod();
            string className = method.DeclaringType.Name ?? "Unknown Class";
            string methodName = method.Name ?? "Unknown Method";
            string level = type.ToString();

            string fullMessage = $"[{level}] [{className}] [{methodName}] -> {message}";

            try
            {
                if (!EventLog.SourceExists(source))
                    EventLog.CreateEventSource(source, logName);

                EventLog.WriteEntry(source, fullMessage, type, ++clsGlobal.EventLogID);

                SaveValueIntoCurrentUserRegistry(clsGlobal.source, "EventID", clsGlobal.EventLogID, kind : RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }

        public static bool SaveValueIntoCurrentUserRegistry(string subKeyName, string valueName, object valueData, RegistryValueKind kind = RegistryValueKind.String, string superkeyName = "Software")
        {
            string keyPath = $@"{superkeyName}\{subKeyName}";

            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(keyPath))
                {
                    key.SetValue(valueName, valueData, kind);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }

            return true;
        }

        public static object LoadValueFromCurrentUserRegistry(string subKeyName, string valueName, string superkeyName = "Software")
        {
            object valueData = null;
            string keyPath = $@"{superkeyName}\{subKeyName}";

            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(keyPath))
                {
                    valueData = key.GetValue(valueName, null);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return valueData;
        }
    }
}
