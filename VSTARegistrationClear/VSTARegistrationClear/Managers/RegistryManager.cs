using System;
using System.Diagnostics;
using Microsoft.Win32;
using Ninject.Extensions.Logging;

namespace VSTARegistrationClear.Managers
{
    public class RegistryManager : IRegistryManager
    {
        private readonly ILogger mLogger;

        public RegistryManager(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException("logger");

            this.mLogger = logger;
        }

        public bool KeyExists(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("key");

            string sHive = key.Substring(0, key.IndexOf('\\'));
            string sKey = key.Substring(key.IndexOf('\\') + 1);

            RegistryKey oKey = null;

            try
            {
                switch (sHive)
                {
                    case "HKEY_CLASSES_ROOT":
                        oKey = Registry.ClassesRoot.OpenSubKey(sKey, false);
                        break;
                    case "HKEY_CURRENT_USER":
                        oKey = Registry.CurrentUser.OpenSubKey(sKey, false);
                        break;
                    case "HKEY_LOCAL_MACHINE":
                        oKey = Registry.LocalMachine.OpenSubKey(sKey, false);
                        break;
                    case "HKEY_USERS":
                        oKey = Registry.Users.OpenSubKey(sKey, false);
                        break;
                    case "HKEY_CURRENT_CONFIG":
                        oKey = Registry.CurrentConfig.OpenSubKey(sKey, false);
                        break;
                }

                if (oKey != null)
                    return true;
                else
                    return false;
            }
            finally
            {
                if (oKey != null)
                    oKey.Dispose();
            }
        }

        public bool DeleteKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("key");

            string sHive = key.Substring(0, key.IndexOf('\\'));
            string sKey = key.Substring(key.IndexOf('\\') + 1);

            switch (sHive)
            {
                case "HKEY_CLASSES_ROOT":
                    Registry.ClassesRoot.DeleteSubKeyTree(sKey, false);
                    break;
                case "HKEY_CURRENT_USER":
                    Registry.CurrentUser.DeleteSubKeyTree(sKey, false);
                    break;
                case "HKEY_LOCAL_MACHINE":
                    Registry.LocalMachine.DeleteSubKeyTree(sKey, false);
                    break;
                case "HKEY_USERS":
                    Registry.Users.DeleteSubKeyTree(sKey, false);
                    break;
                case "HKEY_CURRENT_CONFIG":
                    Registry.CurrentConfig.DeleteSubKeyTree(sKey, false);
                    break;
            }

            return true;
        }

        public void ExportKey(string key, string filename)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("key");

            if (string.IsNullOrEmpty(filename))
                throw new ArgumentException("filename");

            string path = "\"" + filename + "\"";
            string registryKey = "\"" + key + "\"";

            Process oProc = new Process();

            try
            {
                oProc.StartInfo.FileName = "regedit.exe";
                oProc.StartInfo.UseShellExecute = false;
                oProc = Process.Start("regedit.exe", "/e " + path + " " + registryKey + "");

                if (oProc != null)
                    oProc.WaitForExit();
            }
            catch (Exception er)
            {
                throw new Exception(string.Format("Error backing up registry key: {0}", er.Message), er);
            }
            finally
            {
                if (oProc != null)
                    oProc.Dispose();
            }
        }
    }
}
