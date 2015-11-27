using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Voronoi
{
    public class HalfEdge
    {
        private Vertex origin;

        public HalfEdge(Vertex v)
        {
            origin = new Vertex(v.x, v.y);
            
            Twin = this;
            Next = null;
            Prev = null;
        }

        public Vertex Origin
        {
            get { return origin; }
            set { origin = new Vertex(value.x, value.y); }
        }

        public Face Face;
        public HalfEdge Twin;
        public HalfEdge Next;
        public HalfEdge Prev;
    }

}
