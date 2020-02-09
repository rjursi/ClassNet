namespace KeyboardMouseHookerGUI
{
    partial class form_keyMouseCtrl
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
            this.btn_ctrl = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_status
            // 
            this.lbl_status.AutoSize = true;
            this.lbl_status.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_status.Location = new System.Drawing.Point(81, 79);
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(220, 16);
            this.lbl_status.TabIndex = 0;
            this.lbl_status.Text = "키보드 마우스 제어 중입니다.";
            // 
            // btn_ctrl
            // 
            this.btn_ctrl.Location = new System.Drawing.Point(84, 249);
            this.btn_ctrl.Name = "btn_ctrl";
            this.btn_ctrl.Size = new System.Drawing.Size(217, 69);
            this.btn_ctrl.TabIndex = 1;
            this.btn_ctrl.Text = "제어 시작";
            this.btn_ctrl.UseVisualStyleBackColor = true;
            this.btn_ctrl.Click += new System.EventHandler(this.btn_ctrl_Click);
            // 
            // form_keyMouseCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 439);
            this.Controls.Add(this.btn_ctrl);
            this.Controls.Add(this.lbl_status);
            this.Name = "form_keyMouseCtrl";
            this.Text = "키보드 마우스 제어";
            this.Load += new System.EventHandler(this.form_keyMouseCtrl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_status;
        private System.Windows.Forms.Button btn_ctrl;
    }
}

