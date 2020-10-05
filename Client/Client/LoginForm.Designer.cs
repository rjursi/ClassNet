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
            this.lblLoginId = new System.Windows.Forms.Label();
            this.lblLoginPW = new System.Windows.Forms.Label();
            this.txtLoginID = new System.Windows.Forms.TextBox();
            this.txtLoginPW = new System.Windows.Forms.TextBox();
            this.LoginButton = new System.Windows.Forms.Button();
            this.btnSimpleLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblLoginId
            // 
            this.lblLoginId.AutoSize = true;
            this.lblLoginId.Font = new System.Drawing.Font("맑은 고딕", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLoginId.Location = new System.Drawing.Point(127, 46);
            this.lblLoginId.Name = "lblLoginId";
            this.lblLoginId.Size = new System.Drawing.Size(61, 23);
            this.lblLoginId.TabIndex = 0;
            this.lblLoginId.Text = "아이디";
            // 
            // lblLoginPW
            // 
            this.lblLoginPW.AutoSize = true;
            this.lblLoginPW.Font = new System.Drawing.Font("맑은 고딕", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLoginPW.Location = new System.Drawing.Point(127, 126);
            this.lblLoginPW.Name = "lblLoginPW";
            this.lblLoginPW.Size = new System.Drawing.Size(78, 23);
            this.lblLoginPW.TabIndex = 1;
            this.lblLoginPW.Text = "패스워드";
            // 
            // txtLoginID
            // 
            this.txtLoginID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtLoginID.Location = new System.Drawing.Point(132, 75);
            this.txtLoginID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLoginID.Name = "txtLoginID";
            this.txtLoginID.Size = new System.Drawing.Size(160, 25);
            this.txtLoginID.TabIndex = 2;
            // 
            // txtLoginPW
            // 
            this.txtLoginPW.Location = new System.Drawing.Point(132, 155);
            this.txtLoginPW.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLoginPW.Name = "txtLoginPW";
            this.txtLoginPW.PasswordChar = '*';
            this.txtLoginPW.Size = new System.Drawing.Size(160, 25);
            this.txtLoginPW.TabIndex = 3;
            // 
            // LoginButton
            // 
            this.LoginButton.Location = new System.Drawing.Point(132, 222);
            this.LoginButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(160, 29);
            this.LoginButton.TabIndex = 4;
            this.LoginButton.Text = "로그인";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // btnSimpleLogin
            // 
            this.btnSimpleLogin.Location = new System.Drawing.Point(132, 269);
            this.btnSimpleLogin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSimpleLogin.Name = "btnSimpleLogin";
            this.btnSimpleLogin.Size = new System.Drawing.Size(160, 29);
            this.btnSimpleLogin.TabIndex = 5;
            this.btnSimpleLogin.Text = "간편 로그인";
            this.btnSimpleLogin.UseVisualStyleBackColor = true;
            this.btnSimpleLogin.Click += new System.EventHandler(this.btnSimpleLogin_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 335);
            this.Controls.Add(this.btnSimpleLogin);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.txtLoginPW);
            this.Controls.Add(this.txtLoginID);
            this.Controls.Add(this.lblLoginPW);
            this.Controls.Add(this.lblLoginId);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "LoginForm";
            this.Text = "LoginForm";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLoginId;
        private System.Windows.Forms.Label lblLoginPW;
        private System.Windows.Forms.TextBox txtLoginID;
        private System.Windows.Forms.TextBox txtLoginPW;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.Button btnSimpleLogin;
    }
}