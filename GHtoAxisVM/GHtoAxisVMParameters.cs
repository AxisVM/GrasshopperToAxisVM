using System;
using System.Collections.Generic;
using System.Linq;
using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System.Drawing;
using AxisVM;

namespace GHtoAxisVM
{
    /// <summary>
    /// AxApp - create AxisVMApplication and interfaces
    /// </summary>
    public class AxAppParameter : GH_PersistentParam<GH_AxApp>
    {
        public AxAppParameter() : base("AxApp parameter", "AxisVMApplication parameter", "AxisVM model's COM AxisVMApplication parameter", "AxisVM", "Params") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resources.icAxModel;
        public override Guid ComponentGuid => new Guid("{66cd6cbc-a78f-47b4-986e-c60a86471841}");
        protected override GH_GetterResult Prompt_Singular(ref GH_AxApp value)
        {
            value = new GH_AxApp();
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<GH_AxApp> values)
        {
            values = new List<GH_AxApp>();
            return GH_GetterResult.success;
        }
    }

    /// <summary>
    /// AxMember -  line members in axis
    /// </summary>
    public class AxMemberParameter : GH_PersistentParam<GH_AxMember>
    {
        public AxMemberParameter() : base("AxMember parameter", "AxMember", "AxisVM Member parameter", "AxisVM", "Params") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resources.icAxMembPar;
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

    /// <summary>
    /// AxShell -  domain in axis
    /// </summary>
    public class AxShellParameter : GH_PersistentParam<GH_AxShell>
    {
        public AxShellParameter() : base("AxShell parameter", "AxShell", "AxisVM Domain parameter", "AxisVM", "Params") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resources.icAxShellPar;
        public override Guid ComponentGuid => new Guid("{349fefe4-9725-4783-972d-a77f982c49d5}");
        protected override GH_GetterResult Prompt_Singular(ref GH_AxShell value)
        {
            value = new GH_AxShell();
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<GH_AxShell> values)
        {
            values = new List<GH_AxShell>();
            return GH_GetterResult.success;
        }
    }

}
