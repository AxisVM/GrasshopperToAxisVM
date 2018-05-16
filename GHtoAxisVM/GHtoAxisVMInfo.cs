using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace GHtoAxisVM
{
    public class AxisGHInfo : GH_AssemblyInfo
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
                //Return a short string describing the purpose of this GHA library.
                return "export files to AxisVM finite element and design software";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("986c77d3-dcfd-4711-b2b5-fb5497bfdcb2");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "InterCAD - AxisVM";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "www.axisvm.hu";
            }
        }
    }
}
