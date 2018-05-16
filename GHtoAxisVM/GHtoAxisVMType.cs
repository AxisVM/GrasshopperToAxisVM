using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace GHtoAxisVM
{
    public class GH_AxMember : GH_Goo<AxMembAttr>
    {
        // Default Constructor, sets the state to Unknown.
        public GH_AxMember() => this.Value = AxMembAttr.Default;

        // Copy Constructor
        public GH_AxMember(GH_AxMember axMemberSource)
        {
            this.Value.Name = axMemberSource.Value.Name;
            this.Value.Ln = axMemberSource.Value.Ln;
            this.Value.MatType = axMemberSource.Value.MatType;
            this.Value.ElementType = axMemberSource.Value.ElementType;
            this.Value.SectType = axMemberSource.Value.SectType;
        }

        // Duplication method (technically not a constructor)
        public override IGH_Goo Duplicate() => new GH_AxMember(this);

        // instances are always valid
        public override bool IsValid => true;

        // Return a string with the name of this Type.
        public override string TypeName => "AxMember";

        // Return a string describing what this Type is about.
        public override string TypeDescription => "AxMember";

        // Return a string representation of the state (value) of this instance.
        public override string ToString() => this.Value.ToString();

        //public override object ScriptVariable() => Value;

    }

    public class AxMembAttr
    {
        public string Name { get; set; }

        public Line Ln { get; set; }

        /// <summary>
        /// Material
        /// </summary>
        public string MatType { get; set; }

        public string SectType { get; set; }

        public string ElementType { get; set; }

        public static AxMembAttr Default { get; }

        static AxMembAttr()
        {
            Line lndef = new Line();
            lndef.FromX = 0; lndef.ToX = 0;
            lndef.FromY = 0; lndef.ToY = 0;
            lndef.FromZ = 0; lndef.ToZ = 0;
            Default = new AxMembAttr(mattype: "S 235", ln: lndef, name: "DefaultAxMembAttr", sectype: "IPE 240");
        }

        public AxMembAttr(Line ln, string eltype = "Beam", string mattype = "S 235", string sectype = "IPE 240", string name = null)
        {
            this.Name = name;
            this.Ln = ln;
            this.MatType = mattype;
            this.ElementType = eltype;
            this.SectType = sectype;
        }
    
        public override string ToString() => $"{Name}, {MatType}, {SectType}, L: {Ln.Length:0.0}m, {ElementType}";

        public AxMembAttr ShallowClone() => MemberwiseClone() as AxMembAttr;
    }

    
}
