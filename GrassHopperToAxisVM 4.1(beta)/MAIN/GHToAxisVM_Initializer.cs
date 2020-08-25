using System;
using AxisVM;
using Grasshopper.Kernel;
using System.Collections.Generic;

namespace GrassHopperToAxisVM
{
    public partial class GrassHopperToAxisVMComponent : GH_Component
    {
        public void initialize()
        {
            AxApp = new AxisVMApplication(); 
            AxApp.CloseOnLastReleased = ELongBoolean.lbFalse;
            AxApp.AskCloseOnLastReleased = ELongBoolean.lbTrue; // ez True volt
            AxApp.Visible = ELongBoolean.lbFalse;
            while (((IAxisVMApplication)AxApp).Loaded != ELongBoolean.lbTrue) { }
            AxModels = AxApp.Models;
            AxModel = AxModels.Item[AxModels.New()];
            AxNodes = AxModel.Nodes;
            AxLines = AxModel.Lines;
            axBeams = AxModel.VirtualBeams;
            members = AxModel.Members;
            mats = AxModel.Materials;
            crsec = AxModel.CrossSections;
            axloads = AxModel.Loads;
            axcloads = AxModel.LoadCases;
            axNodalsupport = AxModel.NodalSupports;
            axisSurfaces = AxModel.Surfaces;
            axiscalc = AxModel.Calculation;
            axwin = AxModel.Windows;
            axDomains = AxModel.Domains;
        }
        public void initialize(string filename)
        {
            AxApp = new AxisVMApplication();
            AxModels = AxApp.Models;
            AxModel = AxModels.Item[1];
            ELongBoolean x = AxModel.LoadFromFile(filename); //open existing model
            AxApp.CloseOnLastReleased = ELongBoolean.lbFalse;
            AxApp.AskCloseOnLastReleased = ELongBoolean.lbTrue; // ez True volt
            AxApp.Visible = ELongBoolean.lbFalse;
            while (((IAxisVMApplication)AxApp).Loaded != ELongBoolean.lbTrue) { }
            AxNodes = AxModel.Nodes;
            AxLines = AxModel.Lines;
            axBeams = AxModel.VirtualBeams;
            members = AxModel.Members;
            mats = AxModel.Materials;
            crsec = AxModel.CrossSections;
            axloads = AxModel.Loads;
            axcloads = AxModel.LoadCases;
            axNodalsupport = AxModel.NodalSupports;
            axisSurfaces = AxModel.Surfaces;
            axiscalc = AxModel.Calculation;
            axwin = AxModel.Windows;
            axDomains = AxModel.Domains;
            fillModelCatalog();
        }
        public void fillModelCatalog()
        {
            for(int i = 1; i <= mats.Count; i++)//in AxisVM indexing starts with 1
            {
                matlsInModel.Add(new MaterialData(mats.Item[i].NationalDesignCode, mats.Item[i].Name, i));
            }
            for (int i = 1; i <= crsec.Count; i++)//in AxisVM indexing starts with 1
            {
                crsInModel.Add(new CrossSectionData(crsec.Item[i].CrossSectionShape, crsec.Item[i].Name, i));
            }
        }
    }
}