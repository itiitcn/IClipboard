using IClipboard.Base;
using IClipboard.Properties;
using IClipboard.Win32;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IClipboard
{
    public partial class MainForm : MaterialForm
    {

        string[] args = null;
        public MainForm(string[] args)
        {
            InitializeComponent();
            Context.CLIPBOARD_COUNT = Settings.Default.Count;
            NewViewer();
            this.args = args;
            new Thread(() => {
                update();
            }).Start();
        }


        private IntPtr mNextClipBoardViewerHWnd;

        public void NewViewer()
        {
            mNextClipBoardViewerHWnd = Win32API.SetClipboardViewer(this.Handle);
        }

        string LastString = string.Empty;
        Image LastImage = null;

        protected override void WndProc(ref Message m)
        {
            string Time = DateTime.Now.ToString("yy/MM/dd HH:mm");
            switch (m.Msg)
            {
                case (int)WinMsg.WM_DRAWCLIPBOARD:
                    {
                        if (ICopy == 1) return;
                        Win32API.SendMessage(mNextClipBoardViewerHWnd, m.Msg, m.WParam.ToInt32(), m.LParam.ToInt32());
                        //显示剪贴板中的文本信息
                        if (Clipboard.ContainsText())
                        {
                            string text= Clipboard.GetText();
                            if (text == LastString||string.IsNullOrEmpty(text)) return;
                            LastString = text;
                            string t = "";
                            if (text.Length > 35)
                            {
                                t = text.Substring(0,35);
                            }
                            else
                            {
                                t = text;
                            }
                            ListViewItem item = new ListViewItem(new string[3] { t, "文本", Time });
                            item.Tag = text;
                            item.ToolTipText = "文本";
                            list.Items.Add(item);
                        }
                        //显示剪贴板中的图片信息
                        if (Clipboard.ContainsImage())
                        {
                            Image img= Clipboard.GetImage();
                            if (img == LastImage) return;
                            LastImage = img;
                            string txt= img.Width + "*" + img.Height+"["+ ImageType.GetType(img)+"]";
                            ListViewItem item = new ListViewItem(new string[3] { "图片文件("+ txt + ")", "图片", Time });
                            item.Tag = img;
                            item.ToolTipText = "图片";
                            list.Items.Add(item);
                        }
                        if (Clipboard.ContainsFileDropList())
                        {
                            StringCollection ss=Clipboard.GetFileDropList();
                            foreach (string text in ss)
                            {
                                string t = "";
                                if (text.Length > 35)
                                {
                                    t = text.Substring(0, 35);
                                }
                                else
                                {
                                    t = text;
                                }
                                ListViewItem item = new ListViewItem(new string[3] { t, "文件路径", Time });
                                item.ToolTipText = "路径";
                                item.Tag = text;
                                list.Items.Add(item);
                            }
                        }
                        if (list.Items.Count > Context.CLIPBOARD_COUNT)
                        {
                            int num = list.Items.Count - Context.CLIPBOARD_COUNT;
                            for (int i = 0; i < num; i++)
                            {
                                list.Items.RemoveAt(0);
                               
                            }
                        }
                        break;
                    }
                case (int)WinMsg.WM_CHANGECBCHAIN:
                    {
                        if (m.WParam == (IntPtr)mNextClipBoardViewerHWnd)
                        {
                            mNextClipBoardViewerHWnd = m.LParam;
                        }
                        else
                        {
                            Win32API.SendMessage(mNextClipBoardViewerHWnd, m.Msg, m.WParam.ToInt32(), m.LParam.ToInt32());
                        }
                        break;
                    }
                case (int)WinMsg.WM_HOTKEY:
                    {
                        switch (m.WParam.ToInt32())
                        {
                            case Space: //热键ID  
                                icontab_DoubleClick(null, null);
                                break;
                            default:
                                break;
                        }  
                        break;
                    }
                case (int)WinMsg.WM_CREATE: //窗口消息-创建  
                    RegKey(Handle, Space, KeyModifiers.Alt, Keys.C);
                    break;
                case (int)WinMsg.WM_DESTROY: //窗口消息-销毁  
                    UnRegKey(Handle, Space); //销毁热键  
                    break;  
            }
            base.WndProc(ref m);
        }

        private const int Space = 13682; //热键ID  
        private void ClipboardMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            tscopycontent.Enabled = false;
            tslookimage.Enabled = false;
            tsopenpath.Enabled = false;
            tssaveimage.Enabled = false;
            if (list.SelectedItems != null&& list.SelectedItems.Count>0)
            {
                selectItem = list.SelectedItems[0];
                string type = selectItem.ToolTipText;
                switch (type)
                {
                    case "文本":
                        tscopycontent.Enabled = true;
                        break;
                    case "图片":
                        tscopycontent.Enabled = true;
                        tssaveimage.Enabled = true;
                        tslookimage.Enabled = true;
                        break;
                    case "路径":
                        tscopycontent.Enabled = true;
                        tsopenpath.Enabled = true;
                        break;
                    default:
                        break;
                }
            }
        }
        public static int ICopy = 0;
        ListViewItem selectItem = null;
        private void ClipboardMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string type = e.ClickedItem.Text;
            if(type== "清除列表")
            {
                selectItem = null;
                list.Items.Clear();
                Clipboard.Clear();
                return;
            }
            if (selectItem == null) return;
            Image img = null;
            if (selectItem.ToolTipText == "图片")
            {
                img = (Image)selectItem.Tag;
            }
            switch (type)
            {
                case "打开路径":
                    System.Diagnostics.Process.Start("explorer.exe", selectItem.Tag.ToString());
                    break;
                case "复制内容":
                    ICopy = 1;
                    if(selectItem.ToolTipText== "图片")
                    {
                        Clipboard.SetImage(img);
                    }
                    else
                    {
                        Clipboard.SetText(selectItem.Tag.ToString());
                    }
                    break;
                case "预览图片":
                    new ImageForm(img).ShowDialog();
                    break;
                case "图片另存为":
                    SaveFileDialog save = new SaveFileDialog();
                    save.Filter = "图片文件|*.JPG|*.PNG|*.GIF|*.TIFF|*.BMP|*.ICO|";
                    save.FileName = "IC" + DateTime.Now.ToFileTime().ToString() + "." + ImageType.GetType(img);
                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        string path = save.FileName;
                        img.Save(save.FileName);
                    }
                    break;
                default:
                    break;
            }
            ICopy = 0;
        }


        private void list_DoubleClick(object sender, EventArgs e)
        {
            if (list.SelectedItems != null && list.SelectedItems.Count > 0)
            {
                ListViewItem selectlist = list.SelectedItems[0];
                string type = selectlist.ToolTipText;
                switch (type)
                {
                    case "文本":
                        ICopy = 1;
                        Clipboard.SetText(selectlist.Tag.ToString());
                        break;
                    case "图片":
                        Image im = (Image)selectlist.Tag;
                        new ImageForm(im).ShowDialog();
                        break;
                    case "路径":
                        System.Diagnostics.Process.Start("explorer.exe", selectlist.Tag.ToString());
                        break;
                    default:
                        break;
                }
                ICopy = 0;
            }
        }

        bool AutoExit = false;
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!AutoExit)
            {
                DialogResult r = DialogForm.ShowMsg("关闭后剪贴板会清空,是否关闭软件?", "提示", MessageFormIcon.Doubt, MessageFormButtons.YesNo);
                if (r != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            
        }



        /// <summary>  
        /// 注册热键  
        /// </summary>  
        /// <param name="hwnd">窗口句柄</param>  
        /// <param name="hotKey_id">热键ID</param>  
        /// <param name="keyModifiers">组合键</param>  
        /// <param name="key">热键</param>  
        public static void RegKey(IntPtr hwnd, int hotKey_id, KeyModifiers keyModifiers, Keys key)
        {
            try
            {
                if (!Win32API.RegisterHotKey(hwnd, hotKey_id, keyModifiers, key))
                {
                    if (Marshal.GetLastWin32Error() == 1409) { 
                        MessageBox.Show("热键被占用 ！"); 
                    }
                    else
                    {
                        MessageBox.Show("注册热键失败！");
                    }
                }
            }
            catch (Exception) { }
        }
        /// <summary>  
        /// 注销热键  
        /// </summary>  
        /// <param name="hwnd">窗口句柄</param>  
        /// <param name="hotKey_id">热键ID</param>  
        public static void UnRegKey(IntPtr hwnd, int hotKey_id)
        {
            //注销Id号为hotKey_id的热键设定  
            Win32API.UnregisterHotKey(hwnd, hotKey_id);
        }  

        

        private void icontab_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState!= FormWindowState.Normal)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.Hide();
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void tsExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsshow_Click(object sender, EventArgs e)
        {
            icontab_DoubleClick(null,null);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if(this.WindowState== FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void lblmenu_MouseEnter(object sender, EventArgs e)
        {
            Label l = (Label)sender;
            l.BackColor = Color.FromArgb(61, 71, 76);
        }

        private void lblmenu_MouseLeave(object sender, EventArgs e)
        {
            Label l = (Label)sender;
            l.BackColor = Color.Transparent;
        }

        private void lblmenu_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if(args!=null&& args.Length > 0)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
        }

        public void update()
        {
            string Edition = GetVersion("IClipboard");
            Version curVersion = new Version(AssemblyVersion);//本地资源版本
            Version lowVersion = new Version(Edition);//服务器资源版本
            if (lowVersion > curVersion)
            {
                this.Invoke((EventHandler)delegate
                {
                    DialogResult r = DialogForm.ShowMsg("检测到新版本,是否下载?", "提示", MessageFormIcon.Doubt, MessageFormButtons.YesNo);
                    if (r == DialogResult.Yes)
                    {
                        string url = "http://itiit.cn/file/IClipboard.exe";
                        try
                        {
                            System.Diagnostics.Process.Start(url);
                        }
                        catch (Exception)
                        {
                            System.Diagnostics.Process.Start("iexplore.exe", url);
                        }
                        AutoExit = true;
                        Application.Exit();
                    }
                });
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string GetVersion(string AppName)
        {
            try
            {
                string postData = "AppName=" + AppName;
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                client.Headers.Add("ContentLength", postData.Length.ToString());
                Encoding enc = Encoding.GetEncoding("UTF-8");
                byte[] responseData = client.UploadData("http://itiit.cn/Tools/Update", "POST", bytes);
                return enc.GetString(responseData).Replace("\"", "");
            }
            catch (Exception)
            {
                return "1.0.0.0";
            }
        }
    }
}
