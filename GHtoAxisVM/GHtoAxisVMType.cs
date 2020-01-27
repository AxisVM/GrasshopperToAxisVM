using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using AxisVM;
using System.Collections.Generic;

namespace GHtoAxisVM
{
    /// <summary>
    /// AxApp - AxisVMApplication type
    /// </summary>
    public class GH_AxApp : GH_Goo<AxAppAttr>
    {
        // Default Constructor, sets the state to Unknown.
        public GH_AxApp() => Value = AxAppAttr.Default;

        // Copy Constructor
        public GH_AxApp(GH_AxApp axModelSource)
        {
            Value.ModelName = axModelSource.Value.ModelName;
            Value.AxApp = axModelSource.Value.AxApp;
        }

        // Duplication method (technically not a constructor)
        public override IGH_Goo Duplicate() => new GH_AxApp(this);

        // instances are always valid
        public override bool IsValid => true;

        // Return a string with the name of this Type.
        public override string TypeName => "AxApp";

        // Return a string describing what this Type is about.
        public override string TypeDescription => "AxApp";

        // Return a string representation of the state (value) of this instance.
        public override string ToString() => Value.ModelName.ToString();

        // Return a single object COM that can be used by scripting components
        public override object ScriptVariable()
        {
            return Value.AxApp;
        }

    }

    public class AxAppAttr
    {
        public AxisVMApplication AxApp { get; set; }
        public string ModelName { get; set; }
        public AxisVMModels AxModels;
        public AxisVMModel AxModel;
        public AxisVMNodes AxNodes;
        public AxisVMLines AxLines;
        public AxisVMLine AxLine;
        public ELineGeomType geomType;
        public RLineGeomData geomData;

        public AxisVMCalculation AxCalc;
        public AxisVMMaterials AxMaterials;
        public AxisVMMaterial AxMaterial;
        public ENationalDesignCode code;
        public AxisVMCrossSections AxCrossSections;
        public AxisVMCrossSection AxCrossSection;
        public AxisVMMembers AxMembers;
        public AxisVMMember AxMember;
        public AxisVMLoadCases AxLoadCases;
        public AxisVMLoadCombinations AxLoadComb;
        public AxisVMLoads AxLoads;
        public AxisVMResults AxResults;
        public AxisVMWindows AxWindows;
        public AxisVMNodesSupports AxNodeSupport;

        //public List<string> Mstrs = new List<string>(); // list of material names already loaded
        //public List<int> MIDs = new List<int>(); // list of material IDs already loaded
        //public List<string> CSstrs = new List<string>(); // list of sect names already loaded
        //public List<int> CSIDs = new List<int>(); // list of sect IDs already loaded

        public List<Point3d> pts = new List<Point3d>();
        public List<Line> lns = new List<Line>();
        public List<int> sIDs = new List<int>();
        public List<int> eIDs = new List<int>();
        public List<int[]> membProps = new List<int[]>(); // csID, secID, typeID
        public List<int[]> nodeloadIDs = new List<int[]>(); // load case and loaded node
        public List<bool> sw = new List<bool>(); // line IDs for which self weight was defined
        public List<int> supNodeIDs = new List<int>();

        public static AxAppAttr Default { get; }

        static AxAppAttr()
        {
            Default = new AxAppAttr(modelName: "", axApp: null);
        }

        public AxAppAttr(string modelName = "", AxisVMApplication axApp = null)
        {
            this.ModelName = modelName;
            this.AxApp = axApp;
            if (axApp != null)
            {
                //Create new model
                this.AxModels = axApp.Models;
                this.AxModel = this.AxModels.Item[1];

                //create geometry
                this.AxNodes = this.AxModel.Nodes;
                this.AxLines = this.AxModel.Lines;

                //material, section
                //code = ENationalDesignCode.ndcEuroCode; //currently limited to Eurocode
                this.AxMaterials = this.AxModel.Materials;
                this.AxCrossSections = this.AxModel.CrossSections;

                //support
                this.AxNodeSupport = this.AxModel.NodesSupports;

                //load
                this.AxLoadCases = this.AxModel.LoadCases;
                this.AxLoads = this.AxModel.Loads;

                //calculation
                //this.AxCalc = this.AxModel.Calculation;
                //this.AxResults = this.AxModel.Results;
                //this.AxWindows = this.AxModel.Windows;
            }
        }

        public override string ToString() => $"{AxApp}";

        public AxAppAttr ShallowClone() => MemberwiseClone() as AxAppAttr;
    }

    /// <summary>
    /// AxMember - Axis Line Lement Type
    /// </summary>
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

    /// <summary>
    /// AxShell - Axis Shell Type
    /// </summary>
    public class GH_AxShell : GH_Goo<AxShellAttr>
    {
        // Default Constructor, sets the state to Unknown.
        public GH_AxShell() => this.Value = AxShellAttr.Default;

        // Copy Constructor
        public GH_AxShell(GH_AxShell axShellSource)
        {
            this.Value.Name = axShellSource.Value.Name;
            this.Value.Vertices = axShellSource.Value.Vertices; //TODO Lines
            this.Value.MatType = axShellSource.Value.MatType;
            this.Value.ElementType = axShellSource.Value.ElementType;
            this.Value.Thickness = axShellSource.Value.Thickness;
        }

        // Duplication method (technically not a constructor)
        public override IGH_Goo Duplicate() => new GH_AxShell(this);

        // instances are always valid
        public override bool IsValid => true;

        // Return a string with the name of this Type.
        public override string TypeName => "AxShell";

        // Return a string describing what this Type is about.
        public override string TypeDescription => "AxShell";

        // Return a string representation of the state (value) of this instance.
        public override string ToString() => this.Value.ToString();

        public override object ScriptVariable() => Value;

    }

    public class AxShellAttr
    {
        public string Name { get; set; }

        public List<Point3d> Vertices { get; set; }

        public string MatType { get; set; }

        public double Thickness { get; set; }

        public string ElementType { get; set; }

        public static AxShellAttr Default { get; }

        static AxShellAttr()
        {
            List<Point3d> vtdef = new List<Point3d>();
            Point3d defPt = new Point3d(){ X = 0, Y = 0, Z = 0 };
            vtdef.Add(defPt);
            Default = new AxShellAttr(mattype: "C20/25", vertices: vtdef, name: "DefaultAxShellAttr", thickness: 10);
        }

        public AxShellAttr(List<Point3d> vertices, string eltype = "Shell", string mattype = "C20/25", double thickness = 10, string name = null)
        {
            this.Name = name;
            this.Vertices = vertices;
            this.MatType = mattype;
            this.ElementType = eltype;
            this.Thickness = thickness;
        }

        public override string ToString() => $"{Name}, {MatType}, t: {Thickness:0.0}cm, {ElementType}";

        public AxShellAttr ShallowClone() => MemberwiseClone() as AxShellAttr;
    }



}
