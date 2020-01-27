using System;
using Grasshopper.Kernel;
using AxisVM;
using Rhino.Geometry;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using Grasshopper.Kernel.Attributes;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace GHtoAxisVM
{
    /// <summary>
    /// Open AxisVM model or connect to existing model
    /// </summary>
    public class AxModelComp : GH_Component
    {      
        public AxModelComp() : //base("Sample", "ASpi","Construct an Archimedean, or arithmetic, spiral given its radii and number of turns.","Curve", "Primitive")
            base("Axis Model", "AxModel", "connects to an existing AxisVM model or creates a new one", "AxisVM", "AxisVM")
        {
            //Show AxisVM GUI and setup AxisVM to remain opened when COM client finished
        }

        public override Guid ComponentGuid => new Guid("c8a72b3a-ba7e-4121-ae67-d5c1acb0c04a");

        protected override Bitmap Icon => Properties.Resources.icAxModel;

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("run", "run", "Run component", GH_ParamAccess.item);
            pManager.AddBooleanParameter("isNew", "isNew", "RisNew", GH_ParamAccess.item);                     
            //pManager.AddPathParameter("Path", "Path", "AxisVM model path", GH_ParamAccess.item); //TODO ez valamiert nem mukodik
            pManager.AddTextParameter("path", "path", "AxisVM model path", GH_ParamAccess.item);
            pManager.AddTextParameter("modelName", "modelName", "AxisVM Model Filename", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new AxAppParameter(), "AxisVMApplication", "AxApp", "instance of AxisVMApplication", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool run = false;           

            // Get first input parameter
            if (!DA.GetData(0, ref run)) { return; };

            // If this parameter is set to true, then run the main code
            if (run) { StartAxModel(DA); }
            // otherwise detault output
            else DA.SetDataList(0, null);
        }

        public static void StartAxModel(IGH_DataAccess DA)
        {
            // Get input parameters
            bool isNew = false;
            string FullFileName = "";
            string FileName = "";
            string path = "";
            if (!DA.GetData(1, ref isNew)) { return; };
            if (!DA.GetData(2, ref path)) { return; };
            if (!DA.GetData(3, ref FileName)) { return; };
            FullFileName = path + FileName;

            // Check if Axis is already opened
            bool Opened = false;
            Process[] processlist = Process.GetProcesses();
            foreach (Process theprocess in processlist)
            {
                if (theprocess.ProcessName.Contains("AxisVM_x64")) { Opened = true; }
            }
            if (!Opened)
            {
                // Start AxisVM before starting the COM client - this way the opened AxisVM can be acessed later on
                //TODO kezdokepernyo ne latszodjon
                Process.Start("C://Kitti/Axis/piaci/X5/axisvm_x64.exe", "/MULTIINSTANCECOMCLIENT"); //TODO
                System.Threading.Thread.Sleep(10 * 1000); //wait, since it opens slowly
            }

            // TODO test if another AxApp is already running

            // Start COM client
            AxisVMApplication iAxApp = new AxisVMApplication();
            AxisVMModels iAxModels = new AxisVMModels();
            AxisVMModel iAxModel = new AxisVMModel();
            iAxApp.CloseOnLastReleased = ELongBoolean.lbFalse; //Axis exits when script finishes
            iAxApp.AskCloseOnLastReleased = ELongBoolean.lbTrue; //Show close dialog before exit

            // Check if COM client is loaded, otherwise wait until loaded
            int i = 0;
            ELongBoolean loaded = ELongBoolean.lbFalse;
            while ((i < 30) && (loaded == ELongBoolean.lbFalse))
            {
                i++;
                System.Threading.Thread.Sleep(2 * 1000); //wait for 2 sec, total waiting time is limited to 1 minute
                loaded = ((IAxisVMApplication)iAxApp).Loaded;
            }

            // Access the model
            iAxModels = iAxApp.Models;
            iAxModel = iAxModels.Item[1];

            // TODO test whether FullFileName is ok

            // open a specific axs file if path and filename properly defined
            if (FullFileName != null)
            {
                if (isNew == true)
                {
                    iAxModel.SaveToFile(FullFileName, ELongBoolean.lbTrue);
                }
                else
                {
                    iAxModel.LoadFromFile(FullFileName); //open existing model
                }
            }

            // Visibility settings
            iAxApp.Visible = ELongBoolean.lbTrue;
            iAxApp.BringToFront();

            // TODO close "start window" if opened

            // Output: GH axApp parameter, Use the DA object to assign a new obj to the first output parameter.
            GH_AxApp axm = new GH_AxApp { Value = new AxAppAttr(FullFileName,iAxApp) };
        DA.SetData(0, axm);
        }

    }

    /// <summary>
    /// Create AxMember (line elements / vonalelem)
    /// </summary>
    public class AxMembersComp : GH_Component
    {

        public AxMembersComp() : //base("Sample", "ASpi","Construct an Archimedean, or arithmetic, spiral given its radii and number of turns.","Curve", "Primitive")
            base("AxMembers (AxisVM)", "AxMembers", "create AxisVM line elements", "AxisVM", "Element")
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
            pManager.AddParameter(new AxMemberParameter(), "AxMember", "Memb", "AxisVM Member", GH_ParamAccess.list);
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

    /// <summary>
    /// Create AxShell (domain element / tartomány) - WIP
    /// </summary>
    public class AxShellComp : GH_Component
    {

        public AxShellComp() :
            base("AxShell", "AxShell", "create AxisVM Domain (membrane, plate, or shell)", "AxisVM", "Element")
        {
        }

        public override Guid ComponentGuid => new Guid("178be1d6-38f8-4bd1-ae9e-68c349c4cf66");

        protected override Bitmap Icon => Properties.Resources.icAxDomain;

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Vertices", "Vertices", "Domain vertices", GH_ParamAccess.list);
            pManager.AddTextParameter("Name", "Name", "element or group name", GH_ParamAccess.item);
            pManager.AddTextParameter("Mat", "Mat", "material", GH_ParamAccess.item);
            pManager.AddTextParameter("Thickness", "t", "Thickness [cm]", GH_ParamAccess.item);
            pManager.AddTextParameter("Type", "Type", "domain type (membrane, plate, or shell)", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new AxShellParameter(), "AxShell", "AxShell", "AxisVM Shell element", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Declare variables for the input
            List<Point3d> Vertices = new List<Point3d>(); //TODO: most csak egy tartomanyt lehet letrehozni, kesobb datatreere is mukodjon
            string Name = "";
            string TypeName = "Shell";
            string MatName = "C20/25";
            double Thickness = 10; //[cm] TODO

            // Use the DA object to retrieve the data inside the first input parameter.
            // If the retieval fails (for example if there is no data) we need to abort.
            if (!DA.GetDataList(0, Vertices)) { return; }
            if (!DA.GetData(1, ref Name)) { return; }
            if (!DA.GetData(2, ref MatName)) { return; }
            if (!DA.GetData(3, ref Thickness)) { return; }
            if (!DA.GetData(4, ref TypeName)) { return; }

            GH_AxShell axsh = new GH_AxShell();
            int NotValidLnCount = 0;

            axsh.Value = new AxShellAttr(Vertices, TypeName, MatName, Thickness, Name);

            // Use the DA object to assign a new String to the first output parameter.
            DA.SetData(0, axsh);

        }

    }

    /// <summary>
    /// Add Lines to Axis model
    /// </summary>
    public class AxSendLinesComp : GH_Component
    {
        List<Line> ln = new List<Line>();
        GH_AxApp axm = new GH_AxApp();

        public AxSendLinesComp() : //base("Sample", "ASpi","Construct an Archimedean, or arithmetic, spiral given its radii and number of turns.","Curve", "Primitive")
            base("Send Lines To AxisVM", "LnToAxisVM", "sends the lines to AxisVM", "AxisVM", "Elements")
        {
        }

        public override Guid ComponentGuid => new Guid("35ed95a6-bd54-49e5-a413-73d7855a4364"); 

        protected override Bitmap Icon => Properties.Resources.icLineToAxis;

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddLineParameter("Lines", "Lines", "list of lines that will become beams", GH_ParamAccess.list);
            pManager.AddParameter(new AxAppParameter(), "AxisVMApplication", "AxApp", "instance of AxisVMApplication", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new AxAppParameter(), "AxisVMApplication", "AxApp", "instance of AxisVMApplication", GH_ParamAccess.item);
        }

        public class SettingsComponentAttributes : GH_ComponentAttributes
        {
            public SettingsComponentAttributes(AxSendLinesComp cmp) : base(cmp) { }

            public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
            {
                ((AxSendLinesComp)Owner).StartAx(((AxSendLinesComp)Owner).axm, ((AxSendLinesComp)Owner).ln);
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
            DA.GetData(1, ref axm);
        }

        public string StartAx(GH_AxApp axm, List<Line> ln)
        {

            AxAppAttr axa = axm.Value;
            AxisVMApplication iAxApp = axa.AxApp;

            if (axa != null)
            {

                int k = axa.lns.Count;

                axa.AxModels = iAxApp.Models;
                axa.AxModel = axa.AxModels.Item[1];

                axa.AxNodes = axa.AxModel.Nodes;
                axa.AxLines = axa.AxModel.Lines;

                axa.AxModel.BeginUpdate();

                List<Point3d> pt = new List<Point3d>();
                int ptId = -1;
                int notValidLineCount = 0;
                Boolean bModify = false;
                Point3d newPt = new Point3d();
                if (axm.Value.lns.Count > 0) { bModify = true; }

                for (int i = 0; i < ln.Count; i++)
                {
                    if (ln[i].Length > 0)
                    {
                        if (bModify)
                        {
                            // modify existing line
                            newPt = ln[i].From;
                            ptId = axa.sIDs[i];
                            if (ptId != -1)
                            {
                                axa.pts[ptId - 1] = newPt;
                                axa.lns[i] = ln[i];
                            }
                            newPt = ln[i].To;
                            ptId = axa.eIDs[i];
                            if (ptId != -1)
                            {
                                axa.pts[ptId - 1] = newPt;
                                axa.lns[i] = ln[i];
                            }
                        }
                        else
                        {
                            //create new line                         
                            newPt = ln[i].From;
                            if (Common.GetPointID(axa.pts, newPt, 0.001) == -1)
                            {
                                axa.pts.Add(newPt);
                            }
                            newPt = ln[i].To;
                            if (Common.GetPointID(axa.pts, newPt, 0.001) == -1)
                            {
                                axa.pts.Add(newPt);
                            }
                            axa.sIDs.Add(Common.GetPointID(axa.pts, ln[i].From, 0.001));
                            axa.eIDs.Add(Common.GetPointID(axa.pts, ln[i].To, 0.001));
                            if ((axa.sIDs[i] >= 0) && (axa.eIDs[i] >= 0))
                            {
                                axa.lns.Add(ln[i]);
                            }
                        }

                    }
                    else { notValidLineCount++; }
                }

                if (bModify)
                {
                    // Create safearray from PointList and then use BulkAdd
                    Array pts = Conv.PointListToSA(axa.pts);
                    Array ptIDs = Array.CreateInstance(typeof(long), new int[] { axa.pts.Count }, new int[] { 1 }); // safearray with lower bound 1
                    //TODO fill ptIDs
                    axa.AxNodes.BulkSetNodeCoord(pts, ptIDs);
                }
                else
                {
                    // Create points
                    Array pts = Conv.PointListToSA(axa.pts);
                    Array ptIDs = Array.CreateInstance(typeof(long), new int[] { axa.pts.Count }, new int[] { 1 }); // safearray with lower bound 1
                    axa.AxNodes.BulkAdd(pts, out ptIDs);

                    // Create lines
                    RPoint3d exc = new RPoint3d { x = 0, y = 0, z = 0 };
                    List<RLineData> lineData = new List<RLineData>();
                    ELineGeomType geomType = new ELineGeomType();
                    RCircleArcGeomData cGeomData = new RCircleArcGeomData();
                    REllipseArcGeomData eGeomData = new REllipseArcGeomData();
                    RLineData basicLineData = new RLineData()
                    {
                        GeomType = geomType,
                        CircleArc = cGeomData,
                        EllipseArc = eGeomData
                    };
                    notValidLineCount = 0;
                    for (int i = 0; i < ln.Count; i++)
                    {
                        if (ln[i].Length > 0)
                        {
                            basicLineData.NodeId1 = Common.GetPointID(axa.pts, ln[i].From, 0.001);
                            basicLineData.NodeId2 = Common.GetPointID(axa.pts, ln[i].To, 0.001);
                            if ((basicLineData.NodeId1 >= 0) && (basicLineData.NodeId2 >= 0))
                            {
                                lineData.Add(basicLineData);
                            }
                            else return "node list is not complete";
                        }
                        else { notValidLineCount++; }
                    }

                    Array lines = Conv.LineListToSA(lineData);
                    Array lineIDs = Array.CreateInstance(typeof(long), new int[] { lineData.Count }, new int[] { 1 }); //safearray with lower bound 1
                    int err = axa.AxLines.BulkAdd(lines, out lineIDs);
                }

                //todo: endupdate  only if no analysis or if there is analysis results available, it should be deleted
                axa.AxModel.EndUpdate();

                iAxApp.Visible = ELongBoolean.lbTrue;
                iAxApp.BringToFront();

                return "ok";
            }
            else
            {
                return "AxisVM is not running"; //TODO: ezt a hibauzenetet a felhasznalo nem latja
            }
        }

    }

    /// <summary>
    /// Add members to Axis model
    /// </summary>
    public class AxSendMembersComp : GH_Component
    {
        List<GH_AxMember> Mb = new List<GH_AxMember>();
        GH_AxApp axm = new GH_AxApp();

        public AxSendMembersComp() : //base("Sample", "ASpi","Construct an Archimedean, or arithmetic, spiral given its radii and number of turns.","Curve", "Primitive")
            base("Send Members To AxisVM", "MembToAxisVM", "sends the members to AxisVM", "AxisVM", "Elements")
        {
        }

        public override Guid ComponentGuid => new Guid("60a6d100-4518-4fee-b9bd-0e5d7071e5e9");

        protected override Bitmap Icon => Properties.Resources.icMembToAxis;

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new AxMemberParameter(), "AxisVM members", "AxMembers", "AxisVM structural elements", GH_ParamAccess.list);
            pManager.AddParameter(new AxAppParameter(), "AxisVMApplication", "AxApp", "instance of AxisVMApplication", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new AxAppParameter(), "AxisVMApplication", "AxApp", "instance of AxisVMApplication", GH_ParamAccess.item);
        }

        public class SettingsComponentAttributes : GH_ComponentAttributes
        {
            public SettingsComponentAttributes(AxSendMembersComp cmp) : base(cmp) { }

            public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
            {
                ((AxSendMembersComp)Owner).StartAx(((AxSendMembersComp)Owner).axm, ((AxSendMembersComp)Owner).Mb);
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
            DA.GetDataList(0, Mb);
            DA.GetData(1, ref axm);
        }

        public string StartAx(GH_AxApp axm, List<GH_AxMember> mb)
        {

            AxAppAttr axa = axm.Value;
            AxisVMApplication iAxApp = axa.AxApp;

            if (axm != null)
            {

                axa.AxModels = iAxApp.Models;
                axa.AxModel = axa.AxModels.Item[1];
                axa.AxNodes = axa.AxModel.Nodes;
                axa.AxLines = axa.AxModel.Lines;
                axa.AxMaterials = axa.AxModel.Materials;
                axa.AxCrossSections = axa.AxModel.CrossSections;
                axa.code = ENationalDesignCode.ndcEuroCode; //TODO currently limited to Eurocode

                axa.AxModel.BeginUpdate();

                //Create material
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
                        MatID[i] = axa.AxMaterials.AddFromCatalog(axa.code, Mstr);
                        Mstrs.Add(Mstr);
                        MIDs.Add(MatID[i]);
                        axa.AxMaterial = axa.AxMaterials.Item[MatID[i]];
                    }
                }

                //Add cross sections
                int[] SectID = new int[mb.Count]; //section ID for each structural member
                List<string> CSstrs = new List<string>(); // list of sect names already loaded
                List<int> CSIDs = new List<int>(); // list of sect IDs already loaded
                for (int i = 0; i < mb.Count; i++)
                {
                    string CSstr = mb[i].Value.SectType;

                    //chcek if this cross-section has already been defined or not
                    bool alreadyDefined = false;
                    for (int j = 0; j < CSstrs.Count; j++)
                    { if (CSstrs[j].Equals(CSstr, sc)) { SectID[i] = CSIDs[j]; alreadyDefined = true; } }

                    if (!alreadyDefined)
                    {
                        SectID[i] = Common.GetCrossSection(CSstr, sc, axa.AxCrossSections); //currently limited to pipe, I, Box
                        CSstrs.Add(CSstr);
                        CSIDs.Add(SectID[i]);
                        axa.AxCrossSection = axa.AxCrossSections.Item[SectID[i]];
                    }
                }

                // Geometry
                Point3d newPt = new Point3d(){ X = 0, Y = 0, Z = 0};
                List<Point3d> newpts = new List<Point3d>();
                List<Line> newlns = new List<Line>();
                List<RLineData> lineData = new List<RLineData>();
                List<RLineAttr> lineAttrData = new List<RLineAttr>();
                int ptId = -1;
                RPoint3d exc = new RPoint3d { x = 0, y = 0, z = 0 };
                int notValidLineCount = 0;
                Boolean bModify = false;
                if (axa.lns.Count > 0) { bModify = true; }
                for (int i = 0; i < mb.Count; i++)
                {
                    if (mb[i].Value.Ln.Length > 0)
                    {
                        if (bModify)
                        {
                            // modify existing line
                            newPt = mb[i].Value.Ln.From;
                            ptId = axa.sIDs[i];
                            if (ptId != -1)
                            {
                                RPoint3d aPt = new RPoint3d { x = newPt.X, y = newPt.Y, z = newPt.Z };
                                axa.AxNodes.SetNodeCoord(ptId, aPt);
                                axa.pts[ptId - 1] = newPt;
                                axa.lns[i] = mb[i].Value.Ln;
                            }
                            newPt = mb[i].Value.Ln.To;
                            ptId = axa.eIDs[i];
                            if (ptId != -1)
                            {
                                RPoint3d aPt = new RPoint3d { x = newPt.X, y = newPt.Y, z = newPt.Z };
                                axa.AxNodes.SetNodeCoord(ptId, aPt);
                                axa.pts[ptId - 1] = newPt;
                                axa.lns[i] = mb[i].Value.Ln;
                            }
                            if (axa.membProps[i] != new int[] { SectID[i], MatID[i], 0 })
                            {
                                axa.AxLine = axa.AxLines.Item[i + 1 - notValidLineCount];
                                string Tstr = mb[i].Value.ElementType;
                                if (Tstr.Equals("truss")) { axa.AxLine.DefineAsTruss(MatID[i], SectID[i], ELineNonLinearity.lnlTensionAndCompression, 0); }
                                else if (Tstr.Equals("beam")) { axa.AxLine.DefineAsBeam(MatID[i], SectID[i], SectID[i], exc, exc); }
                                else if (Tstr.Equals("rib")) { axa.AxLine.DefineAsRib(MatID[i], SectID[i], SectID[i], exc, exc); }
                            }
                        }
                        else
                        {
                            //create new line                         
                            newPt = mb[i].Value.Ln.From;
                            if (Common.GetPointID(axa.pts, newPt, 0.001) == -1)
                            {
                                axa.pts.Add(newPt);
                                newpts.Add(newPt);
                            }
                            newPt = mb[i].Value.Ln.To;
                            if (Common.GetPointID(axa.pts, newPt, 0.001) == -1)
                            {
                                axa.pts.Add(newPt);
                                newpts.Add(newPt);
                            }

                            RLineData basicLineData = new RLineData();
                            basicLineData.NodeId1 = Common.GetPointID(axa.pts, mb[i].Value.Ln.From, 0.001);
                            basicLineData.NodeId2 = Common.GetPointID(axa.pts, mb[i].Value.Ln.To, 0.001);
                            axa.sIDs.Add(basicLineData.NodeId1);
                            axa.eIDs.Add(basicLineData.NodeId2);                                                      
                            if ((axa.sIDs[i] >= 0) && (axa.eIDs[i] >= 0))
                            {
                                lineData.Add(basicLineData);
                                string Tstr = mb[i].Value.ElementType;
                                RLineAttr attr = new RLineAttr()
                                {
                                    LineType = ELineType.ltSimpleLine,
                                    MaterialIndex = MatID[i],
                                    StartCrossSectionIndex = SectID[i],
                                    EndCrossSectionIndex = SectID[i],
                                    TrussType = ELineNonLinearity.lnlTensionAndCompression, //truss only
                                    AutoEccentricityType = EAutoExcentricityType.aetMid, // rib only
                                    StartEccentricity = exc, // rib only
                                    EndEccentricity = exc, // rib only
                                };
                                if (Tstr.Equals("truss")) { attr.LineType = ELineType.ltTruss; }
                                else if (Tstr.Equals("beam")) { attr.LineType = ELineType.ltBeam; }
                                else if (Tstr.Equals("rib")) { attr.LineType = ELineType.ltRib; }; //...
                                                                                                   //timber properties: kdef, ServiceClass ...
                                                                                                   //rib: Domain, kx ...
                                axa.membProps.Add(new int[] { SectID[i], MatID[i], (int) attr.LineType });
                                lineAttrData.Add(attr);
                                axa.lns.Add(mb[i].Value.Ln);
                                axa.sw.Add(false);
                            }
                        }
                    }
                }

                // add new elements to Axis using BulkAdd
                Array pts = Conv.PointListToSA(newpts);
                Array ptIDs = Array.CreateInstance(typeof(long), new int[] { newpts.Count }, new int[] { 1 }); // safearray with lower bound 1
                axa.AxNodes.BulkAdd(pts, out ptIDs);
                Array lines = Array.CreateInstance(typeof(RLineData), new int[] { lineData.Count }, new int[] { 1 }); //safearray with lower bound 1
                Array lineAttrs = Array.CreateInstance(typeof(RLineAttr), new int[] { lineData.Count }, new int[] { 1 }); //safearray with lower bound 1           
                for (int i = 0; i < lineData.Count; i++)
                {
                    lines.SetValue(lineData[i], i + 1);
                    lineAttrs.SetValue(lineAttrData[i], i + 1);
                }
                Array lineIDs = Array.CreateInstance(typeof(long), new int[] { lineData.Count }, new int[] { 1 }); //safearray with lower bound 1
                int err = axa.AxLines.BulkAdd(lines, out lineIDs);
                err = axa.AxLines.BulkSetAttr(lineIDs, lineAttrs);

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

                //todo: endupdate  only if no analysis or if there is analysis results available, it should be deleted
                axa.AxModel.EndUpdate();

                iAxApp.Visible = ELongBoolean.lbTrue;
                iAxApp.BringToFront();

                return "ok";
            }


            else
            {
                return "AxisVM is not running"; //TODO: ezt a hibauzenetet a felhasznalo nem latja
            }
        }

    }

    /// <summary>
    /// Supports
    /// </summary>
    public class AxSupportComp : GH_Component
    {

        public AxSupportComp() : //base("Sample", "ASpi","Construct an Archimedean, or arithmetic, spiral given its radii and number of turns.","Curve", "Primitive")
            base("Nodal support", "Nodal support", "create nodal support", "AxisVM", "Load")
        {
        }

        public override Guid ComponentGuid => new Guid("ed775a9b-87b1-4b4a-9151-8ad66d2c2cc4");

        protected override Bitmap Icon => Properties.Resources.icFixedSupport;

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new AxAppParameter(), "AxisVMApplication", "AxApp", "instance of AxisVMApplication", GH_ParamAccess.item);
            pManager.AddPointParameter("Points", "Points", "list of nodes to support", GH_ParamAccess.list);
            pManager.AddNumberParameter("Rx", "Rx", "axial stiffness of support", GH_ParamAccess.item);
            pManager.AddNumberParameter("Ry", "Ry", "lateral stiffness of support", GH_ParamAccess.item);
            pManager.AddNumberParameter("Rz", "Rz", "vertical stiffness of support", GH_ParamAccess.item);
            pManager.AddNumberParameter("Rxx", "Rxx", "Rxx stiffness of support", GH_ParamAccess.item);
            pManager.AddNumberParameter("Ryy", "Ryy", "Ryy stiffness of support", GH_ParamAccess.item);
            pManager.AddNumberParameter("Rzz", "Rzz", "Rzz stiffness of support", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> nodes = new List<Point3d>();
            GH_AxApp axm = new GH_AxApp();
            double Rx = 1e10;
            double Ry = 1e10;
            double Rz = 1e10;
            double Rxx = 1e10;
            double Ryy = 1e10;
            double Rzz = 1e10;

            DA.GetData(0, ref axm);
            DA.GetDataList(1, nodes);
            DA.GetData(2, ref Rx);
            DA.GetData(3, ref Ry);
            DA.GetData(2, ref Rz);
            DA.GetData(3, ref Rxx);
            DA.GetData(2, ref Ryy);
            DA.GetData(3, ref Rzz);

            AxAppAttr axa = axm.Value;
            AxisVMApplication iAxApp = axa.AxApp;

            if (axa != null)
            {

                int k = axa.lns.Count;

                axa.AxModels = iAxApp.Models;
                axa.AxModel = axa.AxModels.Item[1];

                axa.AxModel.BeginUpdate();

                RStiffnesses node_supp_Stiff = new RStiffnesses();
                RNonLinearity node_supp_NonLin = new RNonLinearity
                {
                    x = ELineNonLinearity.lnlTensionAndCompression,
                    y = ELineNonLinearity.lnlTensionAndCompression,
                    z = ELineNonLinearity.lnlTensionAndCompression,
                    xx = ELineNonLinearity.lnlTensionAndCompression,
                    yy = ELineNonLinearity.lnlTensionAndCompression,
                    zz = ELineNonLinearity.lnlTensionAndCompression,
                };
                RResistances node_supp_Resistance = new RResistances { x = 0, y = 0, z = 0, xx = 0, yy = 0, zz = 0, };
                node_supp_Stiff.x = Rx;
                node_supp_Stiff.y = Ry;
                node_supp_Stiff.z = Rz;
                node_supp_Stiff.xx = Rxx;
                node_supp_Stiff.yy = Ryy;
                node_supp_Stiff.zz = Rzz;

                int prevSupCount = axa.supNodeIDs.Count;
                List<int> newSupNodeIDs = new List<int>();
                for (int i = 0; i < nodes.Count; i++)
                {
                    bool notyet = true;
                    int ptID = Common.GetPointID(axa.pts, nodes[i], 0.001);
                    for (int j = 0; j < prevSupCount; j++)
                    {
                        if (axa.supNodeIDs[j] == ptID)
                        {
                            notyet = false;
                            break;
                        }
                    }
                    if (notyet)
                    {
                        axa.AxNodeSupport.AddNodalGlobal(node_supp_Stiff, node_supp_NonLin, node_supp_Resistance, ptID);
                        axa.supNodeIDs.Add(ptID);
                        newSupNodeIDs.Add(ptID);
                    }
                }

                axa.AxModel.EndUpdate();
            }
        }

    }

    /// <summary>
    /// Loading
    /// </summary>
    public class AxNodalLoadComp : GH_Component
    {

        public AxNodalLoadComp() :
            base("Nodal load", "Nodal load", "create nodal load", "AxisVM", "Load")
        {
        }

        public override Guid ComponentGuid => new Guid("aed5365c-9e27-4e3b-b48b-4bf41d2930e5");

        protected override Bitmap Icon => Properties.Resources.icNLoad;

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new AxAppParameter(), "AxisVMApplication", "AxApp", "instance of AxisVMApplication", GH_ParamAccess.item);
            pManager.AddPointParameter("Points", "Points", "list of nodes to apply force to", GH_ParamAccess.list);
            pManager.AddTextParameter("LC name", "LC name", "Name of load case", GH_ParamAccess.item);
            pManager.AddNumberParameter("Fx", "Fx", "axial force", GH_ParamAccess.item);
            pManager.AddNumberParameter("Fy", "Fy", "lateral force", GH_ParamAccess.item);
            pManager.AddNumberParameter("Fz", "Fz", "vertical force", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("A", "A", "string", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> nodes = new List<Point3d>();
            GH_AxApp axm = new GH_AxApp();
            string lcName = "ST1";
            double Fx = 0;
            double Fy = 0;
            double Fz = 0;

            DA.GetData(0, ref axm);
            DA.GetDataList(1, nodes);
            DA.GetData(2, ref lcName);
            DA.GetData(3, ref Fx);
            DA.GetData(4, ref Fy);
            DA.GetData(5, ref Fz);

            AxAppAttr axa = axm.Value;
            AxisVMApplication iAxApp = axa.AxApp;

            if (axa != null)
            {

                axa.AxModels = iAxApp.Models;
                axa.AxModel = axa.AxModels.Item[1];

                axa.AxModel.BeginUpdate();

                // Define Load case
                int lc = -1;
                bool exists = false; //load case defined earlier?
                for (int i = 1; i <= axa.AxLoadCases.Count; i++)
                {
                    if (axa.AxLoadCases.Name[i].Equals(lcName)) { exists = true; lc = i; break; }
                }
                if (exists == false) { lc = axa.AxLoadCases.Add(lcName, ELoadCaseType.lctStandard); }

                // Add nodal loads
                RLoadNodalForce ptLoad = new RLoadNodalForce { LoadCaseId = lc };
                int prevlcCOunt = axa.nodeloadIDs.Count;
                List<int[]> newNodeloadIDs = new List<int[]>(); // load case and loaded node
                for (int i = 0; i < nodes.Count; i++)
                {
                    bool notyet = true;
                    int ptID = Common.GetPointID(axa.pts, nodes[i], 0.001);
                    for (int j = 0; j < prevlcCOunt; j++)
                    {                       
                        if (axa.nodeloadIDs[j][0] == lc && axa.nodeloadIDs[j][1] == ptID)
                        {
                            notyet = false;
                            break;
                        }
                    }
                    if (notyet)
                    {
                        ptLoad.NodeId = ptID;
                        ptLoad.Fx = Fx;
                        ptLoad.Fy = Fy;
                        ptLoad.Fz = Fz;
                        ptLoad.Mx = 0;
                        ptLoad.My = 0;
                        ptLoad.Mz = 0;
                        ptLoad.ReferenceId = 0;
                        axa.AxLoads.AddNodalForce(ptLoad);
                        axa.nodeloadIDs.Add(new int[] { lc, ptID });
                        newNodeloadIDs.Add(new int[] { lc, ptID });
                    }
                }

                axa.AxModel.EndUpdate();
            }
        }

    }

    public class AxSelfWeightComp : GH_Component
    {

        public AxSelfWeightComp() :
            base("Self-weight", "Self-weight", "Add self-weight to elements", "AxisVM", "Load")
        {
        }

        public override Guid ComponentGuid => new Guid("efc69d9e-6f22-4271-b52f-ea33947f1839");

        protected override Bitmap Icon => Properties.Resources.icSW;

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new AxAppParameter(), "AxisVMApplication", "AxApp", "instance of AxisVMApplication", GH_ParamAccess.item);
            pManager.AddTextParameter("LC name", "LC name", "Name of load case", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("A", "A", "string", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_AxApp axm = new GH_AxApp();
            string lcName = "ST1";

            DA.GetData(0, ref axm);
            DA.GetData(1, ref lcName);

            AxAppAttr axa = axm.Value;
            AxisVMApplication iAxApp = axa.AxApp;

            if (axa != null)
            {

                axa.AxModels = iAxApp.Models;
                axa.AxModel = axa.AxModels.Item[1];

                axa.AxModel.BeginUpdate();

                // Define Load case
                int lc = -1;
                bool exists = false; //load case defined earlier?
                for (int i = 1; i <= axa.AxLoadCases.Count; i++)
                {
                    if (axa.AxLoadCases.Name[i].Equals(lcName)) { exists = true; lc = i; break; }
                }
                if (exists == false) { lc = axa.AxLoadCases.Add(lcName, ELoadCaseType.lctStandard); }

                int lnc = 0;

                // Add self weight
                for (int i = 1; i <= axa.lns.Count; i++)
                {
                    if (axa.sw[i - 1] == false)
                    {
                        if (axa.AxLoads.AddBeamSelfWeight(i, lc) < 0) // use only line indexes
                        {
                            if (axa.AxLoads.AddTrussSelfWeight(i, lc) < 0)
                            {
                                axa.AxLoads.AddRibSelfWeight(i, lc);
                            }
                        }
                        axa.sw[i - 1] = true;
                    }
                }

                axa.AxModel.EndUpdate();
            }
        }

    }

}
