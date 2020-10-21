namespace Client
{
    partial class Client
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
            this.screenImage = new System.Windows.Forms.PictureBox();
            this.imageSize = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.screenImage)).BeginInit();
            this.SuspendLayout();
            // 
            // screenImage
            // 
            this.screenImage.Location = new System.Drawing.Point(0, 0);
            this.screenImage.Margin = new System.Windows.Forms.Padding(0);
            this.screenImage.Name = "screenImage";
            this.screenImage.Size = new System.Drawing.Size(640, 360);
            this.screenImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.screenImage.TabIndex = 0;
            this.screenImage.TabStop = false;
            // 
            // imageSize
            // 
            this.imageSize.AutoSize = true;
            this.imageSize.Font = new System.Drawing.Font("맑은 고딕", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.imageSize.Location = new System.Drawing.Point(21, 20);
            this.imageSize.Name = "imageSize";
            this.imageSize.Size = new System.Drawing.Size(83, 19);
            this.imageSize.TabIndex = 1;
            this.imageSize.Text = "Image Size";
            // 
            // notifyIcon
            // 
            this.notifyIcon.Text = "ClassNet Client";
            this.notifyIcon.Visible = true;
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 360);
            this.Controls.Add(this.imageSize);
            this.Controls.Add(this.screenImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Client";
            this.Text = "Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Client_FormClosing);
            this.Load += new System.EventHandler(this.Client_Load);
            ((System.ComponentModel.ISupportInitialize)(this.screenImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox screenImage;
        private System.Windows.Forms.Label imageSize;
        private System.Windows.Forms.NotifyIcon notifyIcon;
    }
}

