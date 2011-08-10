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
            this.Session.Username = "Username";
            this.Session.Password = "Password";
        }

        void UsernameTextBox_LostFocus(object sender, System.EventArgs e)
        {
            if (UsernameTextBox.Text == "")
            {
                UsernameTextBox.Text = "Username";
            }
        }

        void UsernameTextBox_GotFocus(object sender, System.EventArgs e)
        {
            if (UsernameTextBox.Text == "Username")
            {
                UsernameTextBox.Text = "";
            }
        }

        void PasswordtextBox_LostFocus(object sender, System.EventArgs e)
        {
            if (PasswordtextBox.Text == "")
            {
                PasswordtextBox.Text = "Password";
                PasswordtextBox.UseSystemPasswordChar = false;
            }
        }

        void PasswordtextBox_GotFocus(object sender, System.EventArgs e)
        {
            if (PasswordtextBox.Text == "Password")
            {
                PasswordtextBox.UseSystemPasswordChar = true;
                PasswordtextBox.Text = "";
            }
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            if (PasswordtextBox.Text == "Password")
            {
                PasswordtextBox.Text = "";
                this.Session.Password = "";
            }
            if (UsernameTextBox.Text == "Username")
            {
                UsernameTextBox.Text = "";
                this.Session.Username = "";
            }
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

        private void NewWorld_Load(object sender, EventArgs e)
        {

        }

    }
}
