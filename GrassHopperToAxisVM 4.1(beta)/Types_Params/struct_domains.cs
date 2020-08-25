using System;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System.Collections.Generic;
using System.Drawing;
using Rhino.Geometry;
using System.Linq;

namespace GrassHopperToAxisVM
{
    public partial class Structs
    {
        public class AxisDomain : AxisMember
        {
            public int[] lineCount1 = null;
            public List<int> axisid_SPECline = new List<int>();

            public Polyline mesh; public Material material; public double thickness; public CrossSection crs; public int num;
            public List<Polyline> holes;
            public AxisDomain(Polyline mesh, double thickness, Material material, CrossSection crs, int num, List<Polyline> holes, int id = -1) : base(id)
            {
                this.mesh = mesh;
                this.thickness = thickness;
                this.material = material;
                this.crs = crs;
                this.num = num;
                this.holes = holes;
            }
            public AxisDomain(Polyline mesh) : this(mesh, 0, null, null, 0,null) { }
            public AxisDomain(Polyline mesh, double thickness, Material material) : this(mesh, thickness, material, null, 0,null) { }
            public AxisDomain(Polyline mesh, CrossSection crossSection, Material material, int num) : this(mesh, 0, material, crossSection, num,null) { }

            public static AxisDomain Default { get; }

            static AxisDomain()
            {
                Default = new AxisDomain(null, 0, null, null, 0, null);
            }
            public override bool Equals(Object obj)
            {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    AxisDomain p = (AxisDomain)obj;
                    bool eq = true;
                    eq = eq && ((this.crs != null && p.crs != null && this.crs.Equals(p.crs)) || (this.crs == null && p.crs == null));
                    eq = eq && ((this.mesh != null && p.mesh != null && convert.PolyLineEquals(this.mesh, p.mesh)) || (this.mesh == null && p.mesh == null));
                    eq = eq && ((this.material != null && p.material != null && this.material.Equals(p.material)) || (this.material == null && p.material == null));
                    eq = eq && ((this.holes != null && p.holes != null && this.holes.SequenceEqual(p.holes)) || (this.holes == null && p.holes == null));
                    if (p.holes.Count != this.holes.Count) return false;
                    for(int i = 0; i < this.holes.Count; i++)
                    {
                        eq = eq && convert.PolyLineEquals(this.holes[i], p.holes[i]);
                    }
                    return base.Equals(p) && eq && this.num ==p.num && this.thickness == p.thickness;
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
                    AxisDomain p = (AxisDomain)obj;
                    bool eq = true;
                    //eq = eq && ((this.crs != null && p.crs != null && this.crs.Equals(p.crs)) || (this.crs == null && p.crs == null));
                    eq = eq && ((this.mesh != null && p.mesh != null && convert.PolyLineEquals(this.mesh, p.mesh)) || (this.mesh == null && p.mesh == null));
                    //eq = eq && ((this.material != null && p.material != null && this.material.Equals(p.material)) || (this.material == null && p.material == null));
                    eq = eq && ((this.holes != null && p.holes != null && this.holes.SequenceEqual(p.holes)) || (this.holes == null && p.holes == null));
                    if (p.holes.Count != this.holes.Count) return false;
                    for (int i = 0; i < this.holes.Count; i++)
                    {
                        eq = eq && convert.PolyLineEquals(this.holes[i], p.holes[i]);
                    }
                    return base.Equals(p) && eq && this.num == p.num && this.thickness == p.thickness;
                }
            }

            public override int GetHashCode()
            {
                return this.id;
            }

            public override AxisMember Clone()
            {
                AxisDomain returner = null;
                List<Polyline> clonerlist= new List<Polyline>();
                foreach(Polyline x in this.holes) { clonerlist.Add(x.Duplicate()); }
                if(crs != null && material != null) returner = new AxisDomain(mesh.Duplicate(), thickness, material.Clone(), crs.Clone(), num, clonerlist, id);
                else if (crs != null) returner = new AxisDomain(mesh.Duplicate(), thickness, null, crs.Clone(), num, clonerlist, id);
                else if (material != null) returner = new AxisDomain(mesh.Duplicate(), thickness, material.Clone(), null, num, clonerlist, id);
                else returner = new AxisDomain(mesh.Duplicate(), thickness, null, null , num, clonerlist, id);
                returner.axisid_point = this.axisid_point; returner.axisid_line = this.axisid_line;
                returner.axisid_mesh = this.axisid_mesh; returner.axisid_dom = this.axisid_dom;
                returner.axisid_edge = this.axisid_edge;

                returner.axisid_SPECline = this.axisid_SPECline;
                returner.lineCount1 = this.lineCount1;
                return returner;
            }
        }
    }
    public class AxisDomain : GH_Goo<Structs.AxisDomain>
    {
        public AxisDomain(Polyline mesh, double thickness = 0, Structs.Material material = null, Structs.CrossSection crs = null, int num = 0, List<Polyline> hole = null)
        {
            this.Value = new Structs.AxisDomain(mesh, thickness, material, crs, num, hole);
        }
        public AxisDomain(AxisDomain axShellSource)
        {
            this.Value.mesh = axShellSource.Value.mesh;
            this.Value.thickness = axShellSource.Value.thickness;
            this.Value.material = axShellSource.Value.material;
            this.Value.crs = axShellSource.Value.crs;
            this.Value.num = axShellSource.Value.num;
            this.Value.holes = axShellSource.Value.holes;
        }
        public AxisDomain() => this.Value = Structs.AxisDomain.Default;
        public override IGH_Goo Duplicate() => new AxisDomain(this);
        public override bool IsValid => true;
        public override string TypeName => "AxD";
        public override string TypeDescription => "AxisVM Domain";
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
                AxisDomain p = (AxisDomain)obj;
                return this.Value.Equals(p.Value);
            }
        }

        public override int GetHashCode()
        {
            return this.Value.id;
        }
    }
    public class AxisDomainParameter : GH_PersistentParam<AxisDomain>
    {
        public AxisDomainParameter() : base("AxisVM Domain", "AxD", "AxisVM Domain Parameter", "AxisVM", "Params") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resource1.ic_ParAxDomain;
        public override Guid ComponentGuid => new Guid("c8c65a8b-2eb3-4c5f-9a99-7c960b16730c");
        protected override GH_GetterResult Prompt_Singular(ref AxisDomain value)
        {
            value = new AxisDomain();
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<AxisDomain> values)
        {
            values = new List<AxisDomain>();
            return GH_GetterResult.success;
        }
    }
}