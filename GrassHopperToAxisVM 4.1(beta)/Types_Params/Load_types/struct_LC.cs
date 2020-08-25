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
        public class AxLoadCase : AxisLoad
        {
            public List<Structs.AxNodalLoad> NL = new List<Structs.AxNodalLoad>();
            public List<Structs.AxDistributedLineLoad> DLL = new List<Structs.AxDistributedLineLoad>();
            public List<Structs.AxSelfWeight> SW = new List<Structs.AxSelfWeight>();
            public List<Structs.AxDistributedSurfaceLoad> DSL = new List<Structs.AxDistributedSurfaceLoad>();
            public List<Structs.AxDistributedDomainLoad> DDL = new List<Structs.AxDistributedDomainLoad>();
            public List<Structs.AxDomainAreaLoad> DAL = new List<Structs.AxDomainAreaLoad>();


            public AxLoadCase(List<Structs.AxNodalLoad> NL, List<Structs.AxDistributedLineLoad> DLL, List<Structs.AxSelfWeight> SW, List<Structs.AxDistributedSurfaceLoad> DSL,List<Structs.AxDistributedDomainLoad> DDL, List<Structs.AxDomainAreaLoad> DAL,int id = -1) : base(id)
            {
                this.NL = NL;
                this.DLL = DLL;
                this.SW = SW;
                this.DSL = DSL;
                this.DDL = DDL;
                this.DAL = DAL;
            }
            public static AxLoadCase Default { get; }
            static AxLoadCase()
            {
                Default = new AxLoadCase(new List<AxNodalLoad>(), new List<AxDistributedLineLoad>(), new List<AxSelfWeight>(), new List<AxDistributedSurfaceLoad>(), new List<AxDistributedDomainLoad>(), new List<AxDomainAreaLoad>());
            }
            public override bool Equals(Object obj)
            {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    AxLoadCase p = (AxLoadCase)obj;
                    bool eq = true;
                    eq = eq && ((this.DAL != null && p.DAL != null && this.DAL.Equals(p.DAL)) || (this.DAL == null && p.DAL == null));
                    eq = eq && ((this.DLL != null && p.DLL != null && this.DLL.Equals(p.DLL)) || (this.DLL == null && p.DLL == null));
                    eq = eq && ((this.DSL != null && p.DSL != null && this.DSL.Equals(p.DSL)) || (this.DSL == null && p.DSL == null)); eq = eq && ((this.DAL != null && p.DAL != null && this.DAL.Equals(p.DAL)) || (this.DAL == null && p.DAL == null));
                    eq = eq && ((this.DDL != null && p.DDL != null && this.DDL.Equals(p.DLL)) || (this.DDL == null && p.DDL == null));
                    eq = eq && ((this.NL != null && p.NL != null && this.NL.Equals(p.NL)) || (this.NL == null && p.NL == null));
                    eq = eq && ((this.SW != null && p.SW != null && this.SW.Equals(p.SW)) || (this.SW == null && p.SW == null));

                    return eq;
                }
            }

            public override int GetHashCode()
            {
                return this.id;
            }

            public override AxisLoad Clone()
            {
                List<Structs.AxNodalLoad> NL_ = new List<AxNodalLoad>();
                List<Structs.AxDistributedLineLoad> DLL_ = new List<AxDistributedLineLoad>();
                List<Structs.AxSelfWeight> SW_ = new List<AxSelfWeight>();
                List<Structs.AxDistributedSurfaceLoad> DSL_ = new List<AxDistributedSurfaceLoad>();
                List<Structs.AxDistributedDomainLoad> DDL_ = new List<AxDistributedDomainLoad>();
                List<Structs.AxDomainAreaLoad> DAL_ = new List<AxDomainAreaLoad>();
                foreach(AxNodalLoad x in NL) { NL_.Add((AxNodalLoad)x.Clone()); }
                foreach (AxDistributedLineLoad x in DLL) { DLL_.Add((AxDistributedLineLoad)x.Clone()); }
                foreach (AxSelfWeight x in SW) { SW_.Add((AxSelfWeight)x.Clone()); }
                foreach (AxDistributedSurfaceLoad x in DSL) { DSL_.Add((AxDistributedSurfaceLoad)x.Clone()); }
                foreach (AxDistributedDomainLoad x in DDL) { DDL_.Add((AxDistributedDomainLoad)x.Clone()); }
                foreach (AxDomainAreaLoad x in DAL) { DAL_.Add((AxDomainAreaLoad)x.Clone()); }

                return new AxLoadCase(NL_,DLL_,SW_,DSL_,DDL_,DAL_,id);
            }
        }
    }
    public class AxLoadCase : GH_Goo<Structs.AxLoadCase>
    {
        public AxLoadCase(AxLoadCase e)
        {
            this.Value.NL =  e.Value.NL;
            this.Value.DLL = e.Value.DLL;
            this.Value.SW =  e.Value.SW;
            this.Value.DSL = e.Value.DSL;
            this.Value.DDL = e.Value.DDL;
            this.Value.DAL = e.Value.DAL;
        }
        public AxLoadCase(List<Structs.AxNodalLoad> NL, List<Structs.AxDistributedLineLoad> DLL, List<Structs.AxSelfWeight> SW, List<Structs.AxDistributedSurfaceLoad> DSL, List<Structs.AxDistributedDomainLoad> DDL, List<Structs.AxDomainAreaLoad> DAL)
        {
            this.Value = new Structs.AxLoadCase(NL,DLL,SW,DSL,DDL,DAL);
        }
        public AxLoadCase() => this.Value = Structs.AxLoadCase.Default;
        public override IGH_Goo Duplicate() => new AxLoadCase(this);
        public override bool IsValid => true;
        public override string TypeName => "AxLC";
        public override string TypeDescription => "AxisVM LoadCase";
        public override string ToString() => this.Value.ToString();

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                AxLoadCase p = (AxLoadCase)obj;
                return this.Value.Equals(p.Value);
            }
        }

        public override int GetHashCode()
        {
            return this.Value.id;
        }
    }
    public class AxLoadCaseParameter : GH_PersistentParam<AxLoadCase>
    {
        public AxLoadCaseParameter() : base("AxisVM LoadCase", "AxLC", "AxisVM LoadCase", "AxisVM", "Params") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resource1.ic_ParAxLoadCase;
        public override Guid ComponentGuid => new Guid("64c21283-04d7-44d4-99db-3c62a641e18c");
        protected override GH_GetterResult Prompt_Singular(ref AxLoadCase value)
        {
            value = new AxLoadCase();
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<AxLoadCase> values)
        {
            values = new List<AxLoadCase>();
            return GH_GetterResult.success;
        }
    }
}
