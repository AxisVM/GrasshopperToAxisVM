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
        public void drawPonts(List<AxisPoint> lista1)
        {
            foreach (AxisPoint lista in lista1)
            {
                RPoint3d[] sender_p = new RPoint3d[lista.Value.point.Count]; Array out_id;
                int index1 = 0;
                foreach (Point3d x in lista.Value.point)
                {
                    RPoint3d AXP = new RPoint3d();
                    AXP.x = x.X; AXP.y = x.Y; AXP.z = x.Z;
                    sender_p[index1] = AXP;
                    index1++;
                }
                AxNodes.BulkAdd(sender_p, out out_id);
                if(lista.Value.sup.Rx != 0 || lista.Value.sup.Rxx != 0 || lista.Value.sup.Ry != 0 || lista.Value.sup.Ryy != 0 || lista.Value.sup.Rz != 0 || lista.Value.sup.Rzz != 0)
                {
                    RStiffnesses node_supp_Stiff = new RStiffnesses();
                    RNonLinearity node_supp_NonLin = new RNonLinearity
                    {
                        x = ELineNonLinearity.lnlTensionAndCompression,
                        y = ELineNonLinearity.lnlTensionAndCompression,
                        z = ELineNonLinearity.lnlTensionAndCompression,
                        xx = ELineNonLinearity.lnlTensionAndCompression,
                        yy = ELineNonLinearity.lnlTensionAndCompression,
                        zz = ELineNonLinearity.lnlTensionAndCompression,
                    };
                    RResistances node_supp_Resistance = new RResistances { x = 0, y = 0, z = 0, xx = 0, yy = 0, zz = 0, };
                    node_supp_Stiff.x =  lista.Value.sup.Rx;
                    node_supp_Stiff.y =  lista.Value.sup.Ry;
                    node_supp_Stiff.z =  lista.Value.sup.Rz;
                    node_supp_Stiff.xx = lista.Value.sup.Rxx;
                    node_supp_Stiff.yy = lista.Value.sup.Ryy;
                    node_supp_Stiff.zz = lista.Value.sup.Rzz;
                    foreach (int x in out_id)
                    {
                        axNodalsupport.AddNodalGlobal(node_supp_Stiff, node_supp_NonLin, node_supp_Resistance,x);
                    } 
                }
                if (lista.Value.lad.Rx != 0 || lista.Value.lad.Rxx != 0 || lista.Value.lad.Ry != 0 || lista.Value.lad.Ryy != 0 || lista.Value.lad.Rz != 0 || lista.Value.lad.Rzz != 0)
                {
                    foreach (int x in out_id)
                    {
                        RLoadNodalForce rlnf = new RLoadNodalForce();
                        rlnf.Fx = lista.Value.lad.Rx; rlnf.Fy = lista.Value.lad.Ry; rlnf.Fz = lista.Value.lad.Rz; rlnf.Mx = lista.Value.lad.Rxx; rlnf.My = lista.Value.lad.Ryy; rlnf.Mz = lista.Value.lad.Rzz;
                        rlnf.LoadCaseId = 1; rlnf.NodeId = x;
                        rlnf.ReferenceId = 0;
                        axloads.AddNodalForce(rlnf);
                    }
                }
            }
        }
        public void drawLines(List<AxisLine>lista123)
        {
            foreach (AxisLine lista_c in lista123)
            {
                List<Line> lista = lista_c.Value.line;
                RPoint3d[] sender_p1 = new RPoint3d[lista.Count]; Array op1;
                RPoint3d[] sender_p2 = new RPoint3d[lista.Count]; Array op2; Array ol;
                List<RLineData> sender = new List<RLineData>();
                int[] op11 = new int[lista.Count];
                int[] op22 = new int[lista.Count];
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
                AxNodes.BulkAdd(sender_p1, out op1); AxNodes.BulkAdd(sender_p2, out op2);
                index1 = 0;
                foreach (int x in op1)
                {
                    op11[index1] = x;
                    index1++;
                }
                index1 = 0;
                foreach (int x in op2)
                {
                    op22[index1] = x;
                    index1++;
                }
                for (int i = 0; i < lista.Count; i++)
                {
                    RLineData x = new RLineData();
                    x.GeomType = ELineGeomType.lgtStraightLine;
                    x.NodeId1 = op11[i];
                    x.NodeId2 = op22[i];
                    sender.Add(x);
                }
                AxLines.BulkAdd(sender.ToArray(), out ol);

                RLineAttr[] lista1 = new RLineAttr[lista.Count()];
                bool adhato = false;
                int material = -1; int crossi = -1;
                if (lista.Count >= 1 && lista_c.Value.material != null)
                {
                    material = mats.AddFromCatalog((ENationalDesignCode)System.Enum.Parse(typeof(ENationalDesignCode), (lista_c.Value.material.Split('}')[0])), lista_c.Value.material.Split('}')[1]);
                    crossi = crsec.AddFromCatalog((ECrossSectionShape)System.Enum.Parse(typeof(ECrossSectionShape), (lista_c.Value.crosssection.Split('}')[0])), lista_c.Value.crosssection.Split('}')[1]);
                    adhato = true;
                }
                if (adhato)
                {
                    for (int k = 0; k < lista.Count(); k++)
                    {
                        if (lista_c.Value.num == 0){ lista1[k].LineType = ELineType.ltBeam; }
                        else if (lista_c.Value.num == 1) { lista1[k].LineType = ELineType.ltRib; }
                        else { lista1[k].LineType = ELineType.ltTruss; }
                        lista1[k].MaterialIndex = material;
                        lista1[k].EndCrossSectionIndex = crossi;
                        lista1[k].StartCrossSectionIndex = crossi;
                        lista1[k].Resistance = 10;
                    }
                    int xxxxxx = AxLines.BulkSetAttr(ol, lista1);
                }
            }     
        }
        public void drawMeshs(List<AxisMesh> lista)
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
                int[] lineids = new int[edges.Count];
                index1 = 0;
                foreach (int xxxx in lineid)
                {
                    lineids[index1] = xxxx; index1++;
                }
                List<RSurface> listasurf = new List<RSurface>();
                for (int i = 0; i < faces.Count; i++)
                {
                    MeshFace faceindexes = faces.GetFace(i);
                    List<int> edgeindexes = face_to_edgeid[i];
                    RSurface rsf = new RSurface();
                    rsf.Attr = ATTR;
                    if (faceindexes.IsQuad) // Mixed because GH use other data structure (3-3; 4-2) !
                    {
                        rsf.LineIndex1 = lineids[edgeindexes.ElementAt(0)];
                        rsf.LineIndex2 = lineids[edgeindexes.ElementAt(1)];
                        rsf.LineIndex3 = lineids[edgeindexes.ElementAt(3)];
                        rsf.LineIndex4 = lineids[edgeindexes.ElementAt(2)];
                        rsf.N = 4;
                        rsf.DomainIndex = 0;
                    }
                    else
                    {
                        rsf.LineIndex1 = lineids[edgeindexes.ElementAt(0)];
                        rsf.LineIndex2 = lineids[edgeindexes.ElementAt(1)];
                        rsf.LineIndex3 = lineids[edgeindexes.ElementAt(2)];
                        rsf.N = 3;
                        rsf.DomainIndex = 0;
                    }
                    listasurf.Add(rsf);
                }
                Array out_surface;
                RSurface[] rline_listz = new RSurface[faces.Count];
                index1 = 0;
                foreach (RSurface yyy in listasurf)
                {
                    rline_listz[index1] = yyy;
                    index1++;
                }
                int ahhah = axisSurfaces.BulkAdd(rline_listz, out out_surface);
            }
        }
        
    }
}
