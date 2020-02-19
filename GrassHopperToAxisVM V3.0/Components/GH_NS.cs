using System;
using System.Collections.Generic;
using AxisVM;
using Grasshopper.Kernel;
using Rhino.Geometry.Collections;
using Rhino.Geometry;
using System.Linq;
using System.ComponentModel;

namespace GrassHopperToAxisVM
{
    public class GH_NS : GH_Component
    {
        public GH_NS() : base("NodeSupports", "axNS", "Component for NodeSupports", "AxisVM", "Base")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Rx", "Rx", "Rx", GH_ParamAccess.item);
            pManager.AddNumberParameter("Ry", "Ry", "Ry", GH_ParamAccess.item);
            pManager.AddNumberParameter("Rz", "Rz", "Rz", GH_ParamAccess.item);
            pManager.AddNumberParameter("Rxx", "Rxx", "Rxx", GH_ParamAccess.item);
            pManager.AddNumberParameter("Ryy", "Ryy", "Ryy", GH_ParamAccess.item);
            pManager.AddNumberParameter("Rzz", "Rzz", "Rzz", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new AxisNodeSupParameter(), "NodeSupport", "NS", "NodeSupport for AxisVM", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double rx = -1;
            double ry = -1;
            double rz = -1;
            double rxx =-10;
            double ryy =-10;
            double rzz =-10;
            
            DA.GetData(0, ref rx); DA.GetData(1, ref ry); DA.GetData(2, ref rz);
            DA.GetData(3, ref rxx); DA.GetData(4, ref ryy); DA.GetData(5, ref rzz);
            AxisNodeSup returner = new AxisNodeSup(rx, ry, rz, rxx, ryy, rzz);
            //returner.Value = new structs.AxisNodeSup(rx, ry, rz, rxx, ryy, rzz);
            DA.SetData(0, returner);
        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resource1.icFixedSupport;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("63cfefc2-150a-4b3c-ae37-eb98c0a01468"); }
        }
    }
}
