VSTARegistrationClear
=====================
Simple utility that backs up and clears the ClickOnce application registration for a specific VSTO customization to help resolve 'AddInAlreadyInstalledException' exceptions during a ClickOnce application deployment.

### How to use
When you launch the utility, you will be prompted to select a .VSTO file. This would be the VSTO filename (not complete path) that you wish to clear from the ClickOnce application registration.

The utility will then search the ClickOnce application registration for all sub-keys that have a 'Url' value that contains a filename that matches the VSTO file you selected.

You can then back up the registry key and delete it.

### What it backs up
The application will by default back up the registry key 'HKEY_CURRENT_USER\Software\Microsoft\VSTA\Solutions' and all sub-keys.

The back up will be in the form of a .reg file contained within the directory that the utility is ran from.

### What it searches
By default the application goes through each sub-key of 'HKEY_CURRENT_USER\Software\Microsoft\VSTA\Solutions' and lists each registry key where the value of the 'Url' key contains the filename of the VSTO file you selected. For example, if you selected the VSTO file 'C:\TEMP\MyAddIn.vsto', it would return sub-keys that have a filename of 'MyAddIn.vsto', which could be 'file://C:/Users/jdoe/MyAddIn.vsto' or 'file//server/share/program/MyAddIn.vsto'.

### What it deletes
It will delete anything matched using the methods above.

### Requirements
Requires .NET Framework 4.0.

### Disclaimer
This was written quickly and as a small utility to fix issues with a specific application's ClickOnce registration issues, so it may not work correctly for all ClickOnce registration issues.
