namespace IClipboard
{
    partial class ImageForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageForm));
            this.pb1 = new System.Windows.Forms.PictureBox();
            this.ImageMenuStrip = new MaterialSkin.Controls.MaterialContextMenuStrip();
            this.tssave = new System.Windows.Forms.ToolStripMenuItem();
            this.tscopy = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pb1)).BeginInit();
            this.ImageMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pb1
            // 
            this.pb1.ContextMenuStrip = this.ImageMenuStrip;
            this.pb1.Location = new System.Drawing.Point(0, 32);
            this.pb1.Name = "pb1";
            this.pb1.Size = new System.Drawing.Size(450, 500);
            this.pb1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb1.TabIndex = 0;
            this.pb1.TabStop = false;
            // 
            // ImageMenuStrip
            // 
            this.ImageMenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ImageMenuStrip.Depth = 0;
            this.ImageMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tscopy,
            this.tssave});
            this.ImageMenuStrip.MouseState = MaterialSkin.MouseState.HOVER;
            this.ImageMenuStrip.Name = "ImageMenuStrip";
            this.ImageMenuStrip.Size = new System.Drawing.Size(153, 70);
            // 
            // tssave
            // 
            this.tssave.Name = "tssave";
            this.tssave.Size = new System.Drawing.Size(152, 22);
            this.tssave.Text = "图片另存为";
            this.tssave.Click += new System.EventHandler(this.tssave_Click);
            // 
            // tscopy
            // 
            this.tscopy.Name = "tscopy";
            this.tscopy.Size = new System.Drawing.Size(152, 22);
            this.tscopy.Text = "复制";
            this.tscopy.Click += new System.EventHandler(this.tscopy_Click);
            // 
            // ImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 532);
            this.Controls.Add(this.pb1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ImageForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "图片预览";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pb1)).EndInit();
            this.ImageMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pb1;
        private MaterialSkin.Controls.MaterialContextMenuStrip ImageMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tssave;
        private System.Windows.Forms.ToolStripMenuItem tscopy;
    }
}