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
        public void drawMeshs2(List<AxisMesh> lista)
        {
            foreach (AxisMesh x in lista)
            {
                RSurfaceAttr ATTR = new RSurfaceAttr();
                if (x.Value.material != null)
                {
                    ATTR.MaterialId = mats.AddFromCatalog((ENationalDesignCode)System.Enum.Parse(typeof(ENationalDesignCode), (x.Value.material.Split('}')[0])), x.Value.material.Split('}')[1]);

                }
                else
                {
                    ATTR.MaterialId = mats.AddFromCatalog(ENationalDesignCode.ndcEuroCode, "S 235");
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
                MeshTopologyVertexList vertex = x.Value.mesh.TopologyVertices;
                int points = vertex.Count;
                Point3f[] points_ = new Point3f[points];
                for (int i = 0; i < points; i++)
                {
                    points_[i] = vertex.ElementAt(i);
                }
                Array out_points; int[] pointids = new int[points];
                RPoint3d[] points__ = new RPoint3d[points];
                int index1 = 0;
                foreach (Point3d cp in points_)
                {
                    RPoint3d curr = new RPoint3d();
                    curr.x = cp.X; curr.y = cp.Y; curr.z = cp.Z;
                    points__[index1] = curr;
                    index1++;
                }
                AxNodes.BulkAdd(points__, out out_points);
                index1 = 0;
                foreach (int xx in out_points)
                {
                    pointids[index1] = xx;
                    index1++;
                }
                MeshFaceList faces = x.Value.mesh.Faces;
                List<List<int>> face_to_edgeid = new List<List<int>>();
                for (int i = 0; i < faces.Count; i++)
                {
                    face_to_edgeid.Add(new List<int>());
                }
                MeshTopologyEdgeList edges = x.Value.mesh.TopologyEdges;
                List<RLineData> Rlinelist = new List<RLineData>();
                for (int i = 0; i < edges.Count; i++)
                {
                    RLineData currl = new RLineData();
                    currl.GeomType = ELineGeomType.lgtStraightLine;

                    int[] faceindexes = edges.GetConnectedFaces(i);
                    foreach (int xxx in faceindexes)
                    {
                        face_to_edgeid[xxx].Add(i);
                    }
                    Rhino.IndexPair pair = edges.GetTopologyVertices(i);
                    currl.NodeId1 = pointids[pair.I];
                    currl.NodeId2 = pointids[pair.J];
                    Rlinelist.Add(currl);
                }
                Array lineid;
                index1 = 0;
                RLineData[] rline_list = new RLineData[edges.Count];
                foreach (RLineData x1 in Rlinelist)
                {
                    rline_list[index1] = x1;
                    index1++;
                }
                AxLines.BulkAdd(rline_list, out lineid);
                int[] lineids = new int[edges.Count];
                index1 = 0;
                foreach (int curr_id in lineid)
                {
                    lineids[index1] = curr_id;
                    index1++;
                }
                List<int> lista_all_edges = new List<int>();
                List<int> edgeCount = new List<int>();
                List<RSurfaceAttr> listaattr = new List<RSurfaceAttr>();
                for (int i = 0; i < faces.Count; i++)
                {
                    MeshFace faceindexes = faces.GetFace(i);
                    List<int> edgeindexes = face_to_edgeid[i];

                    if (faceindexes.IsQuad) // Mixed because GH use other data structure (3-3; 4-2) !
                    {
                        lista_all_edges.Add(lineids[edgeindexes.ElementAt(0)]);
                        lista_all_edges.Add(lineids[edgeindexes.ElementAt(1)]);
                        lista_all_edges.Add(lineids[edgeindexes.ElementAt(3)]);
                        lista_all_edges.Add(lineids[edgeindexes.ElementAt(2)]);
                        edgeCount.Add(4);
                        listaattr.Add(ATTR);
                    }
                    else
                    {
                        lista_all_edges.Add(lineids[edgeindexes.ElementAt(0)]);
                        lista_all_edges.Add(lineids[edgeindexes.ElementAt(1)]);
                        lista_all_edges.Add(lineids[edgeindexes.ElementAt(2)]);
                        edgeCount.Add(3);
                        listaattr.Add(ATTR);
                    }
                }
                Array out_domain;
                int ahhah = axDomains.BulkAdd(edgeCount.ToArray(), lista_all_edges.ToArray(), listaattr.ToArray(), out out_domain);
            }
        }
        public void drawEdges(List<AxisMesh> lista)
        {
            foreach (AxisMesh x in lista)
            {
                MeshTopologyVertexList vertex = x.Value.mesh.TopologyVertices;
                int points = vertex.Count;
                Point3f[] points_ = new Point3f[points];
                for (int i = 0; i < points; i++)
                {
                    points_[i] = vertex.ElementAt(i);
                }
                Array out_points; int[] pointids = new int[points];
                RPoint3d[] points__ = new RPoint3d[points];
                int index1 = 0;
                foreach (Point3d cp in points_)
                {
                    RPoint3d curr = new RPoint3d();
                    curr.x = cp.X; curr.y = cp.Y; curr.z = cp.Z;
                    points__[index1] = curr;
                    index1++;
                }
                AxNodes.BulkAdd(points__, out out_points);
                index1 = 0;
                foreach (int xx in out_points)
                {
                    pointids[index1] = xx; index1++;
                }
                MeshFaceList faces = x.Value.mesh.Faces;
                List<List<int>> face_to_edgeid = new List<List<int>>();
                for (int i = 0; i < faces.Count; i++)
                {
                    face_to_edgeid.Add(new List<int>());
                }
                MeshTopologyEdgeList edges = x.Value.mesh.TopologyEdges;
                List<RLineData> Rlinelist = new List<RLineData>();
                for (int i = 0; i < edges.Count; i++)
                {
                    RLineData currl = new RLineData();
                    currl.GeomType = ELineGeomType.lgtStraightLine;

                    int[] faceindexes = edges.GetConnectedFaces(i);
                    foreach (int xxx in faceindexes)
                    {
                        face_to_edgeid[xxx].Add(i);
                    }
                    Rhino.IndexPair pair = edges.GetTopologyVertices(i);
                    currl.NodeId1 = pointids[pair.I];
                    currl.NodeId2 = pointids[pair.J];
                    Rlinelist.Add(currl);
                }
                Array lineid;
                index1 = 0;
                RLineData[] rline_list = new RLineData[edges.Count];
                foreach (RLineData x1 in Rlinelist)
                {
                    rline_list[index1] = x1; index1++;
                }
                AxLines.BulkAdd(rline_list, out lineid);

                if (x.Value.crs != null && x.Value.material != null)
                {
                    RLineAttr[] lista1 = new RLineAttr[edges.Count];
                    int material = mats.AddFromCatalog((ENationalDesignCode)System.Enum.Parse(typeof(ENationalDesignCode), (x.Value.material.Split('}')[0])), x.Value.material.Split('}')[1]);
                    int crossi = crsec.AddFromCatalog((ECrossSectionShape)System.Enum.Parse(typeof(ECrossSectionShape), (x.Value.crs.Split('}')[0])), x.Value.crs.Split('}')[1]);
                    for (int k = 0; k < edges.Count; k++)
                    {
                        lista1[k].LineType = ELineType.ltTruss;
                        lista1[k].MaterialIndex = material;
                        lista1[k].EndCrossSectionIndex = crossi;
                        lista1[k].StartCrossSectionIndex = crossi;
                        lista1[k].Resistance = 10;
                    }
                    AxLines.BulkSetAttr(lineid, lista1);
                }
            }
        }
    }
}
