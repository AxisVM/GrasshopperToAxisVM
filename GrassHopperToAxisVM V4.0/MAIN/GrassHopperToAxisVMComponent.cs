using System;
using AxisVM;
using Grasshopper.Kernel;
using System.Collections.Generic;

namespace GrassHopperToAxisVM
{
    public partial class GrassHopperToAxisVMComponent : GH_Component
    {
        //public static Structs.TriggerData triggerData = new Structs.TriggerData(false);

        private bool sendOnlyChanges = false;
        private bool ModellSet = false;
        private AxisVMApplication AxApp = null;
        private AxisVMModels AxModels = null;
        private AxisVMModel AxModel = null;
        private AxisVMNodes AxNodes = null;
        private AxisVMLines AxLines = null;
        private AxisVMVirtualBeams axBeams = null;
        private AxisVMLine AxLine = null;
        private ELineGeomType geomType = new ELineGeomType();
        private RLineGeomData geomData = new RLineGeomData();
        private AxisVMCrossSections crsec = null;
        private AxisVMMaterials mats = null;
        private AxisVMMembers members = null;
        private AxisVMCatalog catalog = null;
        private AxisVMLoads axloads = null;
        private AxisVMLoadCases axcloads = null;
        private AxisVMNodesSupports axnsupp = null;
        private AxisVMNodalSupports axNodalsupport = null;
        private AxisVMSurfaces axisSurfaces = null;
        private AxisVMDomains axDomains = null;
        private AxisVMCalculation axiscalc = null;
        private AxisVMWindows axwin = null;


        private List<Structs.AxisMember> points_sent = new List<Structs.AxisMember>();
        private List<Structs.AxisMember> lines_sent = new List<Structs.AxisMember>();
        private List<Structs.AxisMember> surfaces_sent = new List<Structs.AxisMember>();
        private List<Structs.AxisMember> domains_sent = new List<Structs.AxisMember>();
        private List<Structs.AxisMember> edges_sent = new List<Structs.AxisMember>();
        private List<Structs.AxLoadCase> lc_sent = new List<Structs.AxLoadCase>();
        private List<Structs.AxisMember> domain2s_sent = new List<Structs.AxisMember>();
        
        private List<Structs.AxisMember> points_got = new List<Structs.AxisMember>();
        private List<Structs.AxisMember> lines_got = new List<Structs.AxisMember>();
        private List<Structs.AxisMember> surfaces_got = new List<Structs.AxisMember>();
        private List<Structs.AxisMember> domains_got = new List<Structs.AxisMember>();
        private List<Structs.AxisMember> edges_got = new List<Structs.AxisMember>();
        private List<Structs.AxisMember> domain2s_got = new List<Structs.AxisMember>();

        public GrassHopperToAxisVMComponent(): base("GrasshopperToAxisVM", "AxisVM", "Component for exporting elements to AxisVM", "AxisVM", "Send")
        {
            
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new AxisPointParameter(),"AxisVM Point", "AxP", "Points with AxisVM parameters", GH_ParamAccess.list);
            pManager.AddParameter(new AxisLineParameter(),"AxisVM Line", "AxL", "Lines with AxisVM parameters", GH_ParamAccess.list);
            pManager.AddParameter(new AxisMeshParameter(),"AxisVM Surface", "AxM_S", "AxisVM Meshes to AxisVM Surfaces", GH_ParamAccess.list);
            pManager.AddParameter(new AxisMeshParameter(), "AxisVM Domain", "AxM_D", "AxisVM Meshes to AxisVM Domains ", GH_ParamAccess.list);
            pManager.AddParameter(new AxisMeshParameter(), "AxisVM Edges", "AxM_E", "AxisVM Meshs' edges to AxisVM Lines / Beams", GH_ParamAccess.list);
            pManager.AddParameter(new AxisDomainParameter(), "AxisVM Domain", "AxD", "AxisVM Domains to AxisVM Domains", GH_ParamAccess.list);
            pManager.AddParameter(new AxLoadCaseParameter(), "AxisVM Load Cases", "AxLC", "Load Cases for model", GH_ParamAccess.list);
            pManager.AddParameter(new AxisDataParameter(), "AxisVM Settings", "AxStt", "Settings for AxisVMModel", GH_ParamAccess.item);
            pManager[0].Optional = true; 
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
            pManager[6].Optional = true;
            // 7 is neccessary
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
