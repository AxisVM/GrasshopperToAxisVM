using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Rhino.Geometry;


namespace GrassHopperToAxisVM
{
    public class GH_2 : GH_Component
    {
        public GH_2() : base("Line to AxisVM Line", "AxLine", "Component for changing lines to AxisVM lines", "AxisVM", "Base")
        {

        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddLineParameter("Lines", "L", "Lines to convert", GH_ParamAccess.list);
            pManager.AddParameter(new MaterialParameter(), "AxisVM Material", "AxMAT", "Material", GH_ParamAccess.item);
            pManager.AddParameter(new CrossSectionParameter(),"AxisVM Cross Section", "AxCRS", "Cross Section", GH_ParamAccess.item);
            pManager.AddIntegerParameter("AxisVM Beam / Rib / Truss", "B/R/T", "0=Beam;1=Rib;2=Truss", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new AxisLineParameter(), "AxisVM Line", "AxLine", "Lines with AxisVM parameters", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Line> lines = new List<Line>();
            Material material = null;
            CrossSection crossection = null;
            int num = 0;
            AxisLine axlines = new AxisLine();
            DA.GetDataList<Line>(0, lines);
            if (!DA.GetData(1, ref material)) { }
            if (!DA.GetData(2, ref crossection)) { }
            if (!DA.GetData(3, ref num)) { }
        
            axlines = new AxisLine(lines, new Material(material).Value, new CrossSection(crossection).Value,num);
            //axlines.Value.deleting = true;
            //axlines.Value.drawing = true;
            DA.SetData(0, axlines);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resource1.ic_AxMember;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("d842be29-e008-4a95-b3d8-5b6614041f1a"); }
        }
    }
}
