namespace HookerProcess
{
    partial class form_keyMouseControlling
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
            this.lbl_status = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_status
            // 
            this.lbl_status.AutoSize = true;
            this.lbl_status.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_status.Location = new System.Drawing.Point(12, 14);
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(273, 16);
            this.lbl_status.TabIndex = 0;
            this.lbl_status.Text = "키보드 마우스 제어가 동작 중입니다.";
            // 
            // form_keyMouseControlling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 39);
            this.ControlBox = false;
            this.Controls.Add(this.lbl_status);
            this.Name = "form_keyMouseControlling";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "키보드 마우스 제어";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_keyMouseControlling_FormClosing);
            this.Load += new System.EventHandler(this.form_keyMouseControlling_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_status;
    }
}

