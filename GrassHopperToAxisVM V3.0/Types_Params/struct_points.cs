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
        public class AxisPoint 
        {
            public List<Point3d> point;
            public structs.AxisNodeSup sup;
            public AxisPoint(List<Point3d> point,structs.AxisNodeSup sup)
            {
                this.point = point;
                this.sup = sup;
            }
            public AxisPoint(List<Point3d> point)
            {
                this.point = point;
            }
            public static AxisPoint Default { get; }

            static AxisPoint()
            {
                Default = new AxisPoint(new List<Point3d>(),new structs.AxisNodeSup());
            }
        }
    }
    public class AxisPoint : GH_Goo<structs.AxisPoint>
    {
        public AxisPoint() => this.Value = structs.AxisPoint.Default;

        public AxisPoint(AxisPoint axMemberSource)
        {
            this.Value.point = axMemberSource.Value.point;
            this.Value.sup = axMemberSource.Value.sup;
        }
        public AxisPoint(List<Point3d> p, structs.AxisNodeSup sup)
        {
            this.Value = new structs.AxisPoint(p,sup);
        }
        public AxisPoint(List<Point3d> p)
        {
            this.Value = new structs.AxisPoint(p);
        }

        public override IGH_Goo Duplicate() => new AxisPoint(this);
        public override bool IsValid => true;
        public override string TypeName => "AxPoint";
        public override string TypeDescription => "Axis VM Point";
        public override string ToString() => this.Value.ToString();
    }
    public class AxisPointParameter : GH_PersistentParam<AxisPoint>
    {
        public AxisPointParameter() : base("AxisVM Point", "AxPoint", "AxisVM Point Parameter", "AxisVM", "Params") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resource1.ic_ParAxPoint;
        public override Guid ComponentGuid => new Guid("71940936-b889-4870-a805-b5ca33b4fcdb");
        protected override GH_GetterResult Prompt_Singular(ref AxisPoint value)
        {
            value = new AxisPoint();
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<AxisPoint> values)
        {
            values = new List<AxisPoint>();
            return GH_GetterResult.success;
        }
    }

}