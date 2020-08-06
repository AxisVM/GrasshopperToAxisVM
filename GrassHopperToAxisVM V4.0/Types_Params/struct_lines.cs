using System;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System.Drawing;

using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Rhino.Geometry;

namespace GrassHopperToAxisVM
{
    public partial class Structs
    {
        public class AxisLine : AxisMember
        {
           
            public List<Line> line; public Material material; public CrossSection crosssection; public int num;
            public AxisLine(List<Line> line, Material material, CrossSection crosssection, int num, int id =-1) : base(id)
            {
                this.line = line;
                this.material = material;
                this.crosssection = crosssection;
                this.num = num;//type beam,rib,truss
            }
            public AxisLine(List<Line> line) : this(line, null, null,0) { }

            public static AxisLine Default { get; }
            static AxisLine()
            {
                Default = new AxisLine(new List<Line>(),null,null,0);
            }
            public override bool Equals(Object obj)
            {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    AxisLine p = (AxisLine)obj;
                    bool eq = true;
                    eq = eq && ((this.crosssection != null && p.crosssection != null && this.crosssection.Equals(p.crosssection)) || (this.crosssection == null && p.crosssection == null));
                    eq = eq && ((this.material != null && p.material != null && this.material.Equals(p.material)) || (this.material == null && p.material == null));
                    eq = eq && ((this.line != null && p.line != null && this.line.SequenceEqual(p.line)) || (this.line == null && p.line == null));
                    return eq && base.Equals(p) && this.num == p.num;
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
                    AxisLine p = (AxisLine)obj;
                    bool eq = true;
                    //eq = eq && ((this.crosssection != null && p.crosssection != null && this.crosssection.Equals(p.crosssection)) || (this.crosssection == null && p.crosssection == null));
                    //eq = eq && ((this.material != null && p.material != null && this.material.Equals(p.material)) || (this.material == null && p.material == null));
                    eq = eq && ((this.line != null && p.line != null && this.line.SequenceEqual(p.line)) || (this.line == null && p.line == null));
                    return eq && base.Equals(p) && this.num == p.num;
                }
            }

            public override int GetHashCode()
            {
                return this.id;
            }

            public override AxisMember Clone()
            {
                AxisLine returner = null;
                List<Line> clonerlist = new List<Line>();
                foreach(Line x in this.line) { clonerlist.Add(x); }
                if(material !=null && crosssection!= null)returner = new AxisLine(clonerlist,material.Clone(),crosssection.Clone(),num,id);
                else if(material != null) returner = new AxisLine(clonerlist, material.Clone(), null, num, id);
                else if(crosssection != null) returner = new AxisLine(clonerlist, null, crosssection.Clone(), num, id);
                else returner = new AxisLine(clonerlist, null, null, num, id);
                returner.axisid_point = this.axisid_point; returner.axisid_line = this.axisid_line;
                returner.axisid_mesh = this.axisid_mesh; returner.axisid_dom = this.axisid_dom;
                returner.axisid_edge = this.axisid_edge;
                return returner;
            }
        }
    }

    public class AxisLine : GH_Goo<Structs.AxisLine>
    {
        public AxisLine(AxisLine axLineSource)
        {
            this.Value.line = axLineSource.Value.line;
            this.Value.crosssection = axLineSource.Value.crosssection;
            this.Value.material = axLineSource.Value.material;
        }
        public AxisLine(List<Line> line, Structs.Material material, Structs.CrossSection crosssection,int num)
        {
            this.Value = new Structs.AxisLine(line, material, crosssection, num);
        }
        public AxisLine() => this.Value = Structs.AxisLine.Default;
        public override IGH_Goo Duplicate() => new AxisLine(this);
        public override bool IsValid => true;
        public override string TypeName => "AxL";
        public override string TypeDescription => "AxisVM Line or Line as Members";
        public override string ToString() => this.Value.ToString();
        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                AxisLine p = (AxisLine)obj;
                return this.Value.Equals(p.Value);
            }
        }

        public override int GetHashCode()
        {
            return this.Value.id;
        }
    }
    public class AxisLineParameter : GH_PersistentParam<AxisLine>
    {
        public AxisLineParameter() : base("AxisVM Line", "AxL", "AxisVM Line Parameter", "AxisVM", "Params") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resource1.ic_ParAxMember;
        public override Guid ComponentGuid => new Guid("17f715b5-48c2-442b-904a-a64600a919be");
        protected override GH_GetterResult Prompt_Singular(ref AxisLine value)
        {
            value = new AxisLine();
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<AxisLine> values)
        {
            values = new List<AxisLine>();
            return GH_GetterResult.success;
        }
    }
}