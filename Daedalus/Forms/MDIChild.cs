using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Daedalus
{
    public class MDIChild : Form
    {
        protected internal TabPage tabPage;

        public ToolStripMenuItem menu;

        private MenuStrip menustrip;
        private StatusStrip statusBar;
        ToolStripButton detachbutton;

        public List<ToolStripItem> ToolbarItems = new List<ToolStripItem>();
        public List<ToolStripItem> ToolStripItems = new List<ToolStripItem>();
        public ToolStripStatusLabel StatusLabel = new ToolStripStatusLabel();
        public MDIChild()
        {
            ToolStripItems.Add(StatusLabel);
            detachbutton = new ToolStripButton("Detach");
            detachbutton.Click += new EventHandler(detachbutton_Click);
            ToolbarItems.Add(detachbutton);
        }

        void detachbutton_Click(object sender, EventArgs e)
        {
            if (this.MdiParent == null)
                this.Attach(MainForm.FindMainForm());
            else
                this.Detach();
        }

        public void Detach()
        {
            if (this.MdiParent == null)
                return;
            if (menustrip == null)
                menustrip = new MenuStrip();
            if (menu != null)
                menustrip.Items.Add(menu);
            this.Controls.Add(menustrip);
            if (statusBar == null)
                statusBar = new StatusStrip();
            statusBar.Items.AddRange(ToolStripItems.ToArray());
            this.MdiParent = null;
            this.detachbutton.Text = "Re-attach";
            this.ToolbarItems.Add(detachbutton);
            menustrip.Items.Remove(detachbutton);
        }
        public void Attach(Form form)
        {
            if (this.MdiParent != null)
                return;
            this.Controls.Remove(this.menustrip);
            this.Controls.Remove(this.statusBar);
            this.MdiParent = form;
            this.detachbutton.Text = "Detach";
            if (!ToolbarItems.Contains(detachbutton))
                this.ToolbarItems.Add(detachbutton);
        }
    }
}
