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
    public class GH_DSL : GH_Component
    {
        public GH_DSL() : base("AxisVM Distributed Surface Load", "AxDSL", "Component for Distributed Surface Loads", "AxisVM", "Load")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new AxisMeshParameter(), "AxMesh", "AxM_S", "AxisVM meshes as Surfaces", GH_ParamAccess.item); 
            pManager.AddNumberParameter("Type", "G/L", "Global=0; Local=1", GH_ParamAccess.item); pManager[1].Optional = true;
            pManager.AddTextParameter("Direction", "Dir", "X or Y or Z", GH_ParamAccess.item); pManager[2].Optional = true;
            pManager.AddNumberParameter("intensity", "Int", "intensity", GH_ParamAccess.item); pManager[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new AxDistributedSurfaceLoadParameter(), "AxisVM Distributed Surface Load", "AxDSL", "Distributed Surface Load for AxisVM", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            AxisMesh mesh = new AxisMesh();
            double typ0 = 0; Structs.AxDistributedSurfaceLoad.type typ = Structs.AxDistributedSurfaceLoad.type.global;
            string dir0 = "X"; Structs.AxDistributedSurfaceLoad.direction dir = Structs.AxDistributedSurfaceLoad.direction.X;
            double intensity = 0;
            if (!DA.GetData(0, ref mesh)) { DA.AbortComponentSolution(); };
            if (!DA.GetData(1, ref typ0)) { typ0 = 0; };
            if (!DA.GetData(2, ref dir0)) { dir0 = "X"; };
            if (!DA.GetData(3, ref intensity)) { intensity = 0; };

            if (dir0 == "Y") { dir = Structs.AxDistributedSurfaceLoad.direction.Y; }
            else if (dir0 == "Z") { dir = Structs.AxDistributedSurfaceLoad.direction.Z; }

            if (typ0 == 1) { typ = Structs.AxDistributedSurfaceLoad.type.local; }
            AxDistributedSurfaceLoad returner = new AxDistributedSurfaceLoad(mesh.Value, typ, dir, intensity);
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
                return Properties.Resource1.ic_AxLoadDistSurface;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("dfcc23f4-2105-49c6-a204-41e5cb1c811c"); }
        }
    }
}
/*
 
     SUMMARY:
     SHOULD BE REMASTERED ASAP
     
     
     */
