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
        public class AxDistributedLineLoad : AxisLoad
        {
            public enum type { local, global };
            public enum direction { X, Y, Z };


            public Structs.AxisLine lines;
            public Structs.AxisMesh edges;
            public type typ;
            public direction dir;
            public double start, end;
            public double int1, int2;
         
            public AxDistributedLineLoad(Structs.AxisLine lines, Structs.AxisMesh edges, type typ, direction dir, double start, double end, double int1, double int2, int id = -1) : base(id)
            {
                this.lines = lines;
                this.edges = edges;
                this.typ = typ;
                this.dir = dir;
                this.start = start;
                this.end = end;
                this.int1 = int1;
                this.int2 = int2;
            }
            public static AxDistributedLineLoad Default { get; }
            static AxDistributedLineLoad()
            {
                Default = new AxDistributedLineLoad(Structs.AxisLine.Default,Structs.AxisMesh.Default, type.global, direction.X, 0, 0, 0, 0);
            }
            public override bool Equals(Object obj)
            {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                   AxDistributedLineLoad p = (AxDistributedLineLoad)obj;
                    bool eq = true;
                    eq = eq && ((this.edges!= null && p.edges != null && this.edges.Equals(p.edges)) || (this.edges == null && p.edges == null));
                    eq = eq && ((this.lines != null && p.lines != null && this.lines.Equals(p.lines)) || (this.lines == null && p.lines == null));

                    return eq && this.dir == p.dir && this.int1 == p.int1 && this.int2 == p.int2 && this.typ == p.typ && this.end == p.end && this.start == p.start;
                }
            }

            public override int GetHashCode()
            {
                return this.id;
            }

            public override AxisLoad Clone() { return new AxDistributedLineLoad((AxisLine)lines.Clone(),(AxisMesh)edges.Clone(),typ,dir,start,end,int1,int2,id); }
        }
    }

    public class AxDistributedLineLoad : GH_Goo<Structs.AxDistributedLineLoad>
    {
        public AxDistributedLineLoad(AxDistributedLineLoad axLineSource)
        {
            this.Value.lines =axLineSource.Value.lines;
            this.Value.edges = axLineSource.Value.edges;
            this.Value.typ =  axLineSource.Value.typ;
            this.Value.dir =  axLineSource.Value.dir;
            this.Value.start =axLineSource.Value.start;
            this.Value.end =  axLineSource.Value.end;
            this.Value.int1 = axLineSource.Value.int1;
            this.Value.int2 = axLineSource.Value.int2;
        }
        public AxDistributedLineLoad(Structs.AxisLine lines,Structs.AxisMesh edges, Structs.AxDistributedLineLoad.type typ, Structs.AxDistributedLineLoad.direction dir, double start, double end, double int1, double int2)
        {
            this.Value = new Structs.AxDistributedLineLoad(lines, edges, typ, dir, start, end, int1, int2);
        }
        public AxDistributedLineLoad() => this.Value = Structs.AxDistributedLineLoad.Default;
        public override IGH_Goo Duplicate() => new AxDistributedLineLoad(this);
        public override bool IsValid => true;
        public override string TypeName => "AxDLL";
        public override string TypeDescription => "AxisVM Distributed Line Load";
        public override string ToString() => this.Value.ToString();
        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                AxDistributedLineLoad p = (AxDistributedLineLoad)obj;
                return this.Value.Equals(p.Value);
            }
        }

        public override int GetHashCode()
        {
            return this.Value.id;
        }
    }
    public class AxDistributedLineLoadParameter : GH_PersistentParam<AxDistributedLineLoad>
    {
        public AxDistributedLineLoadParameter() : base("AxisVM Distributed Line Load", "AxDLL", "AxisVM DistributedLineLoad", "AxisVM", "Params") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resource1.ic_ParAxLoadLine;
        public override Guid ComponentGuid => new Guid("9e6ceca7-8205-4909-8b80-3c35a2aeadbe");
        protected override GH_GetterResult Prompt_Singular(ref AxDistributedLineLoad value)
        {
            value = new AxDistributedLineLoad();
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<AxDistributedLineLoad> values)
        {
            values = new List<AxDistributedLineLoad>();
            return GH_GetterResult.success;
        }
    }
}
