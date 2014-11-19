using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
                string cmd = "regedit.exe";

                mLogger.Info("Backing up registry key '{0}' to '{1}'...", key, filename);

                oProc.StartInfo.FileName = cmd;
                oProc.StartInfo.UseShellExecute = false;

                string args = "/e " + path + " " + registryKey + "";

                mLogger.Info("Executing: {0} {1}", cmd, args);

                oProc = Process.Start(cmd, args);

                if (oProc != null)
                    oProc.WaitForExit();
            }
            catch (Exception er)
            {
                mLogger.Error(er, "Error backing up registry key: {0}", er.Message);

                throw new Exception(string.Format("Error backing up registry key: {0}", er.Message), er);
            }
            finally
            {
                if (oProc != null)
                    oProc.Dispose();
            }
        }

        public RegistryKeyData[] GetSubKeys(string key, string name, string value, bool containsSearch = false)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            List<RegistryKeyData> foundKeys = new List<RegistryKeyData>();

            string sHive = key.Substring(0, key.IndexOf('\\'));
            string sKey = key.Substring(key.IndexOf('\\') + 1);

            string valueLower = value.ToLower(); // In lowercase, only used for comparing

            mLogger.Info("Checking key '{0}' for subkeys that contain the value '{1}' for '{2}'...", key, value, name);

            RegistryKey oHiveKey = null;

            try
            {
                // Get the correct key for the hive used
                switch (sHive)
                {
                    case "HKEY_CLASSES_ROOT":
                        oHiveKey = Registry.ClassesRoot;
                        break;
                    case "HKEY_CURRENT_USER":
                        oHiveKey = Registry.CurrentUser;
                        break;
                    case "HKEY_LOCAL_MACHINE":
                        oHiveKey = Registry.LocalMachine;
                        break;
                    case "HKEY_USERS":
                        oHiveKey = Registry.Users;
                        break;
                    case "HKEY_CURRENT_CONFIG":
                        oHiveKey = Registry.CurrentConfig;
                        break;
                }

                using (RegistryKey oKey = Registry.CurrentUser.OpenSubKey(sKey, false))
                {
                    // If the key doesn't exist or has a permissions issue, throw exception
                    if (oKey == null)
                    {
                        string msg = string.Format("Key '{0}' not found or permissions denied.", key);

                        throw new Exception(msg);
                    }

                    // Get sub keys
                    string[] keys = oKey.GetSubKeyNames();

                    for (int i = 0; i < keys.Length; i++)
                    {
                        string sSubKeyName = keys[i];
                        string sSubKeyPath = key + "\\" + sSubKeyName;

                        mLogger.Info("Checking sub key '{0}'...", sSubKeyPath);

                        using (RegistryKey oSubKey = oKey.OpenSubKey(sSubKeyName, false))
                        {
                            // If subkey is null, skip it
                            if (oSubKey == null)
                            {
                                mLogger.Warn("Key '{0}' not found or permissions denied.", sSubKeyPath);

                                continue;
                            }

                            // Try to read the name 
                            string[] valueNames = oSubKey.GetValueNames();
                            if (valueNames.Any())
                            {
                                if (valueNames.Contains(name))
                                {
                                    mLogger.Info("Found value using name '{0}' in key '{1}'.", name, sSubKeyPath);

                                    string keyValue = oSubKey.GetValue(name).ToString();
                                    string keyValueSearchLower = keyValue.ToLower(); // In lowercase, only used for comparing

                                    if (!keyValueSearchLower.StartsWith("file://"))
                                    {
                                        mLogger.Warn("Incorrect url value '{0}' for '{1}' in key '{2}'", keyValueSearchLower, name, sSubKeyPath);
                                        continue;
                                    }

                                    // Make sure the keyValue Url has the correct escape characters (e.g. spaces to %20)
                                    var uri = new System.Uri(keyValueSearchLower);
                                    keyValueSearchLower = uri.AbsoluteUri;

                                    bool searchSuccess = false;

                                    if (!containsSearch)
                                    {
                                        searchSuccess = string.Equals(keyValueSearchLower, valueLower);
                                    }
                                    else
                                    {
                                        searchSuccess = keyValue.Contains(valueLower);
                                    }

                                    if (searchSuccess)
                                    {
                                        mLogger.Info("Found value '{0}' for '{1}' in key '{2}'!", keyValueSearchLower, name, sSubKeyPath);

                                        RegistryKeyData keydata = new RegistryKeyData()
                                        {
                                            Path = sSubKeyPath,
                                            Value = keyValue

                                        };

                                        foundKeys.Add(keydata);
                                    }
                                    else
                                    {
                                        mLogger.Debug("Found incorrect value of '{0}' for '{1}' in key '{2}'.", keyValueSearchLower, name, sSubKeyPath);
                                    }
                                }
                                else
                                {
                                    mLogger.Debug("No value using name '{0}' in key '{1}'!", name, sSubKeyPath);
                                }
                            }
                            else
                            {
                                mLogger.Warn("Key '{0}' has no values.", sSubKeyPath);
                            }
                        }
                    }
                }

                return foundKeys.ToArray();
            }
            catch (Exception er)
            {
                mLogger.Fatal(er, "Error getting registry keys: {0}", er.Message);

                throw;
            }
            finally
            {
                if (oHiveKey != null)
                    oHiveKey.Dispose();
            }
        }
    }
}
