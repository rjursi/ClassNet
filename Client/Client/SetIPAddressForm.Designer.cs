namespace Client
{
    partial class SetIPAddressForm
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
            this.ipAddressControl = new IPAddressControlLib.IPAddressControl();
            this.lblServerIP = new System.Windows.Forms.Label();
            this.btnSetServerIP = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ipAddressControl
            // 
            this.ipAddressControl.AllowInternalTab = false;
            this.ipAddressControl.AutoHeight = true;
            this.ipAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.ipAddressControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ipAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ipAddressControl.Font = new System.Drawing.Font("굴림", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ipAddressControl.Location = new System.Drawing.Point(113, 50);
            this.ipAddressControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ipAddressControl.Name = "ipAddressControl";
            this.ipAddressControl.ReadOnly = false;
            this.ipAddressControl.Size = new System.Drawing.Size(228, 29);
            this.ipAddressControl.TabIndex = 1;
            this.ipAddressControl.Text = "...";
            // 
            // lblServerIP
            // 
            this.lblServerIP.AutoSize = true;
            this.lblServerIP.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblServerIP.Location = new System.Drawing.Point(15, 55);
            this.lblServerIP.Name = "lblServerIP";
            this.lblServerIP.Size = new System.Drawing.Size(92, 20);
            this.lblServerIP.TabIndex = 0;
            this.lblServerIP.Text = "서버 IP :";
            // 
            // btnSetServerIP
            // 
            this.btnSetServerIP.Location = new System.Drawing.Point(20, 95);
            this.btnSetServerIP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSetServerIP.Name = "btnSetServerIP";
            this.btnSetServerIP.Size = new System.Drawing.Size(321, 46);
            this.btnSetServerIP.TabIndex = 1;
            this.btnSetServerIP.Text = "설정";
            this.btnSetServerIP.UseVisualStyleBackColor = true;
            this.btnSetServerIP.Click += new System.EventHandler(this.btnSetServerIP_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "서버 IP 설정";
            // 
            // SetIPAddressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 170);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSetServerIP);
            this.Controls.Add(this.lblServerIP);
            this.Controls.Add(this.ipAddressControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetIPAddressForm";
            this.ShowInTaskbar = false;
            this.Text = "SetIPAddressForm";
      
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblServerIP;
        private System.Windows.Forms.Button btnSetServerIP;
        private System.Windows.Forms.Label label1;
    }
}