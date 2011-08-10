using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chiroptera.Base;

namespace Daedalus.Forms
{
    public partial class NewWorld : Form
    {
        private SavedSession session;
        public SavedSession Session
        {
            get { return session; }
            set
            {
                session = value;
                ServerTextBox.DataBindings.Clear();
                ServerTextBox.DataBindings.Add("Text", session, "Server");
                PortTextBox.DataBindings.Clear();
                PortTextBox.DataBindings.Add("Text", session, "Port");
                UsernameTextBox.DataBindings.Clear();
                UsernameTextBox.DataBindings.Add("Text", session, "Username");
                PasswordtextBox.DataBindings.Clear();
                PasswordtextBox.DataBindings.Add("Text", session, "Password");
            }
        }
        public NewWorld()
        {
            InitializeComponent();
            this.Session = new SavedSession();
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        public string Server
        {
            get
            {
                return ServerTextBox.Text;
            }
            set
            {
                ServerTextBox.Text = value;
            }
        }
        public string Port
        {
            get
            {
                if (PortTextBox.Text == "")
                    return "7777";
                return PortTextBox.Text;
            }
            set
            {
                PortTextBox.Text = value;
            }
        }

    }
}
