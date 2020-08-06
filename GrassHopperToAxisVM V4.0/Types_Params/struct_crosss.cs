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
        public class CrossSection
        {
            public string shape;
            public string name;
            public CrossSection(string shape, string name)
            {
                this.shape = shape; this.name = name;
            }
            public CrossSection() : this(null, null) { }
            public static CrossSection Default { get; }
            static CrossSection()
            {
                Default = new CrossSection(null, null);
            }
            public override bool Equals(Object obj)
            {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    CrossSection p = (CrossSection)obj;
                    return this.name.Equals(p.name) && this.shape.Equals(p.shape);
                }
            }
            public override int GetHashCode()
            {
                return 1;
            }
            public CrossSection Clone() { return new CrossSection(shape, name); }
        }
    }

    public class CrossSection : GH_Goo<Structs.CrossSection>
    {
        public CrossSection(CrossSection axLineSource=null)
        {
            if (axLineSource != null) this.Value = new Structs.CrossSection(axLineSource.Value.shape, axLineSource.Value.name);
            else { this.Value = null; }

        }
        public CrossSection(string shape, string name)
        {
            this.Value = new Structs.CrossSection(shape, name);
        }
        public CrossSection() => this.Value = Structs.CrossSection.Default;
        public override IGH_Goo Duplicate() => new CrossSection(this);
        public override bool IsValid => true;
        public override string TypeName => "AxMAT";
        public override string TypeDescription => "AxisVM CrossSection";
        public override string ToString() => this.Value.ToString();
        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Material p = (Material)obj;
                return p.Value.Equals(this.Value);
            }
        }
        public override int GetHashCode()
        {
            return 1;
        }
    }
    public class CrossSectionParameter : GH_PersistentParam<CrossSection>
    {
        public CrossSectionParameter() : base("AxisVM CrossSection", "AxMat", "AxisVM CrossSection", "AxisVM", "Attr") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resource1.ic_ParAxCrossSections;
        public override Guid ComponentGuid => new Guid("2eda6d59-12c4-4c67-a3ff-28aeaedc8504");
        protected override GH_GetterResult Prompt_Singular(ref CrossSection value)
        {
            value = new CrossSection();
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<CrossSection> values)
        {
            values = new List<CrossSection>();
            return GH_GetterResult.success;
        }
    }
}
