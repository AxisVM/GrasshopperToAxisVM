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
    public class GH_DDL : GH_Component
    {
        public GH_DDL() : base("AxisVM Distributed Domain Load", "AxDDL", "Component for creating Distributed Domain Loads", "AxisVM", "Load")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new AxisMeshParameter(), "AxMesh", "AxM_D", "AxisVM meshes as Domains", GH_ParamAccess.item); pManager[0].Optional = true;
            pManager.AddParameter(new AxisDomainParameter(), "AxDomain", "AxD", "AxisVM Domains", GH_ParamAccess.item); pManager[1].Optional = true;
            pManager.AddNumberParameter("Type", "G/L", "Global=0; Local=1", GH_ParamAccess.item); pManager[2].Optional = true;
            pManager.AddTextParameter("Direction", "Dir", "X or Y or Z", GH_ParamAccess.item); pManager[3].Optional = true;
            pManager.AddNumberParameter("intensity", "Int", "intensity", GH_ParamAccess.item); pManager[4].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new AxDistributedDomainLoadParameter(), "AxisVM Distributed Domain Load", "AxDDL", "Distributed Domain Load for AxisVM", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            AxisMesh mesh = new AxisMesh();
            AxisDomain doma = new AxisDomain();
            double typ0 = 0; Structs.AxDistributedDomainLoad.type typ = Structs.AxDistributedDomainLoad.type.global;
            string dir0 = "X"; Structs.AxDistributedDomainLoad.direction dir = Structs.AxDistributedDomainLoad.direction.X;
            double intensity = 0;
            if (!DA.GetData(0, ref mesh)&&!DA.GetData(1, ref doma)) { DA.AbortComponentSolution(); };
            if (!DA.GetData(2, ref typ0)) { typ0 = 0; };
            if (!DA.GetData(3, ref dir0)) { dir0 = "X"; };
            if (!DA.GetData(4, ref intensity)) { intensity = 0; };

            if (dir0 == "Y") { dir = Structs.AxDistributedDomainLoad.direction.Y; }
            else if (dir0 == "Z") { dir = Structs.AxDistributedDomainLoad.direction.Z; }

            if (typ0 == 1) { typ = Structs.AxDistributedDomainLoad.type.local; }
            AxDistributedDomainLoad returner = new AxDistributedDomainLoad(mesh.Value, doma.Value, typ, dir, intensity);
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
                return Properties.Resource1.ic_AxLoadDistDomain;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("a5f7388b-a9fc-479b-8037-7c737ac2ba31"); }
        }
    }
}
/*
 
     SUMMARY:
     SHOULD BE REMASTERED ASAP
     
     
     */

