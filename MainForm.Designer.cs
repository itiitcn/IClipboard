namespace IClipboard
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.list = new MaterialSkin.Controls.MaterialListView();
            this.content = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ClipboardMenuStrip = new MaterialSkin.Controls.MaterialContextMenuStrip();
            this.tsopenpath = new System.Windows.Forms.ToolStripMenuItem();
            this.tscopycontent = new System.Windows.Forms.ToolStripMenuItem();
            this.tslookimage = new System.Windows.Forms.ToolStripMenuItem();
            this.tssaveimage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsempty = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.TabContextMenuStrip = new MaterialSkin.Controls.MaterialContextMenuStrip();
            this.tsshow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsExit = new System.Windows.Forms.ToolStripMenuItem();
            this.lblmenu = new System.Windows.Forms.Label();
            this.ClipboardMenuStrip.SuspendLayout();
            this.TabContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // list
            // 
            this.list.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.list.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.content,
            this.Type,
            this.Time});
            this.list.ContextMenuStrip = this.ClipboardMenuStrip;
            this.list.Depth = 0;
            this.list.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.list.FullRowSelect = true;
            this.list.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.list.Location = new System.Drawing.Point(0, 32);
            this.list.MouseLocation = new System.Drawing.Point(-1, -1);
            this.list.MouseState = MaterialSkin.MouseState.OUT;
            this.list.Name = "list";
            this.list.OwnerDraw = true;
            this.list.Size = new System.Drawing.Size(750, 400);
            this.list.TabIndex = 0;
            this.list.UseCompatibleStateImageBehavior = false;
            this.list.View = System.Windows.Forms.View.Details;
            this.list.DoubleClick += new System.EventHandler(this.list_DoubleClick);
            // 
            // content
            // 
            this.content.Text = "内容";
            this.content.Width = 500;
            // 
            // Type
            // 
            this.Type.Text = "类别";
            this.Type.Width = 100;
            // 
            // Time
            // 
            this.Time.Text = "时间";
            this.Time.Width = 130;
            // 
            // ClipboardMenuStrip
            // 
            this.ClipboardMenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClipboardMenuStrip.Depth = 0;
            this.ClipboardMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsopenpath,
            this.tscopycontent,
            this.tslookimage,
            this.tssaveimage,
            this.tsempty});
            this.ClipboardMenuStrip.MouseState = MaterialSkin.MouseState.HOVER;
            this.ClipboardMenuStrip.Name = "ClipboardMenuStrip";
            this.ClipboardMenuStrip.Size = new System.Drawing.Size(137, 114);
            this.ClipboardMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ClipboardMenuStrip_Opening);
            this.ClipboardMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ClipboardMenuStrip_ItemClicked);
            // 
            // tsopenpath
            // 
            this.tsopenpath.Name = "tsopenpath";
            this.tsopenpath.Size = new System.Drawing.Size(136, 22);
            this.tsopenpath.Text = "打开路径";
            // 
            // tscopycontent
            // 
            this.tscopycontent.Name = "tscopycontent";
            this.tscopycontent.Size = new System.Drawing.Size(136, 22);
            this.tscopycontent.Text = "复制内容";
            // 
            // tslookimage
            // 
            this.tslookimage.Name = "tslookimage";
            this.tslookimage.Size = new System.Drawing.Size(136, 22);
            this.tslookimage.Text = "预览图片";
            // 
            // tssaveimage
            // 
            this.tssaveimage.Name = "tssaveimage";
            this.tssaveimage.Size = new System.Drawing.Size(136, 22);
            this.tssaveimage.Text = "图片另存为";
            // 
            // tsempty
            // 
            this.tsempty.Name = "tsempty";
            this.tsempty.Size = new System.Drawing.Size(136, 22);
            this.tsempty.Text = "清除列表";
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.TabContextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "IClipboard";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.icontab_DoubleClick);
            // 
            // TabContextMenuStrip
            // 
            this.TabContextMenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.TabContextMenuStrip.Depth = 0;
            this.TabContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsshow,
            this.tsExit});
            this.TabContextMenuStrip.MouseState = MaterialSkin.MouseState.HOVER;
            this.TabContextMenuStrip.Name = "TabContextMenuStrip";
            this.TabContextMenuStrip.Size = new System.Drawing.Size(101, 48);
            // 
            // tsshow
            // 
            this.tsshow.Name = "tsshow";
            this.tsshow.Size = new System.Drawing.Size(100, 22);
            this.tsshow.Text = "显示";
            this.tsshow.Click += new System.EventHandler(this.tsshow_Click);
            // 
            // tsExit
            // 
            this.tsExit.Name = "tsExit";
            this.tsExit.Size = new System.Drawing.Size(100, 22);
            this.tsExit.Text = "退出";
            this.tsExit.Click += new System.EventHandler(this.tsExit_Click);
            // 
            // lblmenu
            // 
            this.lblmenu.BackColor = System.Drawing.Color.Transparent;
            this.lblmenu.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblmenu.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.lblmenu.Location = new System.Drawing.Point(646, 1);
            this.lblmenu.Name = "lblmenu";
            this.lblmenu.Size = new System.Drawing.Size(32, 30);
            this.lblmenu.TabIndex = 2;
            this.lblmenu.Text = "∵";
            this.lblmenu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblmenu.Click += new System.EventHandler(this.lblmenu_Click);
            this.lblmenu.MouseEnter += new System.EventHandler(this.lblmenu_MouseEnter);
            this.lblmenu.MouseLeave += new System.EventHandler(this.lblmenu_MouseLeave);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 432);
            this.Controls.Add(this.lblmenu);
            this.Controls.Add(this.list);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(750, 432);
            this.MinimumSize = new System.Drawing.Size(750, 432);
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IClipboard";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.ClipboardMenuStrip.ResumeLayout(false);
            this.TabContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialListView list;
        private System.Windows.Forms.ColumnHeader content;
        private System.Windows.Forms.ColumnHeader Type;
        private System.Windows.Forms.ColumnHeader Time;
        private MaterialSkin.Controls.MaterialContextMenuStrip ClipboardMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsopenpath;
        private System.Windows.Forms.ToolStripMenuItem tscopycontent;
        private System.Windows.Forms.ToolStripMenuItem tslookimage;
        private System.Windows.Forms.ToolStripMenuItem tssaveimage;
        private System.Windows.Forms.ToolStripMenuItem tsempty;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private MaterialSkin.Controls.MaterialContextMenuStrip TabContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsExit;
        private System.Windows.Forms.ToolStripMenuItem tsshow;
        private System.Windows.Forms.Label lblmenu;
    }
}

