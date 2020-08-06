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
        BackgroundWorker bw1 = new BackgroundWorker();
        IGH_DataAccess arg = null;
        public void bw1_DoWork(object sender, DoWorkEventArgs e)
        {
            IGH_DataAccess DA = (IGH_DataAccess)e.Argument;
            AxisData data = null;
            if(!DA.GetData(7, ref data)) { }
            //TESTNEW
            if (!this.ModellSet)
            {
                bool newy = data.Value.isNew;
                //string data = null;
                if (!newy)
                {
                    if (data.Value.filePath == null || data.Value.filePath == "") { DA.AbortComponentSolution(); return; }
                    else
                    {
                            //INIT EXISTING
                            this.initialize(data.Value.filePath);
                            this.ModellSet = true;
                    }
                }
                else
                {
                    //INIT NEW
                    initialize();
                    this.ModellSet = true;
                }
            }
            
            //TESTNEWend
            bool only_pointcoordinates=data.Value.justPoints;
            if (only_pointcoordinates)
            {
                List<AxisPoint> ponts = new List<AxisPoint>();
                List<AxisLine> lines = new List<AxisLine>();
                List<AxisMesh> meshs = new List<AxisMesh>();
                List<AxisMesh> doms = new List<AxisMesh>();
                List<AxisMesh> edges = new List<AxisMesh>();
                List<AxisDomain> dom2s = new List<AxisDomain>();
                AxModel.BeginUpdate();
                List<int> setid = new List<int>(); List<RPoint3d> setcord = new List<RPoint3d>();
                foreach (Structs.AxisMember curr in this.points_sent)
                {
                    setid.AddRange(curr.axisid_point);
                }
                if (DA.GetDataList(0, ponts)) { drawPonts_JP(ponts,setid.ToArray()); }

                setid = new List<int>();
                foreach (Structs.AxisMember curr in this.lines_sent)
                { 
                    setid.AddRange(curr.axisid_point);
                }
                if (DA.GetDataList(1, lines)) { drawLines_JP(lines,setid.ToArray()); }

                setid = new List<int>();
                foreach (Structs.AxisMember curr in this.surfaces_sent)
                {
                    setid.AddRange(curr.axisid_point);
                }
                if (DA.GetDataList(2, meshs)) { drawMeshs_JP(meshs,setid.ToArray()); }

                setid = new List<int>();
                foreach (Structs.AxisMember curr in this.domains_sent)
                {
                    setid.AddRange(curr.axisid_point);
                }
                if (DA.GetDataList(3, doms)) { drawMeshs2_JP(doms,setid.ToArray()); }

                setid = new List<int>();
                foreach (Structs.AxisMember curr in this.edges_sent)
                {
                    setid.AddRange(curr.axisid_point);
                }
                if (DA.GetDataList(4, edges)) { drawEdges_JP(edges,setid.ToArray()); }

                setid = new List<int>();
                foreach (Structs.AxisMember curr in this.domain2s_sent)
                {
                    setid.AddRange(curr.axisid_point);
                }
                if (DA.GetDataList(5, dom2s)) { drawDomains_JP(dom2s, setid.ToArray()); }

                //int aaa = AxNodes.BulkSetNodeCoord(setid.ToArray(), setcord.ToArray());
                AxModel.EndUpdate();
                AxApp.Visible = ELongBoolean.lbTrue;
            }
            else //IF aT is false then continue //triggered deletion missing and disconnected deletemissing
            {
                if (data.Value.sendOnlyChanges)
                {
                    this.sendOnlyChanges = true;
                    List<int> deleter = new List<int>();
                    //Lists to delete from sent
                    List<Structs.AxisMember> pRem = new List<Structs.AxisMember>();
                    List<Structs.AxisMember> lRem = new List<Structs.AxisMember>();
                    List<Structs.AxisMember> sRem = new List<Structs.AxisMember>();
                    List<Structs.AxisMember> dRem = new List<Structs.AxisMember>();
                    List<Structs.AxisMember> eRem = new List<Structs.AxisMember>();
                    List<Structs.AxisMember> d2Rem = new List<Structs.AxisMember>();
                    List<Structs.AxLoadCase> lcRem = new List<Structs.AxLoadCase>();
                    // Lists were GOT thing stored
                    List<AxisPoint> ponts = new List<AxisPoint>();
                    List<AxisLine> lines = new List<AxisLine>();
                    List<AxisMesh> meshs = new List<AxisMesh>();
                    List<AxisMesh> doms = new List<AxisMesh>();
                    List<AxisMesh> edges = new List<AxisMesh>();
                    List<AxisDomain> dom2s = new List<AxisDomain>();
                    List<AxLoadCase> loadcases = new List<AxLoadCase>();
                    // List of Remoing
                    List<AxisPoint> rponts = new List<AxisPoint>();
                    List<AxisLine> rlines = new List<AxisLine>();
                    List<AxisMesh> rmeshs = new List<AxisMesh>();
                    List<AxisMesh> rdoms = new List<AxisMesh>();
                    List<AxisMesh> redges = new List<AxisMesh>();
                    List<AxisDomain> rdom2s = new List<AxisDomain>();
                    List<AxLoadCase> rloadcases = new List<AxLoadCase>();
                    if (DA.GetDataList(0, ponts)) { }
                    if (DA.GetDataList(1, lines)) { }
                    if (DA.GetDataList(2, meshs)) { }
                    if (DA.GetDataList(3, doms)) { }
                    if (DA.GetDataList(4, edges)) { }
                    if (DA.GetDataList(5, dom2s)) { }
                    if (DA.GetDataList(6, loadcases)) { }
                    
                    // We have the previous ans current dataflow here
                    //add deleter if Find is empty or not empty, and not equaé
                    //delete from local lists if it exists and equal in sent (the reamining will be drawinged)

                    //pts

                    foreach (AxisPoint curr in ponts)
                    {
                        if (this.points_sent.FindAll(x => x.Equals(curr.Value)).Count == 0) { }
                        else
                        {
                            rponts.Add(curr);
                        }
                    }
                    foreach (Structs.AxisPoint curr in this.points_sent)
                    {
                        if (ponts.FindAll(x => x.Value.GeometryEquals(curr)).Count == 0)
                        {
                            pRem.Add(curr);
                            deleter.AddRange(curr.axisid_point);
                        }
                    }
                    this.points_sent.RemoveAll(x => pRem.Contains(x));
                    ponts.RemoveAll(x => rponts.Contains(x));

                    //lns
                    foreach (AxisLine curr in lines)
                    {
                        if (this.lines_sent.FindAll(x => x.Equals(curr.Value)).Count == 0) { }
                        else
                        {
                            rlines.Add(curr);
                        }
                    }
                    foreach (Structs.AxisLine curr in this.lines_sent)
                    {
                        if (lines.FindAll(x => x.Value.GeometryEquals(curr)).Count == 0)
                        {
                            lRem.Add(curr);
                            deleter.AddRange(curr.axisid_point);
                        }
                    }
                    this.lines_sent.RemoveAll(x => lRem.Contains(x));
                    lines.RemoveAll(x => rlines.Contains(x));

                    //srfs
                    foreach (AxisMesh curr in meshs)
                    {
                        if (this.surfaces_sent.FindAll(x => x.Equals(curr.Value)).Count == 0) { }
                        else
                        {
                            rmeshs.Add(curr);
                        }
                    }
                    foreach (Structs.AxisMesh curr in this.surfaces_sent)
                    {
                        if (meshs.FindAll(x => x.Value.GeometryEquals(curr)).Count == 0)
                        {
                            sRem.Add(curr);
                            deleter.AddRange(curr.axisid_point);
                        }
                    }
                    this.surfaces_sent.RemoveAll(x => sRem.Contains(x));
                    meshs.RemoveAll(x => rmeshs.Contains(x));


                    //doms
                    foreach (AxisMesh curr in doms)
                    {
                        if (this.domains_sent.FindAll(x => x.Equals(curr.Value)).Count == 0) { }
                        else
                        {
                            rdoms.Add(curr);
                        }
                    }
                    foreach (Structs.AxisMesh curr in this.domains_sent)
                    {
                        if (doms.FindAll(x => x.Value.GeometryEquals(curr)).Count == 0)
                        {
                            dRem.Add(curr);
                            deleter.AddRange(curr.axisid_point);
                        }
                    }
                    this.domains_sent.RemoveAll(x => dRem.Contains(x));
                    doms.RemoveAll(x => rdoms.Contains(x));


                    //edges
                    foreach (AxisMesh curr in edges)
                    {
                        if (this.edges_sent.FindAll(x => x.Equals(curr.Value)).Count == 0) { }
                        else
                        {
                            redges.Add(curr);
                        }
                    }
                    foreach (Structs.AxisMesh curr in this.edges_sent)
                    {
                        if (edges.FindAll(x => x.Value.GeometryEquals(curr)).Count == 0)
                        {
                            eRem.Add(curr);
                            deleter.AddRange(curr.axisid_point);
                        }
                    }
                    this.edges_sent.RemoveAll(x => eRem.Contains(x));
                    edges.RemoveAll(x => redges.Contains(x));


                    /*    //lc
                        foreach (AxLoadCase curr in loadcases)
                        {
                            //pRem.AddRange(this.points_sent.FindAll(x => !x.Equals(curr.Value)));
                            if (this.lc_sent.FindAll(x => x.Equals(curr.Value)).Count == 0)
                            {
                                //deleter.AddRange(curr.Value.axisid_point);
                            }
                            else
                            {
                                rloadcases.Add(curr);
                            }
                        }
                        foreach (Structs.AxLoadCase curr in this.lc_sent)
                        {
                            // rponts.AddRange(ponts.FindAll(x => x.Value.Equals(curr)));
                            if (loadcases.FindAll(x => x.Value.Equals(curr)).Count == 0)
                            {
                                lcRem.Add(curr);
                                deleter.AddRange(curr.axisid_point);
                            }
                        }
                        this.points_sent.RemoveAll(x => pRem.Contains(x));
                        ponts.RemoveAll(x => rponts.Contains(x));*/


                    //dom2s
                    foreach (AxisDomain curr in dom2s)
                    {
                        if (this.domain2s_sent.FindAll(x => x.Equals(curr.Value)).Count == 0) { }
                        else
                        {
                            rdom2s.Add(curr);
                        }
                    }
                    //ONLY FULL RESEND, NOT ONLY MATERIAL AND CRS CHECK DUE TO HOLES
                    foreach (Structs.AxisDomain curr in this.domain2s_sent)
                    {
                        if (dom2s.FindAll(x => x.Value.Equals(curr)).Count == 0)
                        {
                            d2Rem.Add(curr);
                            deleter.AddRange(curr.axisid_point);
                        }
                    }
                    this.domain2s_sent.RemoveAll(x => d2Rem.Contains(x));
                    dom2s.RemoveAll(x => rdom2s.Contains(x));

                    AxModel.BeginUpdate();
                    //AxNodes.SelectAll(ELongBoolean.lbTrue);
                    //AxNodes.DeleteSelected();
                    AxNodes.BulkDelete(deleter.ToArray());

                    this.points_sent.AddRange(drawPonts(ponts));
                    this.lines_sent.AddRange(drawLines(lines));
                    this.surfaces_sent.AddRange(drawMeshs(meshs));
                    this.domains_sent.AddRange(drawMeshs2(doms));
                    this.edges_sent.AddRange(drawEdges(edges));
                    this.domain2s_sent.AddRange(drawDomains(dom2s));
                    if (DA.GetDataList(6, loadcases)) { loadFill(loadcases); }

                    //CHECKER
                    if (data.Value.checkDouble)
                    {
                        AxNodes.Check(0.000001, ELongBoolean.lbTrue, ELongBoolean.lbTrue);
                    }
                    //CHECKEREND
                    AxModel.EndUpdate();
                    AxApp.Visible = ELongBoolean.lbTrue;
                }
                else
                {
                    this.sendOnlyChanges = false;
                    List<int> deleter = new List<int>();
                    List<Structs.AxisMember> pRem = new List<Structs.AxisMember>();
                    List<Structs.AxisMember> lRem = new List<Structs.AxisMember>();
                    List<Structs.AxisMember> sRem = new List<Structs.AxisMember>();
                    List<Structs.AxisMember> dRem = new List<Structs.AxisMember>();
                    List<Structs.AxisMember> eRem = new List<Structs.AxisMember>();
                    List<Structs.AxisMember> d2Rem = new List<Structs.AxisMember>();
                    foreach (Structs.AxisMember curr in this.points_sent)
                    {
                        //if(curr.deleting == false) { continue; }
                        deleter.AddRange(curr.axisid_point);
                        //curr.deleting = false;
                        pRem.Add(curr);
                    }
                    foreach (Structs.AxisMember curr in this.lines_sent)
                    {
                        //if (curr.deleting == false) { continue; }
                        deleter.AddRange(curr.axisid_point);
                        //curr.deleting = false;
                        lRem.Add(curr);
                    }
                    foreach (Structs.AxisMember curr in this.surfaces_sent)
                    {
                        //if (curr.deleting == false) { continue; }
                        deleter.AddRange(curr.axisid_point);
                        //curr.deleting = false;
                        sRem.Add(curr);
                    }
                    foreach (Structs.AxisMember curr in this.domains_sent)
                    {
                        //if (curr.deleting == false) { continue; }
                        deleter.AddRange(curr.axisid_point);
                        //curr.deleting = false;
                        dRem.Add(curr);
                    }
                    foreach (Structs.AxisMember curr in this.edges_sent)
                    {
                        //if (curr.deleting == false) { continue; }
                        deleter.AddRange(curr.axisid_point);
                        //curr.deleting = false;
                        eRem.Add(curr);
                    }
                    foreach (Structs.AxisMember curr in this.domain2s_sent)
                    {
                        //if (curr.deleting == false) { continue; }
                        deleter.AddRange(curr.axisid_point);
                        //curr.deleting = false;
                        d2Rem.Add(curr);
                    }
                    this.points_sent.Clear();
                    this.lines_sent.Clear();
                    this.surfaces_sent.Clear();
                    this.domains_sent.Clear();
                    this.edges_sent.Clear();
                    this.domain2s_sent.Clear();

                    AxModel.BeginUpdate();
                    //AxNodes.SelectAll(ELongBoolean.lbTrue);
                    //AxNodes.DeleteSelected();
                    AxNodes.BulkDelete(deleter.ToArray());
                    List<AxisPoint> ponts = new List<AxisPoint>();
                    List<AxisLine> lines = new List<AxisLine>();
                    List<AxisMesh> meshs = new List<AxisMesh>();
                    List<AxisMesh> doms = new List<AxisMesh>();
                    List<AxisMesh> edges = new List<AxisMesh>();
                    List<AxisDomain> dom2s = new List<AxisDomain>();
                    List<AxLoadCase> loadcases = new List<AxLoadCase>();
                    if (DA.GetDataList(0, ponts)) { this.points_sent.AddRange(drawPonts(ponts)); }
                    if (DA.GetDataList(1, lines)) { this.lines_sent.AddRange(drawLines(lines)); }
                    if (DA.GetDataList(2, meshs)) { this.surfaces_sent.AddRange(drawMeshs(meshs)); }
                    if (DA.GetDataList(3, doms)) { this.domains_sent.AddRange(drawMeshs2(doms)); }
                    if (DA.GetDataList(4, edges)) { this.edges_sent.AddRange(drawEdges(edges)); }
                    if (DA.GetDataList(5, dom2s)) { this.domain2s_sent.AddRange(drawDomains(dom2s)); }
                    if (DA.GetDataList(6, loadcases)) { loadFill(loadcases); }

                    //CHECKER
                    if (data.Value.checkDouble)
                    {
                        AxNodes.Check(0.000001, ELongBoolean.lbTrue, ELongBoolean.lbTrue);
                    }
                    //CHECKEREND
                    AxModel.EndUpdate();
                    AxApp.Visible = ELongBoolean.lbTrue;
                }
            }
            if (data.Value.axAnalysis)
            {
                this.axisRunAnalysis();
            }
            //  S   E    T   P  R   E   V S 
        }
        private void Bw1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (arg != null) { bw1.RunWorkerAsync(arg); arg = null; }
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (bw1.IsBusy == false)
            {
                bw1.RunWorkerAsync(DA);
                arg = null;
            }
            else
            {
                arg = DA;
            }
        }
    }
}