namespace Daedalus.Forms
{
    partial class WorldSelect
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
            this.components = new System.ComponentModel.Container();
            this.listView1 = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.CloseButton = new System.Windows.Forms.Button();
            this.InfoBox = new System.Windows.Forms.GroupBox();
            this.NewWorldButton = new System.Windows.Forms.Button();
            this.OpenButton = new System.Windows.Forms.Button();
            this.InfoBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(13, 13);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(276, 305);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.listView1_RetrieveVirtualItem);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(32, 32);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // CloseButton
            // 
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(295, 295);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "Cancel";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // InfoBox
            // 
            this.InfoBox.Controls.Add(this.NewWorldButton);
            this.InfoBox.Location = new System.Drawing.Point(295, 13);
            this.InfoBox.Name = "InfoBox";
            this.InfoBox.Size = new System.Drawing.Size(157, 276);
            this.InfoBox.TabIndex = 2;
            this.InfoBox.TabStop = false;
            this.InfoBox.Text = "Information";
            // 
            // NewWorldButton
            // 
            this.NewWorldButton.Location = new System.Drawing.Point(7, 247);
            this.NewWorldButton.Name = "NewWorldButton";
            this.NewWorldButton.Size = new System.Drawing.Size(144, 23);
            this.NewWorldButton.TabIndex = 0;
            this.NewWorldButton.Text = "New World";
            this.NewWorldButton.UseVisualStyleBackColor = true;
            this.NewWorldButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // OpenButton
            // 
            this.OpenButton.Location = new System.Drawing.Point(377, 295);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(75, 23);
            this.OpenButton.TabIndex = 1;
            this.OpenButton.Text = "Open";
            this.OpenButton.UseVisualStyleBackColor = true;
            this.OpenButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // WorldSelect
            // 
            this.AcceptButton = this.OpenButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 330);
            this.Controls.Add(this.InfoBox);
            this.Controls.Add(this.OpenButton);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.listView1);
            this.Name = "WorldSelect";
            this.Text = "WorldSelect";
            this.InfoBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.GroupBox InfoBox;
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.Button NewWorldButton;
        private System.Windows.Forms.ImageList imageList1;
    }
}