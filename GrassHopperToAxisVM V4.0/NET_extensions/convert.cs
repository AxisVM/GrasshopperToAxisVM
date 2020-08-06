using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using Rhino.Geometry.Collections;

namespace GrassHopperToAxisVM
{
    public class convert
    {
        public static int[] toArray(Array input)
        {
            int[] returner = new int[input.Length];
            int index = 0;
            foreach(int x in input)
            {
                returner[index++] = x;
            }
            return returner;
        }
        public static bool PolyLineEquals(Polyline a, Polyline b)
        {
            if (a.SegmentCount != b.SegmentCount) return false;
            else
            {
                for (int i = 0; i < a.SegmentCount; i++)
                {
                    if (!a.SegmentAt(i).Equals(b.SegmentAt(i))) { return false; }
                }
                return true;
            }
        }

        public static bool MeshEquals(Mesh a, Mesh b)
        {
            MeshTopologyVertexList vertex_a = a.TopologyVertices;
            MeshTopologyVertexList vertex_b = b.TopologyVertices;
            int points_a = vertex_a.Count;
            int points_b = vertex_b.Count;
            if (points_a != points_b) return false;
            else
            {
                for (int i = 0; i < points_a; i++)
                {
                    if (vertex_a.ElementAt(i).X != vertex_b.ElementAt(i).X) { return false; }
                    if (vertex_a.ElementAt(i).Y != vertex_b.ElementAt(i).Y) { return false; }
                    if (vertex_a.ElementAt(i).Z != vertex_b.ElementAt(i).Z) { return false; }
                }
                return true;
            }
        }
    }
}
