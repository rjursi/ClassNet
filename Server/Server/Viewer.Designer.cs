namespace Server
{
    partial class Viewer
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
            this.clientsViewPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.viewerToolStrip = new System.Windows.Forms.ToolStrip();
            this.btnAllSave = new System.Windows.Forms.ToolStripButton();
            this.lblAllSave = new System.Windows.Forms.ToolStripLabel();
            this.viewerToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // clientsViewPanel
            // 
            this.clientsViewPanel.AutoScroll = true;
            this.clientsViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clientsViewPanel.Location = new System.Drawing.Point(0, 0);
            this.clientsViewPanel.Name = "clientsViewPanel";
            this.clientsViewPanel.Size = new System.Drawing.Size(909, 351);
            this.clientsViewPanel.TabIndex = 0;
            // 
            // viewerToolStrip
            // 
            this.viewerToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.viewerToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAllSave,
            this.lblAllSave});
            this.viewerToolStrip.Location = new System.Drawing.Point(0, 0);
            this.viewerToolStrip.Name = "viewerToolStrip";
            this.viewerToolStrip.Size = new System.Drawing.Size(909, 27);
            this.viewerToolStrip.TabIndex = 1;
            this.viewerToolStrip.Text = "viewerToolStrip";
            // 
            // btnAllSave
            // 
            this.btnAllSave.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnAllSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAllSave.Image = global::Server.Resource.capture;
            this.btnAllSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAllSave.Margin = new System.Windows.Forms.Padding(0, 1, 10, 2);
            this.btnAllSave.Name = "btnAllSave";
            this.btnAllSave.Size = new System.Drawing.Size(24, 24);
            this.btnAllSave.Text = "전체 화면 캡처";
            this.btnAllSave.Click += new System.EventHandler(this.BtnAllSave_Click);
            // 
            // lblAllSave
            // 
            this.lblAllSave.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblAllSave.Font = new System.Drawing.Font("맑은 고딕", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblAllSave.Name = "lblAllSave";
            this.lblAllSave.Size = new System.Drawing.Size(81, 24);
            this.lblAllSave.Text = "전체 화면 캡처";
            // 
            // Viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(909, 351);
            this.Controls.Add(this.viewerToolStrip);
            this.Controls.Add(this.clientsViewPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximumSize = new System.Drawing.Size(925, 390);
            this.MinimumSize = new System.Drawing.Size(545, 390);
            this.Name = "Viewer";
            this.Text = "화면 목록";
            this.Load += new System.EventHandler(this.Viewer_Load);
            this.viewerToolStrip.ResumeLayout(false);
            this.viewerToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel clientsViewPanel;
        private System.Windows.Forms.ToolStrip viewerToolStrip;
        private System.Windows.Forms.ToolStripLabel lblAllSave;
        private System.Windows.Forms.ToolStripButton btnAllSave;
    }
}