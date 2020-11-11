namespace Client
{
    partial class SimpleLoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleLoginForm));
            this.LoginButton = new System.Windows.Forms.Button();
            this.txtStuCode = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblStuCode = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LoginButton
            // 
            this.LoginButton.FlatAppearance.BorderColor = System.Drawing.Color.Teal;
            this.LoginButton.FlatAppearance.BorderSize = 3;
            this.LoginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoginButton.Font = new System.Drawing.Font("맑은 고딕", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LoginButton.ForeColor = System.Drawing.Color.Teal;
            this.LoginButton.Location = new System.Drawing.Point(39, 177);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(140, 37);
            this.LoginButton.TabIndex = 9;
            this.LoginButton.Text = "로그인";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // txtStuCode
            // 
            this.txtStuCode.Font = new System.Drawing.Font("맑은 고딕", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtStuCode.Location = new System.Drawing.Point(39, 118);
            this.txtStuCode.MaxLength = 10;
            this.txtStuCode.Name = "txtStuCode";
            this.txtStuCode.Size = new System.Drawing.Size(140, 26);
            this.txtStuCode.TabIndex = 8;
            this.txtStuCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtStuCode_KeyPress);
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("맑은 고딕", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtName.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.txtName.Location = new System.Drawing.Point(39, 54);
            this.txtName.MaxLength = 5;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(140, 26);
            this.txtName.TabIndex = 7;
            this.txtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtName_KeyPress);
            // 
            // lblStuCode
            // 
            this.lblStuCode.AutoSize = true;
            this.lblStuCode.Font = new System.Drawing.Font("맑은 고딕", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblStuCode.ForeColor = System.Drawing.Color.Teal;
            this.lblStuCode.Location = new System.Drawing.Point(36, 94);
            this.lblStuCode.Name = "lblStuCode";
            this.lblStuCode.Size = new System.Drawing.Size(37, 19);
            this.lblStuCode.TabIndex = 6;
            this.lblStuCode.Text = "학번";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("맑은 고딕", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblName.ForeColor = System.Drawing.Color.Teal;
            this.lblName.Location = new System.Drawing.Point(36, 30);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(37, 19);
            this.lblName.TabIndex = 5;
            this.lblName.Text = "이름";
            // 
            // SimpleLoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(220, 246);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.txtStuCode);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblStuCode);
            this.Controls.Add(this.lblName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SimpleLoginForm";
            this.Text = "간편 로그인";
            this.Load += new System.EventHandler(this.SimpleLoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.TextBox txtStuCode;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblStuCode;
        private System.Windows.Forms.Label lblName;
    }
}