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
            this.SuspendLayout();
            // 
            // clientsViewPanel
            // 
            this.clientsViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clientsViewPanel.Location = new System.Drawing.Point(0, 0);
            this.clientsViewPanel.Name = "clientsViewPanel";
            this.clientsViewPanel.Size = new System.Drawing.Size(530, 353);
            this.clientsViewPanel.TabIndex = 0;
            // 
            // Viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 353);
            this.Controls.Add(this.clientsViewPanel);
            this.MaximumSize = new System.Drawing.Size(546, 392);
            this.MinimumSize = new System.Drawing.Size(546, 392);
            this.Name = "Viewer";
            this.Text = "클라이언트 화면";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel clientsViewPanel;
    }
}