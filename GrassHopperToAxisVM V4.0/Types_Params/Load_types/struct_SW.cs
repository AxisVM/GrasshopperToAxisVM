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
        public class AxSelfWeight : AxisLoad
        {
            public Structs.AxisLine lines;
            public Structs.AxisMesh edges;

            public Structs.AxisMesh surfs;
            public Structs.AxisMesh doms;
            public Structs.AxisDomain dom2;


            public AxSelfWeight(Structs.AxisLine lines, Structs.AxisMesh edges, Structs.AxisMesh surfs, Structs.AxisMesh doms, Structs.AxisDomain dom2,int id = -1) : base(id)
            {
                this.lines = lines;
                this.edges = edges;
                this.surfs = surfs;
                this.doms = doms;
                this.dom2 = dom2;
            }
            public static AxSelfWeight Default { get; }
            static AxSelfWeight() 
            {
                Default = new AxSelfWeight(Structs.AxisLine.Default, Structs.AxisMesh.Default, Structs.AxisMesh.Default, Structs.AxisMesh.Default, Structs.AxisDomain.Default);
            }

            public override bool Equals(Object obj)
            {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    AxSelfWeight p = (AxSelfWeight)obj;
                    bool eq = true;
                    eq = eq && ((this.lines != null && p.lines != null && this.lines.Equals(p.lines)) || (this.lines == null && p.lines == null));
                    eq = eq && ((this.surfs != null && p.surfs != null && this.surfs.Equals(p.surfs)) || (this.surfs == null && p.surfs == null));
                    eq = eq && ((this.doms != null && p.doms != null && this.doms.Equals(p.doms)) || (this.doms == null && p.doms == null));
                    eq = eq && ((this.edges != null && p.edges != null && this.edges.Equals(p.edges)) || (this.edges == null && p.edges == null));
                    eq = eq && ((this.dom2 != null && p.dom2 != null && this.dom2.Equals(p.dom2)) || (this.dom2 == null && p.dom2 == null));

                    return eq;
                }
            }

            public override int GetHashCode()
            {
                return this.id;
            }

            public override AxisLoad Clone() { return new AxSelfWeight((AxisLine)lines.Clone(),(AxisMesh)edges.Clone(),(AxisMesh)surfs.Clone(),(AxisMesh)doms.Clone(),(AxisDomain)dom2.Clone()); }
        }
    }

    public class AxSelfWeight : GH_Goo<Structs.AxSelfWeight>
    {
        public AxSelfWeight(AxSelfWeight axLineSource)
        {
            this.Value.lines = axLineSource.Value.lines;
            this.Value.edges = axLineSource.Value.edges;
            this.Value.surfs = axLineSource.Value.surfs;
            this.Value.doms = axLineSource.Value.doms;
            this.Value.dom2 = axLineSource.Value.dom2;
        }
        public AxSelfWeight(Structs.AxisLine lines, Structs.AxisMesh edges, Structs.AxisMesh surfs, Structs.AxisMesh doms, Structs.AxisDomain dom2)
        {
            this.Value = new Structs.AxSelfWeight(lines,edges,surfs,doms,dom2);
        }
        public AxSelfWeight() => this.Value = Structs.AxSelfWeight.Default;
        public override IGH_Goo Duplicate() => new AxSelfWeight(this);
        public override bool IsValid => true;
        public override string TypeName => "AxSW";
        public override string TypeDescription => "AxisVM Self Weight for lines / suraces / domains";
        public override string ToString() => this.Value.ToString();

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                AxSelfWeight p = (AxSelfWeight)obj;
                return this.Value.Equals(p.Value);
            }
        }

        public override int GetHashCode()
        {
            return this.Value.id;
        }
    }
    public class AxSelfWeightParameter : GH_PersistentParam<AxSelfWeight>
    {
        public AxSelfWeightParameter() : base("AxisVM Self Weight", "AxSW", "AxisVM Self Weight", "AxisVM", "Params") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resource1.ic_ParAxLoadSelfWeight;
        public override Guid ComponentGuid => new Guid("59f9ed62-e455-4709-a12b-e3093dec22b3");
        protected override GH_GetterResult Prompt_Singular(ref AxSelfWeight value)
        {
            value = new AxSelfWeight();
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<AxSelfWeight> values)
        {
            values = new List<AxSelfWeight>();
            return GH_GetterResult.success;
        }
    }
}
