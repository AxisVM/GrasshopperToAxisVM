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
        public class AxisNodeSup
        {
            public static AxisNodeSup Default { get; }
            public double Rx = -1;
            public double Ry = -1;
            public double Rz = -1;
            public double Rxx =-1;
            public double Ryy =-1;
            public double Rzz =-1;
            public AxisNodeSup(double Rx, double Ry, double Rz, double Rxx, double Ryy, double Rzz)
            {
                this.Rx = Rx;
                this.Ry = Ry;
                this.Rz = Rz;
                this.Rxx = Rxx;
                this.Ryy = Ryy;
                this.Rzz = Rzz;
            }
            public AxisNodeSup()
            {

            }
            static AxisNodeSup()
            {
                Default = new AxisNodeSup();
            }
            public override int GetHashCode()
            {
                return Convert.ToInt32(this.Rz);
            }

            public override bool Equals(object obj)
            {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    AxisNodeSup p = (AxisNodeSup)obj;
                    return this.Rx == p.Rx && this.Rxx == p.Rxx && this.Ry == p.Ry && this.Ryy == p.Ryy && this.Rz == p.Rz && this.Rzz == p.Rzz;
                }
            }
            public AxisNodeSup Clone()
            {
                return new AxisNodeSup(Rx, Ry, Rz, Rxx, Ryy, Rzz);
            }
        }
    }

    public class AxisNodeSup : GH_Goo<Structs.AxisNodeSup>
    {
        public AxisNodeSup(AxisNodeSup axLineSource)
        {
            this.Value.Rx = axLineSource.Value.Rx;
            this.Value.Ry = axLineSource.Value.Ry;
            this.Value.Rz = axLineSource.Value.Rz;
            this.Value.Rxx = axLineSource.Value.Rxx;
            this.Value.Ryy = axLineSource.Value.Ryy;
            this.Value.Rzz = axLineSource.Value.Rzz;
        }
        public AxisNodeSup(double Rx, double Ry, double Rz, double Rxx, double Ryy, double Rzz)
        {
            this.Value = new Structs.AxisNodeSup(Rx, Ry, Rz, Rxx, Ryy, Rzz);
        }
        public AxisNodeSup() => this.Value = Structs.AxisNodeSup.Default;
        public override IGH_Goo Duplicate() => new AxisNodeSup(this);
        public override bool IsValid => true;
        public override string TypeName => "AxNS";
        public override string TypeDescription => "AxisVM NodeSupport";
        public override string ToString() => this.Value.ToString();

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                AxisNodeSup p = (AxisNodeSup)obj;
                return this.Value.Equals(p.Value);
            }
        }
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }
    }
    public class AxisNodeSupParameter : GH_PersistentParam<AxisNodeSup>
    {
        public AxisNodeSupParameter() : base("AxisVM Node Support", "AxNS", "AxisVM NodeSupport", "AxisVM", "Params") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resource1.icParFixedSupport;
        public override Guid ComponentGuid => new Guid("6e022dea-d572-48dc-9f7b-f568cf5f5295");
        protected override GH_GetterResult Prompt_Singular(ref AxisNodeSup value)
        {
            value = new AxisNodeSup();
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<AxisNodeSup> values)
        {
            values = new List<AxisNodeSup>();
            return GH_GetterResult.success;
        }
    }
    
}
