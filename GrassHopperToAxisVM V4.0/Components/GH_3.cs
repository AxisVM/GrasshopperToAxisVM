using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using Rhino.Input;


namespace GrassHopperToAxisVM
{
    public class GH_3 : GH_Component
    {
        public GH_3() : base("Mesh to AxisVM Mesh", "AxMesh", "Component for changing meshes to AxisVM meshes", "AxisVM", "Base")
        {

        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Meshes", "Mesh", "Meshes to convert", GH_ParamAccess.list);
            pManager.AddNumberParameter("Thickness","T","Thicnkess [m] for Surfaces / Domains", GH_ParamAccess.item);
            pManager.AddParameter(new MaterialParameter(), "Material", "AxMAT", "Material for Surfaces / Domains / Edges", GH_ParamAccess.item);
            pManager.AddParameter(new CrossSectionParameter(), "Cross Section", "AxCRS", "Cross Section for Mesh Edges", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Beam / Rib / Truss", "B/R/T", "0=Beam;1=Rib;2=Truss", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            //4. params / structs / type need to be corrected
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new AxisMeshParameter(), "AxisVM Mesh", "AxMesh", "Mesh with AxisVM parameters", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Mesh> meshes = new List<Mesh>();
            List<AxisMesh> axmeshes = new List<AxisMesh>();
            double thickness = 0;
            Material material = null;
            CrossSection crs = null;
            int num = 0;
            DA.GetDataList(0, meshes);
            if (!DA.GetData(1, ref thickness)) { }
            if (!DA.GetData(2, ref material)) { }
            if (!DA.GetData(3, ref crs)) { }
            if (!DA.GetData(4, ref num)) { }

            for (int i = 0; i < meshes.Count; i++)
            {
                axmeshes.Add(new AxisMesh(meshes[i], thickness, new Material(material).Value, new CrossSection(crs).Value, num));
                //axmeshes[i].Value.deleting = true;
                //axmeshes[i].Value.drawing = true;
            }
            
            DA.SetDataList(0, axmeshes);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resource1.ic_AxMesh;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("3d358eea-cdfa-48cd-9868-f57dad9cdd52"); }
        }
    }

    public class AxDomain : GH_Component
    {
        public AxDomain() : base("PolyLine to AxisVM Domain", "axDomain", "Component for changing PolyLine to AxisVM domains", "AxisVM", "Base")
        {

        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Polyline", "PL", "PolyLine to AxisVM Domain", GH_ParamAccess.item);
            pManager.AddNumberParameter("Thickness", "T", "Thicnkess [m] for Surfaces / Domains", GH_ParamAccess.item);
            pManager.AddParameter(new MaterialParameter(), "Material", "MAT", "Material for Surfaces / Domains / Edges", GH_ParamAccess.item);
            pManager.AddParameter(new CrossSectionParameter(), "Cross Section", "CRS", "Cross Section for Mesh Edges", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Beam / Rib / Truss", "B/R/T", "0=Beam;1=Rib;2=Truss", GH_ParamAccess.item);
            pManager.AddCurveParameter("Holes","H","Holes created at domains",GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
            //4. params / structs / type need to be corrected
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new AxisDomainParameter(), "AxisVM Domain", "AxDom", "Domain with AxisVM parameters", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve mesh_ = null;
            List<Curve> holes_ = new List<Curve>();
            AxisDomain axmesh = new AxisDomain();
            double thickness = 0;
            Material material = null;
            CrossSection crs = null;
            int num = 0;
            DA.GetData(0, ref mesh_);
            if (!DA.GetData(1, ref thickness)) { }
            if (!DA.GetData(2, ref material)) { }
            if (!DA.GetData(3, ref crs)) { }
            if (!DA.GetData(4, ref num)) { }
            if (!DA.GetDataList(5, holes_)) { }
            Polyline mesh = new Polyline();
            Polyline[] holes = new Polyline[holes_.Count];
            int index = 0;
            mesh_.TryGetPolyline(out mesh);
            index = 0;
            foreach (Curve x in holes_)
            {
                if (!x.TryGetPolyline(out holes[index])) { holes[index] = null; }
                index++;
            }

            axmesh = (new AxisDomain(mesh, thickness, new Material(material).Value, new CrossSection(crs).Value, num, holes.ToList()));
            //axmesh.Value.deleting = true;
            //axmesh.Value.drawing = true;
            DA.SetData(0, axmesh);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resource1.ic_AxDomain;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("35776b7e-87d7-4974-b6f1-4d3399532546"); }
        }
    }
}
//mesh list tomesh list with same attributes