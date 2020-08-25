using System;
using AxisVM;
using Grasshopper.Kernel;
using System.Collections.Generic;

namespace GrassHopperToAxisVM
{
    public partial class GrassHopperToAxisVMComponent : GH_Component
    {
        private class MaterialData
        {
            public ENationalDesignCode designCode;
            public string name;
            public int index;

            public MaterialData(ENationalDesignCode designCode, string name, int index)
            {
                this.designCode = designCode;
                this.name = name;
                this.index = index;
            }

            public MaterialData(ENationalDesignCode designCode, string name)
            {
                this.designCode = designCode;
                this.name = name;
            }

            public static bool Equals(MaterialData a, MaterialData b)
            {
                return a.designCode == b.designCode && a.name == b.name/* && a.index == b.index*/;
            }
        }

        private class CrossSectionData
        {
            public ECrossSectionShape designCode;
            public string name;
            public int index;

            public CrossSectionData(ECrossSectionShape designCode, string name, int index)
            {
                this.designCode = designCode;
                this.index = index;
                this.name = name;
            }
            public CrossSectionData(ECrossSectionShape designCode, string name)
            {
                this.designCode = designCode;
                this.name = name;
            }
            public static bool Equals(CrossSectionData a, CrossSectionData b)
            {
                return a.designCode == b.designCode && a.name == b.name/* && a.index == b.index*/;
            }
        }

        private int genMat(ENationalDesignCode eNationalDesignCode, string name)
        {
            if(this.matlsInModel.Exists(x => MaterialData.Equals(x,new MaterialData(eNationalDesignCode, name))))
            {
                return this.matlsInModel.Find(x => MaterialData.Equals(x, new MaterialData(eNationalDesignCode, name))).index;
            }
            else
            {
                int x = mats.AddFromCatalog(eNationalDesignCode, name);
                this.matlsInModel.Add(new MaterialData(eNationalDesignCode, name, x));
                return x;
            }
        }

        private int genCrs(ECrossSectionShape shape, string name)
        {
            if (this.crsInModel.Exists(x => CrossSectionData.Equals(x, new CrossSectionData(shape, name))))
            {
                return this.crsInModel.Find(x => CrossSectionData.Equals(x, new CrossSectionData(shape, name))).index;
            }
            else
            {
                int x = crsec.AddFromCatalog(shape, name);
                this.crsInModel.Add(new CrossSectionData(shape, name, x));
                return x;
            }
        }
    }
}