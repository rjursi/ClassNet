namespace Server
{
    partial class FullViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FullViewer));
            this.picFocusView = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picFocusView)).BeginInit();
            this.SuspendLayout();
            // 
            // picFocusView
            // 
            this.picFocusView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picFocusView.Location = new System.Drawing.Point(0, 0);
            this.picFocusView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.picFocusView.Name = "picFocusView";
            this.picFocusView.Size = new System.Drawing.Size(1152, 911);
            this.picFocusView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picFocusView.TabIndex = 0;
            this.picFocusView.TabStop = false;
            // 
            // FullViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1152, 911);
            this.Controls.Add(this.picFocusView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimizeBox = false;
            this.Name = "FullViewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FullViewer_FormClosing);
            this.Load += new System.EventHandler(this.FullViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picFocusView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picFocusView;
    }
}