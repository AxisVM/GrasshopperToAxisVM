using System;
using System.Drawing;
using System.Windows.Forms;
using AxisVM;

namespace GrassHopperToAxisVM
{
    public partial class Form2 : Form
    {
        public CrossSection crs = new CrossSection(); 
        private Button button1;
        private TreeView treeView1;
        private Label label1;
        public AxisVMCatalog catalog = new AxisVMCatalog();
        public Form2(TreeView TTreeView1)
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
            try { crs.Value.name =  treeView1.SelectedNode.Text;
                 crs.Value.shape = "css"+treeView1.SelectedNode.Parent.Parent.Text.Remove(treeView1.SelectedNode.Parent.Parent.Text.Length-6,6);
            } catch (Exception ee) {  }
        }
        private void TreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Node.Level == 2)
            {
                button1.Enabled = true;
                button1.DialogResult = DialogResult.OK;
            }
            else if(e.Node.Level == 0)
            {
                button1.Enabled = false;
                if (e.Node.Nodes.Count != 0) { return; }
                try
                {
                    Array EnNames;
                    int xxxyy = catalog.GetCrossSectionTableNames((ECrossSectionShape)Enum.Parse(typeof(ECrossSectionShape), e.Node.Text.Remove(e.Node.Text.Length - 6, 6).Insert(0,"css")), out EnNames);
                    try
                    {
                        if (EnNames.Length == 0 || EnNames == null)
                        {

                        }
                        else
                        {
                            foreach (string xxx in EnNames)
                            {
                                try
                                {
                                    e.Node.Nodes.Add(xxx);
                                }
                                catch (Exception eee)
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    catch (Exception error)
                    {
                    }
                }
                catch { }
            }
            else if(e.Node.Level == 1)
            {
                button1.Enabled = false;
                if (e.Node.Nodes.Count != 0) { return; }
                Array SecNames;
                catalog.GetCrossSectionNames((ECrossSectionShape)Enum.Parse(typeof(ECrossSectionShape), e.Node.Parent.Text.Remove(e.Node.Parent.Text.Length - 6, 6).Insert(0, "css")), e.Node.Text, out SecNames);
                foreach (string xxxx in SecNames)
                {
                    try
                    {
                        e.Node.Nodes.Add(xxxx);
                    }
                    catch (Exception eeee)
                    {
                        continue;
                    }
                }
            }
        }

        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(322, 226);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(5, 6);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(310, 250);
            this.treeView1.TabIndex = 1;
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView1_NodeMouseClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(322, 210);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select Crosssection";
            // 
            // Form2
            // 
            this.ClientSize = new System.Drawing.Size(434, 261);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.Text = "CrossSection Selector";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(211, 228, 241);
        }
    }
}
