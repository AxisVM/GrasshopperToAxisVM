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
        public class Material
        {
            public string designCode;
            public string name;
            public Material(string designCode, string name)
            {
                this.designCode = designCode; this.name = name;
            }
            public Material() : this(null, null) { }
            public static Material Default { get; }
            static Material()
            {
                Default = new Material(null,null);
            }
            public override bool Equals(Object obj)
            {
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    Material p = (Material)obj;
                    return this.name.Equals(p.name) && this.designCode.Equals(p.designCode);
                }
            }
            public Material Clone() { return new Material(designCode,name); }

            public override int GetHashCode()
            {
                return 1;
            }
        }
    }

    public class Material : GH_Goo<Structs.Material>
    {
        public Material(Material axLineSource=null)
        {
            if (axLineSource != null) this.Value = new Structs.Material(axLineSource.Value.designCode, axLineSource.Value.name);
            else { this.Value = null; }
        }
        public Material(string designCode, string name)
        {
            this.Value = new Structs.Material(designCode, name);
        }
        public Material() => this.Value = Structs.Material.Default;
        public override IGH_Goo Duplicate() => new Material(this);
        public override bool IsValid => true;
        public override string TypeName => "AxMAT";
        public override string TypeDescription => "AxisVM Material";
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
    public class MaterialParameter : GH_PersistentParam<Material>
    {
        public MaterialParameter() : base("AxisVM Material", "AxMAT", "AxisVM Material", "AxisVM", "Attr") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override Bitmap Icon => Properties.Resource1.ic_ParAxMaterials;
        public override Guid ComponentGuid => new Guid("08ed08ea-a439-4a64-91e4-92000a3d5f66");
        protected override GH_GetterResult Prompt_Singular(ref Material value)
        {
            value = new Material();
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Plural(ref List<Material> values)
        {
            values = new List<Material>();
            return GH_GetterResult.success;
        }
    }
}
