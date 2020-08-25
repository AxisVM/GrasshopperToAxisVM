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
        public class AxDistributedSurfaceLoad : AxisLoad
        {
            public enum type { local, global };
            public enum direction { X, Y, Z };

            public Structs.AxisMesh mesh;
            public type typ;
            public direction dir;
            public double intensity = 0;

            public AxDistributedSurfaceLoad(Structs.AxisMesh mesh, type typ, direction dir, double intensity, int id = -1) : base(id)
            {
                this.mesh = mesh;
                this.typ = typ;
                this.dir = dir;
                this.intensity = intensity;
            }
            public static AxDistributedSurfaceLoad Default { get; }
            static AxDistributedSurfaceLoad()
            {
                Default = new AxDistributedSurfaceLoad(Structs.AxisMesh.Default, type.global, direction.X, 0);
            }
            public override bool Equals(Object obj)
            {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    AxDistributedSurfaceLoad p = (AxDistributedSurfaceLoad)obj;
                    bool eq = true;
                    eq = eq && ((this.mesh != null && p.mesh != null && this.mesh.Equals(p.mesh)) || (this.mesh == null && p.mesh == null));

                    return eq && this.dir == p.dir && this.intensity == p.intensity && this.typ == p.typ;
                }
            }

            public override int GetHashCode()
            {
                return this.id;
            }

            public override AxisLoad Clone() { return new AxDistributedSurfaceLoad((AxisMesh)mesh.Clone(), typ, dir, intensity, id); }
        }
    }

    public class AxDistributedSurfaceLoad : GH_Goo<Structs.AxDistributedSurfaceLoad>
    {
        public AxDistributedSurfaceLoad(AxDistributedSurfaceLoad axLineSource)
        {
            this.Value.mesh =      axLineSource.Value.mesh;
            this.Value.typ =       axLineSource.Value.typ;
            this.Value.dir =       axLineSource.Value.dir;
            this.Value.intensity = axLineSource.Value.intensity;
        }
        public AxDistributedSurfaceLoad(Structs.AxisMesh mesh, Structs.AxDistributedSurfaceLoad.type typ, Structs.AxDistributedSurfaceLoad.direction dir, double intensity)
        {
            this.Value = new Structs.AxDistributedSurfaceLoad(mesh,typ,dir,intensity);
        }
        public AxDistributedSurfaceLoad() => this.Value = Structs.AxDistributedSurfaceLoad.Default;
        public override IGH_Goo Duplicate() => new AxDistributedSurfaceLoad(this);
        public override bool IsValid => true;
        public override string TypeName => "AxDSL";
        public override string TypeDescription => "AxisVM Distributed Surface Load";
        public override string ToString() => this.Value.ToString();
        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                AxDistributedSurfaceLoad p = (AxDistributedSurfaceLoad)obj;
                return this.Value.Equals(p.Value);
            }
        }

        public override int GetHashCode()
        {
            return this.Value.id;
        }
    }
    public class AxDistributedSurfaceLoadParameter : GH_PersistentParam<AxDistributedSurfaceLoad>
    {
        public AxDistributedSurfaceLoadParameter() : base("AxisVM Distributed Surface Load", "AxDSL", "AxisVM DistributedSurfaceLoad", "AxisVM", "Params") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resource1.ic_ParAxLoadDistSurface;
        public override Guid ComponentGuid => new Guid("6eefe108-826d-4276-951a-7730af9b15c3");
        protected override GH_GetterResult Prompt_Singular(ref AxDistributedSurfaceLoad value)
        {
            value = new AxDistributedSurfaceLoad();
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<AxDistributedSurfaceLoad> values)
        {
            values = new List<AxDistributedSurfaceLoad>();
            return GH_GetterResult.success;
        }
    }
}
