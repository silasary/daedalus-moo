using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Daedalus.Forms;
using Chiroptera.Base;

namespace Daedalus
{
    public partial class MainForm : Form
    {
        public static MainForm FindMainForm()
        {
            foreach (Form f in Application.OpenForms)
                if (f is MainForm)
                    return (MainForm)f;
            return null; 
        }
        int numToolBarItems;
        public MainForm()
        {
            InitializeComponent();
            numToolBarItems = toolStrip.Items.Count;
            this.Text = Settings.Default.ClientName;
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Forms.NewWorld wsf = new Daedalus.Forms.NewWorld();
            if (wsf.ShowDialog() == DialogResult.OK)
            {
                SavedSession session = wsf.Session;
                NewWorldWindow(session);
            }
        }

        public void NewWorldWindow(SavedSession session)
        {
            WorldForm childForm = new WorldForm();
            childForm.Text = session.Name;
            childForm.MdiParent = this;
            childForm.tabPage = new TabPage(childForm.Text);
            tabControl1.TabPages.Add(childForm.tabPage);
            childForm.Closed += new EventHandler(childForm_Closed);
            childForm.Activated += new EventHandler(childForm_Activated);
            childForm.StatusLabel.Text = new Random().Next().ToString();
            childForm.WindowState = FormWindowState.Maximized;
            childForm.Show();
            if (tabControl1.HasChildren)
                tabControl1.Visible = true;
            childForm._connection.Connect(session);
        }
        
        void childForm_Activated(object sender, EventArgs e)
        {
            if ((sender as MDIChild).MdiParent != this)
                return;
            tabControl1.SelectedTab = (sender as MDIChild).tabPage;

            if (!tabControl1.Visible)
            {
                tabControl1.Visible = true;
            }
            ToolStripMenuItem menu = (sender as MDIChild).menu;
            if (menu == null)
                worldToolStripMenuItem.Enabled = false;
            else
            {
                worldToolStripMenuItem.Enabled = true;
                this.worldToolStripMenuItem = (sender as MDIChild).menu;
            }
            this.statusStrip.Items.Clear();
            this.statusStrip.Items.AddRange((sender as MDIChild).ToolStripItems.ToArray());
            while (toolStrip.Items.Count > numToolBarItems)
                toolStrip.Items.RemoveAt(numToolBarItems);
            toolStrip.Items.AddRange((sender as MDIChild).ToolbarItems.ToArray());
        }

        void childForm_Closed(object sender, EventArgs e)
        {
            (sender as WorldForm).tabPage.Dispose();
            if (!tabControl1.HasChildren)
                tabControl1.Visible = false;
        }

        private void OpenFile(object sender, EventArgs e)
        {
            new WorldSelect().Show(this);
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void MDIForm_MdiChildActivate(object sender, EventArgs e)
        {
            
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                //this.MdiChildren.FirstOrDefault(child => (child as WorldForm).tabPage == tabControl1.SelectedTab).Select();
                foreach (Form f in Application.OpenForms)
                    if ((f is MDIChild) && (f as MDIChild).tabPage == tabControl1.SelectedTab)
                        f.Select();
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MainForm.FindMainForm() == null)
                Application.Exit();
        }
    }
}
