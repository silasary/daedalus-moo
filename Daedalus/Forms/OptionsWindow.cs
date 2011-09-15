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
    public partial class OptionsWindow : Form
    {
        public OptionsWindow()
        {
            InitializeComponent();
            tabControl1.TabPages.AddRange(PluginModel.PluginLoader.OptionsPages);
        }

        private void OptionsWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
