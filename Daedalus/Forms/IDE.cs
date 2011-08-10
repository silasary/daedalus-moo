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
    public partial class IDE : Form
    {
        string reference;
        string name;
        string mode;
        string datatag;

        List<MOO.MOOObject> Objects = new List<Daedalus.MOO.MOOObject>();
        Dictionary<int, TreeNode> ObjectTree = new Dictionary<int, TreeNode>();

        public IDE()
        {
            InitializeComponent();
        }

        public IDE(string reference, string name, string mode, string datatag)
            : this()
        {
            this.name = name;
            this.reference = reference;
            this.datatag = datatag;
            SetMode(mode);
        }

        private void SetMode(string mode)
        {
            this.mode = mode;
            this.splitContainer1.Panel2Collapsed = true;
            if (mode == "moo-code")
            {
                // Syntax Hiliter?
            }
            if (mode == "Full-IDE")
            {
                this.splitContainer1.Panel2Collapsed = false;
                this.Objects.Add(new MOO.MOOObject(reference.Trim(' ', '"')));
                this.propertyGrid1.PropertyTabs.AddTabType(typeof(MOO.MOOPropertyTab), PropertyTabScope.Static);
                BuildTreeView();
            }
        }

        public void BuildTreeView()
        {
            treeView1.Nodes.Clear();
            foreach (MOO.MOOObject obj in Objects)
            {
                TreeNode node;
                if (!(ObjectTree.ContainsKey(obj.id)))
                {
                    node = new TreeNode();
                    node.Tag = obj;
                    ObjectTree.Add(obj.id, node);
                }
                else
                    node = ObjectTree[obj.id];
                node.Text = obj["name"] as string;
                if (node.Text == "")
                    node.Text = obj.ToString();
                if (obj.Parent != null && (node.Parent == null || node.Parent.Tag != obj.Parent))
                {
                    if (ObjectTree.ContainsKey(obj.Parent.id))
                        ObjectTree[obj.Parent.id].Nodes.Add(node);
                }
            }
            TreeNode topmost = ObjectTree[ObjectTree.Keys.First()];
            while (topmost == null || topmost.Parent != null)
                    topmost = topmost.Parent;
            treeView1.Nodes.Add(topmost);
        }

        public MOO.MOOObject FindObject(string obj)
        {
            return FindObject(int.Parse(obj.Substring(1)));
        }
        public MOO.MOOObject FindObject(int id)
        {
            MOO.MOOObject obj = Objects.FirstOrDefault(o => o.id == id);
            if (obj == null)
            {
                Objects.Add(obj = new MOO.MOOObject(id));
            } 
            return obj;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid1.SelectedObject = e.Node.Tag;
        }

        private void IDE_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MCP.Packages.KamahlIDE._idepackage.IDEs.Remove(datatag);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
         
        }
    }
}
