namespace monitorSharing_server
{
    partial class Server
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_serverStart = new System.Windows.Forms.Button();
            this.btn_serverClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_serverStart
            // 
            this.btn_serverStart.Location = new System.Drawing.Point(10, 10);
            this.btn_serverStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_serverStart.Name = "btn_serverStart";
            this.btn_serverStart.Size = new System.Drawing.Size(158, 28);
            this.btn_serverStart.TabIndex = 1;
            this.btn_serverStart.Text = "시작";
            this.btn_serverStart.UseVisualStyleBackColor = true;
            this.btn_serverStart.Click += new System.EventHandler(this.btn_serverStart_Click);
            // 
            // btn_serverClose
            // 
            this.btn_serverClose.Location = new System.Drawing.Point(10, 42);
            this.btn_serverClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_serverClose.Name = "btn_serverClose";
            this.btn_serverClose.Size = new System.Drawing.Size(158, 28);
            this.btn_serverClose.TabIndex = 2;
            this.btn_serverClose.Text = "종료";
            this.btn_serverClose.UseVisualStyleBackColor = true;
            this.btn_serverClose.Click += new System.EventHandler(this.btn_serverClose_Click);
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(179, 80);
            this.Controls.Add(this.btn_serverClose);
            this.Controls.Add(this.btn_serverStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Server";
            this.Text = "Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.serverForm_FormClosing);
            this.Load += new System.EventHandler(this.serverForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_serverStart;
        private System.Windows.Forms.Button btn_serverClose;
    }
}

