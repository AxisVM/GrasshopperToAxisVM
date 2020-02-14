using System;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System.Collections.Generic;
using System.Drawing;
using Rhino.Geometry;

namespace GrassHopperToAxisVM
{
    public partial class structs
    {
        public class AxisMesh
        {
            public Mesh mesh; public string material; public double thickness; public string crs;
            public AxisMesh(Mesh mesh, double thickness = 0, string material = null,string crs = null)
            {
                this.mesh = mesh;
                this.thickness = thickness;
                this.material = material;
                this.crs = crs;
            }
            public static AxisMesh Default { get; }

            static AxisMesh()
            {
                Default = new AxisMesh(null,0,null,null);
            }
        }
    }
    public class AxisMesh: GH_Goo<structs.AxisMesh>
    {
        public AxisMesh(Mesh mesh, double thickness = 0,string material = null, string crs = null)
        {
            this.Value = new structs.AxisMesh(mesh,thickness,material,crs);
        }
        public AxisMesh(AxisMesh axShellSource)
        {
            this.Value.mesh = axShellSource.Value.mesh;
            this.Value.thickness = axShellSource.Value.thickness;
            this.Value.material = axShellSource.Value.material;
            this.Value.crs = axShellSource.Value.crs;
        }
        public AxisMesh() => this.Value = structs.AxisMesh.Default;
        public override IGH_Goo Duplicate() => new AxisMesh(this);
        public override bool IsValid => true;
        public override string TypeName => "AxMesh";
        public override string TypeDescription => "AxMesh";
        public override string ToString() => this.Value.ToString();
        public override object ScriptVariable() => Value;
    }
    public class AxisMeshParameter : GH_PersistentParam<AxisMesh>
    {
        public AxisMeshParameter() : base("AxisVM Mesh", "AxMesh", "AxisVM Mesh Parameter", "AxisVM", "Params") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resource1.ic_ParAxMesh;
        public override Guid ComponentGuid => new Guid("a7791d9f-cdce-4b4e-9b53-a10d3361fa57");
        protected override GH_GetterResult Prompt_Singular(ref AxisMesh value)
        {
            value = new AxisMesh();
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<AxisMesh> values)
        {
            values = new List<AxisMesh>();
            return GH_GetterResult.success;
        }
    }
}