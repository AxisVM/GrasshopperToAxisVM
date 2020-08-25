using System;
using AxisVM;
using Grasshopper.Kernel;
using System.Windows.Forms;
using Grasshopper.Kernel.Attributes;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;

namespace GrassHopperToAxisVM.Components
{
    public class GH_sec : GH_Component
    {
        private int counter = 0;

        public BackgroundWorker bw1 = new BackgroundWorker();
        public TreeView TreeView1 = new TreeView();
        public string crs = null;
        public GH_sec() : base("AxisVM Cross Sections", "AxCRS", "Component for selecting Cross Section", "AxisVM", "Attr")
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
                    //catalog.GetCrossSectionTableNames((ECrossSectionShape)suit, out EnNames);
                    try
                    {
                       // if (EnNames.Length == 0 || EnNames == null){}
                        //else
                        TreeView1.Nodes.Add(Enum.GetName(typeof(ECrossSectionShape), suit).Remove(0,3)+"-Shape"); i++;
                    }
                    catch (Exception error)
                    {
                    }
                    /*int c = -1;
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
                    }*/
                }
                catch (Exception ee)
                {
                    continue;
                }
            }
            
            /*List<RTableCrossSectionID> all_crs = new List<RTableCrossSectionID>();
            AxisVMCrossSectionTables tables;
            int err = catalog.GetAllTables(out tables);
            for (int i = 1; i <= tables.Count; i++)
            {
                TreeView1.Nodes.Add(tables.Item[i].CrossSectionShape.ToString());
                int cc = 4;
                AxisVMCrossSectionTable crs = new AxisVMCrossSectionTable();
                int pi = catalog.GetTableCrossSections(tables.Item[i].Id, out crs);
                for(int j = 1; j <= crs.Count; j++)
                {
                    all_crs.Add(crs.Item[j]);
                    TreeView1.Nodes[i-1].Nodes.Add(crs.Item[j].CrossSectionID.ToString());
                }
            }*/
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
            pManager.AddParameter(new CrossSectionParameter(),"AxisVM Crosssections", "AxCRS", "AxisVM Crosssections", GH_ParamAccess.item);
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
            else if (counter < 1) { counter++; throw new Exception("CrossSection is not chosen"); }
            else
            {
                Form2 form = new Form2(TreeView1);
                DialogResult res = form.ShowDialog();
                if (res == DialogResult.OK)
                {
                    DA.SetData(0, form.crs);
                }
                else
                {
                    throw new Exception("CrossSection is not chosen");
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
            get { return new Guid("f95d1e6f-f6f9-4004-b45a-c2ebb85011e3"); }
        }
    }
}
