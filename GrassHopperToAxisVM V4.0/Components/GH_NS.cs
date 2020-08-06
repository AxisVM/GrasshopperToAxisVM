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
        public GH_NS() : base("AxisVM NodeSupports", "AxNS", "Component for NodeSupports", "AxisVM", "Base")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Rx", "Rx", "Rx", GH_ParamAccess.item); pManager[0].Optional = true;
            pManager.AddNumberParameter("Ry", "Ry", "Ry", GH_ParamAccess.item); pManager[1].Optional = true;
            pManager.AddNumberParameter("Rz", "Rz", "Rz", GH_ParamAccess.item); pManager[2].Optional = true;
            pManager.AddNumberParameter("Rxx", "Rxx", "Rxx", GH_ParamAccess.item); pManager[3].Optional = true;
            pManager.AddNumberParameter("Ryy", "Ryy", "Ryy", GH_ParamAccess.item); pManager[4].Optional = true;
            pManager.AddNumberParameter("Rzz", "Rzz", "Rzz", GH_ParamAccess.item); pManager[5].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new AxisNodeSupParameter(), "AxisVM NodeSupport", "AxNS", "NodeSupport for AxisVM", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double rx = 0;
            double ry = 0;
            double rz = 0;
            double rxx =0;
            double ryy =0;
            double rzz =0;

            if (!DA.GetData(0, ref rx)) { rx = 0; };
            if (!DA.GetData(1, ref ry)) { ry = 0; };
            if (!DA.GetData(2, ref rz)) { rz = 0; };
            if (!DA.GetData(3, ref rxx)) { rxx = 0; };
            if (!DA.GetData(4, ref ryy)) { ryy = 0; };
            if (!DA.GetData(5, ref rzz)) { rzz = 0; };
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
