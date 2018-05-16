using System;
using Grasshopper.Kernel;
using AxisVM;
using Rhino.Geometry;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel.Attributes;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using System.Text.RegularExpressions;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace GHtoAxisVM
{
    public class AxSendLinesComp : GH_Component
    {
        List<Line> ln = new List<Line>();

        public AxSendLinesComp() : //base("Sample", "ASpi","Construct an Archimedean, or arithmetic, spiral given its radii and number of turns.","Curve", "Primitive")
            base("Send Lines To AxisVM", "LnToAxisVM", "sends the lines to AxisVM", "AxisVM", "AxisVM")
        {
        }

        public override Guid ComponentGuid => new Guid("35ed95a6-bd54-49e5-a413-73d7855a4364"); 

        protected override Bitmap Icon => Properties.Resources.icLineToAxis;

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddLineParameter("Line", "L", "list of lines that will become beams", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        public class SettingsComponentAttributes : GH_ComponentAttributes
        {
            public SettingsComponentAttributes(AxSendLinesComp cmp) : base(cmp) { }

            public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
            {
                ((AxSendLinesComp)Owner).StartAx(((AxSendLinesComp)Owner).ln);
                return GH_ObjectResponse.Handled;
            }
        }

        public override void CreateAttributes()
        {
            Attributes = new SettingsComponentAttributes(this);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // this will only read the input, while Axis starts after double-clicking on the component
            // Use the DA object to retrieve the data inside the first input parameter.
            // If the retieval fails (for example if there is no data) we need to abort.
            ln.Clear();
            DA.GetDataList(0, ln);
        }

        public string StartAx(List<Line> ln)
        {
            AxisVMApplication AxApp = new AxisVMApplication();
            AxisVMModels AxModels = new AxisVMModels();
            AxisVMModel AxModel = new AxisVMModel();
            AxisVMNodes AxNodes = new AxisVMNodes();
            AxisVMLines AxLines = new AxisVMLines();
            AxisVMLine AxLine = new AxisVMLine();
            ELineGeomType geomType = new ELineGeomType();
            RLineGeomData geomData = new RLineGeomData();

            //Show AxisVM GUI and setup AxisVM to remain opened when COM client finished
            AxApp.CloseOnLastReleased = ELongBoolean.lbFalse; //Axis doesn't exit when script finishes
            AxApp.AskCloseOnLastReleased = ELongBoolean.lbFalse; //Show close dialog before exit
            AxApp.Visible = ELongBoolean.lbFalse; //set on lbFalse can improve speed

            //Create new model
            AxModels = AxApp.Models;
            AxModel = AxModels.Item[AxModels.New()];

            //create nodes
            AxNodes = AxModel.Nodes;

            List<Point3d> pt = new List<Point3d>();
            Point3d newPt = new Point3d();
            for (int i = 0; i < ln.Count; i++)
            {
                if (ln[i].Length > 0)
                {
                    newPt = ln[i].From;
                    if (GetPointID(pt, newPt, 0.001) == -1)
                    {
                        pt.Add(newPt);
                    }
                    newPt = ln[i].To;
                    if (GetPointID(pt, newPt, 0.001) == -1)
                    {
                        pt.Add(newPt);
                    }
                }
            }

            for (int i = 0; i < pt.Count; i++)
            {
                AxNodes.Add(pt[i].X, pt[i].Y, pt[i].Z);
            }

            //create lines, elements
            AxLines = AxModel.Lines;
            RPoint3d exc = new RPoint3d { x = 0, y = 0, z = 0 };
            int IDs = -1;
            int IDe = -1;
            int notValidLineCount = 0;
            for (int i = 0; i < ln.Count; i++)
            {
                if (ln[i].Length > 0)
                {
                    IDs = GetPointID(pt, ln[i].From, 0.001);
                    IDe = GetPointID(pt, ln[i].To, 0.001);
                    if ((IDs >= 0) && (IDe >= 0))
                    {
                        AxLines.Add(IDs, IDe, geomType, geomData);
                        AxLine = AxLines.Item[i + 1 - notValidLineCount];
                    }
                    else return "node list is not complete";
                }
                else { notValidLineCount++; }
            }

            AxApp.Visible = ELongBoolean.lbTrue;
            AxApp.BringToFront();

            return "ok";
        }

        private int GetPointID(List<Point3d> L, Point3d p, double tol)
        {
            for (int i = 0; i < L.Count; i++)
            {
                if ((Math.Abs(L[i].X - p.X) < tol) & (Math.Abs(L[i].Y - p.Y) < tol) & (Math.Abs(L[i].Z - p.Z) < tol))
                {
                    return i + 1; //numbering in Axis starts with 1
                }
            }
            return -1;
        }

    }

    public class AxMembersComp : GH_Component
    {

        public AxMembersComp() : //base("Sample", "ASpi","Construct an Archimedean, or arithmetic, spiral given its radii and number of turns.","Curve", "Primitive")
            base("AxMembers (AxisVM)", "AxMembers", "create AxisVM members", "AxisVM", "AxisVM")
        {
        }

        public override Guid ComponentGuid => new Guid("a72bd44d-e230-4e0b-9cc8-7f6dcea0e29a"); 

        protected override Bitmap Icon => Properties.Resources.icAxMemb;

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddLineParameter("Line", "L", "list of lines that will become beams", GH_ParamAccess.list);
            pManager.AddTextParameter("Name", "Name", "element or group name", GH_ParamAccess.item);
            pManager.AddTextParameter("Mat", "Mat", "material", GH_ParamAccess.item);
            pManager.AddTextParameter("Cross-section", "CS", "Cross-section", GH_ParamAccess.item);
            pManager.AddTextParameter("Type", "Type", "element type", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new AxMemberParameter(),"AxMember", "Memb", "AxisVM Member", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Declare variables for the input
            List<Line> ln = new List<Line>();
            string Name = "";
            string TypeName = "Beam";
            string MatName = "S 235";
            string SectName = "IPE 240";

            // Use the DA object to retrieve the data inside the first input parameter.
            // If the retieval fails (for example if there is no data) we need to abort.
            if (!DA.GetDataList(0, ln)) { return; }
            if (!DA.GetData(1, ref Name)) { return; }
            if (!DA.GetData(2, ref MatName)) { return; }
            if (!DA.GetData(3, ref SectName)) { return; }
            if (!DA.GetData(4, ref TypeName)) { return; }

            List<GH_AxMember> axm = new List<GH_AxMember>();
            int NotValidLnCount = 0;

            for (int i = 0; i < ln.Count; i++)
            {
                if (ln[i].Length > 0)
                {
                    axm.Add(new GH_AxMember());
                    axm[i - NotValidLnCount].Value = new AxMembAttr(ln[i], TypeName, MatName, SectName, Name);
                }
                else { NotValidLnCount++; };
            }
            // Use the DA object to assign a new String to the first output parameter.
            DA.SetDataList(0, axm);

        }

    }

    public class AxSendMembersComp : GH_Component
    {
        List<GH_AxMember> Mb = new List<GH_AxMember>();

        public AxSendMembersComp() : //base("Sample", "ASpi","Construct an Archimedean, or arithmetic, spiral given its radii and number of turns.","Curve", "Primitive")
            base("Send Members To AxisVM", "MembToAxisVM", "sends the members to AxisVM", "AxisVM", "AxisVM")
        {
        }

        public override Guid ComponentGuid => new Guid("60a6d100-4518-4fee-b9bd-0e5d7071e5e9");

        protected override Bitmap Icon => Properties.Resources.icMembToAxis;

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new AxMemberParameter(), "AxisVM member", "AxMember", "AxisVM structural element", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        public class SettingsComponentAttributes : GH_ComponentAttributes
        {
            public SettingsComponentAttributes(AxSendMembersComp cmp) : base(cmp) { }

            public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
            {
                ((AxSendMembersComp)Owner).StartAx(((AxSendMembersComp)Owner).Mb);
                return GH_ObjectResponse.Handled;
            }
        }

        public override void CreateAttributes()
        {
            Attributes = new SettingsComponentAttributes(this);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Mb.Clear();
            if (!DA.GetDataList(0, Mb)) { return; }
        }

        public string StartAx(List<GH_AxMember> mb)
        {
            AxisVMApplication AxApp = new AxisVMApplication();
            AxisVMModels AxModels = new AxisVMModels();
            AxisVMModel AxModel = new AxisVMModel();
            AxisVMMaterials AxMaterials = new AxisVMMaterials();
            AxisVMMaterial AxMaterial = new AxisVMMaterial();
            ENationalDesignCode code = new ENationalDesignCode();
            AxisVMCrossSections AxCrossSections = new AxisVMCrossSections();
            AxisVMCrossSection AxCrossSection = new AxisVMCrossSection();
            AxisVMNodes AxNodes = new AxisVMNodes();
            AxisVMLines AxLines = new AxisVMLines();
            AxisVMLine AxLine = new AxisVMLine();
            ELineGeomType geomType = new ELineGeomType();
            RLineGeomData geomData = new RLineGeomData();
            AxisVMMembers AxisMembers = new AxisVMMembers();
            AxisVMNodalSupports AxNodalSupports = new AxisVMNodalSupports();
            AxisVMMembers AxMembers = new AxisVMMembers();
            AxisVMMember AxMember = new AxisVMMember();
            //RRelease AxRelease = new RRelease();

            //Show AxisVM GUI and setup AxisVM to remain opened when COM client finished
            AxApp.CloseOnLastReleased = ELongBoolean.lbFalse; //Axis doesn't exit when script finishes
            AxApp.AskCloseOnLastReleased = ELongBoolean.lbFalse; //Show close dialog before exit
            AxApp.Visible = ELongBoolean.lbFalse; //set on lbFalse can improve speed

            //Create new model
            AxModels = AxApp.Models;
            AxModel = AxModels.Item[AxModels.New()];

            //Create material
            code = ENationalDesignCode.ndcEuroCode; //currently limited to Eurocode
            AxMaterials = AxModel.Materials;
            int[] MatID = new int[mb.Count]; //material ID for each structural member
            List<string> Mstrs = new List<string>(); // list of material names already loaded
            List<int> MIDs = new List<int>(); // list of material IDs already loaded
            StringComparison sc = StringComparison.CurrentCultureIgnoreCase;

            for (int i = 0; i < mb.Count; i++)
            {
                string Mstr = mb[i].Value.MatType;

                //chcek if this material has already been defined or not
                bool alreadyDefined = false;
                for (int j = 0; j < Mstrs.Count; j++)
                { if (Mstrs[j].Equals(Mstr, sc)) { MatID[i] = MIDs[j]; alreadyDefined = true; } }

                if (!alreadyDefined)
                {
                    MatID[i] = AxMaterials.AddFromCatalog(code, Mstr);
                    Mstrs.Add(Mstr);
                    MIDs.Add(MatID[i]);
                    AxMaterial = AxMaterials.Item[MatID[i]];
                }
            }

            //Add cross sections
            AxCrossSections = AxModel.CrossSections;
            int[] SectID = new int[mb.Count]; //section ID for each structural member
            List<string> CSstrs = new List<string>(); // list of sect names already loaded
            List<int> CSIDs = new List<int>(); // list of sect IDs already loaded

            for (int i = 0; i < mb.Count; i++)
            {
                string CSstr = mb[i].Value.SectType;

                //chcek if this cross-section has already been defined or not
                bool alreadyDefined = false;
                for (int j = 0; j < CSstrs.Count; j++)
                    { if (CSstrs[j].Equals(CSstr,sc)) { SectID[i] = CSIDs[j]; alreadyDefined = true; } }
                
                if (!alreadyDefined)
                {
                    SectID[i] = GetCrossSection(CSstr, sc, AxCrossSections); //currently limited to pipe, I, Box
                    CSstrs.Add(CSstr);
                    CSIDs.Add(SectID[i]);
                    AxCrossSection = AxCrossSections.Item[SectID[i]];
                }
            }          

            //create nodes
            AxNodes = AxModel.Nodes;

            List<Point3d> pt = new List<Point3d>();
            Point3d newPt = new Point3d();
            for (int i = 0; i < mb.Count; i++)
            {
                newPt = mb[i].Value.Ln.From;
                if (GetPointID(pt, newPt, 0.001) == -1)
                {
                    pt.Add(newPt);
                }
                newPt = mb[i].Value.Ln.To;
                if (GetPointID(pt, newPt, 0.001) == -1)
                {
                    pt.Add(newPt);
                }
            }

            for (int i = 0; i < pt.Count; i++)
            {
                AxNodes.Add(pt[i].X, pt[i].Y, pt[i].Z);
            }

            //create lines, elements
            AxLines = AxModel.Lines;
            RPoint3d exc = new RPoint3d { x = 0, y = 0, z = 0 };
            int IDs = -1;
            int IDe = -1;
            for (int i = 0; i < mb.Count; i++)
            {
                IDs = GetPointID(pt, mb[i].Value.Ln.From, 0.001);
                IDe = GetPointID(pt, mb[i].Value.Ln.To, 0.001);
                if ((IDs >= 0) && (IDe >= 0))
                {
                    AxLines.Add(IDs, IDe, geomType, geomData);
                    AxLine = AxLines.Item[i + 1];
                    string Tstr = mb[i].Value.ElementType;
                    if (Tstr.Equals("truss")) { AxLine.DefineAsTruss(MatID[i], SectID[i], ELineNonLinearity.lnlTensionAndCompression, 0); }
                    else if (Tstr.Equals("beam")) { AxLine.DefineAsBeam(MatID[i], SectID[i], SectID[i], exc, exc); }
                    else if (Tstr.Equals("rib")) { AxLine.DefineAsRib(MatID[i], SectID[i], SectID[i], exc, exc); };
                }
                else return "node list is not complete";
            }

            //set the specified releases - assume rigid connections
            //AxMember = AxMembers.Item[0];
            //AxRelease.ReleaseType = EReleaseType.rtRigid;
            //RReleases AxReleases = new RReleases
            //{
            //    x = AxRelease,
            //    y = AxRelease,
            //    z = AxRelease,
            //    xx = AxRelease,
            //    yy = AxRelease,
            //    zz = AxRelease
            //};
            //AxMember.SetStartReleases(AxReleases);
            //AxMember.SetEndReleases(AxReleases);

            //set supports
            //RNonLinearity NonLinearity = new RNonLinearity
            //{
            //    x = ELineNonLinearity.lnlTensionAndCompression,
            //    y = ELineNonLinearity.lnlTensionAndCompression,
            //    z = ELineNonLinearity.lnlTensionAndCompression,
            //    xx = ELineNonLinearity.lnlTensionAndCompression,
            //    yy = ELineNonLinearity.lnlTensionAndCompression,
            //    zz = ELineNonLinearity.lnlTensionAndCompression
            //};
            //RResistances Resistances = new RResistances
            //{
            //    x = 0,
            //    y = 0,
            //    z = 0,
            //    xx = 0,
            //    yy = 0,
            //    zz = 0
            //};
            //RStiffnesses Stiffness = new RStiffnesses
            //{
            //    x = 1e10,
            //    y = 1e10,
            //    z = 1e10,
            //    xx = 1e10,
            //    yy = 1e10,
            //    zz = 1e10
            //};
            //AxNodalSupports.AddNodalBeamRelative(Stiffness, NonLinearity, Resistances, 1, 1);

            AxApp.Visible = ELongBoolean.lbTrue;
            AxApp.BringToFront();

            return "ok";
        }

        private int GetPointID(List<Point3d> L, Point3d p, double tol)
        {
            for (int i = 0; i < L.Count; i++)
            {
                if ((Math.Abs(L[i].X - p.X) < tol) & (Math.Abs(L[i].Y - p.Y) < tol) & (Math.Abs(L[i].Z - p.Z) < tol))
                {
                    return i + 1; //numbering in Axis starts with 1
                }
            }
            return -1;
        }

        private int GetCrossSection(string str, StringComparison cs, AxisVMCrossSections AxCs)
        {
            Regex regex1 = new Regex("X");
            Regex regex2 = new Regex("x");
            int res = -1;

            if (str.StartsWith("IPE ", cs) || str.StartsWith("I ", cs) || str.StartsWith("HE ", cs) || str.StartsWith("HP ", cs) ||
                str.StartsWith("HL ", cs) || str.StartsWith("HD ", cs) || str.StartsWith("IPN ", cs) || str.StartsWith("UB ", cs) ||
                str.StartsWith("UC ", cs))
            { res = AxCs.AddFromCatalog(ECrossSectionShape.cssI, str); }
            else if (str.StartsWith("ROR", cs))
            { res = AxCs.AddFromCatalog(ECrossSectionShape.cssPipe, str); }
            else if (str.StartsWith("O ", cs) || str.StartsWith("RND ", cs) || str.StartsWith("ROND ", cs))
            { res = AxCs.AddFromCatalog(ECrossSectionShape.cssCircle, str); }
            else if (regex1.Matches(str, 0).Count == 2) // for boxes the format is always 100X5X5
            { res = AxCs.AddFromCatalog(ECrossSectionShape.cssBox, str); }
            else if (regex2.Matches(str, 0).Count == 1) // restangular format is always 100x100
            { res = AxCs.AddFromCatalog(ECrossSectionShape.cssRectangular, str); }
            if (res <= 0)
            {
                res = AxCs.AddFromCatalog(ECrossSectionShape.cssI, str);
                if (res > 0) { return res; }
                else
                {
                    res = AxCs.AddFromCatalog(ECrossSectionShape.cssPipe, str);
                    if (res > 0) { return res; }
                    else
                    {
                        res = AxCs.AddFromCatalog(ECrossSectionShape.cssBox, str);
                        if (res > 0) { return res; }
                        else
                        {
                            res = AxCs.AddFromCatalog(ECrossSectionShape.cssRectangular, str);
                            if (res > 0) { return res; }
                            else
                            {
                                res = AxCs.AddFromCatalog(ECrossSectionShape.cssCircle, str);
                                if (res > 0) { return res; }
                                else return -1;
                            }
                        }
                    }
                }
            }
            return res;
        }

    }

}
