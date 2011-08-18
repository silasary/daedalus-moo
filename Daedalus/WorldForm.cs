using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Daedalus.TextView;
using Chiroptera.Win;

namespace Daedalus
{
    public partial class WorldForm : MDIChild
    {
        public IConnection _connection;
        ITextView _textView;

        public ITextView TextView
        {
            get
            {
                return _textView;
            }
            set
            {
                _textView = value;
            }
        }
        public UserControl TextViewControl
        {
            get
            {
                return _textView as UserControl;
            }
        }
        public Panel Sidebar
        {
            get
            {
                return splitContainer2.Panel2;
            }
        }
        public bool SidebarCollapsed
        {
            get
            {
                return splitContainer2.Panel2Collapsed;
            }
            set
            {
                splitContainer2.Panel2Collapsed = value;
            }
        }

        public WorldForm()
        {
            InitializeComponent();
            _connection = new Connection(this);
            InitTextView();
            _connection.WriteLine("Initialized");
            this.StatusLabel.DoubleClick += new EventHandler(toolStripStatusLabel_DoubleClick);
            
        }

        private void InitTextView()
        {
            switch (System.Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                    TextView = new TextViewWin();
                    break;
                default:
                    MessageBox.Show(System.Environment.OSVersion.Platform.ToString() + " is not currently supported.");
                    TextView = new TextViewBasic();
                    break;
            }
            TextView.ParagraphContainer = new ParagraphContainer();
            TextViewControl.Dock = DockStyle.Fill;
            TextViewControl.BackColor = System.Drawing.Color.Black;
            TextViewControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            //TextViewControl.DataBindings.Add(new System.Windows.Forms.Binding("Font", global::Chiroptera.Win.Properties.Settings.Default, "MainFont", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            TextViewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            //TextViewControl.Font = global::Chiroptera.Win.Properties.Settings.Default.MainFont;
            TextViewControl.ForeColor = System.Drawing.Color.White;
            TextViewControl.TabStop = false;
            splitContainer2.Panel1.Controls.Add(TextViewControl);
            historyTextBox1.ListenOther(TextViewControl); // Pass the keypresses.
            TextView.LinkClicked += new LinkClickDelegate(TextView_LinkClicked);
        }

        void TextView_LinkClicked(string link)
        {
            string proto = link.Substring(0, link.IndexOf(':'));
            switch (proto)
            {
                case "http":
                case "https":
                    System.Diagnostics.Process.Start(link); // Leave that to the default browser.
                    break;
                default:
                    _connection.ServicesDispatcher.DispatchLinkClicked(link);
                    break;
            }
        }

        public ParagraphContainer ParagraphContainer
        {
            get
            {
                return TextView.ParagraphContainer;
            }
        }

       private void historyTextBox1_textEntered(string str)
       {

           _connection.SendLine(str);
       }

       private void WorldForm_FormClosing(object sender, FormClosingEventArgs e)
       {
           _connection.Disconnect();
           if (_connection.Session != null && !Chiroptera.Base.SessionManager.Default.Sessions.Contains(_connection.Session))
           {

               if (Settings.Default.BasicMode != true)
               {
                   switch (MessageBox.Show("Save session?", _connection.Session.Name, MessageBoxButtons.YesNo))
                   {
                       case (DialogResult.Yes):
                           Chiroptera.Base.SessionManager.Default.Sessions.Add(_connection.Session);
                           break;
                       case (DialogResult.Cancel):
                           e.Cancel = true;
                           break;
                   }
               }
               else
               {
                   Application.Exit();
               }
           }
       }

       private void splitContainer2_Panel2_Resize(object sender, EventArgs e)
       {
           foreach (Control c in Sidebar.Controls)
               c.Width = Sidebar.Width;
       }

       private void toolStripStatusLabel_DoubleClick(object sender, EventArgs e)
       {

           if ((((ToolStripLabel)sender).Tag) is Chiroptera.Win.Paragraph.MetaData)
           {
               Chiroptera.Win.Paragraph.MetaData metadata = (Chiroptera.Win.Paragraph.MetaData)(((ToolStripLabel)sender).Tag);
               if (metadata.linkurl != null && metadata.linkurl != "")
               {

               }
           }
       }

       private void WorldForm_Load(object sender, EventArgs e)
       {

       }
    }
}
