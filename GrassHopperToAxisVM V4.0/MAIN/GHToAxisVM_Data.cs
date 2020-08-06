using System.Collections.Generic;
using AxisVM;
using Grasshopper.Kernel;
using System.ComponentModel;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using System;
using System.Linq;

namespace GrassHopperToAxisVM
{
    public partial class GrassHopperToAxisVMComponent : GH_Component
    {
        private void axisRunAnalysis()
        {
            axiscalc.LinearAnalysis(new ECalculationUserInteraction());
            RWriteValuesTo X = new RWriteValuesTo();
            X.Lines = ELongBoolean.lbFalse;
            X.MinMaxOnly = ELongBoolean.lbFalse;
            X.Nodes = ELongBoolean.lbFalse;
            X.Surfaces = ELongBoolean.lbFalse;

            RBasicDisplayParameters basic = new RBasicDisplayParameters();
            basic.DisplayMode = EDisplayMode.dmDiagram;
            basic.WriteValuesTo = X;
            basic.DisplayShape = EDisplayShape.dsUndeformed;
            basic.ResultComponent = EResultComponent.rc_vd_eZ;
            basic.Scale = 1;

            RExtendedDisplayParameters rExtended = new RExtendedDisplayParameters();
            rExtended.DisplayedEnvelopes = EDisplayedEnvelopes.de_Current;
            rExtended.DisplayAnalysisType = EDisplayAnalysisType.datLinear;
            rExtended.BasicDispParams = basic;
            rExtended.MinMaxType = EMinMaxType.mtMinMax;
            rExtended.ResultsType = EResultType.rtCritical;
            rExtended.SectPlaneContour = ELongBoolean.lbTrue;
            Array ismeret = new int[50000];

            axwin.SetStaticDisplayParameters(1, ref rExtended, 1, ref ismeret);
        }
    }
}
