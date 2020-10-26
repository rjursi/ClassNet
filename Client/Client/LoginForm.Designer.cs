namespace Client
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.lblLoginId = new System.Windows.Forms.Label();
            this.lblLoginPW = new System.Windows.Forms.Label();
            this.txtLoginID = new System.Windows.Forms.TextBox();
            this.txtLoginPW = new System.Windows.Forms.TextBox();
            this.LoginButton = new System.Windows.Forms.Button();
            this.btnSimpleLogin = new System.Windows.Forms.Button();
            this.panelInput = new System.Windows.Forms.Panel();
            this.panelInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblLoginId
            // 
            this.lblLoginId.AutoSize = true;
            this.lblLoginId.Font = new System.Drawing.Font("맑은 고딕", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLoginId.ForeColor = System.Drawing.Color.Teal;
            this.lblLoginId.Location = new System.Drawing.Point(24, 21);
            this.lblLoginId.Name = "lblLoginId";
            this.lblLoginId.Size = new System.Drawing.Size(69, 25);
            this.lblLoginId.TabIndex = 0;
            this.lblLoginId.Text = "아이디";
            // 
            // lblLoginPW
            // 
            this.lblLoginPW.AutoSize = true;
            this.lblLoginPW.Font = new System.Drawing.Font("맑은 고딕", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLoginPW.ForeColor = System.Drawing.Color.Teal;
            this.lblLoginPW.Location = new System.Drawing.Point(24, 111);
            this.lblLoginPW.Name = "lblLoginPW";
            this.lblLoginPW.Size = new System.Drawing.Size(88, 25);
            this.lblLoginPW.TabIndex = 1;
            this.lblLoginPW.Text = "패스워드";
            // 
            // txtLoginID
            // 
            this.txtLoginID.Font = new System.Drawing.Font("맑은 고딕", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtLoginID.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.txtLoginID.Location = new System.Drawing.Point(29, 50);
            this.txtLoginID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLoginID.Name = "txtLoginID";
            this.txtLoginID.Size = new System.Drawing.Size(283, 30);
            this.txtLoginID.TabIndex = 2;
            this.txtLoginID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtLoginID_KeyPress);
            // 
            // txtLoginPW
            // 
            this.txtLoginPW.Font = new System.Drawing.Font("맑은 고딕", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtLoginPW.Location = new System.Drawing.Point(29, 140);
            this.txtLoginPW.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLoginPW.Name = "txtLoginPW";
            this.txtLoginPW.PasswordChar = '*';
            this.txtLoginPW.Size = new System.Drawing.Size(283, 30);
            this.txtLoginPW.TabIndex = 3;
            this.txtLoginPW.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtLoginPW_KeyPress);
            // 
            // LoginButton
            // 
            this.LoginButton.BackColor = System.Drawing.Color.White;
            this.LoginButton.FlatAppearance.BorderColor = System.Drawing.Color.Teal;
            this.LoginButton.FlatAppearance.BorderSize = 3;
            this.LoginButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.LoginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoginButton.Font = new System.Drawing.Font("맑은 고딕", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LoginButton.ForeColor = System.Drawing.Color.Teal;
            this.LoginButton.Location = new System.Drawing.Point(27, 219);
            this.LoginButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(283, 49);
            this.LoginButton.TabIndex = 4;
            this.LoginButton.Text = "로그인";
            this.LoginButton.UseVisualStyleBackColor = false;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // btnSimpleLogin
            // 
            this.btnSimpleLogin.BackColor = System.Drawing.Color.White;
            this.btnSimpleLogin.FlatAppearance.BorderColor = System.Drawing.Color.Teal;
            this.btnSimpleLogin.FlatAppearance.BorderSize = 3;
            this.btnSimpleLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSimpleLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimpleLogin.Font = new System.Drawing.Font("맑은 고딕", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSimpleLogin.ForeColor = System.Drawing.Color.Teal;
            this.btnSimpleLogin.Location = new System.Drawing.Point(27, 292);
            this.btnSimpleLogin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSimpleLogin.Name = "btnSimpleLogin";
            this.btnSimpleLogin.Size = new System.Drawing.Size(283, 49);
            this.btnSimpleLogin.TabIndex = 5;
            this.btnSimpleLogin.Text = "간편 로그인";
            this.btnSimpleLogin.UseVisualStyleBackColor = false;
            this.btnSimpleLogin.Click += new System.EventHandler(this.BtnSimpleLogin_Click);
            // 
            // panelInput
            // 
            this.panelInput.Controls.Add(this.btnSimpleLogin);
            this.panelInput.Controls.Add(this.LoginButton);
            this.panelInput.Controls.Add(this.txtLoginPW);
            this.panelInput.Controls.Add(this.txtLoginID);
            this.panelInput.Controls.Add(this.lblLoginPW);
            this.panelInput.Controls.Add(this.lblLoginId);
            this.panelInput.Location = new System.Drawing.Point(12, 12);
            this.panelInput.Name = "panelInput";
            this.panelInput.Size = new System.Drawing.Size(340, 365);
            this.panelInput.TabIndex = 6;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 389);
            this.Controls.Add(this.panelInput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "LoginForm";
            this.Text = "로그인";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.panelInput.ResumeLayout(false);
            this.panelInput.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblLoginId;
        private System.Windows.Forms.Label lblLoginPW;
        private System.Windows.Forms.TextBox txtLoginID;
        private System.Windows.Forms.TextBox txtLoginPW;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.Button btnSimpleLogin;
        private System.Windows.Forms.Panel panelInput;
    }
}