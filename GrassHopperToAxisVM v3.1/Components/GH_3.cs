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
        public GH_3() : base("Mesh to AxisVM Mesh", "axMesh", "Component for changing meshes to AxisVM meshes", "AxisVM", "Base")
        {

        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Meshes", "Mesh", "Meshes to convert", GH_ParamAccess.list);
            pManager.AddNumberParameter("Thickness","T","Thicnkess [m] for Surfaces / Domains", GH_ParamAccess.item);
            pManager.AddTextParameter("Material", "AxMat", "Material for Surfaces / Domains / Edges", GH_ParamAccess.item);
            pManager.AddTextParameter("Cross Section", "AxCrs", "Cross Section for Mesh Edges", GH_ParamAccess.item);
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
            double thickness = -1;
            string material = null;
            string crs = null; int num = 0;
            DA.GetDataList(0, meshes);
            if (DA.GetData(1, ref thickness) && DA.GetData(2, ref material) && DA.GetData(3, ref crs) && DA.GetData(4, ref num))
            {
                for (int i = 0; i < meshes.Count; i++)
                {
                    axmeshes.Add(new AxisMesh(meshes[i], thickness, material, crs, num));
                }
            }
            else if (DA.GetData(1, ref thickness) && DA.GetData(2, ref material))
            {
                for (int i = 0; i < meshes.Count; i++)
                {
                    axmeshes.Add(new AxisMesh(meshes[i], thickness, material));
                }
            }
            else if (DA.GetData(2, ref material) && DA.GetData(3, ref crs) && DA.GetData(4, ref num))
            {
                for (int i = 0; i < meshes.Count; i++)
                {
                    axmeshes.Add(new AxisMesh(meshes[i], -1, material, crs, num));
                }
            }
            else if (DA.GetData(2, ref material) && DA.GetData(3, ref crs))
            {
                for (int i = 0; i < meshes.Count; i++)
                {
                    axmeshes.Add(new AxisMesh(meshes[i], -1, material, crs));
                }
            }
            else
            {
                for (int i = 0; i < meshes.Count; i++)
                {
                    axmeshes.Add(new AxisMesh(meshes[i]));
                }
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
}
//mesh list tomesh list with same attributes