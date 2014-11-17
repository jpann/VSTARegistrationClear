using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace VSTARegistrationClear.Utility
{
    public static class WindowsUtilities
    {
        [DllImport("Shell32.DLL")]
        public static extern bool IsUserAnAdmin();

        public static bool IsUserLocalAdmin()
        {
            WindowsIdentity oUser = WindowsIdentity.GetCurrent();

            WindowsPrincipal oPrincipal = new WindowsPrincipal(oUser);

            bool value = oPrincipal.IsInRole(WindowsBuiltInRole.Administrator);

            return value;
        }
    }
}
