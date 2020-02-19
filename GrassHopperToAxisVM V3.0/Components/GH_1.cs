using System;
using System.Collections.Generic;
using AxisVM;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.IO;
using System.Drawing;
using System.Reflection;

namespace GrassHopperToAxisVM
{
    public class GH_1 : GH_Component
    {
        public GH_1() : base("Point to AxisVM Point", "axPoint", "Component for changing points to AxisVM points", "AxisVM", "Base")
        {
          
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "Points to convert", GH_ParamAccess.list);
            pManager.AddParameter(new AxisNodeSupParameter(),"NodeSupport", "NS", "Nodesupport", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new AxisPointParameter(), "AxisVM Point", "AxPoint", "Points with AxisVM parameters", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> points = new List<Point3d>();
            AxisPoint points2 = new AxisPoint();
            AxisNodeSup rp = new AxisNodeSup();
            DA.GetDataList<Point3d>(0, points);

            points2.Value.point = points;
            if (DA.GetData(1, ref rp))
            {
                points2.Value.sup = new structs.AxisNodeSup(rp.Value.Rx, rp.Value.Ry, rp.Value.Rz, rp.Value.Rxx, rp.Value.Ryy, rp.Value.Rzz);
            }
            else
            {
                points2.Value.sup = new structs.AxisNodeSup(-1, -1, -1, -1, -1, -1);
            }
            DA.SetData(0,points2);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resource1.ic_AxPoint;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("6490d1ea-8d92-4d29-bb28-1bf19f83ef86"); }
        }
    }
}
// In this version AxisPoints only store information about the coordinates of Rhino points.
// Using BulkAdd for the PointList