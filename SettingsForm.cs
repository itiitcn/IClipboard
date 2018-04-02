using IClipboard.Properties;
using MaterialSkin.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IClipboard
{
    public partial class SettingsForm : MaterialForm
    {
        public SettingsForm()
        {
            InitializeComponent();
            ckkj.Checked = Settings.Default.iszq;
            numsize.Value = Settings.Default.Count;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Settings.Default.iszq = ckkj.Checked;
            Settings.Default.Count = (int)numsize.Value;
            Context.CLIPBOARD_COUNT = (int)numsize.Value;
            SetAutoBootStatu(ckkj.Checked);
            Settings.Default.Save();
        }

        public static int SetAutoBootStatu(bool isAutoBoot)
        {
            try
            {
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                if (isAutoBoot)
                {
                    rk2.SetValue("IClipboard", "\""+path+"\" -m");
                }
                else
                {
                    rk2.DeleteValue("IClipboard", false);
                }
                rk2.Close();
                rk.Close();
                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }  
    }
}
