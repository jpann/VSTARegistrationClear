using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
        private bool mDebug = false;
        private StandardKernel mKernel;
        private IRegistryManager mRegistryManager;
        private ILoggerFactory mLoggerFactory;
        private ILogger mLogger;
        private string mVSTOFileUri;

        public Main()
        {
            XmlConfigurator.Configure();

            InitializeComponent();

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

                if (!WindowsUtils.IsUserAnAdmin())
                {
                    string sMsg = "You must run this program as an administrator.";

                    MessageBox.Show(
                       sMsg,
                       "Administrator Error",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error);

                    mLog.Fatal(sMsg);

                    Environment.Exit(-1);
                }
#endif

                mVSTOFileUri = GetVSTOFilenameAsUri();
            }
            catch (Exception er)
            {
                string sMsg = string.Format("Start up error:\n{0}", er.Message);

                MessageBox.Show(
                   sMsg,
                   "Start Up Error",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error);

                mLogger.Fatal(sMsg);

                Environment.Exit(-1);
            }
        }


        private string GetVSTOFilenameAsUri()
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.InitialDirectory = "c:\\";
            openFile.Filter = "VSTO files (*.vsto)|*.vsto|All files (*.*)|*.*";
            openFile.FilterIndex = 1;
            openFile.RestoreDirectory = true;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string file = openFile.FileName;
                if (!File.Exists(file))
                    throw new FileNotFoundException(file);

                return file;
            }
            else
            {
                throw new Exception("You must select a file.");
            }
        }

        private void LoadSettings()
        {
            mDebug = Properties.Settings.Default.Debug;
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            About aboutForm = mKernel.Get<About>();
            aboutForm.ShowDialog();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        #region Backup Methods
        private void btnRegistryBackup_Click(object sender, EventArgs e)
        {
            try
            {
                string savePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), Path.GetFileNameWithoutExtension(Application.ExecutablePath) + "_Backup.reg");

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
        #endregion

        #region Registration Deleting Methods
        private void btnRegistryDelete_Click(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
