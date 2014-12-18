using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using log4net.Config;
using Ninject;
using Ninject.Extensions.Logging;
using Ninject.Extensions.Logging.Log4net;
using VSTARegistrationClear.Managers;

namespace VSTARegistrationClear
{
    public partial class Main : Form
    {
        private const string cDefaultVSTARegistryKey = "HKEY_CURRENT_USER\\Software\\Microsoft\\VSTA\\Solutions";
        private const string cDefaultVSTAValueName = "Url";

        private string mVSTARegistryKey;
        private bool mDebug = false;
        private StandardKernel mKernel;
        private IRegistryManager mRegistryManager;
        private ILoggerFactory mLoggerFactory;
        private ILogger mLogger;
        private string mVSTOFileUri;
        private string mVSTAValueName;
        private bool mContainsSearch = false;

        public Main()
        {
            XmlConfigurator.Configure();

            InitializeComponent();
        }


        private string GetVSTOFilenameAsUri()
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.InitialDirectory = "c:\\";
            openFile.Filter = "VSTO files (*.vsto)|*.vsto|All files (*.*)|*.*";
            openFile.FilterIndex = 1;
            openFile.RestoreDirectory = true;
            openFile.Title = "Select VSTO application file to check";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string file = openFile.FileName;
                if (!File.Exists(file))
                    throw new FileNotFoundException(file);

                var uri = new System.Uri(file);
                return uri.AbsoluteUri;
            }
            else
            {
                throw new Exception("Please select a VSTO file.");
            }
        }

        private void LoadSettings()
        {
            mDebug = Properties.Settings.Default.Debug;
            mVSTARegistryKey = Properties.Settings.Default.VSTARegistryKey;
            mVSTAValueName = Properties.Settings.Default.VSTAValueName;
            mContainsSearch = Properties.Settings.Default.ContainsSearching;

            if (string.IsNullOrEmpty(mVSTARegistryKey))
                mVSTARegistryKey = cDefaultVSTARegistryKey;

            if (string.IsNullOrEmpty(mVSTAValueName))
                mVSTAValueName = cDefaultVSTAValueName;
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            About aboutForm = mKernel.Get<About>();
            aboutForm.ShowDialog();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            try
            {
                var settings = new NinjectSettings()
                {
                    LoadExtensions = false
                };

                mKernel = new StandardKernel(
                    settings,
                    new Log4NetModule(),
                    new DependencyModule());

                mLoggerFactory = mKernel.Get<ILoggerFactory>();
                mLogger = mLoggerFactory.GetCurrentClassLogger();
                mRegistryManager = mKernel.Get<IRegistryManager>();

                LoadSettings();

#if RELEASE  
                if (UacHelper.IsUacEnabled)
                {
                    if (UacHelper.IsProcessElevated)
                    {
                        string sMsg = "You must run this program with UAC elevation.";

                        MessageBox.Show(
                           sMsg,
                           "UAC Error",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error);

                        mLog.Fatal(sMsg);

                        Environment.Exit(-1);
                    }
                }
#endif

                DialogResult warning = MessageBox.Show("This utility is meant to be used to clear VSTA registration corruption when a VSTO application does not appear to be installed but appears in the VSTA registration, causing '*InstalledException' errors during ClickOnce deployment.\n\r\n\rPrior to running this, make sure that the VSTO application does not appear to be installed.", 
                    "WARNING",
                    MessageBoxButtons.OKCancel, 
                    MessageBoxIcon.Question);

                if (warning == DialogResult.Cancel)
                {
                    Environment.Exit(-1);
                }

                mVSTOFileUri = GetVSTOFilenameAsUri();
            }
            catch (Exception er)
            {
                string sMsg = string.Format("Start up error:\n\n{0}", er.Message);

                MessageBox.Show(
                   sMsg,
                   "Start Up Error",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error);

                mLogger.Fatal(sMsg);

                Environment.Exit(-1);
            }

            LoadRegistrations();
        }

        #region Backup Methods
        private void btnRegistryBackup_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = string.Format("{0}_{1}_Backup.reg",
                    Path.GetFileNameWithoutExtension(Application.ExecutablePath),
                    Environment.UserName);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "reg files (*.reg)|*.reg|All files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.RestoreDirectory = true;
                saveFile.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                saveFile.FileName = fileName;

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    string savePath = saveFile.FileName;

                    mRegistryManager.ExportKey(mVSTARegistryKey, savePath);

                    btnRegistryDelete.Enabled = true;

                    lblStatus.Text = string.Format("Backed up registry to '{0}'", savePath);

                    System.Diagnostics.Process.Start(Path.GetDirectoryName(Application.ExecutablePath));
                }
            }
            catch (Exception er)
            {
                string sMsg = string.Format("Error backing up registration registry:\n{0}",
                    er.Message);

                mLogger.Error(sMsg, er);

                MessageBox.Show(
                    sMsg,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Registration Loading Methods
        private void LoadRegistrations()
        {
            try
            {
                string fileName = mVSTOFileUri;
                
                if (mContainsSearch)
                    fileName = Path.GetFileName(mVSTOFileUri);

                RegistryKeyData[] keys = mRegistryManager.GetSubKeys(mVSTARegistryKey, mVSTAValueName, fileName, mContainsSearch);

                if (keys.Any())
                {
                    foreach (RegistryKeyData key in keys)
                    {
                        ListViewItem oItem = new ListViewItem(key.Path);
                        oItem.SubItems.Add(key.Value);
                        oItem.Checked = true;

                        lstRegistry.Items.Add(oItem);
                    }

                    btnRegistryBackup.Enabled = true;
                }
            }
            catch (Exception er)
            {
                string sMsg = string.Format("Error loading registrations:\n\n{0}", er.Message);

                MessageBox.Show(
                   sMsg,
                   "Load Error",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error);

                mLogger.Fatal(sMsg);

                Environment.Exit(-1);
            }
        }
        #endregion

        #region Registration Deleting Methods
        private void btnRegistryDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstRegistry.CheckedItems.Count <= 0)
                    return;

                int deleteCount = 0;
                foreach (ListViewItem item in lstRegistry.CheckedItems)
                {
                    if (item.Checked)
                    {
                        string key = item.Text;

                        mLogger.Info("Deleting key '{0}'...", key);

                        bool success = mRegistryManager.DeleteKey(key);

                        if (success)
                        {
                            deleteCount++;

                            mLogger.Info("Deleted key '{0}'!", key);
                        }
                        else
                        {
                            mLogger.Warn("Failed to delete key '{0}'!", key);
                        }
                    }
                }

                lblStatus.Text = string.Format("Deleted {0} keys.", deleteCount);
            }
            catch (Exception er)
            {
                string sMsg = string.Format("Error deleting registrations:\n\n{0}", er.Message);

                MessageBox.Show(
                   sMsg,
                   "Delete Error",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error);

                mLogger.Fatal(sMsg);
            }
        }

        #endregion
    }
}
