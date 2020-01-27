using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using AxisVM;
using System.Text.RegularExpressions;

namespace GHtoAxisVM
{
    class Conv
    {

        public static Array PointListToSA(List<Point3d> pt)
        {
            // convert list of Rhino points to safearray of AxisVM points 
            Array pts = Array.CreateInstance(typeof(RPoint3d), new int[] { pt.Count }, new int[] { 1 }); // safearray with lower bound 1
            for (int i = 0; i < pt.Count; i++)
            {
                RPoint3d pt_ = new RPoint3d
                {
                    x = pt[i].X,
                    y = pt[i].Y,
                    z = pt[i].Z
                };
                pts.SetValue(pt_, i + 1);
            }
            return pts;
        }

        public static Array LineListToSA(List<RLineData> ln)
        {
            // convert list of AxisVM linedata to safearray of AxisVM linedata 
            Array lines = Array.CreateInstance(typeof(RLineData), new int[] { ln.Count }, new int[] { 1 }); //safearray with lower bound 1
            for (int i = 0; i < ln.Count; i++)
            {
                lines.SetValue(ln[i], i + 1);
            }
            return lines;
        }

    }

    class Common
    {

        public static int GetPointID(List<Point3d> L, Point3d p, double tol)
        {
            int id = -1;
            for (int i = 0; i < L.Count; i++)
            {
                if ((Math.Abs(L[i].X - p.X) < tol) & (Math.Abs(L[i].Y - p.Y) < tol) & (Math.Abs(L[i].Z - p.Z) < tol))
                {
                    id = i + 1; //numbering in Axis starts with 1
                    break;
                }
            }
            return id;
        }

        public static int GetCrossSection(string str, StringComparison cs, AxisVMCrossSections AxCs)
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
