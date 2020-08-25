using System;
using System.Collections.Generic;
using AxisVM;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using System.Windows.Forms;
using System.ComponentModel;

namespace GrassHopperToAxisVM.Components
{
    public class GH_mat : GH_Component
    {
        private int counter = 0;

        public TreeView TreeView1 = new TreeView();
        public BackgroundWorker bw1 = new BackgroundWorker();
        public static string globbi = "S 235";
        public GH_mat() : base("AxisVM Materials", "AxMAT", "Component for selecting AxisVM material", "AxisVM", "Attr")
        { }
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            this.bw1.DoWork += Bw1_DoWork; ;
            this.bw1.RunWorkerCompleted += Bw1_RunWorkerCompleted; ;
            this.bw1.WorkerSupportsCancellation = true;
            bw1.RunWorkerAsync();
        }

        private void Bw1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ExpireSolution(true);
        }

        private void Bw1_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = -1;
            AxisVMCatalog catalog = new AxisVMCatalog();
            foreach (ENationalDesignCode suit in (ENationalDesignCode[])Enum.GetValues(typeof(ENationalDesignCode)))
            {
                try
                {
                    Array MatNames;
                    catalog.GetMaterialNames(suit, out MatNames);
                    TreeView1.Nodes.Add(Enum.GetName(typeof(ENationalDesignCode), suit).Remove(0,3)); i++;
                    foreach (string xxx in MatNames)
                    {
                        TreeView1.Nodes[i].Nodes.Add(xxx);
                    }
                }
                catch (Exception ee)
                {
                    continue;
                }
            }
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new MaterialParameter(),"AxisVM Material", "AxMAT", "AxisVM Material", GH_ParamAccess.item);
        }
        public class SettingsComponentAttributes : GH_ComponentAttributes
        {
            public SettingsComponentAttributes(GH_mat cmp) : base(cmp){}
            
            public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
            {
                this.Owner.ExpireSolution(true);
                return GH_ObjectResponse.Handled;
            }
        }

        public override void CreateAttributes()
        {
            Attributes = new SettingsComponentAttributes(this);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (bw1.IsBusy) { }
            else if(counter < 1) { counter++; throw new Exception("Material is not chosen"); }
            else
            {
                Form1 form = new Form1(TreeView1);
                DialogResult res = form.ShowDialog();
                if (res == DialogResult.OK)
                {
                    DA.SetData(0, form.mat);
                }
                else
                {
                    throw new Exception("Material is not chosen");
                }
            }
        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resource1.ic_AxMaterials;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("85a20d72-92f1-42a2-bae7-ca466793a37e"); }
        }
    }
}
