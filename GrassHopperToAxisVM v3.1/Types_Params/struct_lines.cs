using System;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System.Drawing;
using System.Collections.Generic;
using Rhino.Geometry;

namespace GrassHopperToAxisVM
{
    public partial class structs
    {
        public class AxisLine
        {
            public static AxisLine Default { get; }
            public List<Line> line; public string material, crosssection; public int num;
            public AxisLine(List<Line> line, string material = null, string crosssection = null,int num = 0)
            {
                this.line = line;
                this.material = material;
                this.crosssection = crosssection;
                this.num = num;
            }
            static AxisLine()
            {
                Default = new AxisLine(new List<Line>(),null,null,0);
            }
        }
    }

    public class AxisLine : GH_Goo<structs.AxisLine>
    {
        public AxisLine(AxisLine axLineSource)
        {
            this.Value.line = axLineSource.Value.line;
            this.Value.crosssection = axLineSource.Value.crosssection;
            this.Value.material = axLineSource.Value.material;
            this.Value.num = axLineSource.Value.num;
        }
        public AxisLine(List<Line> line, string material = null, string crosssection = null, int num = 0)
        {
            this.Value = new structs.AxisLine(line, material, crosssection, num);
        }
        public AxisLine() => this.Value = structs.AxisLine.Default;
        public override IGH_Goo Duplicate() => new AxisLine(this);
        public override bool IsValid => true;
        public override string TypeName => "Axline";
        public override string TypeDescription => "AxisVM Line or Line as Members";
        public override string ToString() => this.Value.ToString();
    }
    public class AxisLineParameter : GH_PersistentParam<AxisLine>
    {
        public AxisLineParameter() : base("AxisVM Line", "AxLine", "AxisVM Line Parameter", "AxisVM", "Params") { }
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