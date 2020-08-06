// In the following Code, the last Three JP Methods are equal. (NOT DOMAINS PARAMETER!!)

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
        public List<Structs.AxisMember> drawPonts_JP(List<AxisPoint> lista1, Array ids)
        {
            List<RPoint3d> sender_p = new List<RPoint3d>();
            List<Structs.AxisMember> returner = new List<Structs.AxisMember>();
            foreach (AxisPoint lista in lista1)
            {
                foreach (Point3d x in lista.Value.point)
                {
                    RPoint3d AXP = new RPoint3d();
                    AXP.x = x.X; AXP.y = x.Y; AXP.z = x.Z;
                    sender_p.Add(AXP);
                }
            }
            AxNodes.BulkSetNodeCoord(ids, sender_p.ToArray());
            return returner;
        }
        public List<Structs.AxisMember> drawLines_JP(List<AxisLine> lista123, Array ids)
        {
            List<RPoint3d> sender_p = new List<RPoint3d>();
            List<Structs.AxisMember> returner = new List<Structs.AxisMember>();
            foreach (AxisLine lista_c in lista123)
            {
                List<Line> lista = lista_c.Value.line;
                RPoint3d[] sender_p1 = new RPoint3d[lista.Count];
                RPoint3d[] sender_p2 = new RPoint3d[lista.Count];
                List<RLineData> sender = new List<RLineData>();
                int index1 = 0;
                foreach (Line x in lista)
                {
                    RPoint3d AXP1 = new RPoint3d();
                    RPoint3d AXP2 = new RPoint3d();
                    AXP1.x = x.FromX; AXP1.y = x.FromY; AXP1.z = x.FromZ;
                    AXP2.x = x.ToX; AXP2.y = x.ToY; AXP2.z = x.ToZ;
                    sender_p1[index1] = AXP1; sender_p2[index1] = AXP2;
                    index1++;
                }
                sender_p.AddRange(sender_p1);
                sender_p.AddRange(sender_p2);
            }
            AxNodes.BulkSetNodeCoord(ids, sender_p.ToArray());
            return returner;
        }
        public List<Structs.AxisMember> drawMeshs_JP(List<AxisMesh> lista, Array ids)
        {
            List<RPoint3d> sender_p = new List<RPoint3d>();
            List<Structs.AxisMember> returner = new List<Structs.AxisMember>();
            foreach (AxisMesh x in lista)
            {
                MeshTopologyVertexList vertex = x.Value.mesh.TopologyVertices;
                int points = vertex.Count;
                Point3f[] points_ = new Point3f[points];
                for (int i = 0; i < points; i++)
                {
                    points_[i] = vertex.ElementAt(i);
                }

                Array out_points;
                RPoint3d[] points__ = new RPoint3d[points];
                int index1 = 0;
                foreach (Point3d cp in points_)
                {
                    RPoint3d curr = new RPoint3d();
                    curr.x = cp.X; curr.y = cp.Y; curr.z = cp.Z;
                    points__[index1] = curr;
                    index1++;
                }
                sender_p.AddRange(points__);
                
            }
            AxNodes.BulkSetNodeCoord(ids, sender_p.ToArray());
            return returner;
        }
        public List<Structs.AxisMember> drawMeshs2_JP(List<AxisMesh> lista, Array ids)
        {
            List<RPoint3d> sender_p = new List<RPoint3d>();
            List<Structs.AxisMember> returner = new List<Structs.AxisMember>();
            foreach (AxisMesh x in lista)
            {
                MeshTopologyVertexList vertex = x.Value.mesh.TopologyVertices;
                int points = vertex.Count;
                Point3f[] points_ = new Point3f[points];
                for (int i = 0; i < points; i++)
                {
                    points_[i] = vertex.ElementAt(i);
                }

                Array out_points;
                RPoint3d[] points__ = new RPoint3d[points];
                int index1 = 0;
                foreach (Point3d cp in points_)
                {
                    RPoint3d curr = new RPoint3d();
                    curr.x = cp.X; curr.y = cp.Y; curr.z = cp.Z;
                    points__[index1] = curr;
                    index1++;
                }
                sender_p.AddRange(points__);

            }
            AxNodes.BulkSetNodeCoord(ids, sender_p.ToArray());
            return returner;
        }
        public List<Structs.AxisMember> drawEdges_JP(List<AxisMesh> lista, Array ids)
        {
            List<RPoint3d> sender_p = new List<RPoint3d>();
            List<Structs.AxisMember> returner = new List<Structs.AxisMember>();
            foreach (AxisMesh x in lista)
            {
                MeshTopologyVertexList vertex = x.Value.mesh.TopologyVertices;
                int points = vertex.Count;
                Point3f[] points_ = new Point3f[points];
                for (int i = 0; i < points; i++)
                {
                    points_[i] = vertex.ElementAt(i);
                }

                Array out_points;
                RPoint3d[] points__ = new RPoint3d[points];
                int index1 = 0;
                foreach (Point3d cp in points_)
                {
                    RPoint3d curr = new RPoint3d();
                    curr.x = cp.X; curr.y = cp.Y; curr.z = cp.Z;
                    points__[index1] = curr;
                    index1++;
                }
                sender_p.AddRange(points__);

            }
            AxNodes.BulkSetNodeCoord(ids, sender_p.ToArray());
            return returner;
        }

        public List<Structs.AxisMember> drawDomains_JP(List<AxisDomain> lista, Array ids)
        {
            List<RPoint3d> sender_p = new List<RPoint3d>();
            List<Structs.AxisMember> returner = new List<Structs.AxisMember>();
            foreach (AxisDomain x in lista)
            {
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
                sender_p.AddRange(pointsAx);

            }
            AxNodes.BulkSetNodeCoord(ids, sender_p.ToArray());
            return returner;
        }
    }
}