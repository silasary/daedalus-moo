namespace Daedalus.Forms
{
    partial class NewWorld
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
            System.Windows.Forms.Label ServerLabel;
            System.Windows.Forms.Label PortLabel;
            this.Connect = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.ServerTextBox = new System.Windows.Forms.TextBox();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PasswordtextBox = new System.Windows.Forms.TextBox();
            this.UsernameTextBox = new System.Windows.Forms.TextBox();
            ServerLabel = new System.Windows.Forms.Label();
            PortLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ServerLabel
            // 
            ServerLabel.AutoSize = true;
            ServerLabel.Location = new System.Drawing.Point(12, 17);
            ServerLabel.Name = "ServerLabel";
            ServerLabel.Size = new System.Drawing.Size(38, 13);
            ServerLabel.TabIndex = 2;
            ServerLabel.Text = "Server";
            // 
            // PortLabel
            // 
            PortLabel.AutoSize = true;
            PortLabel.Location = new System.Drawing.Point(12, 46);
            PortLabel.Name = "PortLabel";
            PortLabel.Size = new System.Drawing.Size(26, 13);
            PortLabel.TabIndex = 2;
            PortLabel.Text = "Port";
            // 
            // Connect
            // 
            this.Connect.Location = new System.Drawing.Point(206, 12);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(75, 23);
            this.Connect.TabIndex = 0;
            this.Connect.Text = "Connect";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(206, 41);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 0;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // ServerTextBox
            // 
            this.ServerTextBox.Location = new System.Drawing.Point(68, 14);
            this.ServerTextBox.Name = "ServerTextBox";
            this.ServerTextBox.Size = new System.Drawing.Size(132, 20);
            this.ServerTextBox.TabIndex = 1;
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(68, 43);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(132, 20);
            this.PortTextBox.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.UsernameTextBox);
            this.groupBox1.Controls.Add(this.PasswordtextBox);
            this.groupBox1.Location = new System.Drawing.Point(13, 75);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 71);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Optional details";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "user/pass";
            // 
            // PasswordtextBox
            // 
            this.PasswordtextBox.Location = new System.Drawing.Point(161, 19);
            this.PasswordtextBox.Name = "PasswordtextBox";
            this.PasswordtextBox.Size = new System.Drawing.Size(100, 20);
            this.PasswordtextBox.TabIndex = 1;
            // 
            // UsernameTextBox
            // 
            this.UsernameTextBox.Location = new System.Drawing.Point(67, 19);
            this.UsernameTextBox.Name = "UsernameTextBox";
            this.UsernameTextBox.Size = new System.Drawing.Size(88, 20);
            this.UsernameTextBox.TabIndex = 0;
            // 
            // NewWorld
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 158);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(PortLabel);
            this.Controls.Add(ServerLabel);
            this.Controls.Add(this.ServerTextBox);
            this.Controls.Add(this.PortTextBox);
            this.Controls.Add(this.Connect);
            this.Controls.Add(this.Cancel);
            this.Name = "NewWorld";
            this.Text = "WorldSelectForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Connect;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.TextBox ServerTextBox;
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PasswordtextBox;
        private System.Windows.Forms.TextBox UsernameTextBox;
    }
}