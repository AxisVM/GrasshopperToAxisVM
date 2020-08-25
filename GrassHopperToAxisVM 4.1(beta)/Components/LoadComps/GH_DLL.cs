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
    public class GH_DLL : GH_Component
    {
        public GH_DLL() : base("AxisVM Distributed Line Load", "AxDLL", "Component for Distributed Line Loads", "AxisVM", "Load")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new AxisLineParameter(), "AxLine", "AxL", "AxisVM lines", GH_ParamAccess.item); pManager[0].Optional = true;
            pManager.AddParameter(new AxisMeshParameter(), "AxMesh", "AxM_E", "AxisVM meshes as edges", GH_ParamAccess.item); pManager[1].Optional = true;
            pManager.AddNumberParameter("Type", "G/L", "Global=0; Local=1", GH_ParamAccess.item); pManager[2].Optional = true;
            pManager.AddTextParameter("Direction", "Dir", "X or Y or Z", GH_ParamAccess.item); pManager[3].Optional = true;
            pManager.AddNumberParameter("pos1", "Pos1", "Start position[0;1]", GH_ParamAccess.item); pManager[4].Optional = true;
            pManager.AddNumberParameter("pos2", "Pos2", "End Position[0;1]", GH_ParamAccess.item); pManager[5].Optional = true;
            pManager.AddNumberParameter("int1", "Int1", "Start intensity", GH_ParamAccess.item); pManager[6].Optional = true;
            pManager.AddNumberParameter("int2", "Int2", "End intensity", GH_ParamAccess.item); pManager[7].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new AxDistributedLineLoadParameter(), "Distributed Line Load", "DLL", "Distributed Line Loads", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            AxisLine point = new AxisLine();
            AxisMesh edges = new AxisMesh();
            double typ0 = 0; Structs.AxDistributedLineLoad.type typ = Structs.AxDistributedLineLoad.type.global;
            string dir0 = "X"; Structs.AxDistributedLineLoad.direction dir = Structs.AxDistributedLineLoad.direction.X;
            double pos1 = 0;
            double pos2 = 1;
            double int1 = 0;
            double int2 = 0;
            if (!DA.GetData(0, ref point) && !DA.GetData(1, ref edges)) { DA.AbortComponentSolution(); };
            if (!DA.GetData(2, ref typ0)) { typ0 = 0; };
            if (!DA.GetData(3, ref dir0)) { dir0 = "X"; };
            if (!DA.GetData(4, ref pos1)) { pos1 = 0; };
            if (!DA.GetData(5, ref pos2)) { pos2 = 1; };
            if (!DA.GetData(6, ref int1)) { int1 = 0; };
            if (!DA.GetData(7, ref int2)) { int2 = 0; };
            if (dir0 == "Y") { dir = Structs.AxDistributedLineLoad.direction.Y; }
            else if(dir0 == "Z") { dir = Structs.AxDistributedLineLoad.direction.Z; }

            if(typ0 == 1) { typ = Structs.AxDistributedLineLoad.type.local; }
            AxDistributedLineLoad returner = new AxDistributedLineLoad(point.Value, edges.Value, typ, dir, pos1, pos2, int1, int2 );
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
                return Properties.Resource1.ic_AxLoadLine;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("925e415b-d4b4-4d4a-ac32-2efcf8258ab8"); }
        }
    }
}
/*
 
     SUMMARY:
     SHOULD BE REMASTERED ASAP
     
     
     */
