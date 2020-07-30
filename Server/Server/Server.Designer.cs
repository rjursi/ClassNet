namespace Server
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server));
            this.btnScreenSend = new System.Windows.Forms.Button();
            this.btnShutdown = new System.Windows.Forms.Button();
            this.btnControl = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnClientsView = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnScreenSend
            // 
            this.btnScreenSend.Location = new System.Drawing.Point(10, 9);
            this.btnScreenSend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnScreenSend.Name = "btnScreenSend";
            this.btnScreenSend.Size = new System.Drawing.Size(172, 28);
            this.btnScreenSend.TabIndex = 0;
            this.btnScreenSend.Text = "화면 전송";
            this.btnScreenSend.UseVisualStyleBackColor = true;
            this.btnScreenSend.Click += new System.EventHandler(this.BtnScreenSend_Click);
            // 
            // btnShutdown
            // 
            this.btnShutdown.Location = new System.Drawing.Point(10, 144);
            this.btnShutdown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnShutdown.Name = "btnShutdown";
            this.btnShutdown.Size = new System.Drawing.Size(172, 28);
            this.btnShutdown.TabIndex = 1;
            this.btnShutdown.Text = "종료";
            this.btnShutdown.UseVisualStyleBackColor = true;
            this.btnShutdown.Click += new System.EventHandler(this.BtnShutdown_Click);
            // 
            // btnControl
            // 
            this.btnControl.Location = new System.Drawing.Point(10, 42);
            this.btnControl.Name = "btnControl";
            this.btnControl.Size = new System.Drawing.Size(172, 28);
            this.btnControl.TabIndex = 2;
            this.btnControl.Text = "조작 제어";
            this.btnControl.UseVisualStyleBackColor = true;
            this.btnControl.Click += new System.EventHandler(this.BtnControl_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "MOSH";
            this.notifyIcon.Visible = true;
            // 
            // btnClientsView
            // 
            this.btnClientsView.Location = new System.Drawing.Point(10, 76);
            this.btnClientsView.Name = "btnClientsView";
            this.btnClientsView.Size = new System.Drawing.Size(172, 28);
            this.btnClientsView.TabIndex = 3;
            this.btnClientsView.Text = "화면 보기";
            this.btnClientsView.UseVisualStyleBackColor = true;
            this.btnClientsView.Click += new System.EventHandler(this.btnClientsView_Click);
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(192, 181);
            this.Controls.Add(this.btnClientsView);
            this.Controls.Add(this.btnControl);
            this.Controls.Add(this.btnShutdown);
            this.Controls.Add(this.btnScreenSend);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Server";
            this.Text = "Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Server_FormClosing);
            this.Load += new System.EventHandler(this.Server_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnScreenSend;
        private System.Windows.Forms.Button btnShutdown;
        private System.Windows.Forms.Button btnControl;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Button btnClientsView;
    }
}

