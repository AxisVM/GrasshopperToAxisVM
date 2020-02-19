using System;
using AxisVM;
using Grasshopper.Kernel;
using System.Windows.Forms;
using Grasshopper.Kernel.Attributes;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using System.IO;
using System.ComponentModel;

namespace GrassHopperToAxisVM.Components
{
    public class GH_sec : GH_Component
    {
        public BackgroundWorker bw1 = new BackgroundWorker();
        public TreeView TreeView1 = new TreeView();
        public string crs = null;
        public GH_sec() : base("Cross Sections", "axCrs", "Component for selecting Cross Section", "AxisVM", "Attr")
        {
        
        }
        private void Bw1_DoWork(object sender, DoWorkEventArgs e)
        {
            AxisVMCatalog catalog = new AxisVMCatalog();
            int i = -1;
            foreach (ECrossSectionShape suit in (ECrossSectionShape[])Enum.GetValues(typeof(ECrossSectionShape)))
            {
                try
                {
                    Array EnNames;
                    catalog.GetCrossSectionTableNames((ECrossSectionShape)suit, out EnNames);
                    try {
                        if (EnNames.Length == 0 || EnNames == null)
                        {

                        }
                        else
                        {
                            TreeView1.Nodes.Add(Enum.GetName(typeof(ECrossSectionShape), suit).Remove(0,3)+"-Shape"); i++;
                        }
                    }
                    catch (Exception error)
                    {
                    }
                    int c = -1;
                    foreach (string xxx in EnNames)
                    {
                        try
                        {
                            TreeView1.Nodes[i].Nodes.Add(xxx); c++;
                            Array SecNames;
                            catalog.GetCrossSectionNames((ECrossSectionShape)suit, xxx.ToString(), out SecNames);
                            foreach (string xxxx in SecNames)
                            {
                                try
                                {
                                    TreeView1.Nodes[i].Nodes[c].Nodes.Add(xxxx); 
                                }
                                catch (Exception eeee)
                                {
                                    continue;
                                }
                            }
                            
                        }
                        catch (Exception eee)
                        {
                            continue;
                        }
                    }
                }
                catch (Exception ee)
                {
                    continue;
                }
            }
        }
        private void Bw1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ExpireSolution(true);
        }
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            this.bw1.DoWork += Bw1_DoWork;
            this.bw1.RunWorkerCompleted += Bw1_RunWorkerCompleted;
            this.bw1.WorkerSupportsCancellation = true;
            bw1.RunWorkerAsync();
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Crosssections", "AxCrs", "Crosssections", GH_ParamAccess.item);
        }
        public class SettingsComponentAttributes : GH_ComponentAttributes
        {
            public SettingsComponentAttributes(GH_sec cmp) : base(cmp) { }

            public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
            {
                this.Owner.ExpireSolution(true);
                return GH_ObjectResponse.Handled;
            }
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (bw1.IsBusy) { }
            else
            {
                Form2 form = new Form2(TreeView1);
                DialogResult res = form.ShowDialog();
                if (res == DialogResult.OK)
                {
                    DA.SetData(0, Convert.ToString(form.crs_t) + "}" + form.crs);
                }
                else
                {
                    DA.AbortComponentSolution();
                }
            }
        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }
        public override void CreateAttributes()
        {
            Attributes = new SettingsComponentAttributes(this);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resource1.ic_AxCrossSections;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("6ee846d3-a2d4-4536-bede-b4a7e1039bd6"); }
        }
    }
}
