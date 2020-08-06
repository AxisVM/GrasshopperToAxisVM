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
    public class GH_LC : GH_Component
    {
        public GH_LC() : base("AxisVM LoadCase", "AxLC", "Component for Load Cases", "AxisVM", "Load")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new AxNodalLoadParameter(), "AxisVM Nodal Load", "AxNL", "AxisVM Nodal Load", GH_ParamAccess.list); pManager[0].Optional = true;
            pManager.AddParameter(new AxDistributedLineLoadParameter(), "AxisVM Distributed Line Load", "AxDLL", "AxisVM Distributed Line Load", GH_ParamAccess.list); pManager[1].Optional = true;
            pManager.AddParameter(new AxSelfWeightParameter(), "AxisVM Self Weight", "AxSW", "AxisVM Self Weight for the inputs", GH_ParamAccess.list); pManager[2].Optional = true;
            pManager.AddParameter(new AxDistributedSurfaceLoadParameter(), "AxisVM Distributed Surface Load", "AxDSL", "AxisVM Distributed Surface Load", GH_ParamAccess.list); pManager[3].Optional = true;
            pManager.AddParameter(new AxDistributedDomainLoadParameter(), "AxisVM Distributed Domain Load", "AxDDL", "AxisVM Distributed Domain Load", GH_ParamAccess.list); pManager[4].Optional = true;
            pManager.AddParameter(new AxDomainAreaLoadParameter(), "AxisVM Domain Area Load", "AxDAL", "AxisVM Domain Area Load via polygon points", GH_ParamAccess.list);pManager[5].Optional = true;

        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new AxLoadCaseParameter(), "AxisVM Load Case", "AxLC", "Load Cse for AxisVM model", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<AxNodalLoad> NL = new List<AxNodalLoad>();
            List<AxDistributedLineLoad> DLL = new List<AxDistributedLineLoad>();
            List<AxSelfWeight> SW = new List<AxSelfWeight>();
            List<AxDistributedSurfaceLoad> DSL = new List<AxDistributedSurfaceLoad>();
            List<AxDistributedDomainLoad> DDL = new List<AxDistributedDomainLoad>();
            List<AxDomainAreaLoad> DAL = new List<AxDomainAreaLoad>();

            List<Structs.AxNodalLoad> sNL = new List<Structs.AxNodalLoad>();
            List<Structs.AxDistributedLineLoad> sDLL = new List<Structs.AxDistributedLineLoad>();
            List<Structs.AxSelfWeight> sSW = new List<Structs.AxSelfWeight>();
            List<Structs.AxDistributedSurfaceLoad> sDSL = new List<Structs.AxDistributedSurfaceLoad>();
            List<Structs.AxDistributedDomainLoad> sDDL = new List<Structs.AxDistributedDomainLoad>();
            List<Structs.AxDomainAreaLoad> sDAL = new List<Structs.AxDomainAreaLoad>();

            

            if (DA.GetDataList(0, NL))
            {
                foreach (AxNodalLoad x in NL)
                {
                    sNL.Add(x.Value);
                }
            }
            if (DA.GetDataList(1, DLL))
            {
                foreach (AxDistributedLineLoad x in DLL)
                {
                    sDLL.Add(x.Value);
                }
            }
            if (DA.GetDataList(2, SW))
            {
                foreach (AxSelfWeight x in SW)
                {
                    sSW.Add(x.Value);
                }
            }
            if (DA.GetDataList(3, DSL))
            {
                foreach (AxDistributedSurfaceLoad x in DSL)
                {
                    sDSL.Add(x.Value);
                }
            }
            if (DA.GetDataList(4, DDL))
            {
                foreach (AxDistributedDomainLoad x in DDL)
                {
                    sDDL.Add(x.Value);
                }
            }
            if (DA.GetDataList(5, DAL))
            {
                foreach (AxDomainAreaLoad x in DAL)
                {
                    sDAL.Add(x.Value);
                }
            }


            AxLoadCase returner = new AxLoadCase(sNL,sDLL,sSW,sDSL,sDDL,sDAL);
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
                return Properties.Resource1.ic_AxLoadCase;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("ec1d049e-1ca0-4732-a254-8953aad14a78"); }
        }
    }
}
/*
 
     SUMMARY:
     SHOULD BE REMASTERED ASAP
     
     
     */
