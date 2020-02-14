using System;
using System.Windows.Forms;
using System.Drawing;

namespace GrassHopperToAxisVM
{
    public partial class Form1 : Form
    {
        public string material_t = null;
        public string material = null;
        public Form1(TreeView TTreeView1)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            foreach (TreeNode node in TTreeView1.Nodes)
            {
                treeView1.Nodes.Add((TreeNode)node.Clone());
            }
            button1.Enabled = false;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            try { material = treeView1.SelectedNode.Text;
                material_t = "ndc"+treeView1.SelectedNode.Parent.Text;
            } catch (Exception ee) { }
        }

        private void TreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 1)
            {
                button1.Enabled = true;
                button1.DialogResult = DialogResult.OK;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(211, 228, 241);
        }
    }
}
