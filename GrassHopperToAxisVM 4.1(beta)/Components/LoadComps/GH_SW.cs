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
    public class GH_SW : GH_Component
    {
        public GH_SW() : base("AxisVM Self Weight", "AxSW", "Component for creating SelfWeights", "AxisVM", "Load")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new AxisLineParameter(), "AxLine", "AxL", "AxisVM lines", GH_ParamAccess.item); pManager[0].Optional = true;
            pManager.AddParameter(new AxisMeshParameter(), "AxMesh", "AxM_E", "AxisVM meshes as edges", GH_ParamAccess.item); pManager[1].Optional = true;

            pManager.AddParameter(new AxisMeshParameter(), "AxMesh", "AxM_S", "AxisVM meshes as surfaces", GH_ParamAccess.item); pManager[2].Optional = true;
            pManager.AddParameter(new AxisMeshParameter(), "AxMesh", "AxM_D", "AxisVM meshes as domains", GH_ParamAccess.item); pManager[3].Optional = true;
            pManager.AddParameter(new AxisDomainParameter(), "AxDomain", "AxD", "AxisVM domains", GH_ParamAccess.item); pManager[4].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new AxSelfWeightParameter(), "AxisVM SelfWeight", "AxSW", "SelfWeight for lines, surfaces, domains", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            AxisLine point = new AxisLine();
            AxisMesh edges = new AxisMesh();
            AxisMesh surfs = new AxisMesh();
            AxisMesh doms = new AxisMesh();
            AxisDomain dom2 = new AxisDomain();
            if (!DA.GetData(0, ref point) && !DA.GetData(1, ref edges) && !DA.GetData(2, ref surfs) && !DA.GetData(3, ref doms) && !DA.GetData(4,ref dom2)) { DA.AbortComponentSolution(); };
            AxSelfWeight returner = new AxSelfWeight(point.Value, edges.Value, surfs.Value, doms.Value, dom2.Value);
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
                return Properties.Resource1.ic_AxLoadSelfWeight;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("cbba019f-a4e9-415c-8077-acb9fa65ceea"); }
        }
    }
}
/*
 
     SUMMARY:
     SHOULD BE REMASTERED ASAP
     
     
     */
