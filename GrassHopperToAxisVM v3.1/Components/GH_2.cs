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
        public GH_2() : base("Line to AxisVM Line", "axLine", "Component for changing lines to AxisVM lines", "AxisVM", "Base")
        {

        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddLineParameter("Lines", "L", "Lines to convert", GH_ParamAccess.list);
            pManager.AddTextParameter("Material", "AxMat", "Material", GH_ParamAccess.item);
            pManager.AddTextParameter("Cross Section", "AxCrs", "Cross Section", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Beam / Rib / Truss", "B/R/T", "0=Beam;1=Rib;2=Truss", GH_ParamAccess.item);
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
            string material = null;
            string crossection = null;
            int num = 0;
            AxisLine axlines = new AxisLine();
            DA.GetDataList<Line>(0, lines);
            if (DA.GetData(1, ref material) && DA.GetData(2, ref crossection) && DA.GetData(3, ref num))
            {
                axlines = new AxisLine(lines, material, crossection,num);
            }
            else if (DA.GetData(1, ref material) && DA.GetData(2, ref crossection))
            {
                axlines = new AxisLine(lines, material, crossection);
            }
            else
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    axlines = new AxisLine(lines);
                }
            }
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
// Input: Linelist; material; crs
//Output AxLines as one item [object which contains list]