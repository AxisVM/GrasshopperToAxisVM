using System;
using AxisVM;
using Grasshopper.Kernel;

namespace GrassHopperToAxisVM
{
    public partial class GrassHopperToAxisVMComponent : GH_Component
    {
        public AxisVMApplicationClass AxApp = new AxisVMApplicationClass();
        public AxisVMModels AxModels = new AxisVMModels();
        public AxisVMModel AxModel = new AxisVMModel();
        public AxisVMNodes AxNodes = new AxisVMNodes();
        public AxisVMLines AxLines = null;
        public AxisVMVirtualBeams axBeams = null;
        public AxisVMLine AxLine = new AxisVMLine();
        public ELineGeomType geomType = new ELineGeomType();
        public RLineGeomData geomData = new RLineGeomData();
        public AxisVMCrossSections crsec = new AxisVMCrossSections();
        public AxisVMMaterials mats = new AxisVMMaterials();
        public AxisVMMembers members = new AxisVMMembers();
        public AxisVMCatalog catalog = new AxisVMCatalog();
        public AxisVMLoads axloads = new AxisVMLoads();
        public AxisVMLoadCases axcloads = new AxisVMLoadCases();
        public AxisVMNodesSupports axnsupp = new AxisVMNodesSupports();
        public AxisVMNodalSupports axNodalsupport = new AxisVMNodalSupports();
        public AxisVMSurfaces axisSurfaces = new AxisVMSurfaces();
        public AxisVMDomains axDomains = new AxisVMDomains();
        public AxisVMCalculation axiscalc = new AxisVMCalculation();
        public AxisVMWindows axwin = new AxisVMWindows();
        public GrassHopperToAxisVMComponent(): base("GrasshopperToAxisVM", "AxisVM", "Component for exporting elements to AxisVM", "AxisVM", "Send")
        {
            AxApp.CloseOnLastReleased = ELongBoolean.lbFalse;
            AxApp.AskCloseOnLastReleased = ELongBoolean.lbTrue; // ez True volt
            AxApp.Visible = ELongBoolean.lbFalse;
            while (AxApp.Loaded != ELongBoolean.lbTrue) { }
            AxModels = AxApp.Models;
            AxModel = AxModels.Item[AxModels.New()];
            AxNodes = AxModel.Nodes;
            AxLines = AxModel.Lines;
            axBeams = AxModel.VirtualBeams;
            members = AxModel.Members;
            mats = AxModel.Materials;
            crsec = AxModel.CrossSections;
            axloads = AxModel.Loads;
            axcloads = AxModel.LoadCases;
            axNodalsupport = AxModel.NodalSupports;
            axisSurfaces = AxModel.Surfaces;
            axiscalc = AxModel.Calculation;
            axwin = AxModel.Windows;
            axDomains = AxModel.Domains;
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new AxisPointParameter(),"AxisVM Point", "AxPoint", "Points with AxisVM parameters", GH_ParamAccess.list);
            pManager.AddParameter(new AxisLineParameter(),"AxisVM Line", "AxLine", "Lines with AxisVM parameters", GH_ParamAccess.list);
            pManager.AddParameter(new AxisMeshParameter(),"AxisVM Surface", "AxSurf", "AxisVM Meshes to AxisVM Surfaces", GH_ParamAccess.list);
            pManager.AddParameter(new AxisMeshParameter(), "AxisVM Domain", "AxDom", "AxisVM Meshes to AxisVM Domains ", GH_ParamAccess.list);
            pManager.AddParameter(new AxisMeshParameter(), "AxisVM Edges", "AxEdge", "AxisVM Meshs' edges to AxisVM Lines / Beams", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            this.bw1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw1_DoWork);
            this.bw1.RunWorkerCompleted += Bw1_RunWorkerCompleted;
            this.bw1.WorkerSupportsCancellation = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        { }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resource1.ic_AxModel;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("7028a0c3-0f30-4aae-bfff-91dc41f64006"); }
        }
    }
}
