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
        public class AxNodalLoad
        {
            public static AxNodalLoad Default { get; }
            public double Rx = -1;
            public double Ry = -1;
            public double Rz = -1;
            public double Rxx = -1;
            public double Ryy = -1;
            public double Rzz = -1;
            public AxNodalLoad(double Rx, double Ry, double Rz, double Rxx, double Ryy, double Rzz)
            {
                this.Rx = Rx;
                this.Ry = Ry;
                this.Rz = Rz;
                this.Rxx = Rxx;
                this.Ryy = Ryy;
                this.Rzz = Rzz;
            }
            public AxNodalLoad()
            {

            }
            static AxNodalLoad()
            {
                Default = new AxNodalLoad();
            }
        }
    }

    public class AxNodalLoad : GH_Goo<structs.AxNodalLoad>
    {
        public AxNodalLoad(AxNodalLoad axLineSource)
        {
            this.Value.Rx = axLineSource.Value.Rx;
            this.Value.Ry = axLineSource.Value.Ry;
            this.Value.Rz = axLineSource.Value.Rz;
            this.Value.Rxx = axLineSource.Value.Rxx;
            this.Value.Ryy = axLineSource.Value.Ryy;
            this.Value.Rzz = axLineSource.Value.Rzz;
        }
        public AxNodalLoad(double Rx, double Ry, double Rz, double Rxx, double Ryy, double Rzz)
        {
            this.Value = new structs.AxNodalLoad(Rx, Ry, Rz, Rxx, Ryy, Rzz);
        }
        public AxNodalLoad() => this.Value = structs.AxNodalLoad.Default;
        public override IGH_Goo Duplicate() => new AxNodalLoad(this);
        public override bool IsValid => true;
        public override string TypeName => "AxNodalLoad";
        public override string TypeDescription => "AxisVM Nodal Load";
        public override string ToString() => this.Value.ToString();
    }
    public class AxNodalLoadParameter : GH_PersistentParam<AxNodalLoad>
    {
        public AxNodalLoadParameter() : base("AxisVM NodealLoad", "AxNL", "AxisVM NodalLoad", "AxisVM", "Params") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resource1.icParFixedSupport;
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
