using System.Collections.Generic;
using AxisVM;
using Grasshopper.Kernel;
using System.ComponentModel;

namespace GrassHopperToAxisVM
{
    public partial class GrassHopperToAxisVMComponent : GH_Component
    {
        BackgroundWorker bw1 = new BackgroundWorker();
        IGH_DataAccess arg = null;
        public void bw1_DoWork(object sender, DoWorkEventArgs e)
        {
            IGH_DataAccess DA = (IGH_DataAccess) e.Argument;
            AxModel.BeginUpdate();
            AxNodes.SelectAll(ELongBoolean.lbTrue);
            AxNodes.DeleteSelected();
            List<AxisPoint> ponts = new List<AxisPoint>();
            List<AxisLine> lines = new List<AxisLine>();
            List<AxisMesh> meshs = new List<AxisMesh>();
            List<AxisMesh> doms = new List<AxisMesh>();
            List<AxisMesh> edges = new List<AxisMesh>();
            if (DA.GetDataList(0, ponts)) { drawPonts(ponts); }
            if (DA.GetDataList(1, lines)) { drawLines(lines); }
            if (DA.GetDataList(2, meshs)) { drawMeshs(meshs); }
            if (DA.GetDataList(3, doms)) { drawMeshs2(doms); }
            if (DA.GetDataList(4, edges)) { drawEdges(edges); }
            AxModel.EndUpdate();
            AxApp.Visible = ELongBoolean.lbTrue;
        }
        private void Bw1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (arg != null) { bw1.RunWorkerAsync(arg); arg = null; }
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (bw1.IsBusy == false)
            {
                bw1.RunWorkerAsync(DA);
                arg = null;
            }
            else
            {
                arg = DA;
            }
        }
    }
}