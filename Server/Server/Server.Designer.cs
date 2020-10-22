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
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnCtrlTaskMgr = new System.Windows.Forms.Button();
            this.btnInternet = new System.Windows.Forms.Button();
            this.btnViewer = new System.Windows.Forms.Button();
            this.btnLock = new System.Windows.Forms.Button();
            this.btnPower = new System.Windows.Forms.Button();
            this.btnStreaming = new System.Windows.Forms.Button();
            this.cbMonitor = new MetroFramework.Controls.MetroComboBox();
            this.lblSelectMonitor = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "ClassNet";
            this.notifyIcon.Visible = true;
            // 
            // btnCtrlTaskMgr
            // 
            this.btnCtrlTaskMgr.FlatAppearance.BorderSize = 0;
            this.btnCtrlTaskMgr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCtrlTaskMgr.Font = new System.Drawing.Font("맑은 고딕", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCtrlTaskMgr.Image = ((System.Drawing.Image)(resources.GetObject("btnCtrlTaskMgr.Image")));
            this.btnCtrlTaskMgr.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCtrlTaskMgr.Location = new System.Drawing.Point(0, 460);
            this.btnCtrlTaskMgr.Margin = new System.Windows.Forms.Padding(0);
            this.btnCtrlTaskMgr.Name = "btnCtrlTaskMgr";
            this.btnCtrlTaskMgr.Size = new System.Drawing.Size(370, 90);
            this.btnCtrlTaskMgr.TabIndex = 1;
            this.btnCtrlTaskMgr.UseVisualStyleBackColor = true;
            this.btnCtrlTaskMgr.Click += new System.EventHandler(this.BtnCtrlTaskMgr_Click);
            // 
            // btnInternet
            // 
            this.btnInternet.FlatAppearance.BorderSize = 0;
            this.btnInternet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInternet.Font = new System.Drawing.Font("맑은 고딕", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInternet.Image = ((System.Drawing.Image)(resources.GetObject("btnInternet.Image")));
            this.btnInternet.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInternet.Location = new System.Drawing.Point(0, 280);
            this.btnInternet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnInternet.Name = "btnInternet";
            this.btnInternet.Size = new System.Drawing.Size(370, 90);
            this.btnInternet.TabIndex = 3;
            this.btnInternet.UseVisualStyleBackColor = true;
            this.btnInternet.Click += new System.EventHandler(this.BtnInternet_Click);
            // 
            // btnViewer
            // 
            this.btnViewer.FlatAppearance.BorderSize = 0;
            this.btnViewer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewer.Font = new System.Drawing.Font("맑은 고딕", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnViewer.Image = ((System.Drawing.Image)(resources.GetObject("btnViewer.Image")));
            this.btnViewer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnViewer.Location = new System.Drawing.Point(0, 190);
            this.btnViewer.Margin = new System.Windows.Forms.Padding(0);
            this.btnViewer.Name = "btnViewer";
            this.btnViewer.Size = new System.Drawing.Size(370, 90);
            this.btnViewer.TabIndex = 3;
            this.btnViewer.UseVisualStyleBackColor = true;
            this.btnViewer.Click += new System.EventHandler(this.BtnMonitoring_Click);
            // 
            // btnLock
            // 
            this.btnLock.FlatAppearance.BorderSize = 0;
            this.btnLock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLock.Font = new System.Drawing.Font("맑은 고딕", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLock.Image = ((System.Drawing.Image)(resources.GetObject("btnLock.Image")));
            this.btnLock.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLock.Location = new System.Drawing.Point(0, 370);
            this.btnLock.Margin = new System.Windows.Forms.Padding(0);
            this.btnLock.Name = "btnLock";
            this.btnLock.Size = new System.Drawing.Size(370, 90);
            this.btnLock.TabIndex = 2;
            this.btnLock.UseVisualStyleBackColor = true;
            this.btnLock.Click += new System.EventHandler(this.BtnLock_Click);
            // 
            // btnPower
            // 
            this.btnPower.FlatAppearance.BorderSize = 0;
            this.btnPower.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPower.Font = new System.Drawing.Font("맑은 고딕", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPower.Image = ((System.Drawing.Image)(resources.GetObject("btnPower.Image")));
            this.btnPower.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPower.Location = new System.Drawing.Point(0, 550);
            this.btnPower.Margin = new System.Windows.Forms.Padding(0);
            this.btnPower.Name = "btnPower";
            this.btnPower.Size = new System.Drawing.Size(370, 90);
            this.btnPower.TabIndex = 1;
            this.btnPower.UseVisualStyleBackColor = true;
            this.btnPower.Click += new System.EventHandler(this.BtnPower_Click);
            // 
            // btnStreaming
            // 
            this.btnStreaming.FlatAppearance.BorderSize = 0;
            this.btnStreaming.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStreaming.Font = new System.Drawing.Font("맑은 고딕", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnStreaming.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnStreaming.Image = ((System.Drawing.Image)(resources.GetObject("btnStreaming.Image")));
            this.btnStreaming.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStreaming.Location = new System.Drawing.Point(0, 100);
            this.btnStreaming.Margin = new System.Windows.Forms.Padding(0);
            this.btnStreaming.Name = "btnStreaming";
            this.btnStreaming.Size = new System.Drawing.Size(370, 90);
            this.btnStreaming.TabIndex = 0;
            this.btnStreaming.UseVisualStyleBackColor = true;
            this.btnStreaming.Click += new System.EventHandler(this.BtnStreaming_Click);
            // 
            // cbMonitor
            // 
            this.cbMonitor.FormattingEnabled = true;
            this.cbMonitor.ItemHeight = 24;
            this.cbMonitor.Location = new System.Drawing.Point(23, 47);
            this.cbMonitor.Name = "cbMonitor";
            this.cbMonitor.Size = new System.Drawing.Size(319, 30);
            this.cbMonitor.TabIndex = 6;
            this.cbMonitor.UseSelectable = true;
            this.cbMonitor.SelectedIndexChanged += new System.EventHandler(this.CbMonitor_SelectedIndexChanged);
            // 
            // lblSelectMonitor
            // 
            this.lblSelectMonitor.AutoSize = true;
            this.lblSelectMonitor.FontSize = MetroFramework.MetroLabelSize.Small;
            this.lblSelectMonitor.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblSelectMonitor.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblSelectMonitor.Location = new System.Drawing.Point(23, 17);
            this.lblSelectMonitor.Name = "lblSelectMonitor";
            this.lblSelectMonitor.Size = new System.Drawing.Size(94, 17);
            this.lblSelectMonitor.TabIndex = 7;
            this.lblSelectMonitor.Text = "방송 화면 선택";
            this.lblSelectMonitor.UseCustomForeColor = true;
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;

            this.ClientSize = new System.Drawing.Size(367, 638);
            this.Controls.Add(this.lblSelectMonitor);

            this.Controls.Add(this.cbMonitor);
            this.Controls.Add(this.btnCtrlTaskMgr);
            this.Controls.Add(this.btnInternet);
            this.Controls.Add(this.btnViewer);
            this.Controls.Add(this.btnLock);
            this.Controls.Add(this.btnPower);
            this.Controls.Add(this.btnStreaming);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Server";
            this.Text = "ClassNet";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Server_FormClosing);
            this.Load += new System.EventHandler(this.Server_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStreaming;


        private System.Windows.Forms.Button btnCtrlTaskMgr;

        private System.Windows.Forms.Button btnLock;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Button btnViewer;
        private System.Windows.Forms.Button btnInternet;
        private System.Windows.Forms.Button btnPower;
        private MetroFramework.Controls.MetroComboBox cbMonitor;
        private MetroFramework.Controls.MetroLabel lblSelectMonitor;
    }
}
