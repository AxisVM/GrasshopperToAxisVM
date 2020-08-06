using System;
using Grasshopper.Kernel;
using System.Reflection;

namespace GrassHopperToAxisVM
{
    public class GrassHopperToAxisVMInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "AxisVM";
            }
        }

        public override string Description
        {
            get
            {
                return "Component for exporting elements to AxisVM";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("e02a1c24-2006-4082-9a03-2d137e1540e8");
            }
        }

        public override string AuthorName
        {
            get
            {
                return "InterCAD - AxisVM";
            }
        }
        public override string AuthorContact
        {
            get
            {
                return "www.axisvm.hu";
            }
        }
    }
}
