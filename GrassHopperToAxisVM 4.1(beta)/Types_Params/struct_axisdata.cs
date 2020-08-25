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
        public class AxisData
        {
            public static AxisData Default { get; }

            public string filePath = null;
            public bool isNew           = false;      
            public bool justPoints      = false;
            public bool axAnalysis      = false;
            public bool checkDouble     = false;
            public bool sendOnlyChanges = false;

            public AxisData(string filePath, bool isNew, bool justPoints, bool axAnalysis, bool checkDouble, bool sendOnlyChanges)
            {
                this.filePath = filePath;
                this.isNew = isNew;
                this.justPoints = justPoints;
                this.axAnalysis = axAnalysis;
                this.checkDouble = checkDouble;
                this.sendOnlyChanges = sendOnlyChanges;
            }
            public AxisData() { }
            static AxisData() { Default = new AxisData(); }

        }
    }

    public class AxisData : GH_Goo<Structs.AxisData>
    {
        public AxisData(AxisData axLineSource)
        {
            this.Value.filePath =       axLineSource.Value.filePath;
            this.Value.isNew =          axLineSource.Value.isNew;
            this.Value.justPoints =     axLineSource.Value.justPoints;
            this.Value.axAnalysis =     axLineSource.Value.axAnalysis;
            this.Value.checkDouble =    axLineSource.Value.checkDouble;
            this.Value.sendOnlyChanges =axLineSource.Value.sendOnlyChanges;
        }
        public AxisData(string filePath, bool isNew, bool justPoints, bool axAnalysis, bool checkDouble, bool sendOnlyChanges)
        {
            this.Value = new Structs.AxisData(filePath,isNew,justPoints,axAnalysis,checkDouble,sendOnlyChanges);
        }
        public AxisData() => this.Value = Structs.AxisData.Default;
        public override IGH_Goo Duplicate() => new AxisData(this);
        public override bool IsValid => true;
        public override string TypeName => "AxSettings";
        public override string TypeDescription => "AxisVM Settings";
        public override string ToString() => this.Value.ToString();
    }
    public class AxisDataParameter : GH_PersistentParam<AxisData>
    {
        public AxisDataParameter() : base("AxisVM Settings", "AxStt", "AxisVM Settings", "AxisVM", "Params") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resource1.ic_ParAxSettings;
        public override Guid ComponentGuid => new Guid("259e6fd3-a029-4e9f-8121-f523c28b23fc");
        protected override GH_GetterResult Prompt_Singular(ref AxisData value)
        {
            value = new AxisData();
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<AxisData> values)
        {
            values = new List<AxisData>();
            return GH_GetterResult.success;
        }
    }
}

//UNNECCESSARY AT THE MOMENT