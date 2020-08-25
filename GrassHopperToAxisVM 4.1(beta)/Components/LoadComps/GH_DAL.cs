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
    public class GH_DAL : GH_Component
    {
        public GH_DAL() : base("AxisVM Domain Area Load", "AxDAL", "Component for creating Domain Area Load", "AxisVM", "Load")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new AxisMeshParameter(), "AxMesh", "AxM_D", "AxisVM meshes as Domains", GH_ParamAccess.item); pManager[0].Optional = true;
            pManager.AddParameter(new AxisDomainParameter(), "AxDom", "AxD", "AxisVM Domains", GH_ParamAccess.item); pManager[1].Optional = true;
            pManager.AddNumberParameter("Type", "G/L", "Global=0; Local=1", GH_ParamAccess.item); pManager[2].Optional = true;
            pManager.AddTextParameter("Direction", "Dir", "X or Y or Z", GH_ParamAccess.item); pManager[3].Optional = true;
            pManager.AddNumberParameter("intensity", "Int", "intensity", GH_ParamAccess.item); pManager[4].Optional = true;
            pManager.AddCurveParameter("Polyline", "PL", "PolyLines", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new AxDomainAreaLoadParameter(), "AxisVM Domain Area Load", "AxDAL", "Domain Area Loads for AxisVM", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            AxisMesh mesh = new AxisMesh();
            AxisDomain doma = new AxisDomain();
            Curve rawPoint = null;
            Polyline axisPoint = new Polyline();
            double typ0 = 0; Structs.AxDomainAreaLoad.type typ = Structs.AxDomainAreaLoad.type.global;
            string dir0 = "X"; Structs.AxDomainAreaLoad.direction dir = Structs.AxDomainAreaLoad.direction.X;
            double intensity = 0;
            if (!DA.GetData(0, ref mesh)&&!DA.GetData(1,ref doma)) { DA.AbortComponentSolution(); };
            if (!DA.GetData(2, ref typ0)) { typ0 = 0; };
            if (!DA.GetData(3, ref dir0)) { dir0 = "X"; };
            if (!DA.GetData(4, ref intensity)) { intensity = 0; };
            if (!DA.GetData(5, ref rawPoint)) { DA.AbortComponentSolution(); }
            else { rawPoint.TryGetPolyline(out axisPoint); }
            if (dir0 == "Y") { dir = Structs.AxDomainAreaLoad.direction.Y; }
            else if (dir0 == "Z") { dir = Structs.AxDomainAreaLoad.direction.Z; }

            if (typ0 == 1) { typ = Structs.AxDomainAreaLoad.type.local; }
            AxDomainAreaLoad returner = new AxDomainAreaLoad(mesh.Value,doma.Value, typ, dir, intensity, axisPoint);
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
                return Properties.Resource1.ic_AxLoadArea;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("728078fe-a3ce-40d8-95eb-42ea0f8c0d58"); }
        }
    }
}
/*
 
     SUMMARY:
     SHOULD BE REMASTERED ASAP
     
     
     */
