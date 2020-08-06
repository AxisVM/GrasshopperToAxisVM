using System.Collections.Generic;
using System;
using System.Linq;
using AxisVM;
using Grasshopper.Kernel;
using System.ComponentModel;

namespace GrassHopperToAxisVM
{
    public partial class GrassHopperToAxisVMComponent : GH_Component
    {
        public void loadFill(List<AxLoadCase> loadlist)
        {
            int loadcaseid = 0;
            foreach(AxLoadCase loadCase in loadlist)
            {
                loadcaseid++;
                //nodal load
                foreach (Structs.AxNodalLoad axNodalLoad in loadCase.Value.NL)
                {
                    if (axNodalLoad.Rx != 0 || axNodalLoad.Rxx != 0 || axNodalLoad.Ry != 0 || axNodalLoad.Ryy != 0 || axNodalLoad.Rz != 0 || axNodalLoad.Rzz != 0)
                    {
                        foreach (int cpoint in axNodalLoad.points.axisid_point)
                        {
                            RLoadNodalForce rlnf = new RLoadNodalForce();
                            rlnf.Fx = axNodalLoad.Rx; rlnf.Fy = axNodalLoad.Ry; rlnf.Fz = axNodalLoad.Rz; rlnf.Mx = axNodalLoad.Rxx; rlnf.My = axNodalLoad.Ryy; rlnf.Mz = axNodalLoad.Rzz;
                            rlnf.LoadCaseId = loadcaseid;
                            rlnf.ReferenceId = 0; rlnf.NodeId = cpoint;
                            axloads.AddNodalForce(rlnf);

                        }
                    }     
                }
                // DDL under Test
                foreach (Structs.AxDistributedDomainLoad axddl in loadCase.Value.DDL)
                {
                    if ((axddl.mesh != null || axddl.doma != null) && axddl.intensity != 0)
                    {
                        if(axddl.mesh!=null)
                        foreach (int cpoint in axddl.mesh.axisid_dom)
                        {
                            RLoadDomainConstant_V154 rldc= new RLoadDomainConstant_V154();rldc.qx = 0;rldc.qy = 0;rldc.qz = 0;
                            rldc.DomainId = cpoint;
                            rldc.LoadCaseId = loadcaseid;
                            rldc.DistributionType = ESurfaceDomainDistributionType.sddtSurface;
                            if (axddl.typ == Structs.AxDistributedDomainLoad.type.global) { rldc.SystemGLR = ESystem.sysGlobal; }
                            else { rldc.SystemGLR = ESystem.sysLocal; }
                            if (axddl.dir == Structs.AxDistributedDomainLoad.direction.X) { rldc.qx = axddl.intensity; }
                            else if(axddl.dir== Structs.AxDistributedDomainLoad.direction.Y){ rldc.qy = axddl.intensity; }
                            else{ rldc.qz = axddl.intensity; }
                            axloads.AddDomainConstant_V154(rldc);
                        }

                        if (axddl.doma != null)
                            foreach (int cpoint in axddl.doma.axisid_dom)
                            {
                                RLoadDomainConstant_V154 rldc = new RLoadDomainConstant_V154(); rldc.qx = 0; rldc.qy = 0; rldc.qz = 0;
                                rldc.DomainId = cpoint;
                                rldc.LoadCaseId = loadcaseid;
                                rldc.DistributionType = ESurfaceDomainDistributionType.sddtSurface;
                                if (axddl.typ == Structs.AxDistributedDomainLoad.type.global) { rldc.SystemGLR = ESystem.sysGlobal; }
                                else { rldc.SystemGLR = ESystem.sysLocal; }
                                if (axddl.dir == Structs.AxDistributedDomainLoad.direction.X) { rldc.qx = axddl.intensity; }
                                else if (axddl.dir == Structs.AxDistributedDomainLoad.direction.Y) { rldc.qy = axddl.intensity; }
                                else { rldc.qz = axddl.intensity; }
                                axloads.AddDomainConstant_V154(rldc);
                            }
                        //axloads.AddDomainLinear(rlsd);
                    }
                }
                //DLL 
                foreach (Structs.AxDistributedLineLoad axdll in loadCase.Value.DLL)
                {
                    if (axdll.int2!=0 || axdll.int2 !=0)
                    {
                        foreach (int cpoint in axdll.lines.axisid_line)
                        {
                            if (axdll.lines.num == 0)//beam
                            {
                                RLoadBeamDistributed rlbd = new RLoadBeamDistributed();
                                rlbd.DistributionType = EBeamRibDistributionType.brdtLength;
                                rlbd.Position1 = axdll.start;
                                rlbd.Position2 = axdll.end;
                                rlbd.LineId = cpoint;
                                rlbd.LoadCaseId = loadcaseid;
                                rlbd.Trapezoid = ELongBoolean.lbTrue;
                                rlbd.LoadCaseId = loadcaseid;
                                if (axdll.dir == Structs.AxDistributedLineLoad.direction.X)
                                {
                                    rlbd.qx1 = axdll.int1;
                                    rlbd.qx2 = axdll.int2;   
                                }
                                else if (axdll.dir == Structs.AxDistributedLineLoad.direction.Y)
                                {
                                    rlbd.qy1 = axdll.int1;
                                    rlbd.qy2 = axdll.int2;
                                }
                                else
                                {
                                    rlbd.qz1 = axdll.int1;
                                    rlbd.qz2 = axdll.int2;
                                }
                                int result = axloads.AddBeamDistributed(rlbd);
                            }
                            else if (axdll.lines.num == 1)//rib
                            {
                                RLoadRibDistributed rlbd = new RLoadRibDistributed();
                                rlbd.DistributionType = EBeamRibDistributionType.brdtLength;
                                rlbd.Position1 = axdll.start;
                                rlbd.Position2 = axdll.end;
                                rlbd.LineId = cpoint;
                                rlbd.LoadCaseId = loadcaseid;
                                rlbd.Trapezoid = ELongBoolean.lbFalse;
                                rlbd.LoadCaseId = loadcaseid;
                                if (axdll.dir == Structs.AxDistributedLineLoad.direction.X)
                                {
                                    rlbd.qx1 = axdll.int1;
                                    rlbd.qx2 = axdll.int2;
                                }
                                else if (axdll.dir == Structs.AxDistributedLineLoad.direction.Y)
                                {
                                    rlbd.qy1 = axdll.int1;
                                    rlbd.qy2 = axdll.int2;
                                }
                                else
                                {
                                    rlbd.qz1 = axdll.int1;
                                    rlbd.qz2 = axdll.int2;
                                }
                                int result = axloads.AddRibDistributed(rlbd);
                            }
                            else//truss
                            {
                                //EXISTS?
                            }
                        }
                    }
                }
                //DSL
                foreach (Structs.AxDistributedSurfaceLoad axdsl in loadCase.Value.DSL)
                {
                    if (axdsl.intensity != 0)
                    {
                        Array loadids;
                        List<RLoadSurfaceDistributed> list = new List<RLoadSurfaceDistributed>(); 
                        foreach (int cpoint in axdsl.mesh.axisid_mesh)
                        {
                            RLoadSurfaceDistributed rlsd = new RLoadSurfaceDistributed();
                            rlsd.DistributionType = ESurfaceDomainDistributionType.sddtSurface;
                            rlsd.SurfaceId = cpoint;
                            rlsd.LoadCaseId = loadcaseid;
                            if (axdsl.typ == Structs.AxDistributedSurfaceLoad.type.global)
                            { rlsd.SystemGLR = ESystem.sysGlobal; }
                            else
                            {
                                rlsd.SystemGLR = ESystem.sysLocal;
                            }
                            if (axdsl.dir == Structs.AxDistributedSurfaceLoad.direction.X) { rlsd.qx = axdsl.intensity; }
                            else if (axdsl.dir == Structs.AxDistributedSurfaceLoad.direction.Y) { rlsd.qy = axdsl.intensity; }
                            else { rlsd.qz = axdsl.intensity; }
                            list.Add(rlsd);
                        }
                        axloads.AddSurfaceDistributeds(list.ToArray(), out loadids);
                    }
                }
                //DAL //needs recreation of type Domain
                foreach (Structs.AxDomainAreaLoad axdal in loadCase.Value.DAL)
                {
                    if ((axdal.mesh != null || axdal.doma != null) && axdal.intensity != 0)
                    {
                        Array loadids;
                        if(axdal.mesh!=null)
                        foreach (int cpoint in axdal.mesh.axisid_dom)
                        {
                            AxisVMLines3d l3d = new AxisVMLines3d();
                            RLoadDomainPolyArea rldp = new RLoadDomainPolyArea();
                            if (axdal.typ == Structs.AxDomainAreaLoad.type.global) rldp.DistributionType = EDistributionType.dtGlobal;
                            else { rldp.DistributionType = EDistributionType.dtLocal; }
                            rldp.LoadCaseId = loadcaseid;
                            if(axdal.dir == Structs.AxDomainAreaLoad.direction.X)rldp.Component = EAxis.aX;
                            else if (axdal.dir == Structs.AxDomainAreaLoad.direction.Y) rldp.Component = EAxis.aY;
                            else rldp.Component = EAxis.aZ;

                                rldp.P1 = axdal.intensity;
                            rldp.LoadDistributionType = ELoadDistributionType.ldtConst;
                            Rhino.Geometry.Line[] allLines = axdal.polygon.GetSegments();
                            foreach (Rhino.Geometry.Line x in allLines)
                            {
                                RLine3d axLine = new RLine3d();
                                RPoint3d P1 = new RPoint3d(); RPoint3d P2 = new RPoint3d();
                                P1.x = x.FromX; P1.y = x.FromY; P1.z = x.FromZ;
                                P2.x = x.ToX; P2.y = x.ToY; P2.z = x.ToZ; 
                                axLine.LineType = ELine3dType.ltStraightLine3d;
                                axLine.P1 = P1;
                                axLine.P2 = P2;
                                l3d.Add(axLine);
                            }
                            int s = axloads.AddDomainPolyArea(l3d, rldp);
                        }

                        if (axdal.doma != null)
                            foreach (int cpoint in axdal.doma.axisid_dom)
                            {
                                AxisVMLines3d l3d = new AxisVMLines3d();
                                RLoadDomainPolyArea rldp = new RLoadDomainPolyArea();
                                if (axdal.typ == Structs.AxDomainAreaLoad.type.global) rldp.DistributionType = EDistributionType.dtGlobal;
                                else { rldp.DistributionType = EDistributionType.dtLocal; }
                                rldp.LoadCaseId = loadcaseid;
                                if (axdal.dir == Structs.AxDomainAreaLoad.direction.X) rldp.Component = EAxis.aX;
                                else if (axdal.dir == Structs.AxDomainAreaLoad.direction.Y) rldp.Component = EAxis.aY;
                                else rldp.Component = EAxis.aZ;
                                rldp.P1 = axdal.intensity;
                                rldp.LoadDistributionType = ELoadDistributionType.ldtConst;
                                Rhino.Geometry.Line[] allLines = axdal.polygon.GetSegments();
                                foreach (Rhino.Geometry.Line x in allLines)
                                {
                                    RLine3d axLine = new RLine3d();
                                    RPoint3d P1 = new RPoint3d(); RPoint3d P2 = new RPoint3d();
                                    P1.x = x.FromX; P1.y = x.FromY; P1.z = x.FromZ;
                                    P2.x = x.ToX; P2.y = x.ToY; P2.z = x.ToZ;
                                    axLine.LineType = ELine3dType.ltStraightLine3d;
                                    axLine.P1 = P1;
                                    axLine.P2 = P2;
                                    l3d.Add(axLine);
                                }
                                axloads.AddDomainPolyArea(l3d, rldp);

                            }
                        //AxisVMLines3d x;x.
                        //axloads.AddDomainPolyArea();
                    }
                }
                //SW NEEDS MORE IMPL
                foreach (Structs.AxSelfWeight axsw in loadCase.Value.SW)
                {
                    if (true)
                    {
                        foreach (int cpoint in axsw.lines.axisid_line)
                        {
                            if (axsw.lines.num == 1) axloads.AddRibSelfWeight(cpoint, loadcaseid);
                            else if (axsw.lines.num == 2) axloads.AddTrussSelfWeight(cpoint, loadcaseid);
                            else { axloads.AddBeamSelfWeight(cpoint, loadcaseid); }
                        }
                        foreach (int cpoint in axsw.edges.axisid_edge)
                        {
                            if (axsw.edges.num == 1) axloads.AddRibSelfWeight(cpoint, loadcaseid);
                            else if (axsw.edges.num == 2) axloads.AddTrussSelfWeight(cpoint, loadcaseid);
                            else { axloads.AddBeamSelfWeight(cpoint, loadcaseid); }
                        }
                        foreach (int cpoint in axsw.surfs.axisid_mesh)
                        {
                            axloads.AddSurfaceSelfWeight(cpoint, loadcaseid);
                        }
                        foreach (int cpoint in axsw.doms.axisid_dom)
                        {
                            axloads.AddDomainSelfWeight(cpoint, loadcaseid);
                        }
                        foreach (int cpoint in axsw.dom2.axisid_dom)
                        {
                            axloads.AddDomainSelfWeight(cpoint, loadcaseid);
                        }

                    }
                }
            }
        }
    }
}
/*if (axNodalLoad.Rx != 0 || axNodalLoad.Rxx != 0 || axNodalLoad.Ry != 0 || axNodalLoad.Ryy != 0 || axNodalLoad.Rz != 0 || axNodalLoad.Rzz != 0)
               {
                   foreach (int x in out_id)
                   {
                       RLoadNodalForce rlnf = new RLoadNodalForce();
                       rlnf.Fx = axNodalLoad.Rx; rlnf.Fy = axNodalLoad.Ry; rlnf.Fz = axNodalLoad.Rz; rlnf.Mx = axNodalLoad.Rxx; rlnf.My = axNodalLoad.Ryy; rlnf.Mz = axNodalLoad.Rzz;
                       rlnf.LoadCaseId = 1; rlnf.NodeId = x;
                       rlnf.ReferenceId = 0;
                       axloads.AddNodalForce(rlnf);
                   }
               }*/
