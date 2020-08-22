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
            this.loginLbl1 = new System.Windows.Forms.Label();
            this.loginLbl2 = new System.Windows.Forms.Label();
            this.loginTextBoxID = new System.Windows.Forms.TextBox();
            this.loginTextBoxName = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // loginLbl1
            // 
            this.loginLbl1.AutoSize = true;
            this.loginLbl1.Location = new System.Drawing.Point(108, 57);
            this.loginLbl1.Name = "loginLbl1";
            this.loginLbl1.Size = new System.Drawing.Size(33, 12);
            this.loginLbl1.TabIndex = 0;
            this.loginLbl1.Text = "학 번";
            // 
            // loginLbl2
            // 
            this.loginLbl2.AutoSize = true;
            this.loginLbl2.Location = new System.Drawing.Point(108, 117);
            this.loginLbl2.Name = "loginLbl2";
            this.loginLbl2.Size = new System.Drawing.Size(33, 12);
            this.loginLbl2.TabIndex = 1;
            this.loginLbl2.Text = "이 름";
            // 
            // loginTextBoxID
            // 
            this.loginTextBoxID.Location = new System.Drawing.Point(184, 54);
            this.loginTextBoxID.Name = "loginTextBoxID";
            this.loginTextBoxID.Size = new System.Drawing.Size(100, 21);
            this.loginTextBoxID.TabIndex = 2;
            // 
            // loginTextBoxName
            // 
            this.loginTextBoxName.Location = new System.Drawing.Point(184, 114);
            this.loginTextBoxName.Name = "loginTextBoxName";
            this.loginTextBoxName.Size = new System.Drawing.Size(100, 21);
            this.loginTextBoxName.TabIndex = 3;
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(209, 161);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 4;
            this.loginButton.Text = "로그인";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 240);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.loginTextBoxName);
            this.Controls.Add(this.loginTextBoxID);
            this.Controls.Add(this.loginLbl2);
            this.Controls.Add(this.loginLbl1);
            this.Name = "LoginForm";
            this.Text = "LoginForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label loginLbl1;
        private System.Windows.Forms.Label loginLbl2;
        private System.Windows.Forms.TextBox loginTextBoxID;
        private System.Windows.Forms.TextBox loginTextBoxName;
        private System.Windows.Forms.Button loginButton;
    }
}