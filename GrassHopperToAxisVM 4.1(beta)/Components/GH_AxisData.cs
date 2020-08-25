using System;
using System.Collections.Generic;
using AxisVM;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.IO;
using System.Drawing;
using System.Reflection;

namespace GrassHopperToAxisVM
{
    public class GH_AxisData : GH_Component
    {
        public GH_AxisData() : base("AxisVM Settings", "AxStt", "Component for adding AxisVM model settings", "AxisVM", "Send")
        {

        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("NEW", "N", "Fully new AxisVM Modell", GH_ParamAccess.item);
            pManager.AddTextParameter("AxisVM Model File Path", "AxFP", "AxisVM model file path", GH_ParamAccess.item);

            pManager.AddBooleanParameter("Just PointCoordinates", "Just_P", "Set the value true, if connections of points are the same.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Send only changes", "SOC", "Send only the differences from Grasshopper to AxisVM", GH_ParamAccess.item);

            pManager.AddBooleanParameter("Analysis", "AxAn", "Run linear analysis on the AxisVM model", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Check Double", "CH", "Check and delete double points and lines.", GH_ParamAccess.item);

            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new AxisDataParameter(), "AxisVM Settings", "AxStt", "AxisVM Settings for AxisVM model", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string filePath = null;
            bool isNew = false;
            bool justPoints = false;
            bool axAnalysis = false;
            bool checkDouble = false;
            bool sendOnlyChanges = false;
            if (!DA.GetData(0, ref isNew)) { DA.AbortComponentSolution(); }
            if (!DA.GetData(1, ref filePath)) { }
            if (!DA.GetData(2, ref justPoints)) { }
            if (!DA.GetData(3, ref sendOnlyChanges)) { }
            if (!DA.GetData(4, ref axAnalysis)) { }
            if (!DA.GetData(5, ref checkDouble)) { }

            DA.SetData(0, new AxisData(filePath, isNew, justPoints, axAnalysis, checkDouble, sendOnlyChanges));
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resource1.ic_AxSettings;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("7f8652a7-a3aa-4cde-9c2c-5b8d617f995e"); }
        }
    }
}
