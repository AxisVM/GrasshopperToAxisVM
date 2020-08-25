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
        public class AxNodalLoad : AxisLoad
        {
            public Structs.AxisPoint points;
            public double Rx = 0;
            public double Ry = 0;
            public double Rz = 0;
            public double Rxx = 0;
            public double Ryy = 0;
            public double Rzz = 0;
            public AxNodalLoad(Structs.AxisPoint points, double Rx, double Ry, double Rz, double Rxx, double Ryy, double Rzz, int id = -1) : base(id)
            {
                this.points = points;
                this.Rx = Rx;
                this.Ry = Ry;
                this.Rz = Rz;
                this.Rxx = Rxx;
                this.Ryy = Ryy;
                this.Rzz = Rzz;
            }
            public static AxNodalLoad Default { get; }
            static AxNodalLoad()
            {
                Default = new AxNodalLoad(Structs.AxisPoint.Default, 0, 0, 0, 0, 0, 0);
            }

            public override bool Equals(Object obj)
            {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    AxNodalLoad p = (AxNodalLoad)obj;
                    bool eq = true;
                    eq = eq && ((this.points != null && p.points != null && this.points.Equals(p.points)) || (this.points == null && p.points == null));
                    return eq && this.Rx == p.Rx && this.Rxx == p.Rxx && this.Ry == p.Ry && this.Ryy == p.Ryy && this.Rz == p.Rz && this.Rzz == p.Rzz;
                }
            }

            public override int GetHashCode()
            {
                return this.id;
            }

            public override AxisLoad Clone() { return new AxNodalLoad((AxisPoint)points.Clone(),Rx,Ry,Rz,Rxx,Ryy,Rzz,id); }
        }
    }

    public class AxNodalLoad : GH_Goo<Structs.AxNodalLoad>
    {
        public AxNodalLoad(AxNodalLoad axLineSource)
        {
            this.Value.points = axLineSource.Value.points;
            this.Value.Rx = axLineSource.Value.Rx;
            this.Value.Ry = axLineSource.Value.Ry;
            this.Value.Rz = axLineSource.Value.Rz;
            this.Value.Rxx = axLineSource.Value.Rxx;
            this.Value.Ryy = axLineSource.Value.Ryy;
            this.Value.Rzz = axLineSource.Value.Rzz;
        }
        public AxNodalLoad(Structs.AxisPoint point, double Rx, double Ry, double Rz, double Rxx, double Ryy, double Rzz)
        {
            this.Value = new Structs.AxNodalLoad(point, Rx, Ry, Rz, Rxx, Ryy, Rzz);
        }
        public AxNodalLoad() => this.Value = Structs.AxNodalLoad.Default;
        public override IGH_Goo Duplicate() => new AxNodalLoad(this);
        public override bool IsValid => true;
        public override string TypeName => "AxNL";
        public override string TypeDescription => "AxisVM Nodal Load";
        public override string ToString() => this.Value.ToString();

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                AxNodalLoad p = (AxNodalLoad)obj;
                return this.Value.Equals(p.Value);
            }
        }

        public override int GetHashCode()
        {
            return this.Value.id;
        }
    }
    public class AxNodalLoadParameter : GH_PersistentParam<AxNodalLoad>
    {
        public AxNodalLoadParameter() : base("AxisVM Nodal Load", "AxNL", "AxisVM Nodal Load", "AxisVM", "Params") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resource1.ic_ParAxLoadNodal;
        public override Guid ComponentGuid => new Guid("82e675f3-543d-4659-bd1e-670583e7b539");
        protected override GH_GetterResult Prompt_Singular(ref AxNodalLoad value)
        {
            value = new AxNodalLoad();
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<AxNodalLoad> values)
        {
            values = new List<AxNodalLoad>();
            return GH_GetterResult.success;
        }
    }
}
