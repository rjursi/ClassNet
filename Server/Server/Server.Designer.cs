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
            this.btnStreaming = new System.Windows.Forms.Button();
            this.btnCtrlTaskMgr = new System.Windows.Forms.Button();
            this.btnLock = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnViewer = new System.Windows.Forms.Button();
            this.btnInternet = new System.Windows.Forms.Button();
            this.btnPower = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStreaming
            // 
            this.btnStreaming.Location = new System.Drawing.Point(12, 11);
            this.btnStreaming.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStreaming.Name = "btnStreaming";
            this.btnStreaming.Size = new System.Drawing.Size(221, 35);
            this.btnStreaming.TabIndex = 0;
            this.btnStreaming.Text = "실시간 방송";
            this.btnStreaming.UseVisualStyleBackColor = true;
            this.btnStreaming.Click += new System.EventHandler(this.BtnStreaming_Click);
            // 
            // btnCtrlTaskMgr
            // 
            this.btnCtrlTaskMgr.Location = new System.Drawing.Point(12, 291);
            this.btnCtrlTaskMgr.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCtrlTaskMgr.Name = "btnCtrlTaskMgr";
            this.btnCtrlTaskMgr.Size = new System.Drawing.Size(221, 35);
            this.btnCtrlTaskMgr.TabIndex = 1;
            this.btnCtrlTaskMgr.Text = "작업관리자 활성화";
            this.btnCtrlTaskMgr.UseVisualStyleBackColor = true;
            this.btnCtrlTaskMgr.Click += new System.EventHandler(this.BtnCtrlTaskMgr_Click);
            // 
            // btnLock
            // 
            this.btnLock.Location = new System.Drawing.Point(12, 123);
            this.btnLock.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLock.Name = "btnLock";
            this.btnLock.Size = new System.Drawing.Size(221, 35);
            this.btnLock.TabIndex = 2;
            this.btnLock.Text = "키보드 및 마우스 잠금";
            this.btnLock.UseVisualStyleBackColor = true;
            this.btnLock.Click += new System.EventHandler(this.BtnLock_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "MOSH";
            this.notifyIcon.Visible = true;
            // 
            // btnViewer
            // 
            this.btnViewer.Location = new System.Drawing.Point(12, 67);
            this.btnViewer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnViewer.Name = "btnViewer";
            this.btnViewer.Size = new System.Drawing.Size(221, 35);
            this.btnViewer.TabIndex = 3;
            this.btnViewer.Text = "학생 PC 모니터링";
            this.btnViewer.UseVisualStyleBackColor = true;
            this.btnViewer.Click += new System.EventHandler(this.BtnMonitoring_Click);
            // 
            // btnInternet
            // 
            this.btnInternet.Location = new System.Drawing.Point(12, 179);
            this.btnInternet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnInternet.Name = "btnInternet";
            this.btnInternet.Size = new System.Drawing.Size(221, 35);
            this.btnInternet.TabIndex = 3;
            this.btnInternet.Text = "인터넷 차단";
            this.btnInternet.UseVisualStyleBackColor = true;
            this.btnInternet.Click += new System.EventHandler(this.BtnInternet_Click);
            // 
            // btnPower
            // 
            this.btnPower.Location = new System.Drawing.Point(12, 235);
            this.btnPower.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPower.Name = "btnPower";
            this.btnPower.Size = new System.Drawing.Size(221, 35);
            this.btnPower.TabIndex = 1;
            this.btnPower.Text = "강의실 PC 전원 종료";
            this.btnPower.UseVisualStyleBackColor = true;
            this.btnPower.Click += new System.EventHandler(this.BtnPower_Click);
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 340);
            this.Controls.Add(this.btnInternet);
            this.Controls.Add(this.btnViewer);
            this.Controls.Add(this.btnLock);
            this.Controls.Add(this.btnPower);
            this.Controls.Add(this.btnCtrlTaskMgr);
            this.Controls.Add(this.btnStreaming);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Server";
            this.Text = "Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Server_FormClosing);
            this.Load += new System.EventHandler(this.Server_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStreaming;
        private System.Windows.Forms.Button btnCtrlTaskMgr;
        private System.Windows.Forms.Button btnLock;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Button btnViewer;
        private System.Windows.Forms.Button btnInternet;
        private System.Windows.Forms.Button btnPower;
    }
}

