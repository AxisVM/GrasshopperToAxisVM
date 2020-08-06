using System;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System.Drawing;
using System.Collections.Generic;
using Rhino.Geometry;
using System.Linq;

namespace GrassHopperToAxisVM
{
    public partial class Structs
    {
        public class AxisPoint : AxisMember
        {
            public List<Point3d> point;
            public AxisNodeSup sup;
            public AxisPoint(List<Point3d> point,AxisNodeSup sup, int id =-1) : base(id)
            {
                this.point = point;
                this.sup = sup;
            }
            public AxisPoint(List<Point3d> point) : this(point, new AxisNodeSup()){}
            public static AxisPoint Default { get; }
            static AxisPoint()
            {
                Default = new AxisPoint(new List<Point3d>(),new Structs.AxisNodeSup());
            }

            public override bool Equals(Object obj)
            {
                bool returner = true;
                //Check for null and compare run-time types.
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    AxisPoint p = (AxisPoint)obj;
                    if (this.sup != null && p.sup != null)
                        return this.sup.Equals(p.sup) && base.Equals(p) && this.point.SequenceEqual(p.point);
                    else if (this.sup == null && p.sup == null)
                        return this.point.SequenceEqual(p.point) && base.Equals(p);
                    else return false;
                }
            }

            public override bool GeometryEquals(Object obj)
            {
                bool returner = true;
                //Check for null and compare run-time types.
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    AxisPoint p = (AxisPoint)obj;
                    if (this.sup != null && p.sup != null)
                        return this.sup.Equals(p.sup) && base.Equals(p) && this.point.SequenceEqual(p.point);
                    else if (this.sup == null && p.sup == null)
                        return this.point.SequenceEqual(p.point) && base.Equals(p);
                    else return false;
                }
            }

            public override int GetHashCode()
            {
                return this.id;
            }

            public override AxisMember Clone()
            {
                AxisPoint returner = null;
                if (this.sup != null)
                    returner = new AxisPoint(point.ToList(), sup.Clone(), id);
                else returner= new AxisPoint(point.ToList(), null, id);
                returner.axisid_point = this.axisid_point;
                return returner;
            }
        }
    }
    public class AxisPoint : GH_Goo<Structs.AxisPoint>
    {
        public AxisPoint() => this.Value = Structs.AxisPoint.Default;

        public AxisPoint(AxisPoint axMemberSource)
        {
            this.Value.point = axMemberSource.Value.point;
            this.Value.sup = axMemberSource.Value.sup;
        }
        public AxisPoint(List<Point3d> p, Structs.AxisNodeSup sup)
        {
            this.Value = new Structs.AxisPoint(p,sup);
        }

        public override IGH_Goo Duplicate() => new AxisPoint(this);
        public override bool IsValid => true;
        public override string TypeName => "AxP";
        public override string TypeDescription => "AxisVM Point";
        public override string ToString() => this.Value.ToString();

        public override bool Equals(Object obj)
        {
            bool returner = true;
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                AxisPoint p = (AxisPoint)obj;
                return this.Value.Equals(p.Value);
            }
        }

        public override int GetHashCode()
        {
            return this.Value.id;
        }
    }
    public class AxisPointParameter : GH_PersistentParam<AxisPoint>
    {
        public AxisPointParameter() : base("AxisVM Point", "AxP", "AxisVM Point Parameter", "AxisVM", "Params") { }
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