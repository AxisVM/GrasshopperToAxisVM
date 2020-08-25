using System;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System.Collections.Generic;
using System.Drawing;
using Rhino.Geometry;
using AxisVM;

namespace GrassHopperToAxisVM
{
    public partial class Structs
    {
        public class AxisMesh : AxisMember
        {
            public RSurface[] rSurfaceDataList = null;// used for only non-geometrical change
            public int[] lineCount1 = null;
            public List<int> axisid_SPECline = new List<int>();
            public Mesh mesh; public Material material; public double thickness; public CrossSection crs; public int num;
            public AxisMesh(Mesh mesh, double thickness, Material material,CrossSection crs, int num, int id = -1) : base(id)
            {
                this.mesh = mesh;
                this.thickness = thickness;
                this.material = material;
                this.crs = crs;
                this.num = num;
            }
            public AxisMesh(Mesh mesh):this(mesh,0,null,null,0){ }
            public AxisMesh(Mesh mesh, double thickness, Material material) : this(mesh, thickness, material, null, 0) { }
            public AxisMesh(Mesh mesh, CrossSection crossSection, Material material, int num) : this(mesh, 0, material, crossSection, num){ }

            public static AxisMesh Default { get; }

            static AxisMesh() 
            {
                Default = new AxisMesh(null,0,null,null,0);
            }

            public override bool Equals(Object obj)
            {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    AxisMesh p = (AxisMesh)obj;
                    bool eq = true;
                    eq = eq && ((this.crs != null && p.crs != null && this.crs.Equals(p.crs)) || (this.crs == null && p.crs == null));
                    eq = eq && ((this.material != null && p.material != null && this.material.Equals(p.material)) || (this.material == null && p.material == null));
                    eq = eq && ((this.mesh != null && p.mesh != null && convert.MeshEquals(this.mesh, p.mesh)) || (this.mesh == null && p.mesh == null));
                    return eq && base.Equals(p) && this.num == p.num && this.thickness == p.thickness;
                }
            }

            public override bool GeometryEquals(Object obj)
            {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    AxisMesh p = (AxisMesh)obj;
                    bool eq = true;
                    //eq = eq && ((this.crs != null && p.crs != null && this.crs.Equals(p.crs)) || (this.crs == null && p.crs == null));
                    //eq = eq && ((this.material != null && p.material != null && this.material.Equals(p.material)) || (this.material == null && p.material == null));
                    eq = eq && ((this.mesh != null && p.mesh != null && convert.MeshEquals(this.mesh, p.mesh)) || (this.mesh == null && p.mesh == null));
                    return eq && base.Equals(p) && this.num == p.num && this.thickness == p.thickness;
                }
            }

            public override int GetHashCode()
            {
                return this.id;
            }

            public override AxisMember Clone()
            {
                AxisMesh returner = null;
                if(material != null && crs !=null)returner = new AxisMesh(mesh.DuplicateMesh(),thickness,material.Clone(),crs.Clone(),num,id);
                else if(material!=null) returner = new AxisMesh(mesh.DuplicateMesh(), thickness, material.Clone(), null, num, id);
                else if(crs!= null) returner = new AxisMesh(mesh.DuplicateMesh(), thickness, null, crs.Clone(), num, id);
                else returner = new AxisMesh(mesh.DuplicateMesh(), thickness, null,null, num, id);
                returner.axisid_point = this.axisid_point; returner.axisid_line = this.axisid_line;
                returner.axisid_mesh = this.axisid_mesh; returner.axisid_dom = this.axisid_dom;
                returner.axisid_edge = this.axisid_edge;
                returner.rSurfaceDataList = this.rSurfaceDataList;
                returner.lineCount1 = this.lineCount1;
                returner.axisid_SPECline = this.axisid_SPECline;
                return returner;

            }
        }
    }
    public class AxisMesh: GH_Goo<Structs.AxisMesh>
    {
        public AxisMesh(Mesh mesh, double thickness = 0,Structs.Material material = null, Structs.CrossSection crs = null, int num = 0)
        {
            this.Value = new Structs.AxisMesh(mesh,thickness,material,crs,num);
        }
        public AxisMesh(AxisMesh axShellSource)
        {
            this.Value.mesh = axShellSource.Value.mesh;
            this.Value.thickness = axShellSource.Value.thickness;
            this.Value.material = axShellSource.Value.material;
            this.Value.crs = axShellSource.Value.crs;
            this.Value.num = axShellSource.Value.num;
        }
        public AxisMesh() => this.Value = Structs.AxisMesh.Default;
        public override IGH_Goo Duplicate() => new AxisMesh(this);
        public override bool IsValid => true;
        public override string TypeName => "AxM";
        public override string TypeDescription => "AxisVM Mesh";
        public override string ToString() => this.Value.ToString();
        public override object ScriptVariable() => Value;

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                AxisMesh p = (AxisMesh)obj;
                return this.Value.Equals(p.Value);
            }
        }

        public override int GetHashCode()
        {
            return this.Value.id;
        }
    }
    public class AxisMeshParameter : GH_PersistentParam<AxisMesh>
    {
        public AxisMeshParameter() : base("AxisVM Mesh", "AxM", "AxisVM Mesh Parameter", "AxisVM", "Params") { }
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