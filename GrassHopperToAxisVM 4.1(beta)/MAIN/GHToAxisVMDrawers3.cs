using System;
using System.Collections.Generic;
using AxisVM;
using Grasshopper.Kernel;
using Rhino.Geometry.Collections;
using Rhino.Geometry;
using System.Linq;

namespace GrassHopperToAxisVM
{
    public partial class GrassHopperToAxisVMComponent : GH_Component
    {
        //Need to send AxisMember (((AxisDomain2)))
        public List<Structs.AxisMember> drawDomains(List<AxisDomain> lista)
        {
            List<Structs.AxisMember> returner = new List<Structs.AxisMember>();
            foreach (AxisDomain x in lista)
            {
                //if (x.Value.drawing == false) { continue; }
                //A   T   T   R
                RSurfaceAttr ATTR = new RSurfaceAttr();
                if (x.Value.material != null)
                {
                    ATTR.MaterialId = genMat((ENationalDesignCode)System.Enum.Parse(typeof(ENationalDesignCode), (x.Value.material.designCode)), x.Value.material.name);

                }
                else
                {
                    ATTR.MaterialId = genMat(ENationalDesignCode.ndcEuroCode, "S 235");
                }
                if (x.Value.thickness != 0)
                {
                    ATTR.Thickness = x.Value.thickness;
                }
                else
                {
                    ATTR.Thickness = 0.05;
                }
                ATTR.SurfaceType = ESurfaceType.stShell;

                //MAIN 1 
                if (!x.Value.mesh.IsClosed) { continue; } //CHECK IF POLYLINE IS CLOSED
                List<Point3d> points = new List<Point3d>();
                Action<Point3d> action = delegate (Point3d yup) { points.Add(yup); };
                x.Value.mesh.ForEach(action);
                RPoint3d[] pointsAx = new RPoint3d[points.Count];
                Array out_points;
                int index1 = 0;
                foreach (Point3d cp in points)
                {
                    RPoint3d curr = new RPoint3d();
                    curr.x = cp.X; curr.y = cp.Y; curr.z = cp.Z;
                    pointsAx[index1] = curr;
                    index1++;
                }
                AxNodes.BulkAdd(pointsAx, out out_points);
                x.Value.clearp();
                x.Value.addp(out_points);
                int[] pointids = convert.toArray(out_points);

                List<RLineData> Rlinelist = new List<RLineData>();
                for (int i = 0; i < pointids.Length; i++)
                {
                    RLineData currl = new RLineData();
                    currl.GeomType = ELineGeomType.lgtStraightLine;
                    if (i != 0)
                    {
                        currl.NodeId1 = pointids[i - 1];
                        currl.NodeId2 = pointids[i];
                    }
                    else
                    {
                        currl.NodeId1 = pointids[pointids.Length - 1];
                        currl.NodeId2 = pointids[i];
                    }
                    Rlinelist.Add(currl);
                }
                Array lineid;
                index1 = 0;
                RLineData[] rline_list = new RLineData[Rlinelist.Count];
                foreach (RLineData x1 in Rlinelist)
                {
                    rline_list[index1] = x1;
                    index1++;
                }
                AxLines.BulkAdd(rline_list, out lineid);
                x.Value.clearl();
                x.Value.addl(lineid);
                /// ADDDOMAIN
                int ahhah = axDomains.Add(lineid, ref ATTR);
                ///HOLES CREATION 
                foreach (Polyline pl in x.Value.holes)
                {
                    if (!pl.IsClosed) { continue; } //CHECK IF POLYLINE IS CLOSED
                    List<Point3d> pointsH = new List<Point3d>();
                    Action<Point3d> actionH = delegate (Point3d yup) { pointsH.Add(yup); };
                    pl.ForEach(actionH);
                    RPoint3d[] pointsAxH = new RPoint3d[pointsH.Count];
                    Array out_pointsH = null;
                    int index1H = 0;
                    foreach (Point3d cp in pointsH)
                    {
                        RPoint3d curr = new RPoint3d();
                        curr.x = cp.X; curr.y = cp.Y; curr.z = cp.Z;
                        pointsAxH[index1H] = curr;
                        index1H++;
                    }
                    int xyyyy = AxNodes.BulkAdd(pointsAxH, out out_pointsH);
                    x.Value.addp(out_pointsH);
                    int[] pointidsH = convert.toArray(out_pointsH);

                    List<RLineData> RlinelistH = new List<RLineData>();
                    for (int i = 0; i < pointidsH.Length; i++)
                    {
                        RLineData currl = new RLineData();
                        currl.GeomType = ELineGeomType.lgtStraightLine;
                        if (i != 0)
                        {
                            currl.NodeId1 = pointidsH[i - 1];
                            currl.NodeId2 = pointidsH[i];
                        }
                        else
                        {
                            currl.NodeId1 = pointidsH[pointidsH.Length - 1];
                            currl.NodeId2 = pointidsH[i];
                        }
                        RlinelistH.Add(currl);
                    }
                    Array lineidH;
                    index1H = 0;
                    RLineData[] rline_listH = new RLineData[RlinelistH.Count];
                    foreach (RLineData x1 in RlinelistH)
                    {
                        rline_listH[index1H] = x1;
                        index1H++;
                    }
                    AxLines.BulkAdd(rline_listH, out lineidH);
                    x.Value.addl(lineidH);
                    int xxx = axDomains.Item[ahhah].AddHole(lineidH);
                    ;
                }
                ///HOLES CREATION END 
                int[] sender = new int[1]; sender[0] = ahhah;
                //x.Value.activeTriggered = false;
                //x.Value.drawing = false;
                x.Value.cleard();
                x.Value.addd(sender);
                returner.Add(x.Value.Clone());
            }
            return returner;
        }
    }
}
