using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSTARegistrationClear.Managers
{
    public interface IRegistryManager
    {
        RegistryKeyData[] GetSubKeys(string key, string name, string value, bool containsSearch = false);
        bool KeyExists(string key);
        bool DeleteKey(string key);
        void ExportKey(string key, string filename);
    }
}
