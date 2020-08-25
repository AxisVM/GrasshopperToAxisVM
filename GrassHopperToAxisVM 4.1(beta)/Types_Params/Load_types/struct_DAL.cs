using System;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System.Drawing;
using System.Collections.Generic;
using Rhino.Geometry;

namespace GrassHopperToAxisVM
{
    public partial class Structs
    {
        public class AxDomainAreaLoad : AxisLoad
        {
            public enum type { local, global };
            public enum direction { X, Y, Z };

            public Structs.AxisMesh mesh;
            public Structs.AxisDomain doma;
            public type typ;
            public direction dir;
            public double intensity = 0;
            public Polyline polygon;

            public AxDomainAreaLoad(Structs.AxisMesh mesh, Structs.AxisDomain doma, type typ, direction dir, double intensity, Polyline polygon, int id = -1) : base(id)
            {
                this.mesh = mesh;
                this.doma = doma;
                this.typ = typ;
                this.dir = dir;
                this.intensity = intensity;
                this.polygon = polygon;
            }
            public static AxDomainAreaLoad Default { get; }
            static AxDomainAreaLoad()
            {
                Default = new AxDomainAreaLoad(Structs.AxisMesh.Default,Structs.AxisDomain.Default, type.global, direction.X, 0, new Polyline());
            }

            public override bool Equals(Object obj)
            {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    AxDomainAreaLoad p = (AxDomainAreaLoad)obj;
                    bool eq = true;
                    eq = eq && ((this.doma != null && p.doma != null && this.doma.Equals(p.doma)) || (this.doma == null && p.doma == null));
                    eq = eq && ((this.mesh != null && p.mesh != null && this.mesh.Equals(p.mesh)) || (this.mesh == null && p.mesh == null));
                    eq = eq && ((this.polygon != null && p.polygon != null && convert.PolyLineEquals(this.polygon, p.polygon)) || (this.polygon == null && p.polygon == null));

                    return eq && this.dir == p.dir && this.intensity == p.intensity && this.typ == p.typ; 
                }
            }

            public override int GetHashCode()
            {
                return this.id;
            }

            public override AxisLoad Clone() { return new AxDomainAreaLoad((AxisMesh)mesh.Clone(),(AxisDomain)doma.Clone(),typ,dir,intensity,polygon.Duplicate(),id); }
        }
    }

    public class AxDomainAreaLoad : GH_Goo<Structs.AxDomainAreaLoad>
    {
        public AxDomainAreaLoad(AxDomainAreaLoad cop)
        {
            this.Value.mesh =       cop.Value.mesh;
            this.Value.typ =        cop.Value.typ;
            this.Value.dir =        cop.Value.dir;
            this.Value.intensity =  cop.Value.intensity;
            this.Value.polygon =    cop.Value.polygon;
        }
        public AxDomainAreaLoad(Structs.AxisMesh mesh, Structs.AxisDomain doma, Structs.AxDomainAreaLoad.type typ, Structs.AxDomainAreaLoad.direction dir, double intensity, Polyline polygon)
        {
            this.Value = new Structs.AxDomainAreaLoad(mesh, doma, typ,dir,intensity,polygon);
        }
        public AxDomainAreaLoad() => this.Value = Structs.AxDomainAreaLoad.Default;
        public override IGH_Goo Duplicate() => new AxDomainAreaLoad(this);
        public override bool IsValid => true;
        public override string TypeName => "AxDAL";
        public override string TypeDescription => "AxisVM Domain Area Load";
        public override string ToString() => this.Value.ToString();
        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                AxDomainAreaLoad p = (AxDomainAreaLoad)obj;
                return this.Value.Equals(p.Value);
            }
        }

        public override int GetHashCode()
        {
            return this.Value.id;
        }
    }
    public class AxDomainAreaLoadParameter : GH_PersistentParam<AxDomainAreaLoad>
    {
        public AxDomainAreaLoadParameter() : base("AxisVM Domain Area Load", "AxDAL", "AxisVM DomainAreaLoad", "AxisVM", "Params") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resource1.ic_ParAxLoadArea;
        public override Guid ComponentGuid => new Guid("960b6ab9-d099-4f10-a6a4-5b9a333adb0e");
        protected override GH_GetterResult Prompt_Singular(ref AxDomainAreaLoad value)
        {
            value = new AxDomainAreaLoad();
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<AxDomainAreaLoad> values)
        {
            values = new List<AxDomainAreaLoad>();
            return GH_GetterResult.success;
        }
    }
}
