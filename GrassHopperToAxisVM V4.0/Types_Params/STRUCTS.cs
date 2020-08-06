using System;
using System.Collections.Generic;
using AxisVM;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace GrassHopperToAxisVM
{
    public partial class Structs
    {
        public abstract class AxisLoad
        {
            public static int counter = 0;
            public int id;
            public AxisLoad(int x = -1)
            {
                if (x == -1) this.id = counter++;
                else this.id = x;
            }
            public abstract AxisLoad Clone();
        }
        public abstract class AxisMember 
        {
            public static int counter = 0;
            public int id;
            public List<int> axisid_point = new List<int>();
            public List<int> axisid_line = new List<int>();
            public List<int> axisid_mesh = new List<int>();
            public List<int> axisid_dom = new List<int>();
            public List<int> axisid_edge = new List<int>();
            //public bool deleting = true; // DEFAULT should be true, to send all new data at first
            //public bool drawing = true; //DEFAULT should be false, to send all new data at first
            public override bool Equals(Object obj)
            {
                //Check for null and compare run-time types.
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    return true;
                    //AxisMember p  = (AxisMember)obj;
                    //return p.id==this.id && this.axisid_point.SequenceEqual(p.axisid_point) && this.axisid_line.SequenceEqual(p.axisid_line) && this.axisid_mesh.SequenceEqual(p.axisid_mesh) && this.axisid_edge.SequenceEqual(p.axisid_edge) && this.axisid_dom.SequenceEqual(p.axisid_dom);
                }
            }

            public override int GetHashCode()
            {
                return this.id;
            }

            public abstract bool GeometryEquals(Object obj);

            public AxisMember(int x = -1)
            {
                if (x == -1) this.id = counter++;
                else this.id = x;
            }
            public abstract AxisMember Clone();
            //points
            public void clearp()
            {
                this.axisid_point.Clear();
            }
            public void addp(Array input)
            {
                foreach(int i in input)
                {
                    this.axisid_point.Add(i);
                }
            }
            //lines
            public void clearl()
            {
                this.axisid_line.Clear();
            }
            public void addl(Array input)
            {
                foreach (int i in input)
                {
                    this.axisid_line.Add(i);
                }
            }
            //mesh
            public void clearm()
            {
                this.axisid_mesh.Clear();
            }
            public void addm(Array input)
            {
                foreach (int i in input)
                {
                    this.axisid_mesh.Add(i);
                }
            }
            //dom
            public void cleard()
            {
                this.axisid_dom.Clear();
            }
            public void addd(Array input)
            {
                foreach (int i in input)
                {
                    this.axisid_dom.Add(i);
                }
            }
            //edges
            public void cleare()
            {
                this.axisid_edge.Clear();
            }
            public void adde(Array input)
            {
                foreach (int i in input)
                {
                    this.axisid_edge.Add(i);
                }
            }
        }
        public struct TriggerData //ALLOWS TO KNOW WHICH PARAMETERS TRIGGERED SENDER
        {
            public bool lineTrigerred;
            public bool meshToSurfacesTriggered;
            public bool meshToDomainTriggered;
            public bool meshToEdgesTriggered;
            public bool loadCaseTriggered;
            public bool domainTriggered;
            public TriggerData(bool noReply)
            {
                lineTrigerred = true;
                meshToDomainTriggered = true;
                meshToSurfacesTriggered = true;
                loadCaseTriggered = true;
                meshToEdgesTriggered = true;
                domainTriggered = true;
            }
        }
    }
}
