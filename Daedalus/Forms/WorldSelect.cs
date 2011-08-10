using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Daedalus.Forms
{
    public partial class WorldSelect : Form
    {
        Chiroptera.Base.SessionManager manager;
        public WorldSelect()
        {
            manager = Chiroptera.Base.SessionManager.Default;
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
