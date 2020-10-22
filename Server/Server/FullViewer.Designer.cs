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
            this.picFocusView = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picFocusView)).BeginInit();
            this.SuspendLayout();
            // 
            // picFocusView
            // 
            this.picFocusView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picFocusView.Location = new System.Drawing.Point(0, 0);
            this.picFocusView.Name = "picFocusView";
            this.picFocusView.Size = new System.Drawing.Size(1008, 729);
            this.picFocusView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picFocusView.TabIndex = 0;
            this.picFocusView.TabStop = false;
            // 
            // FullViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.picFocusView);
            this.Name = "FullViewer";
            this.Load += new System.EventHandler(this.FullViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picFocusView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picFocusView;
    }
}