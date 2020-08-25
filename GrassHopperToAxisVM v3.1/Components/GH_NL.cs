﻿using System;
using System.Collections.Generic;
using AxisVM;
using Grasshopper.Kernel;
using Rhino.Geometry.Collections;
using Rhino.Geometry;
using System.Linq;
using System.ComponentModel;

namespace GrassHopperToAxisVM
{
    public class GH_NL : GH_Component
    {
        public GH_NL() : base("NodalLoads", "axNS", "Component for Nodal Loads", "AxisVM", "Base")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Rx", "Rx", "Rx", GH_ParamAccess.item); pManager[0].Optional = true;
            pManager.AddNumberParameter("Ry", "Ry", "Ry", GH_ParamAccess.item); pManager[1].Optional = true;
            pManager.AddNumberParameter("Rz", "Rz", "Rz", GH_ParamAccess.item); pManager[2].Optional = true;
            pManager.AddNumberParameter("Mx", "Mx", "Mx", GH_ParamAccess.item); pManager[3].Optional = true;
            pManager.AddNumberParameter("My", "My", "My", GH_ParamAccess.item); pManager[4].Optional = true;
            pManager.AddNumberParameter("Mz", "Mz", "Mz", GH_ParamAccess.item); pManager[5].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new AxNodalLoadParameter(), "Nodal Loads", "NL", "Nodal Loads for AxisVM", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double rx = 0;
            double ry = 0;
            double rz = 0;
            double Mx = 0;
            double My = 0;
            double Mz = 0;

            if (!DA.GetData(0, ref rx)) { rx = 0; };
            if (!DA.GetData(1, ref ry)) { ry = 0; };
            if (!DA.GetData(2, ref rz)) { rz = 0; };
            if (!DA.GetData(3, ref Mx)) { Mx = 0; };
            if (!DA.GetData(4, ref My)) { My = 0; };
            if (!DA.GetData(5, ref Mz)) { Mz = 0; };
            AxNodalLoad returner = new AxNodalLoad(rx, ry, rz, Mx, My, Mz);
            //returner.Value = new structs.AxNodalLoad(rx, ry, rz, Mx, My, Mz);
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
            get { return new Guid("3745fa5a-60ba-4204-bb2e-0bc2bcec48c0"); }
        }
    }
}