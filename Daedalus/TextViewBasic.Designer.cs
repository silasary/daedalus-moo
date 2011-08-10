namespace Daedalus
{
    partial class TextViewBasic
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.scrollBar = new System.Windows.Forms.VScrollBar();
            this.SuspendLayout();
            // 
            // scrollBar
            // 
            this.scrollBar.Cursor = System.Windows.Forms.Cursors.Default;
            this.scrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.scrollBar.Location = new System.Drawing.Point(332, 0);
            this.scrollBar.Name = "scrollBar";
            this.scrollBar.Size = new System.Drawing.Size(17, 290);
            this.scrollBar.TabIndex = 1;
            // 
            // TextViewBasic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scrollBar);
            this.Name = "TextViewBasic";
            this.Size = new System.Drawing.Size(349, 290);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar scrollBar;
    }
}
