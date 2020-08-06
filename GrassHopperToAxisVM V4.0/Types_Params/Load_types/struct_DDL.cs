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
        public class AxDistributedDomainLoad : AxisLoad
        {
            public enum type { local, global };
            public enum direction { X, Y, Z };

            public Structs.AxisMesh mesh;
            public Structs.AxisDomain doma;
            public type typ;
            public direction dir;
            public double intensity = 0;

            public AxDistributedDomainLoad(Structs.AxisMesh mesh, Structs.AxisDomain doma, type typ, direction dir, double intensity, int id = -1) : base(id)
            {
                this.mesh = mesh;
                this.doma = doma;
                this.typ = typ;
                this.dir = dir;
                this.intensity = intensity;
            }
            public static AxDistributedDomainLoad Default { get; }
            static AxDistributedDomainLoad()
            {
                Default = new AxDistributedDomainLoad(Structs.AxisMesh.Default, Structs.AxisDomain.Default, type.global, direction.X, 0);
            }

            public override bool Equals(Object obj)
            {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    AxDistributedDomainLoad p = (AxDistributedDomainLoad)obj;
                    bool eq = true;
                    eq = eq && ((this.doma != null && p.doma != null && this.doma.Equals(p.doma)) || (this.doma == null && p.doma == null));
                    eq = eq && ((this.mesh != null && p.mesh != null && this.mesh.Equals(p.mesh)) || (this.mesh == null && p.mesh == null));
                    return eq && this.dir == p.dir && this.intensity == p.intensity && this.typ == p.typ;
                }
            }

            public override int GetHashCode()
            {
                return this.id;
            }

            public override AxisLoad Clone() { return new AxDistributedDomainLoad((AxisMesh)mesh.Clone(), (AxisDomain)doma.Clone(),typ,dir,intensity,id); }
        }
    }

    public class AxDistributedDomainLoad : GH_Goo<Structs.AxDistributedDomainLoad>
    {
        public AxDistributedDomainLoad(AxDistributedDomainLoad axLineSource)
        {
            this.Value.mesh = axLineSource.Value.mesh;
            this.Value.typ = axLineSource.Value.typ;
            this.Value.dir = axLineSource.Value.dir;
            this.Value.intensity = axLineSource.Value.intensity;
        }
        public AxDistributedDomainLoad(Structs.AxisMesh mesh, Structs.AxisDomain doma, Structs.AxDistributedDomainLoad.type typ, Structs.AxDistributedDomainLoad.direction dir, double intensity)
        {
            this.Value = new Structs.AxDistributedDomainLoad(mesh, doma, typ, dir, intensity);
        }
        public AxDistributedDomainLoad() => this.Value = Structs.AxDistributedDomainLoad.Default;
        public override IGH_Goo Duplicate() => new AxDistributedDomainLoad(this);
        public override bool IsValid => true;
        public override string TypeName => "AxDDL";
        public override string TypeDescription => "AxisVM Distributed Domain Load";
        public override string ToString() => this.Value.ToString();
        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                AxDistributedDomainLoad p = (AxDistributedDomainLoad)obj;
                return this.Value.Equals(p.Value);
            }
        }

        public override int GetHashCode()
        {
            return this.Value.id;
        }
    }
    public class AxDistributedDomainLoadParameter : GH_PersistentParam<AxDistributedDomainLoad>
    {
        public AxDistributedDomainLoadParameter() : base("AxisVM Distributed Domain Load", "AxDDL", "AxisVM DistributedDomainLoad", "AxisVM", "Params") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resource1.ic_ParAxLoadDistDomain;
        public override Guid ComponentGuid => new Guid("b1be05b9-63f9-48a0-ba6a-08fb3ff32ccd");
        protected override GH_GetterResult Prompt_Singular(ref AxDistributedDomainLoad value)
        {
            value = new AxDistributedDomainLoad();
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<AxDistributedDomainLoad> values)
        {
            values = new List<AxDistributedDomainLoad>();
            return GH_GetterResult.success;
        }
    }
}

