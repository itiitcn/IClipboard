namespace IClipboard
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.ckkj = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numsize = new System.Windows.Forms.NumericUpDown();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numsize)).BeginInit();
            this.SuspendLayout();
            // 
            // ckkj
            // 
            this.ckkj.AutoSize = true;
            this.ckkj.BackColor = System.Drawing.Color.Transparent;
            this.ckkj.Font = new System.Drawing.Font("宋体", 12F);
            this.ckkj.Location = new System.Drawing.Point(57, 81);
            this.ckkj.Name = "ckkj";
            this.ckkj.Size = new System.Drawing.Size(91, 20);
            this.ckkj.TabIndex = 0;
            this.ckkj.Text = "开机自启";
            this.ckkj.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.Location = new System.Drawing.Point(54, 138);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "剪贴板长度:";
            // 
            // numsize
            // 
            this.numsize.Location = new System.Drawing.Point(156, 135);
            this.numsize.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numsize.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numsize.Name = "numsize";
            this.numsize.Size = new System.Drawing.Size(138, 21);
            this.numsize.TabIndex = 2;
            this.numsize.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // btnNo
            // 
            this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNo.Location = new System.Drawing.Point(277, 192);
            this.btnNo.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 28);
            this.btnNo.TabIndex = 10;
            this.btnNo.Text = "否";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOk.Location = new System.Drawing.Point(191, 192);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 28);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "是";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 260);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.numsize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ckkj);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(390, 260);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(390, 260);
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设置";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.numsize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox ckkj;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numsize;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Button btnOk;

    }
}