using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Daedalus.Core;

namespace Daedalus.Forms
{
    public partial class WorldSelect : Form
    {
        Daedalus.Core.SessionManager manager;
        public WorldSelect()
        {
            manager = Daedalus.Core.SessionManager.Default;
            InitializeComponent();
            imageList1.Images.Add(Properties.Resources.globe);
            listView1.VirtualMode = true;
            listView1.VirtualListSize = manager.Sessions.Count;
            this.Owner = MainForm.FindMainForm();
        }

        private void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            e.Item = new ListViewItem(manager.Sessions[e.ItemIndex].Name);
            e.Item.ImageIndex = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NewWorld nw = new NewWorld();
            nw.ShowDialog();
           // manager.Sessions.Add(nw.Session);
            listView1.VirtualListSize = manager.Sessions.Count;
            listView1.Update();

            ((MainForm)this.Owner).NewWorldWindow(nw.Session);
            this.Close();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                foreach (int i in listView1.SelectedIndices)
                {
                    ((MainForm)this.Owner).NewWorldWindow(manager.Sessions[i]);
                }
                this.Close();
            }
            else
                MessageBox.Show("No world selected.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1_DoubleClick(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                foreach (int i in listView1.SelectedIndices)
                {
                    NewWorld nwForm = new NewWorld() { Session = manager.Sessions[i] };
                    nwForm.ShowDialog();
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                List<SavedSession> sessions = new List<SavedSession>();
                foreach (int i in listView1.SelectedIndices)
                {
                    sessions.Add(manager.Sessions[i]);  // Otherwise the indexes get messed up.
                }
                foreach (SavedSession s in sessions)
                {
                    manager.Sessions.Remove(s);
                }
                listView1.VirtualListSize = manager.Sessions.Count;
            }
        }
    }
}
