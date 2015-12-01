using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Voronoi
{
    public class Face
    {
        public Face(HalfEdge halfEdge)
        {
            this.HalfEdge = halfEdge;
            this.Brush = null;
        }

        public HalfEdge HalfEdge;

        public Brush Brush;

        public bool inside(Vertex a_u1)
        {
            int i = 0;
            int j = 0;
            bool inside = false;

            List<HalfEdge> halfEdges = new List<HalfEdge>() { HalfEdge, HalfEdge.Next, HalfEdge.Next.Next };

            for (i = 0, j = 2; i < 3; j = i++)
            {
                if (((halfEdges[i].Origin.y > a_u1.y) != (halfEdges[j].Origin.y > a_u1.y)) &&
                 (a_u1.x < (halfEdges[j].Origin.x - halfEdges[i].Origin.x) * (a_u1.y - halfEdges[i].Origin.y) / (halfEdges[j].Origin.y - halfEdges[i].Origin.y) + halfEdges[i].Origin.x))
                    inside = !inside;
            }
            return inside;
        }
    }
}
