using System;
using System.Collections.Generic;
using System.Linq;
using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System.Drawing;

namespace GHtoAxisVM
{
    public class AxMemberParameter : GH_PersistentParam<GH_AxMember>
    {
        public AxMemberParameter() : base("AxMember parameter", "AxMember", "AxisVM Member parameter", "AxisVM", "AxisVM") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resources.icAxMemb;
        public override Guid ComponentGuid => new Guid("{1ef4dd7a-dfb3-447d-a3c6-2efc177e727c}");
        protected override GH_GetterResult Prompt_Singular(ref GH_AxMember value)
        {
            value = new GH_AxMember();
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<GH_AxMember> values)
        {
            values = new List<GH_AxMember>();
            return GH_GetterResult.success;
        }
    }
}
